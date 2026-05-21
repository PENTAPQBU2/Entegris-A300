using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Initialize;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.Recipe;
using HMI_Master_KX7500.Model.Recipe.Parameter;
using HMI_Master_KX7500.Model.Recipe.Parameter.Boolean;
using HMI_Master_KX7500.Model.Recipe.Parameter.Numeric;
using HMI_Master_KX7500.OPCUA;
using HMI_Master_KX7500.Tags;
using NLog;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HMI_Master_KX7500.Service.Recipe
{
    public class RecipeService
    {
        #region PrivateFields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static string IsInDesignMode = "IsInDesignMode";
        private static RecipeInitializeModel recipeInitializeModel = new RecipeInitializeModel();
        #endregion

        #region Constructor
        public RecipeService()
        {
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeMessenger()
        {
            //initializeRecipe();
            Messenger.Default.Register<RecipeInitializeModel>(this, initializeRecipe);
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializeRecipe(RecipeInitializeModel _recipeInitializeModel)
        {
            recipeInitializeModel = _recipeInitializeModel;

            if (!File.Exists(FilePath.RecipeParameter))
            {
                GenerateRecipeParameters();
            }
        }
        #endregion

        #region PrivateMethods
        private static string getRecipeFilePath(string recipeName)
        {
            string recipeFilePath = string.Empty;

            recipeFilePath = $"{FolderPath.RecipeFolderPath}{recipeName}.xml";
            
            return recipeFilePath;
        }

        private void GenerateRecipeParameters(/*string sourceFilePath*/)
        {
            RecipeParameterModel recipeParameterModel = new RecipeParameterModel();

            string Recipe_Data = "DB_HMI_Recipe_Data";
            string Unit_Recipe = "Unit_Recipe";
            string Vision_Parameter = "Vision_Parameter";
            string Insertion_Stage_Pos = "Insertion_Stage_Pos";
            string Turret_Motor_Pos = "Turret_Motor_Pos";
            string Loader_Carrier_Pos = "Loader_Carrier_Pos";
            string Stage_Pos = string.Empty;
            string YorX = string.Empty;

            if (!File.Exists(FilePath.RecipeParameter))
            {
                List<RecipeParameterModel> recipeParameterLists = new List<RecipeParameterModel>();
                List<RecipeParameterModel> tempGeneralList = new List<RecipeParameterModel>();
                List<RecipeParameterModel> tempNumofPinHole0PerQuadrantList = new List<RecipeParameterModel>();
                List<RecipeParameterModel> tempNumofPinHole90PerQuadrantList = new List<RecipeParameterModel>();
                List<RecipeParameterModel> tempNumofPinHole180PerQuadrantList = new List<RecipeParameterModel>();
                List<RecipeParameterModel> tempNumofPinHole270PerQuadrantList = new List<RecipeParameterModel>();

                List<RecipeParameterModel> tempVisionParameterRecipeList = new List<RecipeParameterModel>();

                List<RecipeParameterModel> tempInsertionStagePosRecipeList = new List<RecipeParameterModel>();
                List<RecipeParameterModel> tempTurretMotorPosRecipeList = new List<RecipeParameterModel>();
                List<RecipeParameterModel> tempLoader_Carrier_PosRecipeList = new List<RecipeParameterModel>();


                tempGeneralList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = "Recipe_ID",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, "Recipe_ID"),
                    }
                });

                tempGeneralList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = "Recipe_Name",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, "Recipe_Name"),
                    }
                });

                //tempGeneralList.Add(new RecipeParameterModel()
                //{
                //    RecipeParameterKey = "Total_Num_Of_DBC",
                //    RecipeParameterTagModel = new RecipeParameterTagModel()
                //    {
                //        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "Total_Num_Of_DBC"),
                //    }
                //});

                tempGeneralList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = "X_Quadrant_Number",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "X_Quadrant_Number"),
                    }
                });

                tempGeneralList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = "Y_Quadrant_Number",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "Y_Quadrant_Number"),
                    }
                });

                tempGeneralList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = "X_Quadrant_Pitch",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "X_Quadrant_Pitch"),
                    }
                });

                tempGeneralList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = "Y_Quadrant_Pitch",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "Y_Quadrant_Pitch"),
                    }
                });

                tempGeneralList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = "Jig_To_Jig_X_Pitch",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "Jig_To_Jig_X_Pitch"),
                    }
                });

                tempGeneralList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = "Jig_To_Jig_Y_Pitch",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "Jig_To_Jig_Y_Pitch"),
                    }
                });

                tempGeneralList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = "Total_Num_Of_Jig",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "Total_Num_Of_Jig"),
                    }
                });

                tempGeneralList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = "Loose_Form_Mode",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "Loose_Form_Mode"),
                    }
                });
                tempGeneralList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = "Reel_Form_Mode",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "Reel_Form_Mode"),
                    }
                });

                for (int i = 0; i <= 5; i++)
                {
                    tempNumofPinHole0PerQuadrantList.Add(new RecipeParameterModel()
                    {
                        RecipeParameterKey = $"Num_of_Pin_Hole_0_Deg_Per_Quadrant[{i}]",
                        RecipeParameterTagModel = new RecipeParameterTagModel()
                        {
                            RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, $"Num_Of_Pin_Hole_0_Deg_Per_Quadrant[{i}]"),
                        }
                    });
                }  
                for (int i = 0; i <= 5; i++)
                {
                    tempNumofPinHole90PerQuadrantList.Add(new RecipeParameterModel()
                    {
                        RecipeParameterKey = $"Num_of_Pin_Hole_90_Deg_Per_Quadrant[{i}]",
                        RecipeParameterTagModel = new RecipeParameterTagModel()
                        {
                            RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, $"Num_Of_Pin_Hole_90_Deg_Per_Quadrant[{i}]"),
                        }
                    });
                }
                for (int i = 0; i <= 5; i++)
                {
                    tempNumofPinHole180PerQuadrantList.Add(new RecipeParameterModel()
                    {
                        RecipeParameterKey = $"Num_of_Pin_Hole_180_Deg_Per_Quadrant[{i}]",
                        RecipeParameterTagModel = new RecipeParameterTagModel()
                        {
                            RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, $"Num_Of_Pin_Hole_180_Deg_Per_Quadrant[{i}]"),
                        }
                    });
                }
                for (int i = 0; i <= 5; i++)
                {
                    tempNumofPinHole270PerQuadrantList.Add(new RecipeParameterModel()
                    {
                        RecipeParameterKey = $"Num_of_Pin_Hole_270_Deg_Per_Quadrant[{i}]",
                        RecipeParameterTagModel = new RecipeParameterTagModel()
                        {
                            RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, $"Num_Of_Pin_Hole_270_Deg_Per_Quadrant[{i}]"),
                        }
                    });
                }

                tempVisionParameterRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Pin_Bottom_X_Lower_Limit",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Vision_Parameter, $"Pin_Bottom_X_Lower_Limit"),
                    }
                });
                
                tempVisionParameterRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Pin_Bottom_X_Upper_Limit",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Vision_Parameter, $"Pin_Bottom_X_Upper_Limit"),
                    }
                });
                
                tempVisionParameterRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Pin_Bottom_Y_Lower_Limit",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Vision_Parameter, $"Pin_Bottom_Y_Lower_Limit"),
                    }
                });
                
                tempVisionParameterRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Pin_Bottom_Y_Upper_Limit",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Vision_Parameter, $"Pin_Bottom_Y_Upper_Limit"),
                    }
                });
                
                tempVisionParameterRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Final_Offset_X_Lower_Limit",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Vision_Parameter, $"Final_Offset_X_Lower_Limit"),
                    }
                });
                
                tempVisionParameterRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Final_Offset_X_Upper_Limit",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Vision_Parameter, $"Final_Offset_X_Upper_Limit"),
                    }
                });
                
                tempVisionParameterRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Final_Offset_Y_Lower_Limit",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Vision_Parameter, $"Final_Offset_Y_Lower_Limit"),
                    }
                });
                
                tempVisionParameterRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Final_Offset_Y_Upper_Limit",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Vision_Parameter, $"Final_Offset_Y_Upper_Limit"),
                    }
                });


                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"X_Loading_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"X_Loading_Pos"),
                    }
                });
                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Y_Loading_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"Y_Loading_Pos"),
                    }
                });
                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"T_Loading_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"T_Loading_Pos"),
                    }
                });
                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"X_Vision_Cal_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"X_Vision_Cal_Pos"),
                    }
                });
                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Y_Vision_Cal_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"Y_Vision_Cal_Pos"),
                    }
                });
                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"T_Vision_Cal_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"T_Vision_Cal_Pos"),
                    }
                });

                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"X_1st_Quadrant_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"X_1st_Quadrant_Pos"),
                    }
                });
                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Y_1st_Quadrant_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"Y_1st_Quadrant_Pos"),
                    }
                });
                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"T_1st_Quadrant_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"T_1st_Quadrant_Pos"),
                    }
                });
                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"X_Turret_Master_Ref_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"X_Turret_Master_Ref_Pos"),
                    }
                });
                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Y_Turret_Master_Ref_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"Y_Turret_Master_Ref_Pos"),
                    }
                });
                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"T_Turret_Master_Ref_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"T_Turret_Master_Ref_Pos"),
                    }
                });
                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Z_Alignment_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"Z_Alignment_Pos"),
                    }
                });
                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Z_Final_Inspection_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"Z_Final_Inspection_Pos"),
                    }
                });
                tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Z_Calibration_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"Z_Calibration_Pos"),
                    }
                });
                for (int i = 1; i <= 200; i++)
                {
                    tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                    {
                        RecipeParameterKey = $"X_Vision_Offset_Correction_Pos[{i}]",
                        RecipeParameterTagModel = new RecipeParameterTagModel()
                        {
                            RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"X_Vision_Offset_Correction_Pos[{i}]"),
                        }
                    });
                    tempInsertionStagePosRecipeList.Add(new RecipeParameterModel()
                    {
                        RecipeParameterKey = $"Y_Vision_Offset_Correction_Pos[{i}]",
                        RecipeParameterTagModel = new RecipeParameterTagModel()
                        {
                            RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Insertion_Stage_Pos, $"Y_Vision_Offset_Correction_Pos[{i}]"),
                        }
                    });
                }
                tempTurretMotorPosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Z_Pick_Loose_Pin_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Turret_Motor_Pos, $"Z_Pick_Loose_Pin_Pos"),
                    }
                });
                tempTurretMotorPosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Z_Pick_Reel_Pin_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Turret_Motor_Pos, $"Z_Pick_Reel_Pin_Pos"),
                    }
                });
                tempTurretMotorPosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Z_Pin_Bottom_Vision_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Turret_Motor_Pos, $"Z_Pin_Bottom_Vision_Pos"),
                    }
                });
                tempTurretMotorPosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Z_Press_Pin_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Turret_Motor_Pos, $"Z_Press_Pin_Pos"),
                    }
                });
                tempLoader_Carrier_PosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Barcode_Scan_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Loader_Carrier_Pos, $"Barcode_Scan_Pos"),
                    }
                });
                tempLoader_Carrier_PosRecipeList.Add(new RecipeParameterModel()
                {
                    RecipeParameterKey = $"Orientation_Check_Pos",
                    RecipeParameterTagModel = new RecipeParameterTagModel()
                    {
                        RecipeParameterTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Loader_Carrier_Pos, $"Orientation_Check_Pos"),
                    }
                });

                recipeParameterLists.AddRange(tempGeneralList);;
                recipeParameterLists.AddRange(tempVisionParameterRecipeList );
                recipeParameterLists.AddRange(tempNumofPinHole270PerQuadrantList);
                recipeParameterLists.AddRange(tempNumofPinHole180PerQuadrantList);
                recipeParameterLists.AddRange(tempNumofPinHole90PerQuadrantList);
                recipeParameterLists.AddRange(tempNumofPinHole0PerQuadrantList );
                recipeParameterLists.AddRange(tempInsertionStagePosRecipeList );
                recipeParameterLists.AddRange(tempTurretMotorPosRecipeList);
                recipeParameterLists.AddRange(tempLoader_Carrier_PosRecipeList);

                SerializeRecipeParameterList(recipeParameterLists);

                #region NotUse
                //recipeEditorModel.RecipeParameterList.Add(new RecipeParameterModel()
                //{
                //    //RecipeParameterKey = "Total_Num_Of_DBC",
                //    RecipeParameterNumericList = new List<RecipeParameterNumericModel>()
                //    {
                //        new RecipeParameterNumericModel
                //        {
                //            RecipeParameterNumericKey = "Total_Num_Of_DBC",
                //            RecipeParameterNumericMaximum = 100,
                //            RecipeParameterNumericMinimum = 0,
                //            RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //            {
                //                RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "Total_Num_Of_DBC"),
                //            }
                //        },
                //        new RecipeParameterNumericModel
                //        {
                //            RecipeParameterNumericKey = "X_Quadrant_Number",
                //            RecipeParameterNumericMaximum = 100,
                //            RecipeParameterNumericMinimum = 0,
                //            RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //            {
                //                RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "X_Quadrant_Number"),
                //            }
                //        },
                //        new RecipeParameterNumericModel
                //        {
                //            RecipeParameterNumericKey = "Y_Quadrant_Number",
                //            RecipeParameterNumericMaximum = 100,
                //            RecipeParameterNumericMinimum = 0,
                //            RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //            {
                //                RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "Y_Quadrant_Number"),
                //            }
                //        },
                //        new RecipeParameterNumericModel
                //        {
                //            RecipeParameterNumericKey = "X_Quadrant_Pitch",
                //            RecipeParameterNumericMaximum = 100,
                //            RecipeParameterNumericMinimum = 0,
                //            RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //            {
                //                RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "X_Quadrant_Pitch"),
                //            }
                //        },
                //        new RecipeParameterNumericModel
                //        {
                //            RecipeParameterNumericKey = "Y_Quadrant_Pitch",
                //            RecipeParameterNumericMaximum = 100,
                //            RecipeParameterNumericMinimum = 0,
                //            RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //            {
                //                RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "Y_Quadrant_Pitch"),
                //            }
                //        },
                //        new RecipeParameterNumericModel
                //        {
                //            RecipeParameterNumericKey = "DBC_To_DBC_X_Pitch",
                //            RecipeParameterNumericMaximum = 100,
                //            RecipeParameterNumericMinimum = 0,
                //            RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //            {
                //                RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "DBC_To_DBC_X_Pitch"),
                //            }
                //        },
                //        new RecipeParameterNumericModel
                //        {
                //            RecipeParameterNumericKey = "DBC_To_DBC_Y_Pitch",
                //            RecipeParameterNumericMaximum = 100,
                //            RecipeParameterNumericMinimum = 0,
                //            RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //            {
                //                RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "DBC_To_DBC_Y_Pitch"),
                //            }
                //        },
                //        new RecipeParameterNumericModel
                //        {
                //            RecipeParameterNumericKey = "Cleaning_Time",
                //            RecipeParameterNumericMaximum = 100,
                //            RecipeParameterNumericMinimum = 0,
                //            RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //            {
                //                RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "Cleaning_Time"),
                //            }
                //        },
                //    },

                //    RecipeParameterBooleanList = new List<RecipeParameterBooleanModel>()
                //    {
                //        new RecipeParameterBooleanModel
                //        {
                //            RecipeParameterBooleanKey = $"Weld_By_Drawing_Pos_Mode",
                //            RecipeParameterBooleanOnLabelKey = "On",
                //            RecipeParameterBooleanOffLabelKey = "Off",
                //            RecipeParameterBooleanTagModel = new RecipeParameterBooleanTagModel()
                //            {
                //                RecipeParameterBooleanTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, "Weld_By_Drawing_Pos_Mode"),
                //            },
                //        },
                //    }
                //});

                //for (int i = 0; i <= 8; i++)
                //{
                //    tempNumofLegPerQuadrantList.Add(new RecipeParameterModel()
                //    {
                //        RecipeParameterNumericList = new List<RecipeParameterNumericModel>()
                //        {
                //            new RecipeParameterNumericModel
                //            {
                //                RecipeParameterNumericKey = $"Num_of_Leg_Per_Quadrant[{i}]",
                //                RecipeParameterNumericMaximum = 100,
                //                RecipeParameterNumericMinimum = 0,
                //                RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                {
                //                    RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, $"Num_of_Leg_Per_Quadrant[{i}]"),
                //                }
                //            },
                //        }
                //    });
                //}

                //for (int i = 1; i <= 40; i++)
                //{
                //    tempLeftLFLegWeldingRecipeList.Add(new RecipeParameterModel()
                //    {
                //        RecipeParameterNumericList = new List<RecipeParameterNumericModel>()
                //        {
                //            new RecipeParameterNumericModel
                //            {
                //                RecipeParameterNumericKey = $"Left_LF_Leg_Welding_Recipe[{i}]",
                //                RecipeParameterNumericMaximum = 100,
                //                RecipeParameterNumericMinimum = 0,
                //                RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                {
                //                    RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, $"Left_LF_Leg_Welding_Recipe[{i}]"),
                //                }
                //            },
                //        }
                //    });

                //    tempRightLFLegWeldingRecipeList.Add(new RecipeParameterModel()
                //    {
                //        RecipeParameterNumericList = new List<RecipeParameterNumericModel>()
                //        {
                //            new RecipeParameterNumericModel
                //            {
                //                RecipeParameterNumericKey = $"Right_LF_Leg_Welding_Recipe[{i}]",
                //                RecipeParameterNumericMaximum = 100,
                //                RecipeParameterNumericMinimum = 0,
                //                RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                {
                //                    RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, $"Right_LF_Leg_Welding_Recipe[{i}]"),
                //                }
                //            },
                //        }
                //    });

                //    tempLeftLFLegDisableList.Add(new RecipeParameterModel()
                //    {
                //        RecipeParameterBooleanList = new List<RecipeParameterBooleanModel>()
                //        {
                //            new RecipeParameterBooleanModel
                //            {
                //                RecipeParameterBooleanKey = $"Left_LF_Leg_Disable[{i}]",
                //                RecipeParameterBooleanOnLabelKey = "On",
                //                RecipeParameterBooleanOffLabelKey = "Off",
                //                RecipeParameterBooleanTagModel = new RecipeParameterBooleanTagModel()
                //                {
                //                    RecipeParameterBooleanTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, $"Left_LF_Leg_Disable[{i}]"),
                //                },
                //            },
                //        }
                //    });

                //    tempRightLFLegDisableList.Add(new RecipeParameterModel()
                //    {
                //        RecipeParameterBooleanList = new List<RecipeParameterBooleanModel>()
                //        {
                //            new RecipeParameterBooleanModel
                //            {
                //                RecipeParameterBooleanKey = $"Right_LF_Leg_Disable[{i}]",
                //                RecipeParameterBooleanOnLabelKey = "On",
                //                RecipeParameterBooleanOffLabelKey = "Off",
                //                RecipeParameterBooleanTagModel = new RecipeParameterBooleanTagModel()
                //                {
                //                    RecipeParameterBooleanTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Unit_Recipe, $"Right_LF_Leg_Disable[{i}]"),
                //                },
                //            },
                //        }
                //    });
                //}

                //for (int i = 1; i <= 2; i++)
                //{
                //    switch (i)
                //    {
                //        case 0:
                //            Stage_Pos = "Left_Stage_Pos";
                //            break;
                //        case 1:
                //            Stage_Pos = "Right_Stage_Pos";
                //            break;
                //        default:
                //            break;
                //    };

                //    for (int ii = 1; ii <= 2; ii++)
                //    {
                //        switch (ii)
                //        {
                //            case 1:
                //                YorX = "X";
                //                break;
                //            case 2:
                //                YorX = "Y";
                //                break;
                //            default:
                //                break;
                //        }

                //        tempStagePosList.Add(new RecipeParameterModel()
                //        {
                //            RecipeParameterNumericList = new List<RecipeParameterNumericModel>()
                //            {
                //                new RecipeParameterNumericModel
                //                {
                //                    RecipeParameterNumericKey = $"{YorX}_Load_DBC_Pos",
                //                    RecipeParameterNumericMaximum = 100,
                //                    RecipeParameterNumericMinimum = 0,
                //                    RecipeParameterDecimalPlace = 2,
                //                    RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                    {
                //                        RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Stage_Pos, $"{YorX}_Load_DBC_Pos"),
                //                    }
                //                },
                //                new RecipeParameterNumericModel
                //                {
                //                    RecipeParameterNumericKey = $"{YorX}_Load_LF_Pos",
                //                    RecipeParameterNumericMaximum = 100,
                //                    RecipeParameterNumericMinimum = 0,
                //                    RecipeParameterDecimalPlace = 2,
                //                    RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                    {
                //                        RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Stage_Pos, $"{YorX}_Load_LF_Pos"),
                //                    }
                //                },
                //                new RecipeParameterNumericModel
                //                {
                //                    RecipeParameterNumericKey = $"{YorX}_Scan_DBC_Barcode_Pos",
                //                    RecipeParameterNumericMaximum = 100,
                //                    RecipeParameterNumericMinimum = 0,
                //                    RecipeParameterDecimalPlace = 2,
                //                    RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                    {
                //                        RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Stage_Pos, $"{YorX}_Scan_DBC_Barcode_Pos"),
                //                    }
                //                },
                //                new RecipeParameterNumericModel
                //                {
                //                    RecipeParameterNumericKey = $"{YorX}_Scan_LF_Barcode_Pos",
                //                    RecipeParameterNumericMaximum = 100,
                //                    RecipeParameterNumericMinimum = 0,
                //                    RecipeParameterDecimalPlace = 2,
                //                    RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                    {
                //                        RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Stage_Pos, $"{YorX}_Scan_LF_Barcode_Pos"),
                //                    }
                //                },
                //                new RecipeParameterNumericModel
                //                {
                //                    RecipeParameterNumericKey = $"{YorX}_1st_Quadrant_Pos",
                //                    RecipeParameterNumericMaximum = 100,
                //                    RecipeParameterNumericMinimum = 0,
                //                    RecipeParameterDecimalPlace = 2,
                //                    RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                    {
                //                        RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Stage_Pos, $"{YorX}_1st_Quadrant_Pos"),
                //                    }
                //                },
                //                new RecipeParameterNumericModel
                //                {
                //                    RecipeParameterNumericKey = $"{YorX}_Welder_XY_Cal_Pos",
                //                    RecipeParameterNumericMaximum = 100,
                //                    RecipeParameterNumericMinimum = 0,
                //                    RecipeParameterDecimalPlace = 2,
                //                    RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                    {
                //                        RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Stage_Pos, $"{YorX}_Welder_XY_Cal_Pos"),
                //                    }
                //                },
                //                new RecipeParameterNumericModel
                //                {
                //                    RecipeParameterNumericKey = $"{YorX}_Welder_Z_Cal_Pos",
                //                    RecipeParameterNumericMaximum = 100,
                //                    RecipeParameterNumericMinimum = 0,
                //                    RecipeParameterDecimalPlace = 2,
                //                    RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                    {
                //                        RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Stage_Pos, $"{YorX}_Welder_Z_Cal_Pos"),
                //                    }
                //                },
                //                new RecipeParameterNumericModel
                //                {
                //                    RecipeParameterNumericKey = $"{YorX}_Cleaning_Pos",
                //                    RecipeParameterNumericMaximum = 100,
                //                    RecipeParameterNumericMinimum = 0,
                //                    RecipeParameterDecimalPlace = 2,
                //                    RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                    {
                //                        RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Stage_Pos, $"{YorX}_Cleaning_Pos"),
                //                    }
                //                },
                //                new RecipeParameterNumericModel
                //                {
                //                    RecipeParameterNumericKey = $"{YorX}_Unloading_Pos",
                //                    RecipeParameterNumericMaximum = 100,
                //                    RecipeParameterNumericMinimum = 0,
                //                    RecipeParameterDecimalPlace = 2,
                //                    RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                    {
                //                        RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Stage_Pos, $"{YorX}_Unloading_Pos"),
                //                    }
                //                },
                //            }
                //        });
                //    }

                //    for (int ii = 1; ii <= 40; ii++)
                //    {
                //        tempXDrawingLFPosList.Add(new RecipeParameterModel()
                //        {
                //            RecipeParameterNumericList = new List<RecipeParameterNumericModel>()
                //            {
                //                new RecipeParameterNumericModel
                //                {
                //                    RecipeParameterNumericKey = $"X_Drawing_LF_Pos[{ii}]",
                //                    RecipeParameterNumericMaximum = 100,
                //                    RecipeParameterNumericMinimum = 0,
                //                    RecipeParameterDecimalPlace = 2,
                //                    RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                    {
                //                        RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Stage_Pos, $"X_Drawing_LF_Pos[{ii}]"),
                //                    }
                //                },
                //            }
                //        });

                //        tempYDrawingLFPosList.Add(new RecipeParameterModel()
                //        {
                //            RecipeParameterNumericList = new List<RecipeParameterNumericModel>()
                //            {
                //                new RecipeParameterNumericModel
                //                {
                //                    RecipeParameterNumericKey = $"Y_Drawing_LF_Pos[{ii}]",
                //                    RecipeParameterNumericMaximum = 100,
                //                    RecipeParameterNumericMinimum = 0,
                //                    RecipeParameterDecimalPlace = 2,
                //                    RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                    {
                //                        RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Stage_Pos, $"Y_Drawing_LF_Pos[{ii}]"),
                //                    }
                //                },
                //            }
                //        });

                //        tempXWeldLegOffsetPosList.Add(new RecipeParameterModel()
                //        {
                //            RecipeParameterNumericList = new List<RecipeParameterNumericModel>()
                //            {
                //                new RecipeParameterNumericModel
                //                {
                //                    RecipeParameterNumericKey = $"X_Weld_Leg_Offset_Pos[{ii}]",
                //                    RecipeParameterNumericMaximum = 100,
                //                    RecipeParameterNumericMinimum = 0,
                //                    RecipeParameterDecimalPlace = 2,
                //                    RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                    {
                //                        RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Stage_Pos, $"X_Weld_Leg_Offset_Pos[{ii}]"),
                //                    }
                //                },
                //            }
                //        });

                //        tempYWeldLegOffsetPosList.Add(new RecipeParameterModel()
                //        {
                //            RecipeParameterNumericList = new List<RecipeParameterNumericModel>()
                //            {
                //                new RecipeParameterNumericModel
                //                {
                //                    RecipeParameterNumericKey = $"Y_Weld_Leg_Offset_Pos[{ii}]",
                //                    RecipeParameterNumericMaximum = 100,
                //                    RecipeParameterNumericMinimum = 0,
                //                    RecipeParameterDecimalPlace = 2,
                //                    RecipeParameterNumericTagModel = new RecipeParameterNumericTagModel()
                //                    {
                //                        RecipeParameterNumericTag = OPCUAHelper.CombinedNodeString(Recipe_Data, Stage_Pos, $"Y_Weld_Leg_Offset_Pos[{ii}]"),
                //                    }
                //                },
                //            }
                //        });
                //    }
                //}
            }

            //List<RecipeParameterModel> recipeParameterLists = new List<RecipeParameterModel>();

            //recipeParameterModel.RecipeParameterName = "";
            //recipeParameterModel.RecipeParameterTagModel = new RecipeParameterTagModel();
            //using (OleDbConnection oleDbConnection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={sourceFilePath};Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1;\""))
            //{
            //    oleDbConnection.Open();

            //    DataTable dataTable = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //    foreach (DataRow dataRow in dataTable.Rows)
            //    {
            //        if (dataRow["TABLE_NAME"].ToString().Contains("Recipe"))
            //        {
            //            using (OleDbCommand oleDbCommand = new OleDbCommand($"SELECT * FROM [{dataRow["TABLE_NAME"].ToString()}]", oleDbConnection))
            //            {
            //                using (OleDbDataReader oleDbDataReader = oleDbCommand.ExecuteReader())
            //                {
            //                    bool start = false;

            //                    while (oleDbDataReader.Read())
            //                    {
            //                        if (oleDbDataReader.FieldCount >= 3)
            //                        {
            //                            if (oleDbDataReader[0].ToString() == "Name")
            //                            {
            //                                start = true;
            //                            }
            //                            else
            //                            {
            //                                if (start)
            //                                {
            //                                    string recipeParameter = oleDbDataReader[0].ToString();
            //                                    recipeParameter = recipeParameter.Replace(' ', '_');
            //                                    recipeParameter = recipeParameter.Replace("\"", "");

            //                                    if (recipeParameter.Length > 0)
            //                                    {
            //                                        string TagDataBlock = oleDbDataReader[1].ToString();
            //                                        TagDataBlock = TagDataBlock.Substring(TagDataBlock.IndexOf("%DB") + 3, TagDataBlock.IndexOf('.') - 3);

            //                                        RecipeParameterModel RecipeParameterModel = new RecipeParameterModel();
            //                                        RecipeParameterModel.RecipeParameterName = recipeParameter;
            //                                        RecipeParameterModel.RecipeParameterTagModel = new RecipeParameterTagModel();

            //                                        if (oleDbDataReader[2].ToString().ToUpper().Trim() == "UINT")
            //                                        {
            //                                            var xx = oleDbDataReader[1].ToString().Substring(oleDbDataReader[1].ToString().IndexOf('.') + 4);
            //                                            RecipeParameterModel.RecipeParameterTagModel = new RecipeParameterTagModel()
            //                                            {
            //                                                RecipeParameterTag = OPCUAHelper.CombinedNodeString("",Convert.ToString(oleDbDataReader[1].ToString().Substring(oleDbDataReader[1].ToString().IndexOf('.') + 4)))
            //                                            };
            //                                        }
            //                                        //else if (oleDbDataReader[2].ToString().ToUpper().Trim() == "INT")
            //                                        //{
            //                                        //    RecipeParameterModel.RecipeParameterTagModel = new RecipeParameterTagModel()
            //                                        //    {
            //                                        //        //RecipeParameterTag = SiemensOPCHelper.GetIntegerTag(1, Convert.ToInt32(TagDataBlock), Convert.ToInt32(oleDbDataReader[1].ToString().Substring(oleDbDataReader[1].ToString().IndexOf('.') + 4)))
            //                                        //    };
            //                                        //}
            //                                        else if (oleDbDataReader[2].ToString().ToUpper().Trim() == "REAL")
            //                                        {
            //                                            RecipeParameterModel.RecipeParameterTagModel = new RecipeParameterTagModel()
            //                                            {
            //                                                RecipeParameterTag = OPCUAHelper.CombinedNodeString("", Convert.ToString(oleDbDataReader[1].ToString().Substring(oleDbDataReader[1].ToString().IndexOf('.') + 4)))
            //                                                // RecipeParameterTag = SiemensOPCHelper.GetRealTag(1, Convert.ToInt32(TagDataBlock), Convert.ToInt32(oleDbDataReader[1].ToString().Substring(oleDbDataReader[1].ToString().IndexOf('.') + 4)))
            //                                            };
            //                                        }
            //                                        else if (oleDbDataReader[2].ToString().ToUpper().Trim() == "BOOL")
            //                                        {
            //                                            RecipeParameterModel.RecipeParameterTagModel = new RecipeParameterTagModel()
            //                                            {
            //                                                RecipeParameterTag = OPCUAHelper.CombinedNodeString("", Convert.ToString(oleDbDataReader[1].ToString().Substring(oleDbDataReader[1].ToString().IndexOf('.') + 4)))
            //                                                //RecipeParameterTag = SiemensOPCHelper.GetByteTag(1, Convert.ToInt32(TagDataBlock), Convert.ToInt32(oleDbDataReader[1].ToString().Substring(oleDbDataReader[1].ToString().IndexOf('.') + 4)))
            //                                            };
            //                                        }
            //                                        else if (recipeParameter == "Recipe_Name")
            //                                        {
            //                                            RecipeParameterModel.RecipeParameterTagModel.RecipeParameterTag = OPCUAHelper.CombinedNodeString("", Convert.ToString(oleDbDataReader[1].ToString().Substring(oleDbDataReader[1].ToString().IndexOf('.') + 4)));
            //                                            //RecipeParameterModel.RecipeParameterTagModel.RecipeParameterTag = SiemensOPCHelper.GetStringTag(1, 455, 2, 20);
            //                                        }

            //                                        recipeParameterLists.Add(RecipeParameterModel);
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    SerializeRecipeParameterList(recipeParameterLists);
            //} 
            #endregion
        }
        #endregion

        #region PublicMethods
        public static bool SerializeRecipeParameterList(List<RecipeParameterModel> recipeParameterList)
        {
            var lst = recipeParameterList.Where(x => string.IsNullOrWhiteSpace(x.RecipeParameterTagModel.RecipeParameterTag)).ToArray();

            bool value = false;

            if (recipeParameterList.Any(x => string.IsNullOrEmpty(x.RecipeParameterKey)))
            {
                MessageBox.Show($"{nameof(RecipeParameterModel.RecipeParameterName)} cannot be null or empty.");
            }
            else if (recipeParameterList.Any(x => string.IsNullOrEmpty(
                x.RecipeParameterTagModel.RecipeParameterTag)))
            {
                //var item = recipeParameterList.Where(x => string.IsNullOrEmpty(
                //x.RecipeParameterTagModel.RecipeParameterTag));

                MessageBox.Show($"{nameof(RecipeParameterTagModel.RecipeParameterTag)} cannot be null or empty.");
            }
            //else if (!recipeParameterList.Exists(x => x.RecipeParameterKey == "Recipe_No"))
            //{
            //    MessageBox.Show($"{nameof(RecipeParameterTagModel.RecipeParameterTag)} cannot be null or empty.");
            //    //MessageBoxHelper.ShowMessageBox(MessageHelper.CannotBeNullOrEmpty("Recipe_No"));
            //}
            else
            {
                recipeParameterList.ForEach(x =>
                {
                    x.RecipeParameter = null;
                });
                
                XmlTools.Serialize(FilePath.RecipeParameter, recipeParameterList);
                
                value = true;
                
                Messenger.Default.Send(recipeInitializeModel);
                
                GetRecipeList().ForEach(x =>
                {
                    List<RecipeParameterModel> _recipeParameterList = DeserializeRecipe(x);
                    
                    _recipeParameterList.Select(y => y.RecipeParameterName).Except(
                        recipeParameterList.Select(y => y.RecipeParameterName)).ToList().ForEach(z =>
                        {
                            if (_recipeParameterList.Exists(i => i.RecipeParameterName == z))
                            {
                                _recipeParameterList.Remove(_recipeParameterList.Find(
                                    i => i.RecipeParameterName == z));
                            }
                        });
                    
                    recipeParameterList.Select(y => y.RecipeParameterName).Except(
                        _recipeParameterList.Select(y => y.RecipeParameterName)).ToList().ForEach(z =>
                        {
                            if (!_recipeParameterList.Exists(i => i.RecipeParameterName == z) &&
                                recipeParameterList.Exists(i => i.RecipeParameterName == z))
                            {
                                _recipeParameterList.Add(new RecipeParameterModel
                                {
                                    RecipeParameterName = z,
                                    RecipeParameter = "0"
                                });
                            }
                        });
                    SerializeRecipe(_recipeParameterList, x);
                });
            }
            return value;
        }

        public static List<RecipeParameterModel> DeserializeRecipeParameterList()
        {
            List<RecipeParameterModel> recipeParameterList = new List<RecipeParameterModel>();

            if (!File.Exists(FilePath.RecipeParameter))
            {
                SerializeRecipeParameterList(recipeParameterList);
            }

            recipeParameterList = XmlTools.Deserialize<List<RecipeParameterModel>>(
                FilePath.RecipeParameter);

            return recipeParameterList;
        }

        public static List<string> GetRecipeList()
        {
            List<string> recipeList = new List<string>();

            if (!Directory.Exists(FolderPath.RecipeFolderPath))
            {
                Directory.CreateDirectory(FolderPath.RecipeFolderPath);
            }

            Directory.GetFiles(FolderPath.RecipeFolderPath, "*.xml",
                SearchOption.TopDirectoryOnly).ToList().ForEach(x =>
                {
                    recipeList.Add(Path.GetFileNameWithoutExtension(x));
                });

            return recipeList;
        }

        #region NotUse
        public static List<string> GetRecipeNoList()
        {
            List<string> recipeNoList = new List<string>();

            if (!Directory.Exists(recipeInitializeModel.RecipeFolderPath))
            {
                Directory.CreateDirectory(recipeInitializeModel.RecipeFolderPath);
            }

            Directory.GetFiles(recipeInitializeModel.RecipeFolderPath, ".xml",
                SearchOption.TopDirectoryOnly).ToList().ForEach(x =>
                {
                    List<RecipeParameterModel> recipeParameterModels = DeserializeRecipe(Path.GetFileNameWithoutExtension(x));
                    recipeNoList.Add(recipeParameterModels.Find(y => y.RecipeParameterName == "Recipe_No").RecipeParameter);
                });

            return recipeNoList;
        }
        #endregion

        public static bool SerializeRecipe(List<RecipeParameterModel> recipeParameterList, string newRecipeName)
        {
            bool value = false;
            
            List<RecipeParameterModel> parameterTagModel = DeserializeRecipeParameterList();

            recipeParameterList.ForEach(x =>
            {
                if (parameterTagModel.Exists(y => y.RecipeParameterKey == x.RecipeParameterKey))
                {
                    x.RecipeParameterTagModel.RecipeParameterTag = parameterTagModel.Find(
                        y => y.RecipeParameterKey == x.RecipeParameterKey).RecipeParameterTagModel.RecipeParameterTag;
                }
            });

            string recipeFilePath = getRecipeFilePath(newRecipeName);

            XmlTools.Serialize(recipeFilePath, recipeParameterList);

            Messenger.Default.Send(recipeInitializeModel);

            value = true;

            return value;
        }

        public static List<RecipeParameterModel> DeserializeRecipe(string recipeName)
        {
            List<RecipeParameterModel> recipeParameterList = new List<RecipeParameterModel>();
            
            string recipeFilePath = getRecipeFilePath(recipeName);

            recipeParameterList = XmlTools.Deserialize<List<RecipeParameterModel>>(recipeFilePath);

            return recipeParameterList;
        }

        public static bool DeleteRecipe(string recipeName)
        {
            bool value = false;

            string recipeFilePath = getRecipeFilePath(recipeName);

            File.Delete(recipeFilePath);

            value = true;

            return value;
        }

        #region NotUse
        public static void LoadRecipe(string recipeName)
        {
            List<RecipeParameterModel> recipeParameterList = DeserializeRecipeParameterList();
            
            List<RecipeParameterModel> recipe = DeserializeRecipe(recipeName);
            
            recipe.ForEach(x =>
            {
                if (recipeParameterList.Exists(y => y.RecipeParameterName == x.RecipeParameterName))
                {
                    x.RecipeParameterTagModel = new RecipeParameterTagModel
                    {
                        RecipeParameterTag = recipeParameterList.Find(y => y.RecipeParameterName ==
                        x.RecipeParameterName).RecipeParameterTagModel.RecipeParameterTag
                    };
                }
            });

            Messenger.Default.Send(recipe);
        }
        #endregion

        public static List<string> GetRecipeParameterTagList()
        {
            List<string> recipeParameterTagList = new List<string>();

            List<RecipeParameterModel> recipeParameterList = DeserializeRecipeParameterList();

            recipeParameterTagList = recipeParameterList.Select(x => x.RecipeParameterTagModel).SelectMany(
                x => x.GetType().GetProperties().Where(y => y.Name != "IsInDesignMode").Select(
                    y => y.GetValue(x).ToString())).ToList();

            return recipeParameterTagList;
        }

        public static List<string> GetRecipeTagList()
        {
            List<string> recipeTagList = new List<string>()
            {
                RecipeTag.RecipeID,
                RecipeTag.RecipeName,
                RecipeTag.SaveRecipeHMI,
                RecipeTag.SaveRecipeDataReady,
                RecipeTag.LoadRecipeHMI,
                RecipeTag.LoadRecipeComplete,
                RecipeTag.CheckRecipePass,
                RecipeTag.CheckRecipeFail,
                RecipeTag.RecipeNotExist,
                RecipeTag.DeleteRecipeHMI,
            };

            return recipeTagList;
        }

        #region NotUse
        public static string GetRecipeNoByName(string recipeName)
        {
            string RecipeNo = "";

            List<RecipeParameterModel> recipeParameterModels = DeserializeRecipe(recipeName);
            RecipeNo = recipeParameterModels.Find(x => x.RecipeParameterName == "Recipe_No").RecipeParameter;

            return RecipeNo;
        }
        #endregion
        #endregion
    }
}
