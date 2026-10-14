namespace HMI_Master_KX7500.Model.LotRecipe
{
    //PLC shell colour bits (MR4500-MR4504). The PLC turns the bit into shell_int for the robot,
    //and the robot program picks that recipe's taught point set (Switch shell_int).
    public enum ShellColour
    {
        Black,
        Transparent,
        Orange,
        Clear,
        Red
    }

    //One recipe file: recipe identity + which shell colour bit (= robot point set) it uses.
    //The robot points themselves are taught on the robot pendant.
    public class LotRecipeModel
    {
        public int RecipeID { get; set; }
        public string RecipeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ShellColour ShellColour { get; set; } = ShellColour.Black;
    }
}
