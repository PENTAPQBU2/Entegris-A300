using CsvHelper;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Models;
using HMI_Master_KX7500.Modules.Frame;
//using HMI_Master_KX7500.OPCUA;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

namespace HMI_Master_KX7500.Services
{
    public class CycleTimeService
    {
        #region PrivateFields
        //private static CycleTimeInitializeModel cycleTimeInitializeModel = new CycleTimeInitializeModel();
        private static string IsInDesignMode = "IsInDesignMode";
        public static string ShortDateFormat = "yyyy-MM-dd";
        public static string LongDateTimeFormat = "yyyy-MM-dd HH:mm:ss.ffff";
        #endregion

        #region Constructor
        //public CycleTimeService()
        //{
        //    initializeMessenger();
        //}
        #endregion

        #region PrivateInitializeMethods
        //private void initializeMessenger()
        //{
        //    Messenger.Default.Register<CycleTimeInitializeModel>(this, initializeCycleTime);
        //}
        #endregion

        #region PrivateMessengerMethods
        //private void initializeCycleTime(CycleTimeInitializeModel _cycleTimeInitializeModel)
        //{
        //    cycleTimeInitializeModel = _cycleTimeInitializeModel;
        //}
        #endregion

        #region PublicMethods
        public static bool SerializeCycleTime(CycleTimeMainModel cycleTimeMainModel)
        {
            bool value = false;

            if (cycleTimeMainModel.CycleTimeList.Any(x => string.IsNullOrEmpty(
                x.CycleTimeTagModel.CycleTimeEnableTag)))
            {
                MessageBox.Show($"Value Cannot Be Null/Empty {nameof(CycleTimeTagModel.CycleTimeEnableTag)}");
            }
            else if (cycleTimeMainModel.CycleTimeList.Any(x => string.IsNullOrEmpty(
                x.CycleTimeTagModel.CycleTimeNameTag)))
            {
                MessageBox.Show($"Value Cannot Be Null/Empty {nameof(CycleTimeTagModel.CycleTimeNameTag)}");
            }
            else if (cycleTimeMainModel.CycleTimeList.Any(x => string.IsNullOrEmpty(
                x.CycleTimeTagModel.CycleTimeCaptureTag)))
            {
                MessageBox.Show($"Value Cannot Be Null/Empty {nameof(CycleTimeTagModel.CycleTimeCaptureTag)}");
            }
            else if (cycleTimeMainModel.CycleTimeList.SelectMany(x => x.CycleTimeNumericStatusList).Any(
                x => string.IsNullOrEmpty(x.CycleTimeNumericStatusKey)))
            {
                MessageBox.Show($"Value Cannot Be Null/Empty {nameof(CycleTimeNumericStatusModel.CycleTimeNumericStatusKey)}");
            }
            else if (cycleTimeMainModel.CycleTimeList.SelectMany(x => x.CycleTimeNumericStatusList).Any(
                x => string.IsNullOrEmpty(x.CycleTimeNumericStatusTagModel.CycleTimeNumericStatusTag)))
            {
                MessageBox.Show($"Value Cannot Be Null/Empty {nameof(CycleTimeNumericStatusTagModel.CycleTimeNumericStatusTag)}");
            }
            else if (cycleTimeMainModel.CycleTimeList.SelectMany(
                x => x.CycleTimeHistoryModel.CycleTimeHistoryTypeList).Any(
                x => string.IsNullOrEmpty(x.CycleTimeHistoryTypeKey)))
            {
                MessageBox.Show($"Value Cannot Be Null/Empty {nameof(CycleTimeHistoryTypeModel.CycleTimeHistoryTypeKey)}");
            }
            else if (cycleTimeMainModel.CycleTimeList.SelectMany(
                x => x.CycleTimeHistoryModel.CycleTimeHistoryTypeList).SelectMany(
                x => x.CycleTimeHistoryTypeTimerList).Any(
                x => string.IsNullOrEmpty(x.CycleTimeHistoryTypeTimerTagModel.CycleTimeHistoryTypeTimerTag)))
            {
                MessageBox.Show($"Value Cannot Be Null/Empty {nameof(CycleTimeHistoryTypeTimerTagModel.CycleTimeHistoryTypeTimerTag)}");
            }
            else if (cycleTimeMainModel.CycleTimeList.SelectMany(
                x => x.CycleTimeHistoryModel.CycleTimeHistoryLabelList).Any(
                x => string.IsNullOrEmpty(x.CycleTimeHistoryLabelKey)))
            {

                MessageBox.Show($"Value Cannot Be Null/Empty {nameof(CycleTimeHistoryLabelModel.CycleTimeHistoryLabelKey)}");
            }
            else
            {
                XmlTools.Serialize(FilePath.CycleTime, cycleTimeMainModel);

                value = true;

                //Messenger.Default.Send(cycleTimeInitializeModel);
            }
            return value;
        }

