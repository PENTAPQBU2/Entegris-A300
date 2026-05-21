using CsvHelper;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model.OEE.Data;
using HMI_Master_KX7500.Model.OEE.Data.Count;
using HMI_Master_KX7500.Model.OEE.Data.ReadOnly;
using HMI_Master_KX7500.Model.OEE.Data.Time;
using HMI_Master_KX7500.Model.OEE.HourlyUPH;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace HMI_Master_KX7500.Model.OEE
{
    public enum OEEUpdate
    {
        Uph
    }

    public class OEEService
    {
        #region PrivateFields
        private static SecondsToTimeConverter secondsConverter = new SecondsToTimeConverter();
        #endregion PrivateFields

        #region PublicFields

        public const string IsInDesignMode = "IsInDesignMode";
        public const string ShortDateFormat = "yyyy-MM-dd";

        #endregion PublicFields

        #region Constructor

        //public OEEService()
        //{
        //    initializeMessenger();
        //}

        #endregion Constructor

        #region PrivateInitializeMethods

        //private void initializeMessenger()
        //{
        //    Messenger.Default.Register<OEEInitializeModel>(this, initializeOEE);
        //}

        #endregion PrivateInitializeMethods

        #region PrivateMessengerMethods

        //private void initializeOEE(OEEInitializeModel _oEEInitializeModel)
        //{
        //    oEEInitializeModel = _oEEInitializeModel;
        //}

        #endregion PrivateMessengerMethods

        #region PublicMethods

        public static bool SerializeOEE(OEEModel oEEModel)
        {
            bool value = false;

            XmlTools.Serialize(FilePath.OEE, oEEModel);

            value = true;

            return value;
        }

        public static OEEModel DeserializeOEESiemensOPC()
        {
            OEEModel oEEModel = new OEEModel();

            if(!File.Exists(FilePath.OEE))
            {
                int oEEHourlyUphNumber = 0;
                string oEEHourlyUphAbbreviation = string.Empty;
                int OEEHourStartID = 1200;
                for(int i = 0; i < 24; i++)
                {
                    oEEHourlyUphNumber = i == 0 ? 12 : i < 13 ? i : i - 12;
                    oEEHourlyUphAbbreviation = i < 12 ? "am" : "pm";

                    oEEModel.OEEHourlyUphList.Add(new OEEHourlyUphModel
                    {
                        OEEHourlyUphKey = $"{oEEHourlyUphNumber}{oEEHourlyUphAbbreviation}",
                        OEEHourlyUphTagModel = new OEEHourlyUphTagModel
                        {
                            OEEHourlyUphTag = $"EM{OEEHourStartID + (i * 2)}",
                            OEEHourlyUphTagType = "DINT"
                        }
                    });
                    ;
                }

                string oEEDataKey = string.Empty;

                for(int i = 0; i <= 1; i++)
                {
                    oEEDataKey = i == 0 ? "LotOEE" : "ShiftOEE";
                    //oEEDataDataBlockNumber = i == 0 ? 1050 : 1051;

                    oEEModel.OEEDataList.Add(new OEEDataModel
                    {
                        OEEDataKey = oEEDataKey,
                        OEECsvFolderPath = $@"{FolderPath.OEE}{oEEDataKey}\",
                        OEEDataTagModel = new OEEDataTagModel
                        {
                            OEEGenerateCsvTag = ""
                        },

                        OEETimeList = new ObservableCollection<OEETimeModel>
                                {
                                    new OEETimeModel
                                    {
                                        OEETimeKey = "SystemUpTime",
                                        OEETimeTagModel = new OEETimeTagModel
                                        {
                                            OEETimeTag = oEEDataKey = i == 0 ? "EM1782" : "EM1076",
                                            OEETimeTagType = "DINT"
                                        }
                                    },
                                    new OEETimeModel
                                    {
                                        OEETimeKey = "OperationTime",
                                        OEETimeTagModel = new OEETimeTagModel
                                        {
                                           OEETimeTag = oEEDataKey = i == 0 ? "EM1738" : "EM1032",
                                            OEETimeTagType = "DINT"
                                        }
                                    },
                                    new OEETimeModel
                                    {
                                        OEETimeKey = "DownTime",
                                        OEETimeTagModel = new OEETimeTagModel
                                        {
                                            OEETimeTag = oEEDataKey = i == 0 ? "EM1740" : "EM1034",
                                            OEETimeTagType = "DINT"
                                        }
                                    },
                                    new OEETimeModel
                                    {
                                        OEETimeKey = "IdleTime",
                                        OEETimeTagModel = new OEETimeTagModel
                                        {
                                             OEETimeTag = oEEDataKey = i == 0 ? "EM1742" : "EM1036",
                                            OEETimeTagType = "DINT"
                                        }
                                    },
                                    new OEETimeModel
                                    {
                                        OEETimeKey = "MTBF",
                                        OEETimeTagModel = new OEETimeTagModel
                                        {
                                             OEETimeTag = oEEDataKey = i == 0 ? "EM1750" : "EM1044",
                                            OEETimeTagType = "DINT"
                                        }
                                    },
                                    new OEETimeModel
                                    {
                                        OEETimeKey = "MTBA",
                                        OEETimeTagModel = new OEETimeTagModel
                                        {
                                           OEETimeTag = oEEDataKey = i == 0 ? "EM1748" : "EM1042",
                                            OEETimeTagType = "DINT"
                                        }
                                    }
                                },
                        OEECountList = new ObservableCollection<OEECountModel>
                                {
                                    new OEECountModel
                                    {
                                        OEECountKey = "TotalQuantityIn",
                                        OEECountTagModel = new OEECountTagModel
                                        {
                                            OEECountTag = oEEDataKey = i == 0 ? "EM1767" : "EM1058",
                                            OEECountTagType = "DINT"
                                        }
                                    },
                                    new OEECountModel
                                    {
                                        OEECountKey = "TotalQuantityOut",
                                        OEECountTagModel = new OEECountTagModel
                                        {
                                             OEECountTag = oEEDataKey = i == 0 ? "EM1763" : "EM1054",
                                            OEECountTagType = "DINT"
                                        }
                                    },
                                    new OEECountModel
                                    {
                                        OEECountKey = "TotalPassed",
                                        OEECountTagModel = new OEECountTagModel
                                        {
                                            OEECountTag = oEEDataKey = i == 0 ? "EM1759" : "EM1050",
                                            OEECountTagType = "DINT"
                                        }
                                    },
                                    new OEECountModel
                                    {
                                        OEECountKey = "TotalFailed",
                                        OEECountTagModel = new OEECountTagModel
                                        {
                                            OEECountTag = oEEDataKey = i == 0 ? "EM1761" : "EM1052",
                                            OEECountTagType = "DINT"
                                        }
                                    },
                                    new OEECountModel
                                    {
                                        OEECountKey = "TotalYield",
                                        OEEMinimumCount = 0,
                                        OEEMaximumCount = 100,
                                        OEECountDecimalPlaces = 2,
                                        OEECountTagModel = new OEECountTagModel
                                        {
                                            OEECountTag = oEEDataKey = i == 0 ? "EM1765" : "EM1056",
                                            OEECountTagType = "DINT"
                                        }
                                    },
                                    new OEECountModel
                                    {
                                        OEECountKey = "SoftJam",
                                        OEECountTagModel = new OEECountTagModel
                                        {
                                             OEECountTag = oEEDataKey = i == 0 ? "EM1756" : "EM1060",
                                            OEECountTagType = "INT"
                                        }
                                    },
                                    new OEECountModel
                                    {
                                        OEECountKey = "HardJam",
                                        OEECountTagModel = new OEECountTagModel
                                        {
                                             OEECountTag = oEEDataKey = i == 0 ? "EM1757" : "EM1061",
                                            OEECountTagType = "INT"
                                        }
                                    },
                                }
                    });

                    if(i == 0)
                    {
                        //int oEELotDataBlockNumber = 850;

                        oEEModel.OEEDataList[i].OEEEnableMultipleCsv = true;

                        oEEModel.OEEDataList[i].OEEReadOnlyList.Insert(0, new OEEReadOnlyModel
                        {
                            OEEReadOnlyKey = "LotID",
                            OEEReadOnlyTagModel = new OEEReadOnlyTagModel
                            {
                                OEEReadOnlyTag = "EM1501",
                                OEEReadOnlyTagType = "STRING[50]"
                            }
                        });

                        oEEModel.OEEDataList[i].OEEReadOnlyList.Insert(0, new OEEReadOnlyModel
                        {
                            OEEReadOnlyKey = "Operator_ID",
                            OEEReadOnlyTagModel = new OEEReadOnlyTagModel
                            {
                                OEEReadOnlyTag = "EM1553",
                                OEEReadOnlyTagType = "STRING[50]"
                            }
                        });
                    }
                    else if(i == 1)
                    {
                        //int oEELotDataBlockNumber = 1051;

                        //oEEModel.OEEDataList[i].OEEReadOnlyList.Insert(0, new OEEReadOnlyModel
                        //{
                        //    OEEReadOnlyKey = "Shift",
                        //    OEEReadOnlyTagModel = new OEEReadOnlyTagModel
                        //    {
                        //        OEEReadOnlyTag = ""
                        //    }
                        //});

                        //oEEModel.OEEDataList[i].OEEReadOnlyList.Insert(0, new OEEReadOnlyModel
                        //{
                        //    OEEReadOnlyKey = "Product Type",
                        //    OEEReadOnlyTagModel = new OEEReadOnlyTagModel
                        //    {
                        //        OEEReadOnlyTag = SiemensOPCHelper.GetStringTag(
                        //            oEES7Connection, 1050, 184, 30)
                        //    }
                        //});
                    }
                }

                SerializeOEE(oEEModel);
            }

            oEEModel = XmlTools.Deserialize<OEEModel>(FilePath.OEE, oEEModel);
            return oEEModel;
        }

        public static List<string> GetOEETagList()
        {
            var oEEModel = DeserializeOEESiemensOPC();
            List<string> oEETagList = new List<string>();

            oEETagList = oEEModel.OEEHourlyUphList.Select(x => x.OEEHourlyUphTagModel).SelectMany(
                    x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                        y => y.GetValue(x).ToString())).ToList();

            oEETagList = oEETagList.Union(oEEModel.OEEDataList.Select(x => x.OEEDataTagModel).SelectMany(
                x => x.GetType().GetProperties().Where(y => y.Name != IsInDesignMode).Select(
                    y => y.GetValue(x).ToString()))).ToList();

            oEETagList = oEETagList.Union(oEEModel.OEEDataList.SelectMany(x => x.OEEReadOnlyList).Select(
                x => x.OEEReadOnlyTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                    y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

            oEETagList = oEETagList.Union(oEEModel.OEEDataList.SelectMany(x => x.OEETimeList).Select(
                x => x.OEETimeTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                    y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

            oEETagList = oEETagList.Union(oEEModel.OEEDataList.SelectMany(x => x.OEECountList).Select(
                x => x.OEECountTagModel).SelectMany(x => x.GetType().GetProperties().Where(
                    y => y.Name != IsInDesignMode).Select(y => y.GetValue(x).ToString()))).ToList();

            return oEETagList;
        }

        public static Dictionary<string, string> GetOEETagDictionary()
        {
            var oEEModel = DeserializeOEESiemensOPC();
            Dictionary<string, string> oEETagList = new Dictionary<string, string>();

            oEETagList = oEEModel.OEEHourlyUphList.Select(x => x.OEEHourlyUphTagModel).ToDictionary(y => y.OEEHourlyUphTag, y => y.OEEHourlyUphTagType);

            //oEETagList = oEETagList.Concat(oEEModel.OEEDataList.Select(x => x.OEEDataTagModel).ToDictionary(y => y.OEEGenerateCsvTag, y => y.OEEGenerateCsvTagType))
            //    .ToDictionary(z => z.Key, z => z.Value);

            oEETagList = oEETagList.Concat(oEEModel.OEEDataList.SelectMany(x => x.OEEReadOnlyList).Select(
                x => x.OEEReadOnlyTagModel).ToDictionary(y => y.OEEReadOnlyTag, y => y.OEEReadOnlyTagType))
                .ToDictionary(z => z.Key, z => z.Value);

            oEETagList = oEETagList.Concat(oEEModel.OEEDataList.SelectMany(x => x.OEETimeList).Select(
                x => x.OEETimeTagModel).ToDictionary(y => y.OEETimeTag, y => y.OEETimeTagType))
                .ToDictionary(z => z.Key, z => z.Value);

            oEETagList = oEETagList.Concat(oEEModel.OEEDataList.SelectMany(x => x.OEECountList).Select(
                x => x.OEECountTagModel).ToDictionary(y => y.OEECountTag, y => y.OEECountTagType))
                .ToDictionary(z => z.Key, z => z.Value);

            return oEETagList;
        }

        public static bool GenerateOEECsv(ObservableCollection<OEEDataModel> oEEDataList, string oEEGenerateCsvTag)
        {
            bool generateOEECsvDone = false;

            if(oEEDataList.ToList().Exists(x => x.OEEDataTagModel.OEEGenerateCsvTag == oEEGenerateCsvTag))
            {
                OEEDataModel oEEDataModel = oEEDataList.ToList().Find(
                    x => x.OEEDataTagModel.OEEGenerateCsvTag == oEEGenerateCsvTag);

                string date = DateTime.Now.ToString(ShortDateFormat);

                string folderPath = $@"{oEEDataModel.OEECsvFolderPath}{date}\";

                if(!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                bool firstOEECsv = true;

                do
                {
                    generateOEECsvDone = false;

                    string key = firstOEECsv ? oEEDataModel.OEEDataKey : oEEDataModel.OEEReadOnlyList[0].OEEReadOnly;

                    string filePath = $"{folderPath}{key}_{date}.csv";

                    bool fileExists = File.Exists(filePath);

                    using(FileStream fileStream = new FileStream(filePath,
                        FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using(StreamWriter streamWriter = new StreamWriter(fileStream))
                    using(CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        if(!fileExists)
                        {
                            oEEDataModel.OEEReadOnlyList.ToList().ForEach(x =>
                            {
                                csvWriter.WriteField(x.OEEReadOnlyKey);
                            });

                            oEEDataModel.OEETimeList.ToList().ForEach(x =>
                            {
                                csvWriter.WriteField(x.OEETimeKey);
                            });

                            oEEDataModel.OEECountList.ToList().ForEach(x =>
                            {
                                csvWriter.WriteField(x.OEECountKey);
                            });

                            csvWriter.NextRecord();
                        }

                        oEEDataModel.OEEReadOnlyList.ToList().ForEach(x =>
                        {
                            csvWriter.WriteField(x.OEEReadOnly);
                        });

                        oEEDataModel.OEETimeList.ToList().ForEach(x =>
                        {
                            csvWriter.WriteField(secondsConverter.Convert(x.OEETime, null, null, null));
                        });

                        oEEDataModel.OEECountList.ToList().ForEach(x =>
                        {
                            csvWriter.WriteField(x.OEECount);
                        });

                        csvWriter.NextRecord();
                    }

                    firstOEECsv = !firstOEECsv;

                    generateOEECsvDone = true;
                } while(firstOEECsv == false && oEEDataModel.OEEEnableMultipleCsv == true);
            }

            return generateOEECsvDone;
        }

        #endregion PublicMethods
    }
}