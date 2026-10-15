using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.ShellRecipe;
using NLog;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HMI_Master_KX7500.Service.ShellRecipe
{
    /// <summary>
    /// Recipe list storage (MT_001): FilePath.ShellRecipe (MachineConfiguration\ShellRecipe.xml).
    /// </summary>
    public class ShellRecipeService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly object fileLock = new object();

        // Recipe ID goes 1:1 to the robot (PLC: Shell_Colour_ID EM4250 -> W0102 on New Lot), robot today: 0 = Black, 1 = Transparent ... 5 = Blue
        public const int MinRecipeID = 0;
        public const int MaxRecipeID = 999;

        #region PublicMethods
        public static List<ShellRecipeModel> GetRecipeList()
        {
            List<ShellRecipeModel> recipeList = new List<ShellRecipeModel>();

            try
            {
                lock (fileLock)
                {
                    if (File.Exists(FilePath.ShellRecipe))
                    {
                        recipeList = XmlTools.Deserialize<List<ShellRecipeModel>>(FilePath.ShellRecipe, recipeList);
                    }
                }
            }
            catch (Exception ex) { logger.Error(ex); }

            return (recipeList ?? new List<ShellRecipeModel>()).OrderBy(x => x.RecipeID).ToList();
        }

        public static bool SaveRecipeList(List<ShellRecipeModel> recipeList)
        {
            try
            {
                lock (fileLock)
                {
                    XmlTools.Serialize(FilePath.ShellRecipe, recipeList.OrderBy(x => x.RecipeID).ToList());
                }
                return true;
            }
            catch (Exception ex) { logger.Error(ex); }

            return false;
        }

        // Recipe name for FOUP CSV / display, "Recipe <ID>" when the ID has no recipe
        public static string GetRecipeName(int recipeID)
        {
            ShellRecipeModel recipe = GetRecipeList().FirstOrDefault(x => x.RecipeID == recipeID);
            return recipe != null ? recipe.RecipeName : $"Recipe {recipeID}";
        }
        #endregion
    }
}
