using HMI_Master_KX7500.CommunicationProtocols.UR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace HMI_Master_KX7500.Service.Robot
{
    //Offline mode: behaves like the robot for the Robot page (no motion timing, moves complete at once)
    public class UrRobotSimulator : IUrRobot
    {
        private const string PosePattern = @"p\[([^\]]+)\]";
        private readonly object stateLock = new object();
        private readonly Dictionary<string, double[]> variables = new Dictionary<string, double[]>();
        private UrRobotState state = new UrRobotState();
        private Timer timer;

        public UrRobotSimulator()
        {
            //Shared points with their current values (spec Appendix B), so move-to works offline
            variables["P_Home"] = new[] { -0.10587, -0.44220, 0.60338, -2.92329, -1.13234, -0.03139 };
            variables["P_Safe"] = new[] { 0.22729, -0.42340, 0.58251, 2.37846, 2.04100, -0.01287 };
            variables["P_CalReady"] = new[] { 0.19342, -0.20692, 0.63237, 2.37851, 2.04097, -0.01287 };
            variables["P_CalStart"] = new[] { 0.19340, -0.20692, 0.63238, 2.37849, 2.04096, -0.01290 };
            variables["P_Cal85Nm"] = new[] { 0.19342, -0.20691, 0.63237, 2.37851, 2.04099, -0.01285 };
            variables["P_Cal14Nm"] = new[] { 0.19342, -0.20691, 0.63236, 2.37853, 2.04105, -0.01290 };
        }

        public bool IsConnected { get; private set; }
        public bool IsRemoteControl => IsConnected;
        public bool InterpreterReady => IsConnected && State.InTeachMode;

        public UrRobotState State
        {
            get { lock(stateLock) { return state.Copy(); } }
        }

        public event Action<UrRobotState> StateChanged;

        public void Connect(string ipAddress)
        {
            lock(stateLock)
            {
                state = new UrRobotState
                {
                    Connected = true,
                    RobotMode = 7,
                    SafetyStatus = 1,
                    RuntimeState = 2,
                    SpeedFraction = 0.3,
                    SpeedScaling = 1,
                    OutputBits = 1u,     //AtHome
                    RecipeIdOut = 1,
                    TcpPose = (double[])variables["P_Home"].Clone()
                };
            }
            IsConnected = true;
            timer = new Timer(_ => publish(), null, 0, 100);
        }

        public void Disconnect()
        {
            timer?.Dispose();
            timer = null;
            IsConnected = false;
            lock(stateLock) { state = new UrRobotState(); }
            publish();
        }

        public string Dashboard(string command)
        {
            lock(stateLock)
            {
                switch(command.Trim().ToLowerInvariant())
                {
                    case "power on": state.RobotMode = 5; return "Powering on";
                    case "brake release": state.RobotMode = 7; return "Brake releasing";
                    case "unlock protective stop": state.SafetyStatus = 1; return "Protective stop releasing";
                    case "close safety popup": return "closing safety popup";
                    case "play": state.RuntimeState = 2; return "Starting program";
                    case "pause": state.RuntimeState = 4; return "Pausing program";
                    case "stop": state.RuntimeState = 1; return "Stopped";
                    default: return "(simulated) " + command;
                }
            }
        }

        public bool Execute(string statement, out string reply)
        {
            reply = "discard: not understood by simulator: " + statement;
            if(!InterpreterReady)
            {
                reply = "Robot is not in HMI teach mode";
                return false;
            }

            string s = statement.Trim();
            Match m;
            lock(stateLock)
            {
                if(s == "end_interpreter()")
                {
                    state.OutputBits &= ~(1u << 9);
                }
                else if((m = Regex.Match(s, @"^Rcp_ID\s*:=\s*(\d+)$")).Success)
                {
                    state.RecipeIdOut = int.Parse(m.Groups[1].Value);
                }
                else if((m = Regex.Match(s, @"^(\w+)\s*:=\s*" + PosePattern + "$")).Success)
                {
                    variables[m.Groups[1].Value] = parsePose(m.Groups[2].Value);
                }
                else if((m = Regex.Match(s, @"^movel\(pose_add\(get_actual_tcp_pose\(\),\s*" + PosePattern)).Success)
                {
                    double[] d = parsePose(m.Groups[1].Value);
                    for(int i = 0; i < 3; i++) { state.TcpPose[i] += d[i]; }
                }
                else if((m = Regex.Match(s, @"^move[lj]\((?:get_inverse_kin\()?" + PosePattern)).Success)
                {
                    state.TcpPose = parsePose(m.Groups[1].Value);
                }
                else if((m = Regex.Match(s, @"^move[lj]\((?:get_inverse_kin\()?(\w+)")).Success)
                {
                    double[] pose;
                    if(!variables.TryGetValue(m.Groups[1].Value, out pose))
                    {
                        reply = "discard: variable not defined: " + m.Groups[1].Value;
                        return false;
                    }
                    state.TcpPose = (double[])pose.Clone();
                }
                else
                {
                    return false;
                }
            }

            reply = "ack: " + s;
            return true;
        }

        public void AbortMotion() { }

        public void SetSpeedSlider(double fraction)
        {
            lock(stateLock) { state.SpeedFraction = Math.Max(0.01, Math.Min(1.0, fraction)); }
        }

        public void NotifyTeachRequest(bool on)
        {
            lock(stateLock)
            {
                state.OutputBits = on ? (1u << 9) : (state.OutputBits & ~(1u << 9));
            }
        }

        private static double[] parsePose(string text)
        {
            return text.Split(',').Select(x => double.Parse(x.Trim(), CultureInfo.InvariantCulture)).ToArray();
        }

        private void publish()
        {
            StateChanged?.Invoke(State);
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
