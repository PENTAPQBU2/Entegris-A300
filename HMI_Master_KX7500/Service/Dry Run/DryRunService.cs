using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.DryRun.Action;
using HMI_Master_KX7500.Model.DryRun.Count;
using HMI_Master_KX7500.Model.DryRun.Time;
using PQ.HMI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace HMI_Master_KX7500.Model.DryRun
{
    public class DryRunService
    {
        #region PrivateFields
        //private static DryRunInitializeModel dryRunInitializeModel = new DryRunInitializeModel();
        #endregion

        #region PublicFields
        public const string IsInDesignMode = "IsInDesignMode";
        public const string ShortDateFormat = "yyyy-MM-dd";

        public static List<DryRunModel> dryRunList = new List<DryRunModel>();
        #endregion

        #region Constructor
        //public DryRunService()
        //{
        //    initializeMessenger();
        //}
        #endregion

        #region PrivateInitializeMethods
        //private void initializeMessenger()
        //{
        //    Messenger.Default.Register<DryRunInitializeModel>(this, initializeDryRunList);
        //}
        #endregion

        #region PrivateMessengerMethods
        //private void initializeDryRunList(DryRunInitializeModel _dryRunInitializeModel)
        //{
        //    dryRunInitializeModel = _dryRunInitializeModel;
        //}
        #endregion

        #region PublicMethods
        public static bool SerializeDryRunList(List<DryRunModel> dryRunList)
        {
            bool value = false;

            //dryRunInitializeModel.DryRunFilePath;

            XmlTools.Serialize(FilePath.DryRun, dryRunList);

            value = true;

            //Messenger.Default.Send(dryRunInitializeModel);

            return value;
         }

        public static List<DryRunModel> DeserializeDryRunList()
        {
            //List<DryRunModel> dryRunList = new List<DryRunModel>();

            if (!File.Exists(FilePath.DryRun))
            {
                int enableDryRunValue = 3908; //4000 - 92
                int dryRunNameValue = 2980; //3000 - 20
                int dryRunStartValue = 3909; //4001 - 92
                int dryRunResetValue = 3910; //4002 - 92
                int dryRun_SoftJamCountValue = 2992; //3012 - 20
                int dryRun_HardJamCountValue = 2993; //3013 - 20
                int dryRun_RunTimeValue = 2994; //3014 - 20
                int dryRun_IdlingTimeValue = 2996; //3016 - 20
                int dryRun_DownTimeValue = 2998; //3018 - 20

                for (int i = 1; i <= 10; i++)
                {
                    if (i % 2 == 0)
                    {
                        enableDryRunValue += 8;
                    }
                    else
                    {
                        enableDryRunValue += 92;
                    }
                    string enableDryRunNode = $"MR{enableDryRunValue}";
                    string dryRunName = $"DM{dryRunNameValue + (i * 20)}";
                    if (i % 2 == 0)
                    {
                        dryRunStartValue += 8;
                    }
                    else
                    {
                        dryRunStartValue += 92;
                    }
                    string dryRunStart = $"MR{dryRunStartValue}";
                    if (i % 2 == 0)
                    {
                        dryRunResetValue += 8;
                    }
                    else
                    {
                        dryRunResetValue += 92;
                    }
                    string dryRunReset = $"MR{dryRunResetValue}";
                    string dryRun_SoftJamCount = $"DM{dryRun_SoftJamCountValue + (i * 20)}";
                    string dryRun_HardJamCount = $"DM{dryRun_HardJamCountValue + (i * 20)}";
                    string dryRun_RunTime = $"DM{dryRun_RunTimeValue + (i * 20)}";
                    string dryRun_IdlingTime = $"DM{dryRun_IdlingTimeValue + (i * 20)}";
                    string dryRun_DownTime = $"DM{dryRun_DownTimeValue + (i * 20)}";

                    dryRunList.Add(new DryRunModel
                    {
                        DryRunName = dryRunName,
                        DryRunTagModel = new DryRunTagModel
                        {
                            DryRunEnableTag = enableDryRunNode,
                            DryRunEnableTagType = "BOOL",
                            //DryRunNameTag = dryRunName,
                            //DryRunNameTagType = "STRING",
                            DryRunStartTag = dryRunStart,
                            DryRunStartTagType = "BOOL"
                        },
                        DryRunTimeList = new ObservableCollection<DryRunTimeModel>
                                {
                                    new DryRunTimeModel
                                    {
                                        DryRunTimeKey = "RunTime",
                                        DryRunTimeTagModel = new DryRunTimeTagModel
                                        {
                                            DryRunTimeTag = dryRun_RunTime,
                                            //DryRunTimeTagType = "INT"
                                        }
                                    },
                                    new DryRunTimeModel
                                    {
                                        DryRunTimeKey = "IdleTime",
                                        DryRunTimeTagModel = new DryRunTimeTagModel
                                        {
                                            DryRunTimeTag = dryRun_IdlingTime
                                        }
                                    },
                                    new DryRunTimeModel
                                    {
                                        DryRunTimeKey = "DownTime",
                                        DryRunTimeTagModel = new DryRunTimeTagModel
                                        {
                                            DryRunTimeTag = dryRun_DownTime
                                        }
                                    }
                                },
                        DryRunCountList = new ObservableCollection<DryRunCountModel>
                                {
                                    new DryRunCountModel
                                    {
                                        DryRunCountKey = "SoftJam",
                                        DryRunCountTagModel = new DryRunCountTagModel
                                        {
                                            DryRunCountTag = dryRun_SoftJamCount
                                        }
                                    },
                                    new DryRunCountModel
                                    {
                                        DryRunCountKey = "HardJam",
                                        DryRunCountTagModel = new DryRunCountTagModel
                                        {
                                            DryRunCountTag = dryRun_HardJamCount
                                        }
                                    }
                                },
                        DryRunActionList = new ObservableCollection<DryRunActionModel>
                                {
                                    new DryRunActionModel
                                    {
                                        DryRunActionKey = "Reset",
                                        DryRunActionIcon = "pack://application:,,,/HMI_SSDC_PIM;component/Icon/Reload.png",
                                        DryRunActionTagModel = new DryRunActionTagModel
                                        {
                                            DryRunActionTag = dryRunReset
                                        }
                                    }
                                }
                    });
                }

                SerializeDryRunList(dryRunList);
            }

            dryRunList = XmlTools.Deserialize<List<DryRunModel>>(FilePath.DryRun, dryRunList);

            return dryRunList;
        }

        public static List<string> GetDryRunTagList()
        {
            var dryRunList = DeserializeDryRunList();
            List<string> dryRunTagList = new List<string>();

            dryRunTagList = dryRunList.Select(x => x.DryRunTagModel).SelectMany(
                   x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                       y => y.GetValue(x).ToString())).ToList();

            dryRunTagList = dryRunTagList.Union(dryRunList.SelectMany(x => x.DryRunTimeList).Select(
                x => x.DryRunTimeTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                    y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

            dryRunTagList = dryRunTagList.Union(dryRunList.SelectMany(x => x.DryRunCountList).Select(
                x => x.DryRunCountTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                    y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

            dryRunTagList = dryRunTagList.Union(dryRunList.SelectMany(x => x.DryRunActionList).Select(
                x => x.DryRunActionTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                    y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

            return dryRunTagList;
        }

        public static Dictionary<string, string> GetDryRunTagDictionary()
        {
            var dryRunList = DeserializeDryRunList();
            Dictionary<string, string> dryRunTagList = new Dictionary<string, string>();

            dryRunTagList = dryRunList.Select(x => x.DryRunTagModel).ToDictionary(y => y.DryRunEnableTag, y => y.DryRunEnableTagType).ToDictionary(z => z.Key, z => z.Value);
            dryRunTagList = dryRunTagList.Concat(dryRunList.Select(x => x.DryRunTagModel).ToDictionary(y => y.DryRunNameTag, y => y.DryRunNameTagType).ToDictionary(z => z.Key, z => z.Value)).ToDictionary(z => z.Key, z => z.Value);
            dryRunTagList = dryRunTagList.Concat(dryRunList.Select(x => x.DryRunTagModel).ToDictionary(y => y.DryRunStartTag, y => y.DryRunStartTagType).ToDictionary(z => z.Key, z => z.Value)).ToDictionary(z => z.Key, z => z.Value);

            //Tags redundant?

            return dryRunTagList;
        }
        
        #endregion
    }
}
