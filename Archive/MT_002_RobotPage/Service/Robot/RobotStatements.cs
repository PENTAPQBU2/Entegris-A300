using HMI_Master_KX7500.CommunicationProtocols.UR;
using HMI_Master_KX7500.Model.Robot;
using System.Collections.Generic;
using System.Globalization;

namespace HMI_Master_KX7500.Service.Robot
{
    //URScript statements the HMI sends in HMI teach (Docs/UR_Robot/UR_Program_Change_Spec.md section 4)
    public static class RobotStatements
    {
        private static string num(double v) => v.ToString("R", CultureInfo.InvariantCulture);

        public static string Assign(string variable, double[] pose) => $"{variable} := {UrPoseMath.ToUrScript(pose)}";

        public static string AssignRecipeId(int recipeId) => $"Rcp_ID := {recipeId}";

        public static string EndInterpreter() => "end_interpreter()";

        //One jog step from the current pose. axis 0..5 = X, Y, Z, Rx, Ry, Rz; step in mm (X/Y/Z) or degrees (Rx/Ry/Rz).
        public static string Jog(double[] currentPose, int axis, double step, bool toolFrame, RobotSettingsModel s)
        {
            double unitStep = axis < 3 ? step * UrPoseMath.MmToM : step * UrPoseMath.DegToRad;
            double[] target = UrPoseMath.JogTarget(currentPose, axis, unitStep, toolFrame);
            return $"movel({UrPoseMath.ToUrScript(target)}, a={num(s.LinearAcceleration)}, v={num(s.ApproachSpeed)})";
        }

        //Move to a recipe screw point: lift, entry point, then down to the screw point
        public static List<string> MoveToScrew(string screwVariable, string entryVariable, RobotSettingsModel s)
        {
            return new List<string>
            {
                lift(s),
                $"movel({entryVariable}, a={num(s.LinearAcceleration)}, v={num(s.LinearSpeed)})",
                $"movel({screwVariable}, a={num(s.LinearAcceleration)}, v={num(s.ApproachSpeed)})"
            };
        }

        //Move to a recipe entry point: lift, then the entry point
        public static List<string> MoveToEntry(string entryVariable, RobotSettingsModel s)
        {
            return new List<string>
            {
                lift(s),
                $"movel({entryVariable}, a={num(s.LinearAcceleration)}, v={num(s.LinearSpeed)})"
            };
        }

        //Move to a shared point (Home, Safe, calibration): joint move
        public static List<string> MoveToShared(string variable, RobotSettingsModel s)
        {
            return new List<string>
            {
                $"movej(get_inverse_kin({variable}), a={num(s.JointAcceleration)}, v={num(s.JointSpeed)})"
            };
        }

        private static string lift(RobotSettingsModel s)
        {
            return $"movel(pose_add(get_actual_tcp_pose(), p[0, 0, {num(s.LiftHeight)}, 0, 0, 0]), a={num(s.LinearAcceleration)}, v={num(s.ApproachSpeed)})";
        }

        public static List<string> LoadRecipe(RobotRecipeModel recipe)
        {
            List<string> statements = new List<string>();
            foreach(RobotPointModel point in recipe.Points)
            {
                statements.Add(Assign(point.Variable, point.ToPose()));
            }
            statements.Add(AssignRecipeId(recipe.RecipeID));
            return statements;
        }
    }
}
