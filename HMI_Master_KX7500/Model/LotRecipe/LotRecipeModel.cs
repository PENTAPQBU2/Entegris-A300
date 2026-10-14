namespace HMI_Master_KX7500.Model.LotRecipe
{
    //One recipe file: recipe identity + which robot point set it uses.
    //RobotPointSet goes to the PLC (HMI_Robot_PointSet), the PLC moves it to W0102 (robot shell_int) and the
    //robot program "Switch shell_int" picks that set's waypoints, taught on the robot pendant.
    //0 = Black set, 1 = Transparent set (as before V1.4.0.0), 2.. = new sets.
    public class LotRecipeModel
    {
        public int RecipeID { get; set; }
        public string RecipeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RobotPointSet { get; set; }
    }
}
