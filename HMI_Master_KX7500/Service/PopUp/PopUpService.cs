using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Initialize;
using HMI_Master_KX7500.Model.PopUp;
using HMI_Master_KX7500.Model.PopUp.Action;
//using HMI_Master_KX7500.OPCUA;
using NLog;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using HMI_Master_KX7500.MachinePath;

namespace HMI_Master_KX7500.Service.PopUp
{
    public class PopUpService
    {
        #region PrivateFields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static PopUpInitializeModel popUpInitializeModel = new PopUpInitializeModel();
        #endregion

        #region PublicFields
        public static string IsInDesignMode = "IsInDesignMode";
        #endregion

        #region Constructor
        public PopUpService()
        {
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeMessenger()
        {
            Messenger.Default.Register<PopUpInitializeModel>(this, initializePopUpList);
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializePopUpList(PopUpInitializeModel _popUpInitializeModel)
        {
            popUpInitializeModel = _popUpInitializeModel;
        }
        #endregion

        #region PrivateMethods
        private static void MessageBoxShowCannotBeNull(string message)
        {
            try
            {
                MessageBox.Show($"{message} Cannot Be Null Or Empty");
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        #endregion

        #region PublicMethods
        public static bool SerializePopUpList(ObservableCollection<PopUpModel> popUpList)
        {
            bool value = false;

            try
            {
                XmlTools.Serialize(popUpInitializeModel.PopUpFilePath, popUpList);

                value = true;

                Messenger.Default.Send(popUpInitializeModel);
            }
            catch (Exception ex) { logger.Error(ex); }
            return value;
        }

        public static ObservableCollection<PopUpModel> DeserializePopUpList()
        {
            ObservableCollection<PopUpModel> popUpList = new ObservableCollection<PopUpModel>();

            if (!File.Exists(popUpInitializeModel.PopUpFilePath))
            {
                //Grey
                popUpList.Add(new PopUpModel
                {
                    PopUpMessageKey = "HMI_Prompt_Load_Shell_Handle",
                    PopUpTagModel = new PopUpTagModel
                    {
                        PopUpTag = "MR1500"
                    },
                    PopUpEnableAutoClose = true,
                    PopUpValueTag = "EM3100",
                    PopUpValueTagLength = 100,
                });
                popUpList.Add(new PopUpModel
                {
                    PopUpMessageKey = "HMI_Prompt_Remov_Side_Handle_Jig",
                    PopUpTagModel = new PopUpTagModel
                    {
                        PopUpTag = "MR1609"
                    },
                    PopUpEnableAutoClose = true,
                    PopUpValueTag = "EM4100",
                    PopUpValueTagLength = 100,
                });
                popUpList.Add(new PopUpModel
                {
                    PopUpMessageKey = "HMI_Prompt_Remove_Side_Column",
                    PopUpTagModel = new PopUpTagModel
                    {
                        PopUpTag = "MR1610"
                    },
                    PopUpEnableAutoClose = true,
                    PopUpValueTag = "EM4160",
                    PopUpValueTagLength = 100,
                });
                popUpList.Add(new PopUpModel
                {
                    PopUpMessageKey = "HMI_Prompt_Load_Convyr_Plate",
                    PopUpTagModel = new PopUpTagModel
                    {
                        PopUpTag = "MR1501"
                    },
                    PopUpEnableAutoClose = true,
                    PopUpValueTag = "EM3160",
                    PopUpValueTagLength = 100
                });
                popUpList.Add(new PopUpModel
                {
                    PopUpMessageKey = "HMI_Prompt_Load_Side_Col",
                    PopUpTagModel = new PopUpTagModel
                    {
                        PopUpTag = "MR1502"
                    },
                    PopUpEnableAutoClose = true,
                    PopUpValueTag = "EM3220",
                    PopUpValueTagLength = 100
                });
                popUpList.Add(new PopUpModel
                {
                    PopUpMessageKey = "HMI_Prompt_Unload_Shell",
                    PopUpTagModel = new PopUpTagModel
                    {
                        PopUpTag = "MR1506"
                    },
                    PopUpEnableAutoClose = true,
                    PopUpValueTag = "EM3460",
                    PopUpValueTagLength = 100
                });
                popUpList.Add(new PopUpModel
                {
                    PopUpMessageKey = "HMI_Prompt_Remove_AKHandle",
                    PopUpTagModel = new PopUpTagModel
                    {
                        PopUpTag = "MR1508"
                    },
                    PopUpEnableAutoClose = true,
                    PopUpValueTag = "EM3580",
                    PopUpValueTagLength = 100
                });
                popUpList.Add(new PopUpModel
                {
                    PopUpMessageKey = "HMI_Prompt_Load_Side_Handle_Jig",
                    PopUpTagModel = new PopUpTagModel
                    {
                        PopUpTag = "MR1512"
                    },
                    PopUpEnableAutoClose = true,
                    PopUpValueTag = "EM3760",
                    PopUpValueTagLength = 100
                });
                //Refill
                //popUpList.Add(new PopUpModel
                //{
                //    PopUpMessageKey = "HMI_Prompt_Refill_22mm_Screw",
                //    PopUpTagModel = new PopUpTagModel
                //    {
                //        PopUpTag = "MR1509"
                //    },
                //    PopUpActionList = new ObservableCollection<PopUpActionModel>
                //    {
                //        new PopUpActionModel
                //        {
                //            PopUpActionKey = "Bowl2_Refill_Done",
                //            PopUpActionTagModel = new PopUpActionTagModel
                //            {
                //                PopUpActionTag = "MR1608"
                //            },
                //        },
                //    },
                //    PopUpEnableAutoClose = true,
                //    PopUpValueTag = "EM3820",
                //    PopUpValueTagLength = 100
                //});
                //popUpList.Add(new PopUpModel
                //{
                //    PopUpMessageKey = "HMI_Prompt_Refill_12mm_Screw",
                //    PopUpTagModel = new PopUpTagModel
                //    {
                //        PopUpTag = "MR1510"
                //    },
                //    PopUpActionList = new ObservableCollection<PopUpActionModel>
                //    {
                //        new PopUpActionModel
                //        {
                //            PopUpActionKey = "Bowl1_Refill_Done",
                //            PopUpActionTagModel = new PopUpActionTagModel
                //            {
                //                PopUpActionTag = "MR1605"
                //            },
                //        },
                //    },
                //    PopUpEnableAutoClose = true,
                //    PopUpValueTag = "EM3640",
                //    PopUpValueTagLength = 100
                //});

                //Red
                popUpList.Add(new PopUpModel
                {
                    PopUpMessageKey = "HMI_Prompt_Shim_Oring_Failed",
                    PopUpTagModel = new PopUpTagModel
                    {
                        PopUpTag = "MR1505"
                    },
                    PopUpActionList = new ObservableCollection<PopUpActionModel>
                    {
                        new PopUpActionModel
                        {
                            PopUpActionKey = "Retry",
                            PopUpActionTagModel = new PopUpActionTagModel
                            {
                                PopUpActionTag = "MR1602"
                            }
                        },
                        new PopUpActionModel
                        {
                            PopUpActionKey = "Remove",
                            PopUpActionTagModel = new PopUpActionTagModel
                            {
                                PopUpActionTag = "MR1601"
                            }
                        },
                    },
                    PopUpEnableAutoClose = true,
                    PopUpValueTag = "EM3400",
                    PopUpValueTagLength = 254
                });
                popUpList.Add(new PopUpModel
                {
                    PopUpMessageKey = "Screw Failed",
                    PopUpTagModel = new PopUpTagModel
                    {
                        PopUpTag = "MR1503"
                    },
                    PopUpActionList = new ObservableCollection<PopUpActionModel>
                    {
                        new PopUpActionModel
                        {
                            PopUpActionKey = "Remove",
                            PopUpActionTagModel = new PopUpActionTagModel
                            {
                                PopUpActionTag = "MR1601"
                            }
                        },
                    },
                    PopUpEnableAutoClose = true,
                    PopUpValueTag = "EM3280",
                    PopUpValueTagLength = 254
                });
                popUpList.Add(new PopUpModel
                {
                    PopUpMessageKey = "HMI_Prompt_Screw_Feed_Failed",
                    PopUpTagModel = new PopUpTagModel
                    {
                        PopUpTag = "MR1507"
                    },
                    PopUpActionList = new ObservableCollection<PopUpActionModel>
                    {
                        new PopUpActionModel
                        {
                            PopUpActionKey = "HMI_Reply_Retry_Screw_Feed",
                            PopUpActionTagModel = new PopUpActionTagModel
                            {
                                PopUpActionTag = "MR1604"
                            }
                        },
                        new PopUpActionModel
                        {
                            PopUpActionKey = "Remove",
                            PopUpActionTagModel = new PopUpActionTagModel
                            {
                                PopUpActionTag = "MR1601"
                            }
                        },
                    },
                    PopUpEnableAutoClose = true,
                    PopUpValueTag = "EM3520",
                    PopUpValueTagLength = 254
                });
                popUpList.Add(new PopUpModel
                {
                    PopUpMessageKey = "HMI_Prompt_Shell_Choice",
                    PopUpTagModel = new PopUpTagModel
                    {
                        PopUpTag = "MR1513"
                    },
                    PopUpActionList = new ObservableCollection<PopUpActionModel>
                    {
                        new PopUpActionModel
                        {
                            PopUpActionKey = "New",
                            PopUpActionTagModel = new PopUpActionTagModel
                            {
                                PopUpActionTag = "MR4507"
                            }
                        },
                        new PopUpActionModel
                        {
                            PopUpActionKey = "Rework",
                            PopUpActionTagModel = new PopUpActionTagModel
                            {
                                PopUpActionTag = "MR4508"
                            }
                        },
                    },
                    PopUpEnableAutoClose = true,
                    PopUpValueTag = "EM3880",
                    PopUpValueTagLength = 100
                });
                popUpList.Add(new PopUpModel
                {
                    PopUpMessageKey = "HMI_Prompt_Rework_Component",
                    PopUpTagModel = new PopUpTagModel
                    {
                        PopUpTag = "MR1514"
                    },
                    PopUpActionList = new ObservableCollection<PopUpActionModel>
                    {
                        new PopUpActionModel
                        {
                            PopUpActionKey = "Rework_Conveyor_Plate",
                            PopUpActionTagModel = new PopUpActionTagModel
                            {
                                PopUpActionTag = "MR4509"
                            }
                        },
                        new PopUpActionModel
                        {
                            PopUpActionKey = "Rework_Side_Column",
                            PopUpActionTagModel = new PopUpActionTagModel
                            {
                                PopUpActionTag = "MR4510"
                            }
                        },
                    },
                    PopUpEnableAutoClose = true,
                    PopUpValueTag = "EM3940",
                    PopUpValueTagLength = 100
                });
                SerializePopUpList(popUpList);
            }
            popUpList = XmlTools.Deserialize<ObservableCollection<PopUpModel>>(popUpInitializeModel.PopUpFilePath, popUpList);
            return popUpList;
        }

        public static List<string> GetPopUpTagList()
        {
            var popUpList = DeserializePopUpList();
            List<string> popUpTagList = new List<string>();

            popUpTagList = popUpList.Select(x => x.PopUpTagModel).SelectMany(
                x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                    y => y.GetValue(x).ToString())).ToList();
            popUpTagList = popUpTagList.Union(popUpList.SelectMany(x => x.PopUpActionList).Select(
                x => x.PopUpActionTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                    y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();
            popUpList.ToList().ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.PopUpValueTag))
                    popUpTagList.Add(x.PopUpValueTag);
            });

            return popUpTagList;
        }
        #endregion
    }
}
