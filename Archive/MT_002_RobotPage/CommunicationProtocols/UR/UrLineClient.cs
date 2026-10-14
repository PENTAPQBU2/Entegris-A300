using HMI_Master_KX7500.Helpers;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HMI_Master_KX7500.CommunicationProtocols.UR
{
    //Text protocol client: one command per line, one reply line back (UR Dashboard server and interpreter socket)
    public class UrLineClient : IDisposable
    {
        private readonly string name;
        private readonly int port;
        private readonly object sendLock = new object();
        private readonly BlockingCollection<string> replies = new BlockingCollection<string>();
        private TcpClient client;
        private StreamWriter writer;
        private Thread readerThread;
        private volatile bool connected;

        public UrLineClient(string name, int port)
        {
            this.name = name;
            this.port = port;
        }

        public bool IsConnected => connected;
        public int ReplyTimeoutMs { get; set; } = 2000;

        //Lines the robot sent that were not a reply to a command (e.g. late or unsolicited messages)
        public event Action<string> LineReceived;

        public void Connect(string ipAddress, int timeoutMs = 2000)
        {
            Disconnect();

            TcpClient tcp = new TcpClient();
            IAsyncResult result = tcp.BeginConnect(ipAddress, port, null, null);
            if(!result.AsyncWaitHandle.WaitOne(timeoutMs))
            {
                tcp.Close();
                throw new TimeoutException($"{name}: no answer from {ipAddress}:{port}");
            }
            tcp.EndConnect(result);
            tcp.NoDelay = true;

            client = tcp;
            NetworkStream stream = tcp.GetStream();
            writer = new StreamWriter(stream, new UTF8Encoding(false)) { NewLine = "\n", AutoFlush = true };
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            connected = true;

            readerThread = new Thread(() => readLoop(reader)) { IsBackground = true, Name = name };
            readerThread.Start();
            LogHelper.KeyValue(GetType().Name, name, $"Connected {ipAddress}:{port}");
        }

        public void Disconnect()
        {
            connected = false;
            try { client?.Close(); } catch(Exception) { }
            client = null;
            writer = null;
            string discard;
            while(replies.TryTake(out discard)) { }
        }

        //Sends one line and returns the next line the robot sends, or null when none comes within the timeout
        public string Send(string line)
        {
            lock(sendLock)
            {
                if(!connected || writer == null)
                {
                    throw new InvalidOperationException($"{name}: not connected");
                }

                string stale;
                while(replies.TryTake(out stale))
                {
                    LineReceived?.Invoke(stale);
                }

                writer.WriteLine(line);
                LogHelper.KeyValue(GetType().Name, name + " >", line);

                string reply;
                if(replies.TryTake(out reply, ReplyTimeoutMs))
                {
                    LogHelper.KeyValue(GetType().Name, name + " <", reply);
                    return reply;
                }
                LogHelper.KeyValue(GetType().Name, name + " <", "(no reply)");
                return null;
            }
        }

        //Reads one unsolicited line, e.g. the Dashboard welcome message
        public string ReadLine(int timeoutMs)
        {
            string line;
            return replies.TryTake(out line, timeoutMs) ? line : null;
        }

        private void readLoop(StreamReader reader)
        {
            try
            {
                string line;
                while(connected && (line = reader.ReadLine()) != null)
                {
                    replies.Add(line);
                }
            }
            catch(IOException) { }
            catch(ObjectDisposedException) { }
            catch(Exception ex) { LogHelper.Error(GetType().Name, ex); }
            finally
            {
                if(connected)
                {
                    LogHelper.KeyValue(GetType().Name, name, "Connection closed by robot");
                }
                connected = false;
            }
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
