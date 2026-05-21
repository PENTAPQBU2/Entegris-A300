using HMI_Master_KX7500.Model.Alarm;
using NLog;
using Pentamaster.HMI.Tools;
using System;
using System.Collections.Generic;
using System.Data;

namespace HMI_Master_KX7500.Services
{
    public class AlarmV1Service
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private MdbTool mdbTool = new MdbTool();

        public AlarmV1Service(string filePath)
        {
            //IL_0001: Unknown result type (might be due to invalid IL or missing references)
            //IL_000b: Expected O, but got Unknown
            try
            {
                mdbTool.CreateConnection(filePath);
            }
            catch(Exception ex)
            {
                logger.Error<Exception>(ex);
            }
        }

        public bool AlarmV1ModelTableExists()
        {
            try
            {
                return mdbTool.IsTableExists("AlarmV1Model");
            }
            catch(Exception ex)
            {
                logger.Error<Exception>(ex);
                return false;
            }
        }

        public void CreateAlarmV1ModelTable()
        {
            try
            {
                mdbTool.CreateTable("AlarmV1Model", "[AlarmType] MEMO, [AlarmModule] MEMO, [AlarmCode] MEMO, [AlarmMessageEnglish] MEMO, [AlarmActionEnglish] MEMO, [AlarmMessageChinese] MEMO, [AlarmActionChinese] MEMO");
            }
            catch(Exception ex)
            {
                logger.Error<Exception>(ex);
            }
        }

        public void DropAlarmV1ModelTable()
        {
            try
            {
                mdbTool.DeleteTable("AlarmV1Model");
            }
            catch(Exception ex)
            {
                logger.Error<Exception>(ex);
            }
        }

        public void InsertAlarmV1Model(AlarmType alarmType, string alarmModule, string alarmCode, string alarmMessageEnglish, string alarmActionEnglish, string alarmMessageChinese, string alarmActionChinese)
        {
            try
            {
                mdbTool.InsertQuery("AlarmV1Model", "AlarmType, AlarmModule, AlarmCode, AlarmMessageEnglish, AlarmActionEnglish, AlarmMessageChinese, AlarmActionChinese", $"'{alarmType}', '{alarmModule}', '{alarmCode}', '{alarmMessageEnglish}', '{alarmActionEnglish}'," + " '" + alarmMessageChinese + "', '" + alarmActionChinese + "'");
            }
            catch(Exception ex)
            {
                logger.Error<Exception>(ex);
            }
        }

        public List<AlarmV1Model> SelectAlarmV1Models()
        {
            try
            {
                DataTable dataTable = mdbTool.SelectQuery("AlarmV1Model");
                List<AlarmV1Model> list = new List<AlarmV1Model>();
                for(int i = 0; i < dataTable.Rows.Count; i++)
                {
                    list.Add(new AlarmV1Model
                    {
                        AlarmType = ((!(dataTable.Rows[i]["AlarmType"].ToString() == "Error")) ? AlarmType.Warning : AlarmType.Error),
                        AlarmModule = dataTable.Rows[i]["AlarmModule"].ToString(),
                        AlarmCode = dataTable.Rows[i]["AlarmCode"].ToString(),
                        AlarmMessageEnglish = dataTable.Rows[i]["AlarmMessageEnglish"].ToString(),
                        AlarmActionEnglish = dataTable.Rows[i]["AlarmActionEnglish"].ToString(),
                        AlarmMessageChinese = dataTable.Rows[i]["AlarmMessageChinese"].ToString(),
                        AlarmActionChinese = dataTable.Rows[i]["AlarmActionChinese"].ToString()
                    });
                }
                return list;
            }
            catch(Exception ex)
            {
                logger.Error<Exception>(ex);
                return null;
            }
        }
    }
}