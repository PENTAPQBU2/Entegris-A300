using HMI_Master_KX7500.Modules.Alarm;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace HMI_Master_KX7500
{
    /// <summary>
    /// Interaction logic for AlarmModule.xaml
    /// </summary>
    public partial class AlarmModule : UserControl
    {
        #region PrivateFields

        private Logger logger = LogManager.GetCurrentClassLogger();
        private List<string> CurAlarmList = new List<string>();

        #endregion PrivateFields

        #region Construtor

        public AlarmModule()
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception ex) { logger.Error(ex); }
        }

        #endregion Construtor

        private Dictionary<string, string> Dic_Code_Time = new Dictionary<string, string>();

        public void Define(List<AlarmMessageModel> AlarmList)
        {
            try
            {
                Transitioner_Obj.Items.Clear();
                List<string> Temp = new List<string>();
                foreach(AlarmMessageModel Alarm in AlarmList)
                {
                    Temp.Add(Alarm.Code);
                    if(!CurAlarmList.Contains(Alarm.Code))
                    {
                        logger.Log(LogLevel.Debug, $"New alarm, Alarm Code: {Alarm.Code}");
                        AlarmLogger.AlarmLog(Alarm.Code, Alarm.Type, Alarm.Message);
                        var alarmHistoryView = this.Parent as AlarmHistoryView;
                    }

                    if(Dic_Code_Time.ContainsKey(Alarm.Code))
                    { Alarm.Time = Dic_Code_Time[Alarm.Code]; }

                    AlarmMessageModule AMM = new AlarmMessageModule();
                    AMM.Time = Alarm.Time;
                    AMM.Code = Alarm.Code;
                    AMM.Type = Alarm.Type;
                    AMM.Message = Alarm.Message;
                    AMM.Action = Alarm.Action;
                    Transitioner_Obj.Items.Add(AMM);
                }
                Transitioner_Obj.SelectedIndex = 0;
                CurAlarmList = Temp;
            }
            catch(Exception ex) { logger.Error(ex); }
        }
    }
}