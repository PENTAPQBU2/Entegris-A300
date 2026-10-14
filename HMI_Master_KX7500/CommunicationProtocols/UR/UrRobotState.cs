using System;

namespace HMI_Master_KX7500.CommunicationProtocols.UR
{
    //Snapshot of the robot read over RTDE (copied, so it can be used on any thread)
    public class UrRobotState
    {
        public bool Connected { get; set; }
        public DateTime Received { get; set; }

        public double[] ActualQ { get; set; } = new double[6];          //joint angles, rad
        public double[] TcpPose { get; set; } = new double[6];          //x, y, z m / rx, ry, rz rad (base frame)
        public int RobotMode { get; set; } = -1;
        public int SafetyStatus { get; set; }
        public uint RuntimeState { get; set; }
        public double SpeedScaling { get; set; }
        public double SpeedFraction { get; set; }                     //speed slider 0..1
        public uint OutputBits { get; set; }                          //general purpose bool output registers 0..31
        public int CurrentPosOut { get; set; }                        //int output 0 (current_Pos)
        public int RecipeIdOut { get; set; }                          //int output 1 (Rcp_ID_Out)

        //Robot program bool outputs (names from the robot installation)
        public bool AtHome => (OutputBits & (1u << 0)) != 0;
        public bool AtSafePos => (OutputBits & (1u << 1)) != 0;
        public bool InScrewPos => (OutputBits & (1u << 2)) != 0;
        public bool InEntryPos => (OutputBits & (1u << 3)) != 0;
        public bool AtScrewExitPt => (OutputBits & (1u << 5)) != 0;
        public bool AtCalReadyPos => (OutputBits & (1u << 6)) != 0;
        public bool AtCalStartPos => (OutputBits & (1u << 7)) != 0;
        public bool AtCalDonePos => (OutputBits & (1u << 8)) != 0;
        public bool InTeachMode => (OutputBits & (1u << 9)) != 0;

        public bool IsProtectiveStop => SafetyStatus == 3;

        public UrRobotState Copy()
        {
            UrRobotState copy = (UrRobotState)MemberwiseClone();
            copy.ActualQ = (double[])ActualQ.Clone();
            copy.TcpPose = (double[])TcpPose.Clone();
            return copy;
        }

        public string CurrentPointText
        {
            get
            {
                if(InTeachMode) { return "HMI teach"; }
                if(AtHome) { return "Home"; }
                if(AtSafePos) { return "Safe"; }
                if(InScrewPos) { return $"Screw {CurrentPosOut}"; }
                if(InEntryPos) { return "Screw entry"; }
                if(AtScrewExitPt) { return "Screw exit"; }
                if(AtCalReadyPos) { return "Calibration ready"; }
                if(AtCalStartPos) { return "Calibration start"; }
                if(AtCalDonePos) { return "Calibration down"; }
                return "-";
            }
        }

        public string RobotModeText
        {
            get
            {
                switch(RobotMode)
                {
                    case -1: return "No controller";
                    case 0: return "Disconnected";
                    case 1: return "Confirm safety";
                    case 2: return "Booting";
                    case 3: return "Power off";
                    case 4: return "Power on";
                    case 5: return "Idle";
                    case 6: return "Backdrive";
                    case 7: return "Running";
                    case 8: return "Updating firmware";
                    default: return $"Mode {RobotMode}";
                }
            }
        }

        public string SafetyStatusText
        {
            get
            {
                switch(SafetyStatus)
                {
                    case 1: return "Normal";
                    case 2: return "Reduced";
                    case 3: return "Protective stop";
                    case 4: return "Recovery";
                    case 5: return "Safeguard stop";
                    case 6: return "System emergency stop";
                    case 7: return "Robot emergency stop";
                    case 8: return "Violation";
                    case 9: return "Fault";
                    case 12: return "Automatic mode safeguard stop";
                    case 13: return "Three-position enabling stop";
                    default: return $"Status {SafetyStatus}";
                }
            }
        }

        public string RuntimeStateText
        {
            get
            {
                switch(RuntimeState)
                {
                    case 0: return "Stopping";
                    case 1: return "Stopped";
                    case 2: return "Playing";
                    case 3: return "Pausing";
                    case 4: return "Paused";
                    case 5: return "Resuming";
                    default: return $"State {RuntimeState}";
                }
            }
        }
    }
}
