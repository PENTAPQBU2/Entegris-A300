using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.Robot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Service.Robot
{
    //Robot recipe files in <HMI folder>\Recipe\, one file per shell type: "<RecipeID>_<RecipeName>.xml"
    public static class RobotRecipeService
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(RobotRecipeModel));

        public static string RecipeFolder => FolderPath.Recipe;

        //First start: copy the starting recipes shipped next to the exe (InitialRecipes\*.xml)
        public static void EnsureDefaultRecipes()
        {
            try
            {
                Directory.CreateDirectory(RecipeFolder);
                if(Directory.GetFiles(RecipeFolder, "*.xml").Length > 0)
                {
                    return;
                }

                string source = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "InitialRecipes");
                if(!Directory.Exists(source))
                {
                    LogHelper.KeyValue(nameof(RobotRecipeService), "InitialRecipes", "not found: " + source);
                    return;
                }

                foreach(string file in Directory.GetFiles(source, "*.xml"))
                {
                    File.Copy(file, Path.Combine(RecipeFolder, Path.GetFileName(file)), false);
                }
            }
            catch(Exception ex) { LogHelper.Error(nameof(RobotRecipeService), ex); }
        }

        public static List<RobotRecipeModel> LoadAll()
        {
            List<RobotRecipeModel> recipes = new List<RobotRecipeModel>();
            if(!Directory.Exists(RecipeFolder))
            {
                return recipes;
            }

            foreach(string file in Directory.GetFiles(RecipeFolder, "*.xml"))
            {
                RobotRecipeModel recipe = Load(file);
                if(recipe != null)
                {
                    recipes.Add(recipe);
                }
            }
            return recipes.OrderBy(x => x.RecipeID).ToList();
        }

        public static RobotRecipeModel Load(string file)
        {
            try
            {
                using(FileStream stream = File.OpenRead(file))
                {
                    return (RobotRecipeModel)serializer.Deserialize(stream);
                }
            }
            catch(Exception ex)
            {
                LogHelper.Error(nameof(RobotRecipeService), $"Cannot read recipe {file}: {ex.Message}");
                return null;
            }
        }

        public static string FileName(RobotRecipeModel recipe)
        {
            string name = string.Concat(recipe.RecipeName.Split(Path.GetInvalidFileNameChars())).Replace(' ', '_');
            return Path.Combine(RecipeFolder, $"{recipe.RecipeID}_{name}.xml");
        }

        //Writes to a temporary file first so a power cut cannot leave a half-written recipe
        public static void Save(RobotRecipeModel recipe)
        {
            Directory.CreateDirectory(RecipeFolder);
            string file = FileName(recipe);
            string temp = file + ".tmp";
            using(FileStream stream = File.Create(temp))
            {
                serializer.Serialize(stream, recipe);
            }

            if(File.Exists(file))
            {
                File.Replace(temp, file, file + ".bak");
            }
            else
            {
                File.Move(temp, file);
            }
        }
    }
}
