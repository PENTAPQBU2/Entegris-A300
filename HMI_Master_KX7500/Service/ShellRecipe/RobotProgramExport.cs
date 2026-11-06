using HMI_Master_KX7500.Model.ShellRecipe;
using NLog;
using PQ.HMI;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Sockets;

namespace HMI_Master_KX7500.Service.ShellRecipe
{
    /// <summary>Files on the robot pendant. Nothing on the robot is ever overwritten.</summary>
    public interface IRobotFiles : IDisposable
    {
        /// <summary>Throws <see cref="RobotNotReachableException"/> when the robot cannot be reached or refuses the login.</summary>
        void Connect();

        bool Exists(string remotePath);

        void Download(string remotePath, string localPath);

        /// <summary>New folder; fails when it exists.</summary>
        void CreateFolder(string remotePath);

        /// <summary>New file; fails when it exists (never overwrites).</summary>
        void Upload(string localPath, string remotePath);
    }

    public class RobotNotReachableException : Exception
    {
        public RobotNotReachableException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>SFTP to the pendant (SSH.NET, password login). The host key is accepted (machine network) and logged.</summary>
    public sealed class SftpRobotFiles : IRobotFiles
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly RobotSettingModel settings;
        private SftpClient client;

        public SftpRobotFiles(RobotSettingModel settings)
        {
            this.settings = settings;
        }

        public void Connect()
        {
            ConnectionInfo info = new ConnectionInfo(settings.Host, settings.Port, settings.User,
                new PasswordAuthenticationMethod(settings.User, settings.Password))
            {
                Timeout = TimeSpan.FromMilliseconds(settings.TimeoutMs),
            };
            client = new SftpClient(info) { OperationTimeout = TimeSpan.FromMilliseconds(settings.TimeoutMs * 4) };
            client.HostKeyReceived += (sender, e) =>
            {
                logger.Info($"Robot {settings.Host} host key {e.HostKeyName} {e.FingerPrintSHA256}");
                e.CanTrust = true;
            };
            try
            {
                client.Connect();
            }
            catch (Exception ex) when (ex is SocketException || ex is SshConnectionException || ex is SshAuthenticationException
                || ex is SshOperationTimeoutException || ex is ProxyException || ex is TimeoutException)
            {
                throw new RobotNotReachableException($"{settings.User}@{settings.Host}:{settings.Port}: {ex.Message}", ex);
            }
        }

        public bool Exists(string remotePath) => sftp.Exists(remotePath);

        public void Download(string remotePath, string localPath)
        {
            using (FileStream file = File.Create(localPath))
            {
                sftp.DownloadFile(remotePath, file);
            }
        }

        public void CreateFolder(string remotePath)
        {
            if (sftp.Exists(remotePath))
            {
                throw new IOException($"{remotePath} exists already.");
            }
            sftp.CreateDirectory(remotePath);
        }

        public void Upload(string localPath, string remotePath)
        {
            using (FileStream file = File.OpenRead(localPath))
            {
                sftp.UploadFile(file, remotePath, false);
            }
        }

        public void Dispose()
        {
            if (client != null && client.IsConnected)
            {
                client.Disconnect();
            }
            client?.Dispose();
            client = null;
        }

        private SftpClient sftp => client ?? throw new InvalidOperationException("Not connected.");
    }

    public enum RobotExportStatus
    {
        Created,

        /// <summary>Robot not reachable / login refused: the pendrive way can be used instead.</summary>
        NotReachable,

        Failed,
    }

    public class RobotExportResult
    {
        public RobotExportResult(RobotExportStatus status, string message, string folder = "", string scriptName = "")
        {
            Status = status;
            Message = message;
            Folder = folder;
            ScriptName = scriptName;
        }

        public RobotExportStatus Status { get; }

        public string Message { get; }

        /// <summary>The new "Robot Program (..)" folder (robot path or local path).</summary>
        public string Folder { get; }

        /// <summary>File name of the new script in <see cref="Folder"/>.</summary>
        public string ScriptName { get; }
    }

    /// <summary>
    /// MT_001 Create Points without a pendrive: reads the installation in use from the robot, makes the new installation and script
    /// (<see cref="RobotPointService.CreatePoints"/>, unchanged) and puts both into a new folder on the robot
    /// "&lt;ProgramFolder&gt;/Robot Program (yyyy-MM-dd HH-mm-ss)/". Nothing on the robot is overwritten: the operator copies the
    /// files over the ones in use on the pendant. <see cref="ToLocal"/> makes the same folder on the PC (pendrive fallback).
    /// Same as V2.0 (Entegris-A300-V2.0 b0b9bbe).
    /// </summary>
    public static class RobotProgramExport
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private const string stamp = "yyyy-MM-dd HH-mm-ss";

