using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.OPCUA;
using NLog;
using Opc.Ua;
using Opc.Ua.Client;

namespace HMI_Master_KX7500
{
    public struct _OPC
    {
        public List<string> TagList;
        public Dictionary<string, string> Datachange;
        public List<OPCModules> Module;
    }

    public class OPCUA_
    {
        #region PrivateFields
        private static Logger opcLogger = LogManager.GetLogger("OPC");
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private UAClientHelperAPI m_Server = null;
        private Subscription m_Subscription = null;
        private UInt16 m_NameSpaceIndex = 0;
        private List<string> m_NodeIdList = null;
        private _OPC m_OPC;
        public _OPC OPC => m_OPC;

        #endregion

        #region Constructor | Destructor
        public OPCUA_()
        {

        }
        /// <summary>
        /// Disconnect from the UA server.
        /// </summary>
        ~OPCUA_()
        {
            try
            {
                if (m_Server != null && m_Subscription != null)
                {
                    m_Server.RemoveSubscription(m_Subscription);
                    m_Subscription = null;
                    m_Server.Disconnect();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Disconnect failed:\n\n" + ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// Connect to the UA server and read the namespace table.
        /// The connect is based on the Server URL entered in _url
        /// </summary>
        public async void InitializeUAOPC(string _url, List<string> _addressList, List<OPCModules> module, string _nameSpace = @"urn:KEYENCE:PLC:OpcUaServer")
        {
            try
            {
//#if !DEBUG
                if (string.IsNullOrEmpty(_url))
                    return;
                logger.Debug("Start To " + nameof(InitializeUAOPC));
                m_Server = new UAClientHelperAPI();
                m_Server.CertificateValidationNotification += new CertificateValidationEventHandler(m_server_CertificateEvent);
                //m_Server.Connect(_url, "none", MessageSecurityMode.None, false, "", "");
                await m_Server.ConnectAnonymousAsync(_url);
                m_NodeIdList = _addressList.Distinct().ToList();

                initializeNodes(_nameSpace);
                initializeM_OPC(_addressList, module);
                await Task.Delay(150);
                await Task.Run(() => subscribeNodeID(_addressList));
                //logger.Debug("Done " + nameof(InitializeUAOPC));
                foreach (var m in module)
                {
                    opcLogger.Debug($"Done {nameof(InitializeUAOPC)}, Module : {m}");
                }
//#endif
                ReadAllAddresses();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                // debug
                // ksfoong
                throw ex;
            }
        }

        public void ReadAllAddresses()
        {
            try
            {
                if (m_Server != null && m_OPC.TagList != null)
                {
                    m_OPC.TagList.ForEach(async (x) =>
                    {
                        try
                        {
                            Read<string>(x);
                        }
                        catch (Exception e)
                        {
                            opcLogger.Error($"{e.Message}, Address : {x}");
                        }
                    });

                    Messenger.Default.Send(m_OPC);
                }
            }
            catch (Exception ex) { opcLogger.Error(ex); throw ex; }
        }


        /// <summary>
        /// Reads the values.
        /// The NodeIds used for the Read are constructed from the variable entered
        /// in the _address and the namespace index detected in the connect method
        /// </summary>
        public T Read<T>(string _address)
        {
            try
            {
                if (m_OPC.Datachange != null && m_OPC.Datachange.ContainsKey(_address) && m_OPC.Datachange[_address] != null)
                {
                    if (m_OPC.Datachange[_address] == null)
                    {
                        return default(T);
                    }
                    return (T)Convert.ChangeType(m_OPC.Datachange[_address], typeof(T));
                }
                else if (m_Server != null)
                {
                    object result = m_Server.ReadValue(new NodeId(_address, m_NameSpaceIndex).ToString());
                    OPCUA_.opcLogger.Trace($"INITIALIZE READ: {_address} ===> {result}");

                    if (result == null)
                    {
                        return default(T);
                    }
                    return (T)Convert.ChangeType(result, typeof(T));
                }

            }
            catch (Exception ex)
            {
                OPCUA_.logger.Error($"{_address} {ex}");
                throw ex;
            }

            return default(T);
        }        

        public void Write(string _address, object _value)
        {
            Task.Run(() => {
                try
                {
                    List<string> nodesToWrite = new List<string>();
                    List<string> values = new List<string>();

                    nodesToWrite.Add(new NodeId(_address, m_NameSpaceIndex).ToString());
                    values.Add(_value.ToString());

                    OPCUA_.opcLogger.Trace($"WRITE : {_address} ===> {_value}");
                    m_Server.WriteValues(values, nodesToWrite);
#if !DEBUG
#endif
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    OPCUA_.logger.Error($"{_address}, {_value} : Writing new value failed: " + ex.Message + " Address is not exist");
                    throw ex;
                }
            });
        }

        public void Write_(string _address, Variant _value)
        {
            //use this one 
            Task.Run(() => {
                try
                {
                    List<string> nodesToWrite = new List<string>();
                    List<string> values = new List<string>();

                    nodesToWrite.Add(new NodeId(_address, m_NameSpaceIndex).ToString());
                    values.Add(_value.ToString());

                    OPCUA_.opcLogger.Trace($"WRITE : {_address} ===> {_value}");
                    m_Server.WriteValues(values, nodesToWrite);
#if !DEBUG
#endif
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    OPCUA_.logger.Error($"{_address}, {_value} : Writing new value failed: " + ex.Message + " Address is not exist");
                    throw ex;
                }
            });
        }

        public List<string> GetAddresses()
        {
            return m_NodeIdList;
        }

#region PrivateEventMethods
        private void m_server_CertificateEvent(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            e.Accept = true;
        }

        /// <summary>
        /// Callback method for data changes from the monitored variables.
        /// </summary>
        private void ClientApi_ValueChanged(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            try
            {

                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (notification == null)
                {
                    return;
                }

                if (m_NodeIdList.Contains(monitoredItem.DisplayName) &&
                    !notification.Value.WrappedValue.ToString().Contains("null"))
                {
                    m_OPC.Datachange[monitoredItem.DisplayName] = notification.Value.WrappedValue.ToString();
                    opcLogger.Trace($"ClientApi_ValueChanged: {monitoredItem.DisplayName} ===> {notification.Value.WrappedValue} ");

                    Messenger.Default.Send(new _OPC
                    {
                        TagList = m_OPC.TagList,
                        Module = m_OPC.Module,
                        Datachange = new Dictionary<string, string>
                        { { monitoredItem.DisplayName, notification.Value.WrappedValue.ToString()} }

                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error(monitoredItem.DisplayName + ": " +
                    "Unexpected error in the data change callback: " + ex.Message);
            }
        }
#endregion

#region PrivateMethods
        private void initializeNodes(string _nameSpace)
        {
            try
            {
                if (m_Server != null)
                {
                    List<string> nodesToRead = new List<string>();
                    List<string> results = new List<string>();

                    nodesToRead.Add("ns=0;i=" + Variables.Server_NamespaceArray.ToString());

                    results = m_Server.ReadValues(nodesToRead);

                    if (results.Count != 1)
                    {
                        throw new Exception("Reading namespace table returned unexpected result");
                    }
                    // Try to find the namespace URI entered by the user
                    string[] nameSpaceArray = results[0].Split(';');

                    m_NameSpaceIndex = (ushort)Array.IndexOf(nameSpaceArray, _nameSpace);

                    if (m_NameSpaceIndex == 0)
                    {
                        throw new Exception("Namespace " + _nameSpace + " not found in server namespace table");
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        private void initializeM_OPC(List<string> _nodeIdAddressList, List<OPCModules> module)
        {
            if (m_Server != null && m_NameSpaceIndex != 0)
            {
                // Create OPC
                m_OPC.TagList = _nodeIdAddressList;
                m_OPC.Datachange = new Dictionary<string, string>();
                m_OPC.Module = module;

                m_NodeIdList.ForEach(x => {
                    m_OPC.Datachange.Add(x, null);
                });
            }
        }

        public void Disconnect()
        {
            m_Server.Disconnect();
        }

        /// <summary>
        /// Starts the monitoring of the values of the addressList.
        /// The NodeIds used for the monitoring are constructed from the address
        /// and the namespace index detected in the connect method
        /// </summary>
        private void subscribeNodeID(List<string> _addressList)
        {
            try
            {
                if (m_Server != null && m_NameSpaceIndex != 0)
                {

                    // Create subscription
                    m_Subscription = m_Server.Subscribe(100);
                    // LEK 20220416 add Subscript setting here
                    /*
                      DisplayNameTB.Text          = subscription.DisplayName;
                PublishingIntervalNC.Value  = subscription.Created ? (decimal)subscription.CurrentPublishingInterval : (decimal)subscription.PublishingInterval;
                KeepAliveCountNC.Value      = subscription.Created ? subscription.CurrentKeepAliveCount : subscription.KeepAliveCount;
                LifetimeCountCTRL.Value     = subscription.Created ? subscription.CurrentLifetimeCount: subscription.LifetimeCount;
                MaxNotificationsCTRL.Value  = subscription.MaxNotificationsPerPublish;
                PriorityNC.Value            = subscription.Created ? subscription.CurrentPriority : subscription.Priority;
                PublishingEnabledCK.Checked = subscription.Created ? subscription.CurrentPublishingEnabled : subscription.PublishingEnabled;

                if (ShowDialog() != DialogResult.OK)
                {
                    return false;
                }

                subscription.DisplayName                = DisplayNameTB.Text;
                subscription.PublishingInterval         = (int)PublishingIntervalNC.Value;
                subscription.KeepAliveCount             = (uint)KeepAliveCountNC.Value;
                subscription.LifetimeCount              = (uint)LifetimeCountCTRL.Value;
                subscription.MaxNotificationsPerPublish = (uint)MaxNotificationsCTRL.Value;
                subscription.Priority                   = (byte)PriorityNC.Value;
                if (subscription.Created)
                {
                    subscription.SetPublishingMode(PublishingEnabledCK.Checked);
                }
                else
                {
                    subscription.PublishingEnabled = PublishingEnabledCK.Checked;
                }
                     */
                    m_Server.ItemChangedNotification += new MonitoredItemNotificationEventHandler(ClientApi_ValueChanged);

                    m_NodeIdList.ForEach(x => {
                        try
                        {
                            m_Server.AddMonitoredItem(m_Subscription, new NodeId(x, m_NameSpaceIndex).ToString(), x, 50);
                        }
                        catch (Exception ex) { logger.Error($"{x} : {ex}"); }
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error("Establishing data monitoring failed:  " + ex.Message);
            }
        }
#endregion
    }
}
