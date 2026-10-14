using System;

namespace HMI_Master_KX7500.CommunicationProtocols.UR
{
    //UR interpreter mode socket (port 30020). Only open while the robot program runs interpreter_mode()
    //(the HMI teach branch, see Docs/UR_Robot/UR_Program_Change_Spec.md).
    public class UrInterpreterClient : IDisposable
    {
        public const int Port = 30020;
        private readonly UrLineClient line = new UrLineClient("Interpreter", Port);

        public bool IsConnected => line.IsConnected;

        public void Connect(string ipAddress) => line.Connect(ipAddress);

        public void Disconnect() => line.Disconnect();

        //Sends one URScript statement. Returns true when the robot answers "ack", false on "discard" or no answer.
        public bool Execute(string statement, out string reply)
        {
            if(statement.Contains("\n"))
            {
                throw new ArgumentException("One statement per line", nameof(statement));
            }

            reply = line.Send(statement);
            return reply != null && reply.StartsWith("ack", StringComparison.OrdinalIgnoreCase);
        }

        public bool Execute(string statement)
        {
            string reply;
            return Execute(statement, out reply);
        }

        //Stops the movej/movel being executed (controlled stop), then drops statements still queued
        public void AbortMotion()
        {
            line.Send("abort");
            line.Send("skipbuffer");
        }

        public bool EndInterpreter() => Execute("end_interpreter()");

        public void Dispose() => line.Dispose();
    }
}
