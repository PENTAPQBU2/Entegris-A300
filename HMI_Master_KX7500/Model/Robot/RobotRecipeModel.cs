using System.Collections.Generic;
using System.Linq;

namespace HMI_Master_KX7500.Model.Robot
{
    //One recipe file: recipe identity + the robot points for that shell type.
    //Point values are in UR units (metres, radians) so they go to the robot unchanged.
    public class RobotRecipeModel
    {
        public int RecipeID { get; set; }
        public string RecipeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<RobotPointModel> Points { get; set; } = new List<RobotPointModel>();

        public RobotPointModel Find(string variable)
        {
            return Points.FirstOrDefault(x => x.Variable == variable);
        }
    }

    public class RobotPointModel
    {
        public string Variable { get; set; } = string.Empty;   //robot installation variable, e.g. Scr05
        public string Label { get; set; } = string.Empty;      //shown on the HMI, e.g. Screw 5
        public string Source { get; set; } = string.Empty;     //where the first value came from
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Rx { get; set; }
        public double Ry { get; set; }
        public double Rz { get; set; }

        public double[] ToPose()
        {
            return new[] { X, Y, Z, Rx, Ry, Rz };
        }

        public void SetPose(double[] pose)
        {
            X = pose[0];
            Y = pose[1];
            Z = pose[2];
            Rx = pose[3];
            Ry = pose[4];
            Rz = pose[5];
        }
    }
}
