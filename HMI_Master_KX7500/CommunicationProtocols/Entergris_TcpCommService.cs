using ControlzEx.Standard;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
//using HMI_Entegris.Views.LotEntryView;
using NLog;
using PQ.HMI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Threading;

namespace CAMLine
{
    public class Entergris_TcpCommService
    {
        #region PrivateFields
        //Logger
        //private static Logger tcpLogger = LogManager.GetLogger("TcpLogger");
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private List<FoupDataCamlineModel> foupDataList = new List<FoupDataCamlineModel>();
        public TcpIpClient tcpIpClient = null;
        //private string ipEndPoint = string.Empty;
        //private SiemensOPC siemensOPC = null;
        private SetupSettingModel _serverSettingModel = new SetupSettingModel();

        private KeyenceDll keyenceDll = null;
        #endregion

        #region Constructor
        public Entergris_TcpCommService()
        {
            keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, Global.PLCPort, DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());

            try
            {
                //Messenger.Default.Register<SiemensOPC>(this, initializeOPC);
                //Messenger.Default.Register<OPC>(this, opcUpdate);
                initialize();
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        #endregion

        #region PrivateMessenger
        //private void initializeOPC(SiemensOPC _siemensOPC)
        //{
        //    try
        //    {
        //        siemensOPC = _siemensOPC;
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}

        //private void opcUpdate(OPC opc)
        //{
        //    try
        //    {
        //        opc.DataChange.ToList().ForEach((Action<KeyValuePair<string, string>>)((KeyValuePair<string, string> x) =>
        //        {
        //        }));
        //    }
        //    catch (Exception ex) { logger.Error(ex); }
        //}
        #endregion

        #region PrivateEventsMethods
        private void tcpIpClient_OnConnectionStatus(string connectionStatus, string iP)
        {
            try
            {
                //MessageBox.Show($"{iP} : {connectionStatus}");
                FileLogger_Utilities.TCPIP_Log($"{iP}:{connectionStatus}");
                FileLogger_Utilities.TCPIP_Log("tcpIpClient_OnConnectionStatus");
                logger.Trace($"{iP} : {connectionStatus}");
                sendMainWindowMessage($"{iP}:{connectionStatus}");
            }
            catch (Exception ex) { logger.Error(ex); }
        }
        #endregion

