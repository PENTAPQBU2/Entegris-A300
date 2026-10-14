using HMI_Master_KX7500.CommunicationProtocols.UR;
using HMI_Master_KX7500.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HMI_Master_KX7500.Service.Robot
{
    //Real UR robot: RTDE for state + heartbeat, Dashboard for start/stop/unlock, interpreter for teach motions.
    //The interpreter socket is opened automatically while the robot reports InTeachMode.
    public class UrRobot : IUrRobot
    {
        private readonly UrRtdeClient rtde = new UrRtdeClient();
        private readonly UrDashboardClient dashboard = new UrDashboardClient();
        private readonly UrInterpreterClient interpreter = new UrInterpreterClient();
        private Timer heartbeatTimer;
        private string ipAddress = string.Empty;
        private int interpreterConnecting;
        private DateTime lastInterpreterAttempt = DateTime.MinValue;

        public UrRobot()
        {
            rtde.StateReceived += onStateReceived;
        }

        public bool IsConnected => rtde.IsConnected;
        public bool IsRemoteControl { get; private set; }
        public bool InterpreterReady => interpreter.IsConnected;
        public UrRobotState State => rtde.State;

        public event Action<UrRobotState> StateChanged;

        public void Connect(string ip)
        {
            Disconnect();
            ipAddress = ip;
            rtde.Connect(ip);

            try
            {
                dashboard.Connect(ip);
                IsRemoteControl = dashboard.IsInRemoteControl();
            }
            catch(Exception ex)
            {
                IsRemoteControl = false;
                LogHelper.Error(GetType().Name, "Dashboard not available: " + ex.Message);
            }

            heartbeatTimer = new Timer(_ => sendHeartbeat(), null, 0, 100);
        }

        public void Disconnect()
        {
            heartbeatTimer?.Dispose();
            heartbeatTimer = null;
            interpreter.Disconnect();
            dashboard.Disconnect();
            rtde.Disconnect();
            IsRemoteControl = false;
        }

        public string Dashboard(string command)
        {
            if(!dashboard.IsConnected)
            {
                dashboard.Connect(ipAddress);
            }
            return dashboard.Send(command);
        }

        public bool Execute(string statement, out string reply)
        {
            if(!interpreter.IsConnected)
            {
                reply = "Robot is not in HMI teach mode";
                return false;
            }
            return interpreter.Execute(statement, out reply);
        }

        public void AbortMotion()
        {
            if(interpreter.IsConnected)
            {
                interpreter.AbortMotion();
            }
        }

        public void SetSpeedSlider(double fraction) => rtde.SetSpeedSlider(fraction);

        public void NotifyTeachRequest(bool on)
        {
            //Real robot: the PLC sets HMI_TeachMode (W0FD bit 0) from MR6001/MR6002
        }

        private void sendHeartbeat()
        {
            try
            {
                if(rtde.IsConnected)
                {
                    rtde.SendHeartbeat();
                }
            }
            catch(Exception ex) { LogHelper.Error(GetType().Name, "Heartbeat: " + ex.Message); }
        }

        private void onStateReceived(UrRobotState state)
        {
            if(state.InTeachMode && !interpreter.IsConnected)
            {
                openInterpreter();
            }
            else if(!state.InTeachMode && interpreter.IsConnected)
            {
                interpreter.Disconnect();
            }

            StateChanged?.Invoke(state);
        }

        private void openInterpreter()
        {
            if((DateTime.Now - lastInterpreterAttempt).TotalSeconds < 1 ||
               Interlocked.Exchange(ref interpreterConnecting, 1) == 1)
            {
                return;
            }
            lastInterpreterAttempt = DateTime.Now;

            Task.Run(() =>
            {
                try { interpreter.Connect(ipAddress); }
                catch(Exception ex) { LogHelper.Error(GetType().Name, "Interpreter: " + ex.Message); }
                finally { Interlocked.Exchange(ref interpreterConnecting, 0); }
            });
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
