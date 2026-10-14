using HMI_Master_KX7500.Model.Robot;
using System;

namespace HMI_Master_KX7500.ModulesView.Robot
{
    public enum RobotPointKind
    {
        Screw,      //recipe screw point (Scr01..Scr21)
        Entry,      //recipe entry point (Scr01_E..Scr21_E)
        Shared      //same for all recipes, value kept on the robot only (P_Home ...)
    }

    //One row of the Robot page point list / move-to drop-down
    public class RobotPointItem
    {
        public string Variable { get; set; }
        public string Label { get; set; }
        public RobotPointKind Kind { get; set; }
        public RobotPointModel RecipePoint { get; set; }   //null for shared points

        public string EntryVariable => Kind == RobotPointKind.Screw ? Variable + "_E" : Variable;

        public string Display => $"{Label}  ({Variable})";

        public string ValueText
        {
            get
            {
                if(RecipePoint == null)
                {
                    return "kept on robot";
                }
                return string.Format("X {0:F2}  Y {1:F2}  Z {2:F2} mm   Rx {3:F2}  Ry {4:F2}  Rz {5:F2} deg",
                    RecipePoint.X * 1000, RecipePoint.Y * 1000, RecipePoint.Z * 1000,
                    RecipePoint.Rx * 180 / Math.PI, RecipePoint.Ry * 180 / Math.PI, RecipePoint.Rz * 180 / Math.PI);
            }
        }

        public override string ToString() => Display;
    }
}