        #region PrivateInitialization
        private void initialize()
        {
            try
            {
                LoadSettingService.OnUpdate_ServerSetting += new LoadSettingService.UpdateServerSetting(LoadServerSetting);
                LoadSettingService.LoadServerSetting();
                InitializeTcpIpClient();
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        public void InitializeTcpIpClient()
        {
            try
            {
                if (Global.IsOfflineMode)
                {
                    FileLogger_Utilities.TCPIP_Log("OfflineMode: TCP server connection skipped");
                    return;
                }

                if (!String.IsNullOrEmpty(_serverSettingModel.ServerIP) && !String.IsNullOrEmpty(_serverSettingModel.ServerPort))
                {
                    string serverIP_tryToConnect = string.Empty;
                    IPAddress ipAddress;
                    if (!IPAddress.TryParse(_serverSettingModel.ServerIP, out ipAddress))
                    {
                        ipAddress = Dns.GetHostEntry(_serverSettingModel.ServerIP).AddressList[0];
                        serverIP_tryToConnect = ipAddress.ToString();
                    }
                    else { serverIP_tryToConnect = ipAddress.ToString(); }

                    tcpIpClient = new TcpIpClient(serverIP_tryToConnect, _serverSettingModel.ServerPort);
                    tcpIpClient.DataReceived += TCP_Client_Received;
                    tcpIpClient.StatusUpdated += TCP_Client_StatusUpdated;
                    tcpIpClient.Connect();
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void TCP_Client_StatusUpdated(string _ip, string _status)
        {
            sendMainWindowMessage($"{_ip}:{_status}");
            FileLogger_Utilities.TCPIP_Log($"{_ip}:{_status}");
        }

        private void TCP_Client_Received(string _ip, string _message)
        {
            try
            {
                //trim the msg 1st since hmi did not know what the format tcpip side using to send response to hmi
                _message = _message.Trim();
                FileLogger_Utilities.TCPIP_Log($"Message After Trim : {_message}.");

                //send the _message to lotentryview 
                //Note: Removed since no requirement for explicit display
                //Messenger.Default.Send<Tuple<Entergris_TcpCommService, string>, LotEntryView_>(new Tuple<Entergris_TcpCommService, string>(this, _message));
                Messenger.Default.Send<Tuple<Entergris_TcpCommService, string>, LotEntryView>(new Tuple<Entergris_TcpCommService, string>(this, _message));

                Display_Message(_message, _ip, "Receive");

                if (_message == "OK")
                {
                    FileLogger_Utilities.TCPIP_Log($"OK. Message : {_message}.");
                    //siemensOPC.WriteTag(PLCTag.Start_Job_OK, true);
                    //FileLogger_Utilities.TCPIP_Log($"StartJobOK (For OK Msg) : {siemensOPC.ReadTag<bool>(PLCTag.Start_Job_OK)}.");
                    //siemensOPC.WriteTag(PLCTag.Start_Job_NOK, false);
                    //FileLogger_Utilities.TCPIP_Log($"StartJobNotOK (For OK Msg) : {siemensOPC.ReadTag<bool>(PLCTag.Start_Job_NOK)}.");

                    keyenceDll.Write<bool>(PLCTag.Start_Job_OK, true);
                    FileLogger_Utilities.TCPIP_Log($"StartJobOK (For OK Msg) : {keyenceDll.Read<bool>(PLCTag.Start_Job_OK)}.");
                    keyenceDll.Write<bool>(PLCTag.Start_Job_NOK, false);
                    FileLogger_Utilities.TCPIP_Log($"StartJobNotOK (For OK Msg) : {keyenceDll.Read<bool>(PLCTag.Start_Job_NOK)}.");
                }
                else
                {
                    FileLogger_Utilities.TCPIP_Log($"NOT OK. Message :{_message}");
                    //siemensOPC.WriteTag(PLCTag.Start_Job_OK, false);
                    //FileLogger_Utilities.TCPIP_Log($"StartJobOK (For Not OK Msg) : {siemensOPC.ReadTag<bool>(PLCTag.Start_Job_OK)}.");
                    //siemensOPC.WriteTag(PLCTag.Start_Job_NOK, true);
                    //FileLogger_Utilities.TCPIP_Log($"StartJobNotOK (For Not OK Msg) : {siemensOPC.ReadTag<bool>(PLCTag.Start_Job_NOK)}.");

                    keyenceDll.Write<bool>(PLCTag.Start_Job_OK, false);
                    FileLogger_Utilities.TCPIP_Log($"StartJobOK (For OK Msg) : {keyenceDll.Read<bool>(PLCTag.Start_Job_OK)}.");
                    keyenceDll.Write<bool>(PLCTag.Start_Job_NOK, true);
                    FileLogger_Utilities.TCPIP_Log($"StartJobNotOK (For OK Msg) : {keyenceDll.Read<bool>(PLCTag.Start_Job_NOK)}.");
                }
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        #endregion

        #region PrivateMethods
        public void sendMainWindowMessage(string _message)
        {
            try
            {
                FileLogger_Utilities.TCPIP_Log($"Message : {_message}");
                //Messenger.Default.Send(new Tuple<Entergris_TcpCommService, string>(this, $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} : {_message}"));
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void Display_Message(string _message, string _address, string _mode)
        {
            try
            {
                FileLogger_Utilities.TCPIP_Log($"{DateTime.Now:HH:MM:ss.ffff} {_address} {_mode}: {_message}\n");
                //_message = _message.Trim();
                //textBox.Text = $"{textBox.Text}{DateTime.Now:HH:MM:ss.ffff} {_address} {_mode}: {_message}\n";
                //textBox.ScrollToEnd();
            }
            catch (Exception ex) { logger.Error(ex); }
        }

        private void LoadServerSetting(SetupSettingModel serverSettingModel)
        {
            this._serverSettingModel = serverSettingModel;
        }
        #endregion
    }
}
