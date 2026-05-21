using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model;
using HMI_Master_KX7500.Models;
using PQ.HMI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace HMI_Master_KX7500.Services
{
    public class SettingsGeneralService
    {
        #region PrivateFields
        private static string IsInDesignMode = "IsInDesignMode";
        public const string ResourceDictionarySource = "pack://application:,,,/HMI;component/";
        #endregion

        #region PublicMethods
        public static bool SerializeSettingsGeneral(SettingsGeneralModel settingsGeneralModel)
        {
            bool value = false;

            if(settingsGeneralModel.SettingsGeneralGroupList.Any(
                x => string.IsNullOrEmpty(x.SettingsGeneralGroupKey)))
            {
                MessageBox.Show($"{nameof(SettingsGeneralGroupModel.SettingsGeneralGroupKey)} Cannot Be Null/Empty");
            }
            else if(settingsGeneralModel.SettingsGeneralGroupList.SelectMany(
                x => x.SettingsGeneralGroupNumericList).Any(
                x => string.IsNullOrEmpty(x.SettingsGeneralGroupNumericKey)))
            {
                MessageBox.Show($"{nameof(SettingsGeneralGroupNumericModel.SettingsGeneralGroupNumericKey)} Cannot Be Null/Empty");
            }
            else if(settingsGeneralModel.SettingsGeneralGroupList.SelectMany(
                x => x.SettingsGeneralGroupNumericList).Any(
                x => string.IsNullOrEmpty(x.SettingsGeneralGroupNumericTagModel.SettingsGeneralGroupNumericTag)))
            {
                MessageBox.Show($"{nameof(SettingsGeneralGroupNumericTagModel.SettingsGeneralGroupNumericTag)} Cannot Be Null/Empty");
            }
            else if(settingsGeneralModel.SettingsGeneralGroupList.SelectMany(
                x => x.SettingsGeneralGroupNumericList).Any(
                x => x.SettingsGeneralGroupDecimalPlace < 0))
            {
                MessageBox.Show($"{nameof(SettingsGeneralGroupNumericModel.SettingsGeneralGroupDecimalPlace)} cannot be negative.");
            }
            else if(settingsGeneralModel.SettingsGeneralGroupList.SelectMany(
                x => x.SettingsGeneralGroupToggleList).Any(
                x => string.IsNullOrEmpty(x.SettingsGeneralGroupToggleKey)))
            {
                MessageBox.Show($"{nameof(SettingsGeneralGroupToggleModel.SettingsGeneralGroupToggleKey)} Cannot Be Null/Empty");
            }
            else if(settingsGeneralModel.SettingsGeneralGroupList.SelectMany(
                x => x.SettingsGeneralGroupToggleList).Any(
                x => string.IsNullOrEmpty(x.SettingsGeneralGroupToggleOffIcon)))
            {
                MessageBox.Show($"{nameof(SettingsGeneralGroupToggleModel.SettingsGeneralGroupToggleOffIcon)} Cannot Be Null/Empty");
            }
            else if(settingsGeneralModel.SettingsGeneralGroupList.SelectMany(
                x => x.SettingsGeneralGroupToggleList).Any(
                x => string.IsNullOrEmpty(x.SettingsGeneralGroupToggleOffLabelKey)))
            {
                MessageBox.Show($"{nameof(SettingsGeneralGroupToggleModel.SettingsGeneralGroupToggleOffLabelKey)} Cannot Be Null/Empty");
            }
            else if(settingsGeneralModel.SettingsGeneralGroupList.SelectMany(
                x => x.SettingsGeneralGroupToggleList).Any(
                x => string.IsNullOrEmpty(x.SettingsGeneralGroupToggleOnIcon)))
            {

                MessageBox.Show($"{nameof(SettingsGeneralGroupToggleModel.SettingsGeneralGroupToggleOnIcon)} Cannot Be Null/Empty");
            }
            else if(settingsGeneralModel.SettingsGeneralGroupList.SelectMany(
                x => x.SettingsGeneralGroupToggleList).Any(
                x => string.IsNullOrEmpty(x.SettingsGeneralGroupToggleOnLabelKey)))
            {
                MessageBox.Show($"{nameof(SettingsGeneralGroupToggleModel.SettingsGeneralGroupToggleOnLabelKey)} Cannot Be Null/Empty");
            }
            else if(settingsGeneralModel.SettingsGeneralGroupList.SelectMany(
                x => x.SettingsGeneralGroupToggleList).Any(
                x => string.IsNullOrEmpty(x.SettingsGeneralGroupToggleTagModel.SettingsGeneralGroupToggleTag)))
            {
                MessageBox.Show($"{nameof(SettingsGeneralGroupToggleTagModel.SettingsGeneralGroupToggleTag)} Cannot Be Null/Empty");
            }
            else
            {
                bool valid = true;

                if(valid)
                {
                    XmlTools.Serialize(FilePath.Settings, settingsGeneralModel);

                    value = true;

                }
            }

            return value;
        }

        public static SettingsGeneralModel DeserializeSettingsGeneral()
        {
            SettingsGeneralModel settingsGeneralModel = new SettingsGeneralModel();

            if(!File.Exists(FilePath.Settings))
            {
                settingsGeneralModel.SettingsGeneralGroupList.Add(new SettingsGeneralGroupModel()
                {
                    SettingsGeneralGroupKey = "Bypass",
                    SettingsGeneralGroupToggleList = new List<SettingsGeneralGroupToggleModel>
                    {
                        new SettingsGeneralGroupToggleModel
                        {
                            SettingsGeneralGroupToggleKey = "Bypass_Door_Switch",
                            SettingsGeneralGroupToggleOnLabelKey = "On",
                            SettingsGeneralGroupToggleOffLabelKey = "Off",
                            SettingsGeneralGroupToggleTagModel = new SettingsGeneralGroupToggleTagModel()
                            {
                                SettingsGeneralGroupToggleTag = "MR1200"
                            },
                            SettingsGeneralGroupToggleOffIcon = GetIconSource("LightBulbOff"),
                            SettingsGeneralGroupToggleOnIcon = GetIconSource("LightBulbOn"),
                        },
                        new SettingsGeneralGroupToggleModel
                        {
                            SettingsGeneralGroupToggleKey = "Bypass_E-Stop",
                            SettingsGeneralGroupToggleOnLabelKey = "On",
                            SettingsGeneralGroupToggleOffLabelKey = "Off",
                            SettingsGeneralGroupToggleTagModel = new SettingsGeneralGroupToggleTagModel()
                            {
                                SettingsGeneralGroupToggleTag = "MR1201"
                            },
                            SettingsGeneralGroupToggleOffIcon = GetIconSource("LightBulbOff"),
                            SettingsGeneralGroupToggleOnIcon = GetIconSource("LightBulbOn"),
                        },
                        new SettingsGeneralGroupToggleModel
                        {
                            SettingsGeneralGroupToggleKey = "Bypass_Safety_Relay",
                            SettingsGeneralGroupToggleOnLabelKey = "On",
                            SettingsGeneralGroupToggleOffLabelKey = "Off",
                            SettingsGeneralGroupToggleTagModel = new SettingsGeneralGroupToggleTagModel()
                            {
                                SettingsGeneralGroupToggleTag = "MR1202"
                            },
                            SettingsGeneralGroupToggleOffIcon = GetIconSource("LightBulbOff"),
                            SettingsGeneralGroupToggleOnIcon = GetIconSource("LightBulbOn"),
                        },
                        new SettingsGeneralGroupToggleModel
                        {
                            SettingsGeneralGroupToggleKey = "Bypass_Pressure_Switch",
                            SettingsGeneralGroupToggleOnLabelKey = "On",
                            SettingsGeneralGroupToggleOffLabelKey = "Off",
                            SettingsGeneralGroupToggleTagModel = new SettingsGeneralGroupToggleTagModel()
                            {
                                SettingsGeneralGroupToggleTag = "MR1203"
                            },
                            SettingsGeneralGroupToggleOffIcon = GetIconSource("LightBulbOff"),
                            SettingsGeneralGroupToggleOnIcon = GetIconSource("LightBulbOn"),
                        },
                        new SettingsGeneralGroupToggleModel
                        {
                            SettingsGeneralGroupToggleKey = "Bypass_Buzzer",
                            SettingsGeneralGroupToggleOnLabelKey = "On",
                            SettingsGeneralGroupToggleOffLabelKey = "Off",
                            SettingsGeneralGroupToggleTagModel = new SettingsGeneralGroupToggleTagModel()
                            {
                                SettingsGeneralGroupToggleTag = "MR1204"
                            },
                            SettingsGeneralGroupToggleOffIcon = GetIconSource("LightBulbOff"),
                            SettingsGeneralGroupToggleOnIcon = GetIconSource("LightBulbOn"),
                        },
                        new SettingsGeneralGroupToggleModel
                        {
                            SettingsGeneralGroupToggleKey = "Bypass_Shell_Load_Module_Sensor",
                            SettingsGeneralGroupToggleOnLabelKey = "On",
                            SettingsGeneralGroupToggleOffLabelKey = "Off",
                            SettingsGeneralGroupToggleTagModel = new SettingsGeneralGroupToggleTagModel()
                            {
                                SettingsGeneralGroupToggleTag = "MR1205"
                            },
                            SettingsGeneralGroupToggleOffIcon = GetIconSource("LightBulbOff"),
                            SettingsGeneralGroupToggleOnIcon = GetIconSource("LightBulbOn"),
                        },
                        new SettingsGeneralGroupToggleModel
                        {
                            SettingsGeneralGroupToggleKey = "Bypass_Side_Column_Module_Sensor",
                            SettingsGeneralGroupToggleOnLabelKey = "On",
                            SettingsGeneralGroupToggleOffLabelKey = "Off",
                            SettingsGeneralGroupToggleTagModel = new SettingsGeneralGroupToggleTagModel()
                            {
                                SettingsGeneralGroupToggleTag = "MR1206"
                            },
                            SettingsGeneralGroupToggleOffIcon = GetIconSource("LightBulbOff"),
                            SettingsGeneralGroupToggleOnIcon = GetIconSource("LightBulbOn"),
                        },
                        new SettingsGeneralGroupToggleModel
                        {
                            SettingsGeneralGroupToggleKey = "Bypass_Curtain_Sensor",
                            SettingsGeneralGroupToggleOnLabelKey = "On",
                            SettingsGeneralGroupToggleOffLabelKey = "Off",
                            SettingsGeneralGroupToggleTagModel = new SettingsGeneralGroupToggleTagModel()
                            {
                                SettingsGeneralGroupToggleTag = "MR1207"
                            },
                            SettingsGeneralGroupToggleOffIcon = GetIconSource("LightBulbOff"),
                            SettingsGeneralGroupToggleOnIcon = GetIconSource("LightBulbOn"),
                        },
                        new SettingsGeneralGroupToggleModel
                        {
                            SettingsGeneralGroupToggleKey = "Bypass_Screw_Driver_Sequence",
                            SettingsGeneralGroupToggleOnLabelKey = "On",
                            SettingsGeneralGroupToggleOffLabelKey = "Off",
                            SettingsGeneralGroupToggleTagModel = new SettingsGeneralGroupToggleTagModel()
                            {
                                SettingsGeneralGroupToggleTag = "MR1208"
                            },
                            SettingsGeneralGroupToggleOffIcon = GetIconSource("LightBulbOff"),
                            SettingsGeneralGroupToggleOnIcon = GetIconSource("LightBulbOn"),
                        },
                        new SettingsGeneralGroupToggleModel
                        {
                            SettingsGeneralGroupToggleKey = "Bypass_CAMLINE",
                            SettingsGeneralGroupToggleOnLabelKey = "On",
                            SettingsGeneralGroupToggleOffLabelKey = "Off",
                            SettingsGeneralGroupToggleTagModel = new SettingsGeneralGroupToggleTagModel()
                            {
                                SettingsGeneralGroupToggleTag = "MR1209"
                            },
                            SettingsGeneralGroupToggleOffIcon = GetIconSource("LightBulbOff"),
                            SettingsGeneralGroupToggleOnIcon = GetIconSource("LightBulbOn"),
                        },
                        new SettingsGeneralGroupToggleModel
                        {
                            SettingsGeneralGroupToggleKey = "Bypass_Soft_Start_Valve",
                            SettingsGeneralGroupToggleOnLabelKey = "On",
                            SettingsGeneralGroupToggleOffLabelKey = "Off",
                            SettingsGeneralGroupToggleTagModel = new SettingsGeneralGroupToggleTagModel()
                            {
                                SettingsGeneralGroupToggleTag = "MR1210"
                            },
                            SettingsGeneralGroupToggleOffIcon = GetIconSource("LightBulbOff"),
                            SettingsGeneralGroupToggleOnIcon = GetIconSource("LightBulbOn"),
                        },
                    }
                });
                settingsGeneralModel.SettingsGeneralGroupList.Add(new SettingsGeneralGroupModel()
                {
                    SettingsGeneralGroupKey = "Monitor",                 
                    SettingsGeneralGroupReadOnlyList = new List<SettingsGeneralGroupReadOnlyModel>
                    {
                        new SettingsGeneralGroupReadOnlyModel
                        {
                            SettingsGeneralGroupReadOnlyKey = "O-Ring_Loaded",
                            SettingsGeneralGroupReadOnlyOnLabelKey = "On",
                            SettingsGeneralGroupReadOnlyOffLabelKey = "Off",
                            SettingsGeneralGroupReadOnlyTagModel = new SettingsGeneralGroupReadOnlyTagModel()
                            {
                                SettingsGeneralGroupReadOnlyTag = "EM13.14"
                            },
                            SettingsGeneralGroupReadOnlyOffIcon = GetIconSource("LightBulbOff"),
                            SettingsGeneralGroupReadOnlyOnIcon = GetIconSource("LightBulbOn"),
                        },
                        new SettingsGeneralGroupReadOnlyModel
                        {
                            SettingsGeneralGroupReadOnlyKey = "Shim_Loaded",
                            SettingsGeneralGroupReadOnlyOnLabelKey = "On",
                            SettingsGeneralGroupReadOnlyOffLabelKey = "Off",
                            SettingsGeneralGroupReadOnlyTagModel = new SettingsGeneralGroupReadOnlyTagModel()
                            {
                                SettingsGeneralGroupReadOnlyTag = "EM13.15"
                            },
                            SettingsGeneralGroupReadOnlyOffIcon = GetIconSource("LightBulbOff"),
                            SettingsGeneralGroupReadOnlyOnIcon = GetIconSource("LightBulbOn"),
                        },
                    }
                });



                SerializeSettingsGeneral(settingsGeneralModel);
            }

            settingsGeneralModel = XmlTools.Deserialize<SettingsGeneralModel>(
                FilePath.Settings);

            return settingsGeneralModel;
        }

        public static List<string> GetSettingsGeneralTagList()
        {
            SettingsGeneralModel settingsGeneralModel = DeserializeSettingsGeneral();
            List<string> settingsGeneralTagList = new List<string>();

            settingsGeneralTagList = settingsGeneralTagList.Union(
                settingsGeneralModel.SettingsGeneralGroupList.SelectMany(
                    x => x.SettingsGeneralGroupNumericList).Select(
                    x => x.SettingsGeneralGroupNumericTagModel).SelectMany(
                    x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                        y => y.GetValue(x).ToString()))).ToList();

            settingsGeneralTagList = settingsGeneralTagList.Union(
                settingsGeneralModel.SettingsGeneralGroupList.SelectMany(
                    x => x.SettingsGeneralGroupToggleList).Select(
                    x => x.SettingsGeneralGroupToggleTagModel).SelectMany(
                    x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                        y => y.GetValue(x).ToString()))).ToList();

            settingsGeneralTagList = settingsGeneralTagList.Union(
                settingsGeneralModel.SettingsGeneralGroupList.SelectMany(
                    x => x.SettingsGeneralGroupReadOnlyList).Select(
                    x => x.SettingsGeneralGroupReadOnlyTagModel).SelectMany(
                    x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                        y => y.GetValue(x).ToString()))).ToList();

            return settingsGeneralTagList;
        }

        public static string GetIconSource(string icon)
        {
            string iconSource = string.Empty;
            iconSource = $"{ResourceDictionarySource}Icons/{icon}.png";
            return iconSource;
        }
        #endregion
    }
}