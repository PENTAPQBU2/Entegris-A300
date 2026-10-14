using DATABUILDERAXLibLB;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Models.KeyencePLC;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace HMI_Master_KX7500.CommunicationProtocols
{
    public class KeyenceDll
    {
        #region PrivateFields

        private Dictionary<string, string> KeyencePLCAddressList = new Dictionary<string, string>();
        private readonly List<KeyencePLCTagModel> KeyenceDllValueList = new List<KeyencePLCTagModel>();

        //Created only when talking to a real PLC, so Offline mode also works on a PC without KV DataBuilder installed
        private DBCommManager DBCommManager1 = null;
        private DBTriggerManager DBTriggerManager1 = null;

        //Instances created before MachineConfig.xml is loaded connect on first Read/Write instead
        private bool deferredConnect = false;
        private DBPlcId deferredPLCModel;
        private readonly object connectLock = new object();

        //Offline mode: one simulated PLC memory shared by all instances
        private static readonly ConcurrentDictionary<string, object> simulatedMemory = new ConcurrentDictionary<string, object>();

        private Thread th_ReadPLCValue;
        private bool bolLoopReadPLC = false;
        private List<OPCModules> _Module = new List<OPCModules>();
        public List<OPCModules> Module => _Module;

        #endregion PrivateFields

        #region Constructor

        public KeyenceDll()
        {
            MethodHelper.Initialize(GetType().Name, GetType().Name, () =>
            {
                if(!Global.IsOfflineMode)
                {
                    createComObjects();
                }
            });
        }

        ~KeyenceDll()
        {
            bolLoopReadPLC = false;
            try
            {
                if(DBTriggerManager1 != null) { DBTriggerManager1.Active = false; }
                if(DBCommManager1 != null) { DBCommManager1.Disconnect(); }
            }
            catch (Exception ex)
            {
                throw new Exception("Disconnect failed:\n\n" + ex.Message);
            }
        }

        public KeyenceDll(Dictionary<string, string> _KeyencePLCAddressList, string PLCIPAddress, string PLCPort, DBPlcId _PLCModel, List<OPCModules> Module)
        {
            //Task.Run(() =>
            //{
            MethodHelper.Initialize(GetType().Name, GetType().Name, () =>
            {
                LogHelper.KeyValue(GetType().Name, nameof(_KeyencePLCAddressList), _KeyencePLCAddressList.Count);

                KeyencePLCAddressList = _KeyencePLCAddressList;
                LogHelper.KeyValue(GetType().Name, nameof(KeyencePLCAddressList), KeyencePLCAddressList.Count);

                if(!Global.MachineConfigLoaded)
                {
                    //Offline/IP settings are not known yet. Connect on first Read/Write using Global.OPCUA_Url and Global.PLCPort.
                    deferredConnect = true;
                    deferredPLCModel = _PLCModel;
                    return;
                }

                if(Global.IsOfflineMode)
                {
                    LogHelper.KeyValue(GetType().Name, "OfflineMode", "Simulated PLC, not connecting");
                    return;
                }

                createComObjects();
                DBCommManager1.KVSimulator = Global.MachineConfigModel.UseKVStudioSimulator;
                DBCommManager1.PLC = _PLCModel; //DBPlcId.DBPLC_DKV8K;
                DBCommManager1.Peer = PLCIPAddress + ":" + PLCPort;
                DBTriggerManager1 = new DBTriggerManager();

                //if(KeyencePLCAddressList.Count <= 0)
                //{
                //    return;
                //}

                //KeyencePLCAddressList.Where(x => !string.IsNullOrEmpty(x.Key)).Distinct().ToList().ForEach(x =>
                //{
                //    MethodHelper.Action(GetType().Name, () =>
                //    {
                //        KeyenceDllValueList.Add(new KeyencePLCTagModel
                //        {
                //            KeyencePLCTag = x.Key,
                //            KeyencePLCValue = string.Empty,
                //            KeyencePLCStrType = x.Value,
                //            KeyencePLCType = ConvertTypetoPLCDevice(x.Key, x.Value)
                //        });
                //    });
                //});

                // LEK 20220326, Need to check first is it need to use
                YaskawaMEMOBUS.IniEngine();

                connect();

                //th_ReadPLCValue = new Thread(new ThreadStart(LoopReadPLC));
                //bolLoopReadPLC = true;
                //th_ReadPLCValue.Start();

                //LEK read 1 set value init
                //asyncReading
            });
            //});
        }

        #endregion Constructor

        #region PrivateMethods

        private void connect()
        {
            try
            {
                LogHelper.StartTo(GetType().Name, nameof(connect));
                connectKVStudio(ref DBCommManager1, ref DBTriggerManager1, ref HMI_KeyenceGlobal.G_DisabledPLC1);

                // Initial PLC Address

                LogHelper.Done(GetType().Name, nameof(connect));
            }
            catch (Exception exception)
            {
                LogHelper.FailTo(GetType().Name, nameof(connect));

                throw new Exception($"Error while creating group object:-{exception.Message} Result - Create Server.");
            }
        }

        public void Disconnect()
        {
            try
            {
                if(DBTriggerManager1 != null) { DBTriggerManager1.Active = false; }
                if(DBCommManager1 != null) { DBCommManager1.Disconnect(); }
            }
            catch (Exception ex)
            {
                throw new Exception("Disconnect failed:\n\n" + ex.Message);
            }
        }

        private void createComObjects()
        {
            if(DBCommManager1 == null) { DBCommManager1 = new DBCommManager(); }
            if(DBTriggerManager1 == null) { DBTriggerManager1 = new DBTriggerManager(); }
        }

        //Completes a connection that was postponed because MachineConfig.xml was not loaded when this instance was created
        private void ensureDeferredConnect()
        {
            if(!deferredConnect || !Global.MachineConfigLoaded)
            {
                return;
            }

            lock(connectLock)
            {
                if(!deferredConnect)
                {
                    return;
                }
                deferredConnect = false;

                if(Global.IsOfflineMode)
                {
                    return;
                }

                MethodHelper.Initialize(GetType().Name, nameof(ensureDeferredConnect), () =>
                {
                    createComObjects();
                    DBCommManager1.KVSimulator = Global.MachineConfigModel.UseKVStudioSimulator;
                    DBCommManager1.PLC = deferredPLCModel;
                    DBCommManager1.Peer = Global.OPCUA_Url + ":" + Global.PLCPort;
                    DBTriggerManager1 = new DBTriggerManager();
                    YaskawaMEMOBUS.IniEngine();
                    connect();
                });
            }
        }

        private static string simulatedKey(string raw_address)
        {
            return (raw_address ?? string.Empty).Trim().ToUpperInvariant();
        }

        private static T readSimulated<T>(string raw_address)
        {
            object value;
            if(simulatedMemory.TryGetValue(simulatedKey(raw_address), out value) && value != null)
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch(Exception) { }
            }

            if(typeof(T) == typeof(string))
            {
                return (T)(object)string.Empty;
            }
            return default(T);
        }

        //Offline mode only: set a simulated PLC value, e.g. SetSimulatedValue("EM103", (short)2)
        public static void SetSimulatedValue(string raw_address, object value)
        {
            simulatedMemory[simulatedKey(raw_address)] = value;
        }

        //Offline mode only: copy of all simulated PLC values written so far
        public static Dictionary<string, object> GetSimulatedValues()
        {
            return simulatedMemory.ToDictionary(x => x.Key, x => x.Value);
        }

        private void connectKVStudio(ref DBCommManager CommManager, ref DBTriggerManager TriggerManager, ref bool PLCDisabled)
        {
            try
            {
                //CommManager.KVSimulator = true;
                CommManager.Connect();
                TriggerManager.Active = true;
                // lblVersionHMI.Text = Me.ProductVersion
                //lblVersionPLC.Text = ReaddeviceD(5230) + "." + ReaddeviceD(5231) + "." + ReaddeviceD(5232) + "." + ReaddeviceD(5233);
                //rbV1connection.BackColor = Color.Lime;
            }
            catch (Exception ex)
            {
                PLCDisabled = true;
                LogHelper.FailTo(GetType().Name, nameof(connectKVStudio));
                throw new Exception($"Fail to connect controller. Please check network communcation between PC and controller. Error code: " + "0" + ex.Message);
            }
        }

        private DBPlcDevice ConvertTypetoPLCDevice(string Address, string strType)
        {
            string Var_Type = "";
            string Var_Type2 = "";

            Var_Type = Address.Substring(0, 2);//Strings.Left(address, 2);
            Var_Type2 = Address.Substring(0, 3);//Strings.Left(address, 3);

            if (DBCommManager1.PLC == DBPlcId.DBPLC_DKV7K)
            {
                if (Var_Type == "DM")
                { return DBPlcDevice.DKV7K_DM; }
                else if (Var_Type == "R")
                { return DBPlcDevice.DKV7K_RLY_B; }
                else if (Var_Type == "B")
                { return DBPlcDevice.DKV7K_B; }
                else if (Var_Type == "MR")
                { return DBPlcDevice.DKV7K_MR; }
                else if (Var_Type == "EM")
                { return DBPlcDevice.DKV7K_EM; }
            }
            else if (DBCommManager1.PLC == DBPlcId.DBPLC_DKV8K)
            {
                if (Var_Type == "DM")
                { return DBPlcDevice.DKV8K_DM; }
                else if (Var_Type == "R")
                { return DBPlcDevice.DKV8K_RLY_B; }
                else if (Var_Type == "B")
                { return DBPlcDevice.DKV8K_B; }
                else if (Var_Type == "MR")
                { return DBPlcDevice.DKV8K_MR; }
                else if (Var_Type == "EM")
                { return DBPlcDevice.DKV8K_EM; }
            }
            return DBPlcDevice.DKV7K_EM;
        }

        #endregion PrivateMethods

        #region PublicMethods


        #endregion PublicMethods    

        #region PrivateEventMethods

        //private void LoopReadPLC()
        //{
        //    string str_Result = "";
        //    while (bolLoopReadPLC)
        //    {
        //        if (KeyenceDllValueList != null)
        //        {
        //            KeyenceDllValueList.ForEach(x =>
        //            {
        //                try
        //                {
        //                    if (DBCommManager1.PLC == DBPlcId.DBPLC_DKV7K)
        //                    {
        //                        //str_Result = Read7KDevice(x.KeyencePLCTag, x.KeyencePLCStrType, x.KeyencePLCType);
        //                        if (str_Result != x.KeyencePLCValue)
        //                        {
        //                            Messenger.Default.Send(new _OPC
        //                            {
        //                                Module = _Module,
        //                                DataChange = new Dictionary<string, string>
        //                                { { x.KeyencePLCTag, str_Result} }
        //                            });
        //                            x.KeyencePLCValue = str_Result;
        //                        }
        //                    }
        //                }
        //                catch (Exception e)
        //                {
        //                    LogHelper.Error(GetType().Name, $"{e.Message}, Address : {x.KeyencePLCTag}");
        //                }
        //            });
        //        }
        //        //Thread.Sleep(300);
        //    }
        //}

        #endregion PrivateEventMethods

        #region Test
        private (DBPlcDevice device, string address) AddressPreprocess(string raw_address)
        {
            raw_address.ToUpper();
            string prefixString = string.Empty;
            string numericAddressString = string.Empty;

            bool isBool = false;

            Dictionary<string, string> parsedAddress = new Dictionary<string, string>();
            parsedAddress.Add(prefixString, numericAddressString);

            try
            {
                //Get alphabet characters only
                foreach (Match match in Regex.Matches(raw_address, @"[A-Z]*"))
                {
                    prefixString += match.Value;
                }
                //Get numeric characters only
                foreach (Match match in Regex.Matches(raw_address, @"\d*"))
                {
                    numericAddressString += match.Value;
                }

                char value = '.';
                isBool = raw_address.Contains(value);
            }
            catch (RegexMatchTimeoutException)
            {
                // Do Nothing: Assume that timeout represents no match.
            }

            return (DeviceAssignment(prefixString, isBool), numericAddressString);
        }

        private DBPlcDevice DeviceAssignment(string prefix, bool isBool)
        {
            //Device Type Categorization
            if (DBCommManager1.PLC == DBPlcId.DBPLC_DKV7K)
            {
                switch (prefix)
                {
                    case "DM":
                        return DBPlcDevice.DKV7K_DM;
                    case "R":
                        return DBPlcDevice.DKV7K_RLY_B;
                    case "B":
                        return DBPlcDevice.DKV7K_B;
                    case "MR":
                        return DBPlcDevice.DKV7K_MR;
                    case "EM":
                        if (isBool)
                        {
                            return DBPlcDevice.DKV7K_EM_B;
                        }
                        else
                        {
                            return DBPlcDevice.DKV7K_EM;
                        }
                    default:
                        {
                            return DBPlcDevice.DKV7K_MR;
                        }
                }
            }
            else if (DBCommManager1.PLC == DBPlcId.DBPLC_DKV8K)
            {
                switch (prefix)
                {
                    case "DM":
                        return DBPlcDevice.DKV8K_DM;
                    case "R":
                        return DBPlcDevice.DKV8K_RLY_B;
                    case "B":
                        return DBPlcDevice.DKV8K_B;
                    case "MR":
                        return DBPlcDevice.DKV8K_MR;
                    case "EM":
                        return DBPlcDevice.DKV8K_EM;
                }
            }
            //Default return case
            throw new ArgumentException();
        }

        public T Read<T>(string raw_address, int strLength = 0)
        {
            ensureDeferredConnect();
            if(Global.IsOfflineMode)
            {
                return readSimulated<T>(raw_address);
            }

            T result = default;

            DATABUILDERAXLibLB.DBValueConverterEx CV;
            CV = new DATABUILDERAXLibLB.DBValueConverterEx();
            int lowerAddress, higherAddress;
            //LogHelper.OPCTrace(GetType().Name, MessageHelper.Read(MessageHelper.KeyValue(AddressPreprocess(raw_address).address, higherAddress)));

            //MethodHelper.Action(GetType().Name, () =>
            //{
            try
            {
                higherAddress = DBCommManager1.ReadDevice(AddressPreprocess(raw_address).device, AddressPreprocess(raw_address).address);
                if (typeof(T) == typeof(string))
                {
                    try
                    {
                        string str_result = ".";
                        DBCommManager1.ReadText(AddressPreprocess(raw_address).device, AddressPreprocess(raw_address).address, strLength, out str_result);
                        //LogHelper.OPCTrace(GetType().Name, MessageHelper.Read(MessageHelper.KeyValue(AddressPreprocess(raw_address).address, str_result)));
                        LogHelper.OPCTrace(GetType().Name, MessageHelper.Read(MessageHelper.KeyValue(raw_address, str_result)));
                        result = (T)Convert.ChangeType(str_result, typeof(T));
                    }
                    catch (Exception exception)
                    {
                        LogHelper.Error(GetType().Name, exception);
                        LogHelper.Error(GetType().Name, raw_address + ":" + strLength.ToString());
                    }
                }
                else if (typeof(T) == typeof(float))
                {
                    var addressIncr = int.Parse(AddressPreprocess(raw_address).address) + 1;
                    lowerAddress = DBCommManager1.ReadDevice(AddressPreprocess(raw_address).device, addressIncr.ToString());
                    //LogHelper.OPCTrace(GetType().Name, MessageHelper.Read(MessageHelper.KeyValue(AddressPreprocess(raw_address).address + 1, higherAddress)));
                    float fResult;

                    CV.WValueToFloat(higherAddress, lowerAddress, out fResult);
                    //CV.WValueToFloat(lowerAddress, higherAddress, out fResult);

                    LogHelper.OPCTrace(GetType().Name, MessageHelper.Read(MessageHelper.KeyValue(raw_address, fResult)));
                    result = (T)Convert.ChangeType(fResult, typeof(T));
                }
                else if (typeof(T) == typeof(int))
                {
                    var addressIncr = int.Parse(AddressPreprocess(raw_address).address) + 1;
                    lowerAddress = DBCommManager1.ReadDevice(AddressPreprocess(raw_address).device, addressIncr.ToString());
                    //LogHelper.OPCTrace(GetType().Name, MessageHelper.Read(MessageHelper.KeyValue(AddressPreprocess(raw_address).address + 1, higherAddress)));                       
                    int iResult;
                    //CV.WValueToSigned(lowerAddress, higherAddress, out iResult);
                    CV.WValueToSigned(higherAddress, lowerAddress, out iResult);

                    LogHelper.OPCTrace(GetType().Name, MessageHelper.Read(MessageHelper.KeyValue(raw_address, iResult)));
                    result = (T)Convert.ChangeType(iResult, typeof(T));
                }
                else if (typeof(T) == typeof(uint))
                {
                    lowerAddress = DBCommManager1.ReadDevice(AddressPreprocess(raw_address).device, AddressPreprocess(raw_address).address + 1);
                    //LogHelper.OPCTrace(GetType().Name, MessageHelper.Read(MessageHelper.KeyValue(AddressPreprocess(raw_address).address + 1, higherAddress)));
                    int iResult;
                    uint uiResult;
                    CV.WValueToSigned(lowerAddress, higherAddress, out iResult);
                    uiResult = (uint)iResult;

                    LogHelper.OPCTrace(GetType().Name, MessageHelper.Read(MessageHelper.KeyValue(raw_address, uiResult)));
                    result = (T)Convert.ChangeType(uiResult, typeof(T));
                }
                else //default case for if bool/short/ushort
                {
                    //lowerAddress = DBCommManager1.ReadDevice(AddressPreprocess(raw_address).device, AddressPreprocess(raw_address).address);

                    LogHelper.OPCTrace(GetType().Name, MessageHelper.Read(MessageHelper.KeyValue(raw_address, result)));
                    result = (T)Convert.ChangeType(higherAddress, typeof(T));
                }
            }
            catch (Exception exception)
            {
                LogHelper.Error(GetType().Name, exception);
                LogHelper.Error(GetType().Name, raw_address + ":" + strLength.ToString());
            }
            //});

            //return (T)Convert.ChangeType(result, typeof(T));
            return result;
        }

        public void Write<T>(string raw_addr, T value, int strLength = 0)
        {
            ensureDeferredConnect();
            if(Global.IsOfflineMode)
            {
                SetSimulatedValue(raw_addr, value);
                LogHelper.OPCTrace(GetType().Name, MessageHelper.Write(MessageHelper.KeyValue(raw_addr, Convert.ToString(value))));
                return;
            }

            DATABUILDERAXLibLB.DBValueConverterEx CV;
            CV = new DATABUILDERAXLibLB.DBValueConverterEx();
            int lowerValue, higherValue;

            MethodHelper.Action(GetType().Name, () =>
            {
                try
                {
                    if (typeof(T) == typeof(string))
                    {
                        DBCommManager1.WriteText(AddressPreprocess(raw_addr).device, AddressPreprocess(raw_addr).address, strLength, value.ToString());
                        //LogHelper.OPCTrace(GetType().Name, MessageHelper.Write(MessageHelper.KeyValue(AddressPreprocess(raw_addr).address, value.ToString())));

                        LogHelper.OPCTrace(GetType().Name, MessageHelper.Write(MessageHelper.KeyValue(raw_addr, value.ToString())));
                    }
                    else if (typeof(T) == typeof(float))
                    {
                        float floatValue = (float)(object)value;
                        CV.FloatToWValue(floatValue, out lowerValue, out higherValue);

                        DBCommManager1.WriteDevice(AddressPreprocess(raw_addr).device, AddressPreprocess(raw_addr).address + 1, higherValue);
                        DBCommManager1.WriteDevice(AddressPreprocess(raw_addr).device, AddressPreprocess(raw_addr).address, lowerValue);

                        //LogHelper.OPCTrace(GetType().Name, MessageHelper.Write(MessageHelper.KeyValue(AddressPreprocess(raw_addr).address, value.ToString())));

                        LogHelper.OPCTrace(GetType().Name, MessageHelper.Write(MessageHelper.KeyValue(raw_addr, value.ToString())));
                    }
                    else if (typeof(T) == typeof(int))
                    {
                        int intValue = Convert.ToInt32(value);
                        CV.SignedToWValue(intValue, out lowerValue, out higherValue);
                        //Swap address
                        DBCommManager1.WriteDevice(AddressPreprocess(raw_addr).device, AddressPreprocess(raw_addr).address + 1, higherValue);
                        DBCommManager1.WriteDevice(AddressPreprocess(raw_addr).device, AddressPreprocess(raw_addr).address, lowerValue);

                        //LogHelper.OPCTrace(GetType().Name, MessageHelper.Write(MessageHelper.KeyValue(AddressPreprocess(raw_addr).address, value.ToString())));

                        LogHelper.OPCTrace(GetType().Name, MessageHelper.Write(MessageHelper.KeyValue(raw_addr, value.ToString())));
                    }
                    else if (typeof(T) == typeof(uint))
                    {
                        string uintValue = value.ToString();
                        CV.UnsignedTextToWValue(uintValue, out lowerValue, out higherValue);

                        DBCommManager1.WriteDevice(AddressPreprocess(raw_addr).device, AddressPreprocess(raw_addr).address, higherValue);
                        DBCommManager1.WriteDevice(AddressPreprocess(raw_addr).device, AddressPreprocess(raw_addr).address + 1, lowerValue);

                        //LogHelper.OPCTrace(GetType().Name, MessageHelper.Write(MessageHelper.KeyValue(AddressPreprocess(raw_addr).address, value.ToString())));

                        LogHelper.OPCTrace(GetType().Name, MessageHelper.Write(MessageHelper.KeyValue(raw_addr, value.ToString())));
                    }
                    else if (typeof(T) == typeof(bool) || typeof(T) == typeof(short)) //default case for if bool/short
                    {
                        int writeValue = Convert.ToInt16(value);
                        DBCommManager1.WriteDevice(AddressPreprocess(raw_addr).device, AddressPreprocess(raw_addr).address, writeValue);

                        //LogHelper.OPCTrace(GetType().Name, MessageHelper.Write(MessageHelper.KeyValue(AddressPreprocess(raw_addr).address, value.ToString())));

                        LogHelper.OPCTrace(GetType().Name, MessageHelper.Write(MessageHelper.KeyValue(raw_addr, value.ToString())));
                    }
                    else if (typeof(T) == typeof(ushort))
                    {
                        var bytes = typeof(BitConverter).GetMethod("GetBytes", new Type[] { value.GetType() }).Invoke(null, new object[] { value });
                        int writeValue = BitConverter.ToInt16((byte[])bytes, 0);
                        DBCommManager1.WriteDevice(AddressPreprocess(raw_addr).device, AddressPreprocess(raw_addr).address, writeValue);

                        //LogHelper.OPCTrace(GetType().Name, MessageHelper.Write(MessageHelper.KeyValue(AddressPreprocess(raw_addr).address, value.ToString())));

                        LogHelper.OPCTrace(GetType().Name, MessageHelper.Write(MessageHelper.KeyValue(raw_addr, value.ToString())));
                    }
                }
                catch (Exception exception)
                {
                    LogHelper.Error(GetType().Name, exception);
                }
            });
        }

        #endregion
    }
}