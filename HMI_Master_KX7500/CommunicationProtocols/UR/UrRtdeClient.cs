using HMI_Master_KX7500.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HMI_Master_KX7500.CommunicationProtocols.UR
{
    //UR Real-Time Data Exchange client (port 30004, protocol version 2).
    //Outputs: robot status and positions. Inputs: HMI heartbeat (int register 24) and speed slider.
    public class UrRtdeClient : IDisposable
    {
        public const int Port = 30004;
        public const int HeartbeatRegister = 24;

        private const byte RequestProtocolVersion = 86;  //'V'
        private const byte TextMessage = 77;             //'M'
        private const byte DataPackage = 85;             //'U'
        private const byte SetupOutputs = 79;            //'O'
        private const byte SetupInputs = 73;             //'I'
        private const byte Start = 83;                   //'S'
        private const byte Pause = 80;                   //'P'

        private static readonly string[] outputNames =
        {
            "timestamp", "actual_q", "actual_TCP_pose", "robot_mode", "safety_status", "runtime_state",
            "speed_scaling", "target_speed_fraction", "output_bit_registers0_to_31",
            "output_int_register_0", "output_int_register_1"
        };

        private static readonly string[] inputNames =
        {
            "input_int_register_" + HeartbeatRegister, "speed_slider_mask", "speed_slider_fraction"
        };

        private readonly object sendLock = new object();
        private readonly object stateLock = new object();
        private TcpClient client;
        private NetworkStream stream;
        private Thread readerThread;
        private volatile bool connected;
        private string[] outputTypes;
        private byte inputRecipeId;
        private UrRobotState state = new UrRobotState();
        private int heartbeat;

        public bool IsConnected => connected;
        public double OutputFrequencyHz { get; set; } = 10;

        public event Action<UrRobotState> StateReceived;

        public UrRobotState State
        {
            get { lock(stateLock) { return state.Copy(); } }
        }

        public void Connect(string ipAddress, int timeoutMs = 2000)
        {
            Disconnect();

            TcpClient tcp = new TcpClient();
            IAsyncResult result = tcp.BeginConnect(ipAddress, Port, null, null);
            if(!result.AsyncWaitHandle.WaitOne(timeoutMs))
            {
                tcp.Close();
                throw new TimeoutException($"RTDE: no answer from {ipAddress}:{Port}");
            }
            tcp.EndConnect(result);
            tcp.NoDelay = true;
            tcp.ReceiveTimeout = timeoutMs;
            client = tcp;
            stream = tcp.GetStream();

            //Protocol version 2
            sendPacket(RequestProtocolVersion, BigEndian.UInt16(2));
            if(readPacketOfType(RequestProtocolVersion)[0] != 1)
            {
                throw new InvalidOperationException("RTDE: protocol version 2 not accepted");
            }

            //Outputs
            List<byte> setup = new List<byte>(BigEndian.Double(OutputFrequencyHz));
            setup.AddRange(Encoding.ASCII.GetBytes(string.Join(",", outputNames)));
            sendPacket(SetupOutputs, setup.ToArray());
            byte[] reply = readPacketOfType(SetupOutputs);
            outputTypes = Encoding.ASCII.GetString(reply, 1, reply.Length - 1).Split(',');
            checkTypes("output", outputNames, outputTypes);

            //Inputs
            sendPacket(SetupInputs, Encoding.ASCII.GetBytes(string.Join(",", inputNames)));
            reply = readPacketOfType(SetupInputs);
            inputRecipeId = reply[0];
            checkTypes("input", inputNames, Encoding.ASCII.GetString(reply, 1, reply.Length - 1).Split(','));

            sendPacket(Start, new byte[0]);
            if(readPacketOfType(Start)[0] != 1)
            {
                throw new InvalidOperationException("RTDE: start not accepted");
            }

            tcp.ReceiveTimeout = 0;
            connected = true;
            readerThread = new Thread(readLoop) { IsBackground = true, Name = "RTDE" };
            readerThread.Start();
            LogHelper.KeyValue(GetType().Name, "RTDE", $"Connected {ipAddress}:{Port}");
        }

        public void Disconnect()
        {
            bool wasConnected = connected;
            connected = false;
            try
            {
                if(wasConnected) { sendPacket(Pause, new byte[0]); }
            }
            catch(Exception) { }
            try { client?.Close(); } catch(Exception) { }
            client = null;
            stream = null;
            lock(stateLock) { state = new UrRobotState(); }
        }

        //Changes the heartbeat register; the robot watchdog stops the program when it stops changing in HMI teach
        public void SendHeartbeat()
        {
            sendInputs(unchecked(++heartbeat), 0, 0);
        }

        //Sets the robot speed slider, 0.01 .. 1.00
        public void SetSpeedSlider(double fraction)
        {
            fraction = Math.Max(0.01, Math.Min(1.0, fraction));
            sendInputs(heartbeat, 1, fraction);
        }

        private void sendInputs(int heartbeatValue, uint sliderMask, double sliderFraction)
        {
            if(!connected) { throw new InvalidOperationException("RTDE: not connected"); }
            List<byte> data = new List<byte> { inputRecipeId };
            data.AddRange(BigEndian.Int32(heartbeatValue));
            data.AddRange(BigEndian.UInt32(sliderMask));
            data.AddRange(BigEndian.Double(sliderFraction));
            sendPacket(DataPackage, data.ToArray());
        }

        private static void checkTypes(string kind, string[] names, string[] types)
        {
            for(int i = 0; i < names.Length; i++)
            {
                string type = i < types.Length ? types[i] : "MISSING";
                if(type == "NOT_FOUND" || type == "IN_USE" || type == "MISSING")
                {
                    throw new InvalidOperationException($"RTDE {kind} '{names[i]}': {type}");
                }
            }
        }

        private void readLoop()
        {
            try
            {
                while(connected)
                {
                    byte type;
                    byte[] payload = readPacket(out type);
                    if(type == DataPackage && payload.Length > 0)
                    {
                        parseOutputs(payload);
                    }
                    else if(type == TextMessage)
                    {
                        LogHelper.KeyValue(GetType().Name, "RTDE message", Encoding.ASCII.GetString(payload));
                    }
                }
            }
            catch(Exception ex)
            {
                if(connected) { LogHelper.Error(GetType().Name, ex); }
            }
            finally
            {
                connected = false;
                lock(stateLock) { state.Connected = false; }
            }
        }

        private void parseOutputs(byte[] p)
        {
            UrRobotState s = new UrRobotState { Connected = true, Received = DateTime.Now };
            int pos = 1; //byte 0 = output recipe id

            for(int i = 0; i < outputNames.Length; i++)
            {
                switch(outputNames[i])
                {
                    case "actual_q": s.ActualQ = readVector6(p, ref pos); break;
                    case "actual_TCP_pose": s.TcpPose = readVector6(p, ref pos); break;
                    case "robot_mode": s.RobotMode = BigEndian.ReadInt32(p, ref pos); break;
                    case "safety_status": s.SafetyStatus = BigEndian.ReadInt32(p, ref pos); break;
                    case "runtime_state": s.RuntimeState = (uint)BigEndian.ReadInt32(p, ref pos); break;
                    case "speed_scaling": s.SpeedScaling = BigEndian.ReadDouble(p, ref pos); break;
                    case "target_speed_fraction": s.SpeedFraction = BigEndian.ReadDouble(p, ref pos); break;
                    case "output_bit_registers0_to_31": s.OutputBits = (uint)BigEndian.ReadInt32(p, ref pos); break;
                    case "output_int_register_0": s.CurrentPosOut = BigEndian.ReadInt32(p, ref pos); break;
                    case "output_int_register_1": s.RecipeIdOut = BigEndian.ReadInt32(p, ref pos); break;
                    default: pos += typeSize(outputTypes[i]); break;
                }
            }

            lock(stateLock) { state = s; }
            StateReceived?.Invoke(s.Copy());
        }

        private static double[] readVector6(byte[] p, ref int pos)
        {
            double[] v = new double[6];
            for(int i = 0; i < 6; i++) { v[i] = BigEndian.ReadDouble(p, ref pos); }
            return v;
        }

        private static int typeSize(string type)
        {
            switch(type)
            {
                case "BOOL":
                case "UINT8": return 1;
                case "INT32":
                case "UINT32": return 4;
                case "UINT64":
                case "DOUBLE": return 8;
                case "VECTOR3D": return 24;
                case "VECTOR6D": return 48;
                case "VECTOR6INT32":
                case "VECTOR6UINT32": return 24;
                default: throw new InvalidOperationException("RTDE: unknown type " + type);
            }
        }

        private void sendPacket(byte type, byte[] payload)
        {
            lock(sendLock)
            {
                NetworkStream s = stream;
                if(s == null) { throw new InvalidOperationException("RTDE: not connected"); }
                byte[] packet = new byte[3 + payload.Length];
                byte[] size = BigEndian.UInt16((ushort)packet.Length);
                packet[0] = size[0];
                packet[1] = size[1];
                packet[2] = type;
                Buffer.BlockCopy(payload, 0, packet, 3, payload.Length);
                s.Write(packet, 0, packet.Length);
            }
        }

        private byte[] readPacketOfType(byte wanted)
        {
            for(int i = 0; i < 20; i++)
            {
                byte type;
                byte[] payload = readPacket(out type);
                if(type == wanted) { return payload; }
            }
            throw new InvalidOperationException($"RTDE: no reply of type {(char)wanted}");
        }

        private byte[] readPacket(out byte type)
        {
            byte[] header = readExactly(3);
            int size = (header[0] << 8) | header[1];
            type = header[2];
            return readExactly(size - 3);
        }

        private byte[] readExactly(int count)
        {
            byte[] buffer = new byte[count];
            int read = 0;
            while(read < count)
            {
                int n = stream.Read(buffer, read, count - read);
                if(n <= 0) { throw new IOException("RTDE: connection closed"); }
                read += n;
            }
            return buffer;
        }

        public void Dispose()
        {
            Disconnect();
        }

        private static class BigEndian
        {
            public static byte[] UInt16(ushort v) => new[] { (byte)(v >> 8), (byte)v };

            public static byte[] UInt32(uint v) => new[] { (byte)(v >> 24), (byte)(v >> 16), (byte)(v >> 8), (byte)v };

            public static byte[] Int32(int v) => UInt32(unchecked((uint)v));

            public static byte[] Double(double v)
            {
                byte[] b = BitConverter.GetBytes(v);
                if(BitConverter.IsLittleEndian) { Array.Reverse(b); }
                return b;
            }

            public static int ReadInt32(byte[] p, ref int pos)
            {
                int v = (p[pos] << 24) | (p[pos + 1] << 16) | (p[pos + 2] << 8) | p[pos + 3];
                pos += 4;
                return v;
            }

            public static double ReadDouble(byte[] p, ref int pos)
            {
                byte[] b = new byte[8];
                Buffer.BlockCopy(p, pos, b, 0, 8);
                if(BitConverter.IsLittleEndian) { Array.Reverse(b); }
                pos += 8;
                return BitConverter.ToDouble(b, 0);
            }
        }
    }
}