        /// <summary>Robot settings file; written with the defaults when it is missing.</summary>
        public static RobotSettingModel LoadSettings(string file)
        {
            RobotSettingModel settings = new RobotSettingModel();
            try
            {
                if (File.Exists(file))
                {
                    settings = XmlTools.Deserialize<RobotSettingModel>(file, settings) ?? new RobotSettingModel();
                }
                else
                {
                    XmlTools.Serialize(file, settings);
                }
            }
            catch (Exception ex) { logger.Error(ex); }
            return settings;
        }

        public static string FolderName(DateTime now) => $"Robot Program ({now.ToString(stamp, CultureInfo.InvariantCulture)})";

        public static string ScriptName(RobotSettingModel settings, DateTime now) =>
            $"{settings.ScriptBaseName} {now.ToString(stamp, CultureInfo.InvariantCulture)}.script";

        public static RobotExportResult ToRobot(IRobotFiles files, RobotSettingModel settings, List<ShellRecipeModel> recipes, DateTime now)
        {
            string temp = Path.Combine(Path.GetTempPath(), "A300_CreatePoints_" + Guid.NewGuid().ToString("N"));
            string source = settings.RemotePath(settings.InstallationFileName);
            string folder = settings.RemotePath(FolderName(now));
            string scriptName = ScriptName(settings, now);
            try
            {
                files.Connect();
                if (!files.Exists(source))
                {
                    return new RobotExportResult(RobotExportStatus.Failed, $"{source} not found on the robot.");
                }
                if (files.Exists(folder))
                {
                    return new RobotExportResult(RobotExportStatus.Failed, $"{folder} exists already on the robot, try again.");
                }

                Directory.CreateDirectory(temp);
                string localSource = Path.Combine(temp, "source.installation");
                string localInstallation = Path.Combine(temp, settings.InstallationFileName);
                string localScript = Path.Combine(temp, scriptName);
                files.Download(source, localSource);
                if (!RobotPointService.CreatePoints(localSource, localInstallation, localScript, recipes, out string message))
                {
                    return new RobotExportResult(RobotExportStatus.Failed, message);
                }

                files.CreateFolder(folder);
                files.Upload(localInstallation, $"{folder}/{settings.InstallationFileName}");
                files.Upload(localScript, $"{folder}/{scriptName}");
                return new RobotExportResult(RobotExportStatus.Created, message, folder, scriptName);
            }
            catch (RobotNotReachableException ex)
            {
                return new RobotExportResult(RobotExportStatus.NotReachable, ex.Message);
            }
            catch (Exception ex) when (ex is SshException || ex is IOException || ex is UnauthorizedAccessException)
            {
                logger.Error(ex);
                return new RobotExportResult(RobotExportStatus.Failed, $"Robot file transfer failed: {ex.Message}");
            }
            finally
            {
                try
                {
                    if (Directory.Exists(temp))
                    {
                        Directory.Delete(temp, true);
                    }
                }
                catch (Exception ex) { logger.Error(ex); }
            }
        }

        /// <summary>Same folder and file names in <paramref name="targetDirectory"/> (e.g. a pendrive), from a local installation file.</summary>
        public static RobotExportResult ToLocal(string sourceFile, string targetDirectory, RobotSettingModel settings,
            List<ShellRecipeModel> recipes, DateTime now)
        {
            string folder = Path.Combine(targetDirectory, FolderName(now));
            string scriptName = ScriptName(settings, now);
            if (Directory.Exists(folder))
            {
                return new RobotExportResult(RobotExportStatus.Failed, $"{folder} exists already, try again.");
            }
            try
            {
                Directory.CreateDirectory(folder);
                if (!RobotPointService.CreatePoints(sourceFile, Path.Combine(folder, settings.InstallationFileName),
                    Path.Combine(folder, scriptName), recipes, out string message))
                {
                    Directory.Delete(folder, true);
                    return new RobotExportResult(RobotExportStatus.Failed, message);
                }
                return new RobotExportResult(RobotExportStatus.Created, message, folder, scriptName);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                logger.Error(ex);
                return new RobotExportResult(RobotExportStatus.Failed, ex.Message);
            }
        }
    }
}
