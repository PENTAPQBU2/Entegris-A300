namespace HMI_Master_KX7500.Model.Robot
{
    //Robot page settings, saved in <HMI folder>\MachineSettings\RobotSettings.xml
    public class RobotSettingsModel
    {
        public string RobotIPAddress { get; set; } = string.Empty;

        //New Lot loads the recipe's robot points to the robot first. Set false only during commissioning,
        //before the robot program has the HMI teach branch (Docs/UR_Robot/UR_Program_Change_Spec.md).
        public bool LoadRecipeAtNewLot { get; set; } = true;

        //Teach motion limits (UR units: m/s, m/s2, rad/s, rad/s2). The robot speed slider scales them further.
        public double LinearSpeed { get; set; } = 0.05;
        public double ApproachSpeed { get; set; } = 0.02;
        public double LinearAcceleration { get; set; } = 0.3;
        public double JointSpeed { get; set; } = 0.3;
        public double JointAcceleration { get; set; } = 0.5;

        //Lift before moving to a screw or entry point, metres
        public double LiftHeight { get; set; } = 0.025;

        //Jog step limits from the customer request (mm, or degrees for Rx/Ry/Rz)
        public double JogStepMin { get; set; } = 0.01;
        public double JogStepMax { get; set; } = 10.00;
        public double JogStepDefault { get; set; } = 1.00;
    }
}