        public static CycleTimeMainModel DeserializeCycleTime()
        {
            CycleTimeMainModel cycleTimeMainModel = new CycleTimeMainModel();
            string cycleModuleName = string.Empty;

            if (!File.Exists(FilePath.CycleTime))
            {
                //for (int i = 1; i < 11; i++)
                //{
                    List<CycleTimeHistoryTypeTimerModel> cycleTimeHistoryTypeTimerList = new List<CycleTimeHistoryTypeTimerModel>();
                    List<CycleTimeHistoryLabelModel> cycleTimeHistoryLabelList = new List<CycleTimeHistoryLabelModel>();
                    //cycleModuleName = $"CycleTime_{i}";
                    cycleModuleName = $"CycleTime_1";

                    for (int j = 0; j < 11; j++)
                    {
                        int cycleTimeValue = 3504;

                        cycleTimeHistoryTypeTimerList.Add(new CycleTimeHistoryTypeTimerModel
                        {
                            CycleTimeHistoryTypeTimerTagModel = new CycleTimeHistoryTypeTimerTagModel
                            {
                                CycleTimeHistoryTypeTimerTag = $"DM{cycleTimeValue + (j * 2)}"
                            },
                        });

                        cycleTimeHistoryLabelList.Add(new CycleTimeHistoryLabelModel
                        {
                            CycleTimeHistoryLabelKey = j.ToString()
                        });
                    }

                    cycleTimeMainModel.CycleTimeList.Add(new CycleTimeModel
                    {
                        CycleTimeTagModel = new CycleTimeTagModel
                        {
                            CycleTimeEnableTag = "MR3500",
                            CycleTimeNameTag = "DM3548",
                            CycleTimeCaptureTag = "MR3501",
                            CycleTimeStartTag = "MR3502",
                        },
                        CycleTimeNumericStatusList = new List<CycleTimeNumericStatusModel>
                                {
                                    new CycleTimeNumericStatusModel
                                    {
                                        CycleTimeNumericStatusKey = "CurrentTimer",
                                        CycleTimeNumericStatusDecimalPlaces = 3,
                                        CycleTimeNumericStatusTagModel = new CycleTimeNumericStatusTagModel
                                        {
                                            CycleTimeNumericStatusTag =  "DM3500"
                                        }
                                    },
                                    new CycleTimeNumericStatusModel
                                    {
                                        CycleTimeNumericStatusKey = "LastTimer",
                                        CycleTimeNumericStatusDecimalPlaces = 3,
                                        CycleTimeNumericStatusTagModel = new CycleTimeNumericStatusTagModel
                                        {
                                            CycleTimeNumericStatusTag = "DM3502"
                                        }
                                    }
                                },
                        CycleTimeHistoryModel = new CycleTimeHistoryModel
                        {
                            CycleTimeHistoryTypeList = new List<CycleTimeHistoryTypeModel>
                                    {
                                        new CycleTimeHistoryTypeModel
                                        {
                                            CycleTimeHistoryTypeKey = "LastHistory",
                                            CycleTimeHistoryTypeTimerList = cycleTimeHistoryTypeTimerList
                                        }
                                    },
                            CycleTimeHistoryLabelList = cycleTimeHistoryLabelList
                        }
                    });
                //}

                SerializeCycleTime(cycleTimeMainModel);
            }

            cycleTimeMainModel = XmlTools.Deserialize<CycleTimeMainModel>(
                FilePath.CycleTime);

            return cycleTimeMainModel;
        }

