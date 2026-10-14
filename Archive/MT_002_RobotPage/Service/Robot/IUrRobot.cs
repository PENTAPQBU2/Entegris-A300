using HMI_Master_KX7500.CommunicationProtocols.UR;
using System;

namespace HMI_Master_KX7500.Service.Robot
{
    //The Robot page talks to this: the real UR robot (UrRobot) or, in Offline mode, a simulator (UrRobotSimulator)
    public interface IUrRobot : IDisposable
    {
        bool IsConnected { get; }
        bool IsRemoteControl { get; }

        //Interpreter socket open: the robot is in the HMI teach branch and accepts motion/teach statements
        bool InterpreterReady { get; }

        UrRobotState State { get; }
        event Action<UrRobotState> StateChanged;

        void Connect(string ipAddress);
        void Disconnect();

        //Dashboard server command, e.g. "unlock protective stop". Returns the robot's reply.
        string Dashboard(string command);

        //One URScript statement in HMI teach. True when the robot acknowledged it.
        bool Execute(string statement, out string reply);

        void AbortMotion();
        void SetSpeedSlider(double fraction);

        //HMI teach request changed. The real robot gets it through the PLC; the simulator reacts to it directly.
        void NotifyTeachRequest(bool on);
    }
}
