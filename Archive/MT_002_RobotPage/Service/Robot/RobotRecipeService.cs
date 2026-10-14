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
            RecipesChanged?.Invoke();
        }

        //Recipe files were added, changed, renamed or deleted (any page)
        public static event Action RecipesChanged;

        public static int NextRecipeId()
        {
            List<RobotRecipeModel> all = LoadAll();
            return all.Count == 0 ? 1 : all.Max(x => x.RecipeID) + 1;
        }

        //Returns null when the name/ID can be used, otherwise the reason
        public static string Validate(string name, int recipeId, RobotRecipeModel ignore = null)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                return "Enter a recipe name.";
            }
            if(name.Trim().Length > 20)
            {
                return "Recipe name can have at most 20 characters (PLC string length).";
            }
            if(recipeId <= 0)
            {
                return "Recipe ID must be 1 or more.";
            }

            foreach(RobotRecipeModel other in LoadAll())
            {
                if(ignore != null && other.RecipeID == ignore.RecipeID)
                {
                    continue;
                }
                if(other.RecipeID == recipeId)
                {
                    return $"Recipe ID {recipeId} is already used by {other.RecipeName}.";
                }
                if(string.Equals(other.RecipeName.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return $"Recipe name {name.Trim()} already exists.";
                }
            }
            return null;
        }

        //New recipe with the robot points copied from an existing one
        public static RobotRecipeModel CreateCopy(RobotRecipeModel source, string name, int recipeId)
        {
            RobotRecipeModel copy = new RobotRecipeModel
            {
                RecipeID = recipeId,
                RecipeName = name.Trim(),
                Description = $"Copied from {source.RecipeName} on {DateTime.Now:yyyy-MM-dd HH:mm}",
                Points = source.Points.Select(p => new RobotPointModel
                {
                    Variable = p.Variable, Label = p.Label, Source = $"Copy of {source.RecipeName}",
                    X = p.X, Y = p.Y, Z = p.Z, Rx = p.Rx, Ry = p.Ry, Rz = p.Rz
                }).ToList()
            };
            Save(copy);
            return copy;
        }

        public static void Rename(RobotRecipeModel recipe, string newName)
        {
            string oldFile = FileName(recipe);
            recipe.RecipeName = newName.Trim();
            Save(recipe);
            if(!string.Equals(oldFile, FileName(recipe), StringComparison.OrdinalIgnoreCase) && File.Exists(oldFile))
            {
                File.Delete(oldFile);
            }
            RecipesChanged?.Invoke();
        }

        //Moves the file to Recipe\Deleted\ instead of erasing it
        public static void Delete(RobotRecipeModel recipe)
        {
            string file = FileName(recipe);
            if(File.Exists(file))
            {
                string deletedFolder = Path.Combine(RecipeFolder, "Deleted");
                Directory.CreateDirectory(deletedFolder);
                File.Move(file, Path.Combine(deletedFolder, $"{DateTime.Now:yyyyMMdd_HHmmss}_{Path.GetFileName(file)}"));
            }
            RecipesChanged?.Invoke();
        }
    }
}
