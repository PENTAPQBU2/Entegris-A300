using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Model;
using HMI_Master_KX7500.Models;
using PQ.HMI;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HMI_Master_KX7500.Services
{
    public class EngineeringService
    {
        #region PrivateFields
        private static EngineeringInitializeModel engineeringInitializeModel = new EngineeringInitializeModel();
        private static string IsInDesignMode = "IsInDesignMode";
        #endregion

        #region Constructor
        public EngineeringService()
        {
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeMessenger()
        {
            Messenger.Default.Register<EngineeringInitializeModel>(this, initializeEngineering);
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializeEngineering(EngineeringInitializeModel _engineeringInitializeModel)
        {
            engineeringInitializeModel = _engineeringInitializeModel;
        }
        #endregion

        #region PublicMethods
        public static bool SerializeEngineering(EngineeringMainModel engineeringMainModel)
        {
            bool value = false;
            XmlTools.Serialize(engineeringInitializeModel.EngineeringFilePath, engineeringMainModel);
            value = true;
            Messenger.Default.Send(engineeringInitializeModel);
            return value;
        }

        public static EngineeringMainModel DeserializeEngineering()
        {
            EngineeringMainModel engineeringMainModel = new EngineeringMainModel();

            if (!File.Exists(engineeringInitializeModel.EngineeringFilePath))
            {
                engineeringMainModel.EngineeringGroupModel = new List<EngineeringGroupModel>()
                {
                        new EngineeringGroupModel
                        {
                            EngineeringGroupKey = "Position",
                            //IsAccessControlled = false,
                            EngineeringGroupReadOnlyList = new List<EngineeringGroupReadOnlyModel>
                            {
                                new EngineeringGroupReadOnlyModel
                                {
                                    EngineeringGroupReadOnlyKey = "Axis_Positions[0]",
                                    EngineeringGroupReadOnlyName = "Axis_Positions[0]",
                                    EngineeringGroupReadOnlyTagModel = new EngineeringGroupReadOnlyTagModel
                                    {
                                        EngineeringGroupReadOnlyTag = "EM20000"
                                    }
                                },
                                new EngineeringGroupReadOnlyModel
                                {
                                    EngineeringGroupReadOnlyKey = "Axis_Positions[1]",
                                    EngineeringGroupReadOnlyName = "Axis_Positions[1]",
                                    EngineeringGroupReadOnlyTagModel = new EngineeringGroupReadOnlyTagModel
                                    {
                                        EngineeringGroupReadOnlyTag = "EM20002"
                                    }
                                },
                                new EngineeringGroupReadOnlyModel
                                {
                                    EngineeringGroupReadOnlyKey = "Axis_Positions[2]",
                                    EngineeringGroupReadOnlyName = "Axis_Positions[2]",
                                    EngineeringGroupReadOnlyTagModel = new EngineeringGroupReadOnlyTagModel
                                    {
                                        EngineeringGroupReadOnlyTag = "EM20004"
                                    }
                                }
                            }
                        },
                        new EngineeringGroupModel
                        {
                            EngineeringGroupKey = "Toggle",
                            //IsAccessControlled = false,
                            EngineeringGroupToggleList = new List<EngineeringGroupToggleModel>
                            {
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "HMI_Save_Current_Pos[0]",
                                    EngineeringGroupToggleName = "HMI_Save_Current_Pos[0]",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR2100"
                                    }
                                },
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "HMI_Save_Current_Pos[1]",
                                    EngineeringGroupToggleName = "HMI_Save_Current_Pos[1]",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR2101"
                                    }
                                },
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "HMI_Save_Current_Pos[2]",
                                    EngineeringGroupToggleName = "HMI_Save_Current_Pos[2]",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR2102"
                                    }
                                },
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "HMI_Save_Key-In_Pos[0]",
                                    EngineeringGroupToggleName = "HMI_Save_Key-In_Pos[0]",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR2200"
                                    }
                                },
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "HMI_Save_Key-In_Pos[1]",
                                    EngineeringGroupToggleName = "HMI_Save_Key-In_Pos[1]",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR2201"
                                    }
                                },
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "HMI_Save_Key-In_Pos[2]",
                                    EngineeringGroupToggleName = "HMI_Save_Key-In_Pos[2]",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR2202"
                                    }
                                },
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "HMI_ManualGo[0]",
                                    EngineeringGroupToggleName = "HMI_ManualGo[0]",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR2300"
                                    }
                                },
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "HMI_ManualGo[1]",
                                    EngineeringGroupToggleName = "HMI_ManualGo[1]",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR2301"
                                    }
                                },
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "HMI_ManualGo[2]",
                                    EngineeringGroupToggleName = "HMI_ManualGo[2]",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR2302"
                                    }
                                },
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "Axis_Position_Reached[0]",
                                    EngineeringGroupToggleName = "Axis_Position_Reached[0]",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR2400"
                                    }
                                },
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "Axis_Position_Reached[1]",
                                    EngineeringGroupToggleName = "Axis_Position_Reached[1]",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR2401"
                                    }
                                },
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "Axis_Position_Reached[2]",
                                    EngineeringGroupToggleName = "Axis_Position_Reached[2]",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR2402"
                                    }
                                },
                                //Calibration Mode
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "Calibration_Mode_On/Off",
                                    EngineeringGroupToggleName = "Calibration_Mode_On/Off",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR1004"
                                    }
                                },
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "Start_Calibration",
                                    EngineeringGroupToggleName = "Start_Calibration",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR900"
                                    }
                                },
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "Calibration_Program_1",
                                    EngineeringGroupToggleName = "Calibration_Program_1",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR901"
                                    }
                                },
                                new EngineeringGroupToggleModel
                                {
                                    EngineeringGroupToggleKey = "Calibration_Program_2",
                                    EngineeringGroupToggleName = "Calibration_Program_2",
                                    EngineeringGroupToggleTagModel = new EngineeringGroupToggleTagModel()
                                    {
                                        EngineeringGroupToggleTag = "MR902"
                                    }
                                },
                            },
                            EngineeringGroupNumericList = new List<EngineeringGroupNumericModel>
                            {
                                new EngineeringGroupNumericModel
                                {
                                    EngineeringGroupNumericKey = "HMI_Key-In_PosValue[0]",
                                    EngineeringGroupNumericName = "HMI_Key-In_PosValue[0]",
                                    EngineeringGroupNumericTagModel = new EngineeringGroupNumericTagModel
                                    {
                                        EngineeringGroupNumericTag = ("EM4030")
                                    }
                                },
                                new EngineeringGroupNumericModel
                                {
                                    EngineeringGroupNumericKey = "HMI_Key-In_PosValue[1]",
                                    EngineeringGroupNumericName = "HMI_Key-In_PosValue[1]",
                                    EngineeringGroupNumericTagModel = new EngineeringGroupNumericTagModel
                                    {
                                        EngineeringGroupNumericTag = ("EM4032")
                                    }
                                },
                                new EngineeringGroupNumericModel
                                {
                                    EngineeringGroupNumericKey = "HMI_Key-In_PosValue[2]",
                                    EngineeringGroupNumericName = "HMI_Key-In_PosValue[2]",
                                    EngineeringGroupNumericTagModel = new EngineeringGroupNumericTagModel
                                    {
                                        EngineeringGroupNumericTag = ("EM4034")
                                    }
                                },
                            }
                        }
                };
                SerializeEngineering(engineeringMainModel);
            }

            engineeringMainModel = XmlTools.Deserialize<EngineeringMainModel>(engineeringInitializeModel.EngineeringFilePath, engineeringMainModel);

            return engineeringMainModel;
        }

        public static List<string> GetEngineeringTagList()
        {
            var engineeringMainModel = DeserializeEngineering();
            List<string> engineeringTagList = new List<string>();

            engineeringMainModel.EngineeringGroupModel.ForEach(x =>
            {
                engineeringTagList = engineeringTagList.Union(x.EngineeringGroupNumericList.Select(y => y.EngineeringGroupNumericTagModel).SelectMany(
                    y => y.GetType().GetProperties().Where(z => z.Name != IsInDesignMode).Select(
                        z => z.GetValue(y).ToString()))).ToList();

                engineeringTagList = engineeringTagList.Union(x.EngineeringGroupToggleList.Select(y => y.EngineeringGroupToggleTagModel).SelectMany(
                    y => y.GetType().GetProperties().Where(z => z.Name != IsInDesignMode).Select(
                        z => z.GetValue(y).ToString()))).ToList();

                engineeringTagList = engineeringTagList.Union(x.EngineeringGroupReadOnlyList.Select(y => y.EngineeringGroupReadOnlyTagModel).SelectMany(
                    y => y.GetType().GetProperties().Where(z => z.Name != IsInDesignMode).Select(
                        z => z.GetValue(y).ToString()))).ToList();

                engineeringTagList = engineeringTagList.Union(x.EngineeringGroupTextList.Select(y => y.EngineeringGroupTextTagModel).SelectMany(
                    y => y.GetType().GetProperties().Where(z => z.Name != IsInDesignMode).Select(
                        z => z.GetValue(y).ToString()))).ToList();
            });

            return engineeringTagList;
        }

        #endregion
    }
}