        public static List<string> GetCycleTimeTagList(/*CycleTimeMainModel cycleTimeMainModel*/)
        {
            CycleTimeMainModel cycleTimeMainModel = DeserializeCycleTime();
            List<string> cycleTimeTagList = new List<string>();

            cycleTimeTagList = cycleTimeMainModel.CycleTimeList.Select(x => x.CycleTimeTagModel).SelectMany(
                    x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode &&
                    !string.IsNullOrEmpty(y.GetValue(x).ToString())).Select(y => y.GetValue(x).ToString())).ToList();

            cycleTimeTagList = cycleTimeTagList.Union(cycleTimeMainModel.CycleTimeList.SelectMany(
                x => x.CycleTimeNumericStatusList).Select(x => x.CycleTimeNumericStatusTagModel).SelectMany(
                x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode &&
                !string.IsNullOrEmpty(y.GetValue(x).ToString())).Select(y => y.GetValue(x).ToString()))).ToList();

            cycleTimeTagList = cycleTimeTagList.Union(cycleTimeMainModel.CycleTimeList.SelectMany(
                x => x.CycleTimeHistoryModel.CycleTimeHistoryTypeList).SelectMany(
                x => x.CycleTimeHistoryTypeTimerList).Select(x => x.CycleTimeHistoryTypeTimerTagModel).SelectMany(
                x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode &&
                !string.IsNullOrEmpty(y.GetValue(x).ToString())).Select(y => y.GetValue(x).ToString()))).ToList();

            return cycleTimeTagList;
        }

        public static bool GenerateCycleTimeCsv(CycleTimeMainModel cycleTimeMainModel, string cycleTimeCaptureTag)
        {
            bool value = false;

            if (cycleTimeMainModel.CycleTimeList.Exists(
                x => x.CycleTimeTagModel.CycleTimeCaptureTag == cycleTimeCaptureTag))
            {
                string date = DateTime.Now.ToString(ShortDateFormat);

                string folderPath = $@"{FolderPath.Logs}{date}\";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = $"{folderPath}{nameof(FrameView.UserNameCycleTime)}" + $"_{date}.csv";

                bool fileExists = File.Exists(filePath);

                using (FileStream fileStream = new FileStream(filePath,
                    FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                using (CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    CycleTimeModel cycleTimeModel = cycleTimeMainModel.CycleTimeList.Find(
                        x => x.CycleTimeTagModel.CycleTimeCaptureTag == cycleTimeCaptureTag);

                    if (!fileExists)
                    {
                        csvWriter.WriteField(nameof(DateTime));
                        csvWriter.WriteField(nameof(CycleTimeModel.CycleTimeName));

                        for (int i = 1; i <= cycleTimeModel.CycleTimeHistoryModel.CycleTimeHistoryLabelList.Count; i++)
                        {
                            csvWriter.WriteField(i);
                        }

                        csvWriter.NextRecord();
                    }

                    cycleTimeModel.CycleTimeHistoryModel.CycleTimeHistoryTypeList.ForEach(x =>
                    {
                        csvWriter.WriteField(DateTime.Now.ToString(LongDateTimeFormat));
                        csvWriter.WriteField($"{cycleTimeModel.CycleTimeKey}" + $"_{x.CycleTimeHistoryTypeKey}");

                        x.CycleTimeHistoryTypeTimerList.ForEach(y =>
                        {
                            csvWriter.WriteField(y.CycleTimeHistoryTypeTimer);
                        });

                        csvWriter.NextRecord();
                    });

                    value = true;
                }
            }
            return value;
        }
        #endregion
    }
}