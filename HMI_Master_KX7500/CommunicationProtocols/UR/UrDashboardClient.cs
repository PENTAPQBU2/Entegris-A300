using System;

namespace HMI_Master_KX7500.CommunicationProtocols.UR
{
    //UR Dashboard server (port 29999). Robot must be in Remote control for commands that change state.
    public class UrDashboardClient : IDisposable
    {
        public const int Port = 29999;
        private readonly UrLineClient line = new UrLineClient("Dashboard", Port);

        public bool IsConnected => line.IsConnected;

        public void Connect(string ipAddress)
        {
            line.Connect(ipAddress);
            line.ReadLine(2000); //"Connected: Universal Robots Dashboard Server"
        }

        public void Disconnect() => line.Disconnect();

        public string Send(string command) => line.Send(command);

        public string PowerOn() => line.Send("power on");
        public string PowerOff() => line.Send("power off");
        public string BrakeRelease() => line.Send("brake release");
        public string Play() => line.Send("play");
        public string Pause() => line.Send("pause");
        public string Stop() => line.Send("stop");
        public string CloseSafetyPopup() => line.Send("close safety popup");
        public string UnlockProtectiveStop() => line.Send("unlock protective stop");
        public string CloseInfoPopup() => line.Send("close popup");
        public string RobotMode() => line.Send("robotmode");
        public string SafetyStatus() => line.Send("safetystatus");
        public string ProgramState() => line.Send("programState");
        public string LoadedProgram() => line.Send("get loaded program");

        public bool IsInRemoteControl()
        {
            string reply = line.Send("is in remote control");
            return reply != null && reply.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        public void Dispose() => line.Dispose();
    }
}
