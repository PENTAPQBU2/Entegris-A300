using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.Main;
using HMI_Master_KX7500.Model.Main.Module;
using HMI_Master_KX7500.Model.Main.Module.Indicator;
using HMI_Master_KX7500.Model.Main.Status;
using PQ.HMI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace HMI_Master_KX7500.Service.Main
{
    public class MachineLayoutService
    {
        #region PrivateFields
        private static string IsInDesignMode = "IsInDesignMode";
        //private static MachineLayoutInitializeModel machineLayoutInitializeModel = new MachineLayoutInitializeModel();
        #endregion

        #region PublicMethods
        public static bool SerializeMachineLayout(MachineLayoutModel machineLayoutModel)
        {
            bool value = false;

            if (machineLayoutModel.MachineLayoutModuleList.Any(
                x => string.IsNullOrEmpty(x.MachineLayoutModuleKey)))
            {
                MessageBox.Show($"{nameof(MachineLayoutModuleModel.MachineLayoutModuleKey)} Cannot Be Null/Empty");
            }
            else if (machineLayoutModel.MachineLayoutModuleList.Any(
                x => string.IsNullOrEmpty(x.MachineLayoutModuleImage)))
            {
                MessageBox.Show($"{nameof(MachineLayoutModuleModel.MachineLayoutModuleImage)} Cannot Be Null/Empty");
            }
            else if (machineLayoutModel.MachineLayoutModuleList.SelectMany(
                x => x.MachineLayoutModuleStatusIndicatorList).Any(
                y => string.IsNullOrEmpty(y.MachineLayoutModuleStatusIndicatorKey)))
            {
                MessageBox.Show($"{nameof(MachineLayoutModuleStatusIndicatorModel.MachineLayoutModuleStatusIndicatorKey)} Cannot Be Null/Empty");
            }
            else if (machineLayoutModel.MachineLayoutModuleList.SelectMany(
                x => x.MachineLayoutModuleStatusIndicatorList).Any(y => string.IsNullOrEmpty(
                    y.MachineLayoutModuleStatusIndicatorTagModel.MachineLayoutModuleStatusIndicatorTag)))
            {
                MessageBox.Show($"{nameof(MachineLayoutModuleStatusIndicatorTagModel.MachineLayoutModuleStatusIndicatorTag)} Cannot Be Null/Empty");
            }
            else if (machineLayoutModel.MachineLayoutModuleStatusList.Any(
                x => string.IsNullOrEmpty(x.MachineLayoutModuleStatusKey)))
            {
                MessageBox.Show($"{nameof(MachineLayoutModuleStatusModel.MachineLayoutModuleStatusKey)} Cannot Be Null/Empty");
            }
            else if (machineLayoutModel.MachineLayoutModuleStatusList.Any(
                x => string.IsNullOrEmpty(x.MachineLayoutModuleStatusIndex)))
            {
                MessageBox.Show($"{nameof(MachineLayoutModuleStatusModel.MachineLayoutModuleStatusIndex)} Cannot Be Null/Empty");
            }
            else if (machineLayoutModel.MachineLayoutModuleStatusList.Any(
                x => string.IsNullOrEmpty(x.MachineLayoutModuleStatusColor)))
            {
                MessageBox.Show($"{nameof(MachineLayoutModuleStatusModel.MachineLayoutModuleStatusColor)} Cannot Be Null/Empty");
            }
            //else if (machineLayoutModel.MachineLayoutToggleList.Any(
            //    x => string.IsNullOrEmpty(x.MachineLayoutToggleHeaderKey)))
            //{
            //    MessageBox.Show($"{nameof(MachineLayoutToggleModel.MachineLayoutToggleHeaderKey)} Cannot Be Null/Empty");
            //}
            //else if (machineLayoutModel.MachineLayoutToggleList.Any(
            //    x => string.IsNullOrEmpty(x.MachineLayoutToggleTagModel.MachineLayoutToggleTag)))
            //{
            //    MessageBox.Show($"{nameof(MachineLayoutToggleModel.MachineLayoutToggleTagModel.MachineLayoutToggleTag)} Cannot Be Null/Empty");
            //}
            else
            {
                XmlTools.Serialize(FilePath.MachineLayout, machineLayoutModel);
                value = true;
            }

            return value;
        }

        public static MachineLayoutModel DeserializeMachineLayout()
        {
            MachineLayoutModel machineLayoutModel = new MachineLayoutModel();

            if (!File.Exists(FilePath.MachineLayout))
            {
                machineLayoutModel = new MachineLayoutModel
                {
                    MachineLayoutModuleList = new List<MachineLayoutModuleModel>()
                    {
                        new MachineLayoutModuleModel()
                        {
                            MachineLayoutModuleImage = FolderPath.Images + "MainStatus.png",
                            MachineLayoutModuleKey = "Module Status",
                            MachineLayoutModuleStatusIndicatorList = new List<MachineLayoutModuleStatusIndicatorModel>()
                            {
                                new MachineLayoutModuleStatusIndicatorModel()
                                {
                                    MachineLayoutModuleStatusIndicatorKey = "Shell_Loading_Module",
                                    MachineLayoutModuleStatusIndicatorRightPosition = 280,
                                    MachineLayoutModuleStatusIndicatorTopPosition = 330,
                                    MachineLayoutModuleStatusIndicatorLine1X1 = -315,
                                    MachineLayoutModuleStatusIndicatorLine1X2 = -315,
                                    MachineLayoutModuleStatusIndicatorLine1Y1 = 330,
                                    MachineLayoutModuleStatusIndicatorLine1Y2 = 180,
                                    MachineLayoutModuleStatusIndicatorLine2X1 = 0,
                                    MachineLayoutModuleStatusIndicatorLine2X2 = 0,
                                    MachineLayoutModuleStatusIndicatorLine2Y1 = 0,
                                    MachineLayoutModuleStatusIndicatorLine2Y2 = 0,
                                    MachineLayoutModuleStatusIndicatorTagModel = new MachineLayoutModuleStatusIndicatorTagModel()
                                    {
                                        MachineLayoutModuleStatusIndicatorTag = "EM140"
                                    }
                                },
                                new MachineLayoutModuleStatusIndicatorModel()
                                {
                                    MachineLayoutModuleStatusIndicatorKey = "Top_Clamp_Module",
                                    MachineLayoutModuleStatusIndicatorRightPosition = 90,
                                    MachineLayoutModuleStatusIndicatorTopPosition = -50,
                                    MachineLayoutModuleStatusIndicatorLine1X1 = -250,
                                    MachineLayoutModuleStatusIndicatorLine1X2 = -250,
                                    MachineLayoutModuleStatusIndicatorLine1Y1 = 100,
                                    MachineLayoutModuleStatusIndicatorLine1Y2 = -20,
                                    MachineLayoutModuleStatusIndicatorLine2X1 = 0,
                                    MachineLayoutModuleStatusIndicatorLine2X2 = 0,
                                    MachineLayoutModuleStatusIndicatorLine2Y1 = 0,
                                    MachineLayoutModuleStatusIndicatorLine2Y2 = 0,
                                    MachineLayoutModuleStatusIndicatorTagModel = new MachineLayoutModuleStatusIndicatorTagModel()
                                    {
                                        MachineLayoutModuleStatusIndicatorTag = "EM141"
                                    }
                                },
                                new MachineLayoutModuleStatusIndicatorModel()
                                {
                                    MachineLayoutModuleStatusIndicatorKey = "Flipping_Module",
                                    MachineLayoutModuleStatusIndicatorRightPosition = 300,
                                    MachineLayoutModuleStatusIndicatorTopPosition = -50,
                                    MachineLayoutModuleStatusIndicatorLine1X1 = -320,
                                    MachineLayoutModuleStatusIndicatorLine1X2 = -320,
                                    MachineLayoutModuleStatusIndicatorLine1Y1 = 80,
                                    MachineLayoutModuleStatusIndicatorLine1Y2 = -20,
                                    MachineLayoutModuleStatusIndicatorLine2X1 = 0,
                                    MachineLayoutModuleStatusIndicatorLine2X2 = 0,
                                    MachineLayoutModuleStatusIndicatorLine2Y1 = 0,
                                    MachineLayoutModuleStatusIndicatorLine2Y2 = 0,
                                    MachineLayoutModuleStatusIndicatorTagModel = new MachineLayoutModuleStatusIndicatorTagModel()
                                    {
                                        MachineLayoutModuleStatusIndicatorTag = "EM142"
                                    }
                                },
                                new MachineLayoutModuleStatusIndicatorModel()
                                {
                                    MachineLayoutModuleStatusIndicatorKey = "Shim_and_O-Ring_Alignment_Module",
                                    MachineLayoutModuleStatusIndicatorRightPosition = 100,
                                    MachineLayoutModuleStatusIndicatorTopPosition = 360,
                                    MachineLayoutModuleStatusIndicatorLine1X1 = -250,
                                    MachineLayoutModuleStatusIndicatorLine1X2 = -250,
                                    MachineLayoutModuleStatusIndicatorLine1Y1 = 360,
                                    MachineLayoutModuleStatusIndicatorLine1Y2 = 200,
                                    MachineLayoutModuleStatusIndicatorLine2X1 = -250,
                                    MachineLayoutModuleStatusIndicatorLine2X2 = -220,
                                    MachineLayoutModuleStatusIndicatorLine2Y1 = 200,
                                    MachineLayoutModuleStatusIndicatorLine2Y2 = 200,
                                    MachineLayoutModuleStatusIndicatorTagModel = new MachineLayoutModuleStatusIndicatorTagModel()
                                    {
                                        MachineLayoutModuleStatusIndicatorTag = "EM143"
                                    }
                                },
                                new MachineLayoutModuleStatusIndicatorModel()
                                {
                                    MachineLayoutModuleStatusIndicatorKey = "Side_Column_Module",
                                    MachineLayoutModuleStatusIndicatorRightPosition = 20,
                                    MachineLayoutModuleStatusIndicatorTopPosition = 330,
                                    MachineLayoutModuleStatusIndicatorLine1X1 = -150,
                                    MachineLayoutModuleStatusIndicatorLine1X2 = -150,
                                    MachineLayoutModuleStatusIndicatorLine1Y1 = 330,
                                    MachineLayoutModuleStatusIndicatorLine1Y2 = 200,
                                    MachineLayoutModuleStatusIndicatorLine2X1 = -150,
                                    MachineLayoutModuleStatusIndicatorLine2X2 = -180,
                                    MachineLayoutModuleStatusIndicatorLine2Y1 = 200,
                                    MachineLayoutModuleStatusIndicatorLine2Y2 = 200,
                                    MachineLayoutModuleStatusIndicatorTagModel = new MachineLayoutModuleStatusIndicatorTagModel()
                                    {
                                        MachineLayoutModuleStatusIndicatorTag = "EM144"
                                    }
                                },
                                new MachineLayoutModuleStatusIndicatorModel()
                                {
                                    MachineLayoutModuleStatusIndicatorKey = "Vision_Module",
                                    MachineLayoutModuleStatusIndicatorRightPosition = -200,
                                    MachineLayoutModuleStatusIndicatorTopPosition = 100,
                                    MachineLayoutModuleStatusIndicatorLine1X1 = -120,
                                    MachineLayoutModuleStatusIndicatorLine1X2 = 60,
                                    MachineLayoutModuleStatusIndicatorLine1Y1 = 110,
                                    MachineLayoutModuleStatusIndicatorLine1Y2 = 110,
                                    MachineLayoutModuleStatusIndicatorLine2X1 = 0,
                                    MachineLayoutModuleStatusIndicatorLine2X2 = 0,
                                    MachineLayoutModuleStatusIndicatorLine2Y1 = 0,
                                    MachineLayoutModuleStatusIndicatorLine2Y2 = 0,
                                    MachineLayoutModuleStatusIndicatorTagModel = new MachineLayoutModuleStatusIndicatorTagModel()
                                    {
                                        MachineLayoutModuleStatusIndicatorTag = "EM145"
                                    }
                                },
                                new MachineLayoutModuleStatusIndicatorModel()
                                {
                                    MachineLayoutModuleStatusIndicatorKey = "UR_Robot",
                                    MachineLayoutModuleStatusIndicatorRightPosition = 500,
                                    MachineLayoutModuleStatusIndicatorTopPosition = -50,
                                    MachineLayoutModuleStatusIndicatorLine1X1 = -580,
                                    MachineLayoutModuleStatusIndicatorLine1X2 = -580,
                                    MachineLayoutModuleStatusIndicatorLine1Y1 = 80,
                                    MachineLayoutModuleStatusIndicatorLine1Y2 = -20,
                                    MachineLayoutModuleStatusIndicatorLine2X1 = -580,
                                    MachineLayoutModuleStatusIndicatorLine2X2 = -450,
                                    MachineLayoutModuleStatusIndicatorLine2Y1 = 80, 
                                    MachineLayoutModuleStatusIndicatorLine2Y2 = 80,
                                    MachineLayoutModuleStatusIndicatorTagModel = new MachineLayoutModuleStatusIndicatorTagModel()
                                    {
                                        MachineLayoutModuleStatusIndicatorTag = "EM146"
                                    }
                                },
                            }
                        },
                    },
                    MachineLayoutModuleStatusList = new List<MachineLayoutModuleStatusModel>
                    {
                        new MachineLayoutModuleStatusModel
                        {
                            MachineLayoutModuleStatusKey = "Running",
                            MachineLayoutModuleStatusIndex = "1",
                            MachineLayoutModuleStatusColor = nameof(Brushes.Green)
                        },
                        new MachineLayoutModuleStatusModel
                        {
                            MachineLayoutModuleStatusKey = "Idle",
                            MachineLayoutModuleStatusIndex = "2",
                            MachineLayoutModuleStatusColor = nameof(Brushes.Yellow)
                        },
                        new MachineLayoutModuleStatusModel
                        {
                            MachineLayoutModuleStatusKey = "NotInitDone",
                            MachineLayoutModuleStatusIndex = "3",
                            MachineLayoutModuleStatusColor = nameof(Brushes.Blue)
                        },
                        new MachineLayoutModuleStatusModel
                        {
                            MachineLayoutModuleStatusKey = "Initializing",
                            MachineLayoutModuleStatusIndex = "4",
                            MachineLayoutModuleStatusColor = nameof(Brushes.Yellow)
                        },
                        new MachineLayoutModuleStatusModel
                        {
                            MachineLayoutModuleStatusKey = "Stop",
                            MachineLayoutModuleStatusIndex = "5",
                            MachineLayoutModuleStatusColor = nameof(Brushes.Red)
                        },
                        new MachineLayoutModuleStatusModel
                        {
                            MachineLayoutModuleStatusKey = "Error",
                            MachineLayoutModuleStatusIndex = "6",
                            MachineLayoutModuleStatusColor = nameof(Brushes.Red)
                        },
                        new MachineLayoutModuleStatusModel
                        {
                            MachineLayoutModuleStatusKey = "Warning",
                            MachineLayoutModuleStatusIndex = "7",
                            MachineLayoutModuleStatusColor = nameof(Brushes.Yellow)
                        },
                        new MachineLayoutModuleStatusModel
                        {
                            MachineLayoutModuleStatusKey = "EMG",
                            MachineLayoutModuleStatusIndex = "8",
                            MachineLayoutModuleStatusColor = nameof(Brushes.Red)
                        },
                    },
                };
                
                SerializeMachineLayout(machineLayoutModel);
            }

            machineLayoutModel = XmlTools.Deserialize<MachineLayoutModel>(FilePath.MachineLayout, machineLayoutModel);

            return machineLayoutModel;
        }

        public static List<string> GetMachineLayoutTagList()
        {
            MachineLayoutModel machineLayoutModel = DeserializeMachineLayout();
            List<string> machineLayoutTagList = new List<string>();

            machineLayoutTagList = machineLayoutModel.MachineLayoutModuleList.SelectMany(
                  x => x.MachineLayoutModuleStatusIndicatorList).Select(
                  x => x.MachineLayoutModuleStatusIndicatorTagModel).SelectMany(
                  x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                      y => y.GetValue(x).ToString())).ToList();

            return machineLayoutTagList;
        }
        #endregion
    }
}
