using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.LotRecipe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace HMI_Master_KX7500.Service.LotRecipe
{
    //Recipe files in <HMI folder>\Recipe\, one file per recipe: "<RecipeID>_<RecipeName>.xml"
    public static class LotRecipeService
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(LotRecipeModel));

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
                    LogHelper.KeyValue(nameof(LotRecipeService), "InitialRecipes", "not found: " + source);
                    return;
                }

                foreach(string file in Directory.GetFiles(source, "*.xml"))
                {
                    File.Copy(file, Path.Combine(RecipeFolder, Path.GetFileName(file)), false);
                }
            }
            catch(Exception ex) { LogHelper.Error(nameof(LotRecipeService), ex); }
        }

        public static List<LotRecipeModel> LoadAll()
        {
            List<LotRecipeModel> recipes = new List<LotRecipeModel>();
            if(!Directory.Exists(RecipeFolder))
            {
                return recipes;
            }

            foreach(string file in Directory.GetFiles(RecipeFolder, "*.xml"))
            {
                LotRecipeModel recipe = Load(file);
                if(recipe != null)
                {
                    recipes.Add(recipe);
                }
            }
            return recipes.OrderBy(x => x.RecipeID).ToList();
        }

        public static LotRecipeModel Load(string file)
        {
            try
            {
                using(FileStream stream = File.OpenRead(file))
                {
                    return (LotRecipeModel)serializer.Deserialize(stream);
                }
            }
            catch(Exception ex)
            {
                LogHelper.Error(nameof(LotRecipeService), $"Cannot read recipe {file}: {ex.Message}");
                return null;
            }
        }

        public static string FileName(LotRecipeModel recipe)
        {
            string name = string.Concat(recipe.RecipeName.Split(Path.GetInvalidFileNameChars())).Replace(' ', '_');
            return Path.Combine(RecipeFolder, $"{recipe.RecipeID}_{name}.xml");
        }

        //Writes to a temporary file first so a power cut cannot leave a half-written recipe
        public static void Save(LotRecipeModel recipe)
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
            List<LotRecipeModel> all = LoadAll();
            return all.Count == 0 ? 1 : all.Max(x => x.RecipeID) + 1;
        }

        //Returns null when the name/ID can be used, otherwise the reason
        public static string Validate(string name, int recipeId, LotRecipeModel ignore = null)
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

            foreach(LotRecipeModel other in LoadAll())
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

        //New recipe copied from an existing one, with the chosen shell colour (robot point set)
        public static LotRecipeModel CreateCopy(LotRecipeModel source, string name, int recipeId, ShellColour shellColour)
        {
            LotRecipeModel copy = new LotRecipeModel
            {
                RecipeID = recipeId,
                RecipeName = name.Trim(),
                Description = $"Copied from {source.RecipeName} on {DateTime.Now:yyyy-MM-dd HH:mm}",
                ShellColour = shellColour
            };
            Save(copy);
            return copy;
        }

        public static void Rename(LotRecipeModel recipe, string newName)
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
        public static void Delete(LotRecipeModel recipe)
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
