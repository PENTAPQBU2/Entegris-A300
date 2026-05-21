using GalaSoft.MvvmLight.Messaging;
using HMI_Master_Keyence.Enumerations;
using HMI_Master_Keyence.FileLog;
using HMI_Master_Keyence.Helpers;
using HMI_Master_Keyence.Models;
using OpcRcw.Comn;
using OpcRcw.Da;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HMI_Master_Keyence.CommunicationProtocols
{
    public class SiemensOPC : IOPCDataCallback
    {
        #region PrivateFields
        private List<string> siemensOPCTagList = new List<string>();
        private string acknowledgeSiemensOPCTag = string.Empty;
        private bool isDB = false;
        private readonly List<SiemensOPCTagModel> siemensOPCValueList = new List<SiemensOPCTagModel>();
        private IOPCServer iOPCServer = null;
        internal const int dwLCID = 0x409;
        private object ppUnk = null;
        private IOPCAsyncIO2 iOPCAsyncIO2 = null;
        private IOPCGroupStateMgt iOPCGroupStateMgt = null;
        private IConnectionPointContainer iConnectionPointContainer = null;
        private IConnectionPoint iConnectionPoint = null;
        private int pdwCookie = 0;
        private int[] phServer = null;
        private List<string> dataChangeList = new List<string>();
        private bool acknowledge = false;
        private readonly int dwTransactionID = 0;
        #endregion

        #region Constructor
        public SiemensOPC()
        {
            MethodHelper.Initialize(GetType().Name, GetType().Name);
        }
        public SiemensOPC(List<string> _siemensOPCTagList,
            string _acknowledgeSiemensOPCTag = "", bool _isDB = false)
        {
            Task.Run(() =>
            {
                MethodHelper.Initialize(GetType().Name, GetType().Name, () =>
                {
                    LogHelper.KeyValue(GetType().Name, nameof(_siemensOPCTagList), _siemensOPCTagList.Count);
                    LogHelper.KeyValue(GetType().Name, nameof(_acknowledgeSiemensOPCTag), _acknowledgeSiemensOPCTag);
                    LogHelper.KeyValue(GetType().Name, nameof(_isDB), _isDB);

                    siemensOPCTagList = _siemensOPCTagList;
                    LogHelper.KeyValue(GetType().Name, nameof(siemensOPCTagList), siemensOPCTagList.Count);

                    acknowledgeSiemensOPCTag = _acknowledgeSiemensOPCTag;
                    LogHelper.KeyValue(GetType().Name, nameof(acknowledgeSiemensOPCTag), acknowledgeSiemensOPCTag);

                    isDB = _isDB;
                    LogHelper.KeyValue(GetType().Name, nameof(isDB), isDB);

                    if (!string.IsNullOrEmpty(acknowledgeSiemensOPCTag))
                    {
                        siemensOPCTagList.Add(acknowledgeSiemensOPCTag);
                        LogHelper.KeyValue(GetType().Name, nameof(siemensOPCTagList), siemensOPCTagList.Count);
                    }

                    if (siemensOPCTagList.Count <= 0)
                    {
                        return;
                    }

                    siemensOPCTagList.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList().ForEach(x =>
                    {
                        MethodHelper.Action(GetType().Name, () =>
                        {
                            siemensOPCValueList.Add(new SiemensOPCTagModel
                            {
                                SiemensOPCTag = x,
                                SiemensOPCValue = string.Empty
                            });
                        });
                    });

                    connect();
                    asyncReading();
                });
            });
        }
        #endregion

        #region PrivateMethods
        private void connect()
        {
            try
            {
                LogHelper.StartTo(GetType().Name, nameof(connect));

                iOPCServer = (IOPCServer)Activator.CreateInstance(Type.GetTypeFromProgID("OPC.SimaticNET"));

                GCHandle pTimeBias = GCHandle.Alloc(0, GCHandleType.Pinned);
                GCHandle pPercentDeadband = GCHandle.Alloc(0, GCHandleType.Pinned);

                try
                {
                    Guid riid = typeof(IOPCItemMgt).GUID;

                    iOPCServer.AddGroup("grp1", 0, 250, 1, pTimeBias.AddrOfPinnedObject(),
                        pPercentDeadband.AddrOfPinnedObject(), dwLCID, out int phServerGroup,
                        out int pRevisedUpdateRate, ref riid, out ppUnk);

                    initializeIOInterfacePointer();

                    OPCITEMDEF[] pItemArray = new OPCITEMDEF[siemensOPCTagList.Count];

                    for (int i = 0; i < siemensOPCTagList.Count; i++)
                    {
                        pItemArray[i].szAccessPath = "";
                        pItemArray[i].szItemID = siemensOPCTagList[i];
                        pItemArray[i].bActive = 1;
                        pItemArray[i].hClient = i + 1;
                        pItemArray[i].dwBlobSize = 0;
                        pItemArray[i].pBlob = IntPtr.Zero;

                        string[] s = siemensOPCTagList[i].Split(PunctuationHelper.Comma);
                        if (s.Length > 1)
                        {
                            if (s[1].ToUpper().StartsWith("REAL"))
                            {
                                pItemArray[i].vtRequestedDataType = (short)CanonicalDataType.Real;
                            }
                            else if (s[1].ToUpper().StartsWith("STRING"))
                            {
                                pItemArray[i].vtRequestedDataType = (short)CanonicalDataType.String;
                            }
                            else if (s[1].ToUpper().StartsWith("INT"))
                            {
                                pItemArray[i].vtRequestedDataType = (short)CanonicalDataType.Int;
                            }
                            else if (s[1].ToUpper().StartsWith("DINT"))
                            {
                                pItemArray[i].vtRequestedDataType = (short)CanonicalDataType.Long;
                            }
                            else if (s[1].ToUpper().StartsWith("X"))
                            {
                                pItemArray[i].vtRequestedDataType = (short)CanonicalDataType.Boolean;
                            }
                        }
                        else
                        {
                            if (s[0].ToUpper().Contains('X'))
                            {
                                pItemArray[i].vtRequestedDataType = (short)CanonicalDataType.Boolean;
                            }
                            else if (s[0].ToUpper().Contains('W'))
                            {
                                pItemArray[i].vtRequestedDataType = (short)CanonicalDataType.Int;
                            }
                        }

                        if (s != null)
                        {
                            s = null;
                        }
                    }

                    IntPtr ppAddResults = IntPtr.Zero;
                    IntPtr ppErrors = IntPtr.Zero;

                    try
                    {
                        ((IOPCItemMgt)ppUnk).AddItems(siemensOPCTagList.Count,
                            pItemArray, out ppAddResults, out ppErrors);

                        int[] dwError = new int[siemensOPCTagList.Count];
                        IntPtr ptr = ppAddResults;
                        phServer = new int[siemensOPCTagList.Count];
                        Marshal.Copy(ppErrors, dwError, 0, siemensOPCTagList.Count);

                        for (int i = 0; i < siemensOPCTagList.Count; i++)
                        {
                            if (dwError[i] == 0)
                            {
                                if (i != 0)
                                {
                                    ptr = new IntPtr(ptr.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                                }

                                OPCITEMRESULT oPCITEMRESULT = (OPCITEMRESULT)Marshal.PtrToStructure(
                                    ptr, typeof(OPCITEMRESULT));

                                phServer[i] = oPCITEMRESULT.hServer;
                            }
                            else
                            {
                                LogHelper.Error(GetType().Name, $"OPC {nameof(connect)} error." +
                                    $" Siemens OPC Tag({siemensOPCTagList[i]}), ppAddResults({ppAddResults})," +
                                    $" ppErrors({ppErrors}).");

                                iOPCServer.GetErrorString(dwError[i], dwLCID, out string ppString);
                                throw new Exception($"{ppString} Result - Adding Item 0.");
                            }

                            Marshal.DestroyStructure(ptr, typeof(OPCITEMRESULT));
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new Exception($"{exception.Message} Result - Adding Items.");
                    }
                    finally
                    {
                        if (ppAddResults != IntPtr.Zero)
                        {
                            Marshal.FreeCoTaskMem(ppAddResults);
                            ppAddResults = IntPtr.Zero;
                        }

                        if (ppErrors != IntPtr.Zero)
                        {
                            Marshal.FreeCoTaskMem(ppErrors);
                            ppErrors = IntPtr.Zero;
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception($"Error while creating group object:-{exception.Message} Result - Add group.");
                }
                finally
                {
                    if (pPercentDeadband.IsAllocated)
                    {
                        pPercentDeadband.Free();
                    }

                    if (pTimeBias.IsAllocated)
                    {
                        pTimeBias.Free();
                    }
                }

                LogHelper.Done(GetType().Name, nameof(connect));
            }
            catch (Exception exception)
            {
                LogHelper.FailTo(GetType().Name, nameof(connect));

                throw new Exception($"Error while creating group object:-{exception.Message} Result - Create Server.");
            }
        }

        private void initializeIOInterfacePointer()
        {
            try
            {
                LogHelper.StartTo(GetType().Name, nameof(initializeIOInterfacePointer));

                iOPCAsyncIO2 = (IOPCAsyncIO2)ppUnk;
                iOPCGroupStateMgt = (IOPCGroupStateMgt)ppUnk;
                iConnectionPointContainer = (IConnectionPointContainer)ppUnk;
                Guid riid = typeof(IOPCDataCallback).GUID;
                iConnectionPointContainer.FindConnectionPoint(ref riid, out iConnectionPoint);
                iConnectionPoint.Advise(this, out pdwCookie);

                LogHelper.Done(GetType().Name, nameof(initializeIOInterfacePointer));
            }
            catch (Exception exception)
            {
                LogHelper.FailTo(GetType().Name, nameof(initializeIOInterfacePointer));

                throw new Exception($"Error while advising callbacks:-{exception.Message} Result - Add group.");
            }
        }

        private void asyncReading()
        {
            LogHelper.StartTo(GetType().Name, nameof(asyncReading));

            GCHandle pActive = GCHandle.Alloc(0, GCHandleType.Pinned);
            pActive.Target = 1;

            try
            {
                iOPCGroupStateMgt.SetState(IntPtr.Zero, out int pRevisedUpdateRate,
                    pActive.AddrOfPinnedObject(), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

                LogHelper.Done(GetType().Name, nameof(asyncReading));
            }
            catch (Exception exception)
            {
                LogHelper.FailTo(GetType().Name, nameof(asyncReading));

                throw new Exception($"{exception.Message} Result-Change Group State.");
            }
            finally
            {
                pActive.Free();
            }
        }

        private void checkForUpdate(int dwCount, int[] pErrors, int[] phClientItems,
            object[] pvValues, short[] pwQualities, string methodName)
        {
            MethodHelper.Action(GetType().Name, () =>
            {
                initializeDataChangeList();

                for (int i = 0; i < dwCount; i++)
                {
                    if (pErrors[i] == 0)
                    {
                        checkForIllegalCrossThreadCalls(false);

                        string[] s = dataChangeList[phClientItems[i] - 1].Split(PunctuationHelper.Semicolon);
                        if (s.Length > 0)
                        {
                            update(phClientItems[i] - 1, s[0], pvValues[i], pwQualities[i]);
                        }

                        checkForIllegalCrossThreadCalls(true);
                    }
                    else
                    {
                        LogHelper.Error(GetType().Name, $"OPC error {methodName}." +
                            $" Siemens OPC Tag({dataChangeList[i]}), Quality({getQuality(pwQualities[i])})," +
                            $" pErrors({pErrors[i]}).");

                        iOPCServer.GetErrorString(pErrors[0], dwLCID, out string ppString);
                        throw new Exception(ppString);
                    }

                    setAcknowledge(phClientItems[i] - 1);
                }

                if (acknowledge && isDB == true)
                {
                    checkForIllegalCrossThreadCalls(false);
                    acknowledge = false;
                    dataChangeList = null;
                    checkForIllegalCrossThreadCalls(true);
                }
                if (isDB == false)
                {
                    checkForIllegalCrossThreadCalls(false);
                    dataChangeList = null;
                    checkForIllegalCrossThreadCalls(true);
                }
            });
        }

        private void initializeDataChangeList()
        {
            try
            {
                dataChangeList = new List<string>();

                if (siemensOPCTagList != null)
                {
                    if (siemensOPCTagList.Count > 0)
                    {
                        siemensOPCTagList.ForEach(x =>
                        {
                            try
                            {
                                dataChangeList.Add(x);
                            }
                            catch (Exception)
                            {
                            }
                        });
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void checkForIllegalCrossThreadCalls(bool value)
        {
            MethodHelper.Action(GetType().Name, () => Control.CheckForIllegalCrossThreadCalls = value);
        }

        private void update(int index, string siemensOPCTag, object value, short _quality)
        {
            MethodHelper.Action(GetType().Name, () =>
            {
                Thread.Sleep(1);

                string quality = getQuality(_quality);

                MethodHelper.Action(GetType().Name, () =>
                {
                    //LogHelper.KeyValue(nameof(SiemensOPC), "dataChangeList count Debug", dataChangeList.Count);
                    //LogHelper.KeyValue(nameof(SiemensOPC), "index Debug", index);
                    //LogHelper.KeyValue(nameof(SiemensOPC), "siemensOPCTag Debug", siemensOPCTag);
                    string address = $"{siemensOPCTag}{PunctuationHelper.Semicolon}" +
                        $"{value}{PunctuationHelper.Semicolon}{quality}";

                    if (string.IsNullOrEmpty(address))
                    {
                        DebugLog.SiemensOPCLog($"NullAddress = {address}");
                        DebugLog.SiemensOPCLog($"DataChangeList = {dataChangeList[index]}");
                    }
                    else
                    {
                        dataChangeList[index] = address;
                        DebugLog.SiemensOPCLog($"TagAddress = {address}");
                    }
                });

                if (quality == "Good")
                {
                    if (siemensOPCValueList.Exists(x => x.SiemensOPCTag == siemensOPCTag))
                    {
                        siemensOPCValueList.Find(x => x.SiemensOPCTag ==
                        siemensOPCTag).SiemensOPCValue = value.ToString();

                        LogHelper.SiemensOPCTrace(GetType().Name, MessageHelper.DataChange(
                            MessageHelper.KeyValue(siemensOPCTag, value)));

                        Task.Run(() =>
                        {
                            MethodHelper.Action(GetType().Name, () =>
                            {
                                Messenger.Default.Send(new SiemensOPCModel
                                {
                                    SiemensOPCTagList = siemensOPCTagList,
                                    SiemensOPCTagModel = new SiemensOPCTagModel
                                    {
                                        SiemensOPCTag = siemensOPCTag,
                                        SiemensOPCValue = value.ToString()
                                    }
                                });
                            });
                        });
                    }
                }
            });
        }

        private string getQuality(long _quality)
        {
            string quality = string.Empty;

            MethodHelper.Action(GetType().Name, () =>
            {
                switch (_quality)
                {
                    case Qualities.OPC_QUALITY_GOOD:
                        quality = "Good";
                        break;
                    case Qualities.OPC_QUALITY_BAD:
                        quality = "Bad";
                        break;
                    case Qualities.OPC_QUALITY_CONFIG_ERROR:
                        quality = "BadConfigurationError";
                        break;
                    case Qualities.OPC_QUALITY_NOT_CONNECTED:
                        quality = "BadNotConnected";
                        break;
                    case Qualities.OPC_QUALITY_DEVICE_FAILURE:
                        quality = "BadDeviceFailure";
                        break;
                    case Qualities.OPC_QUALITY_SENSOR_FAILURE:
                        quality = "BadSensorFailure";
                        break;
                    case Qualities.OPC_QUALITY_COMM_FAILURE:
                        quality = "BadCommFailure";
                        break;
                    case Qualities.OPC_QUALITY_OUT_OF_SERVICE:
                        quality = "BadOutOfService";
                        break;
                    case Qualities.OPC_QUALITY_WAITING_FOR_INITIAL_DATA:
                        quality = "BadWaitingForInitialData";
                        break;
                    case Qualities.OPC_QUALITY_EGU_EXCEEDED:
                        quality = "UncertainEGUExceeded";
                        break;
                    case Qualities.OPC_QUALITY_SUB_NORMAL:
                        quality = "UncertainSubNormal";
                        break;
                    default:
                        quality = "Not handled";
                        break;
                }
            });

            return quality;
        }

        private void setAcknowledge(int index)
        {
            MethodHelper.Action(GetType().Name, () =>
            {
                if (siemensOPCTagList[index].StartsWith(acknowledgeSiemensOPCTag))
                {
                    acknowledge = true;
                }
            });
        }
        #endregion

        #region PublicInterfaceMethods
        public void OnDataChange(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount,
            int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {
            MethodHelper.Action(GetType().Name, () => checkForUpdate(dwCount, pErrors,
                phClientItems, pvValues, pwQualities, nameof(OnDataChange)));
        }

        public void OnReadComplete(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount,
            int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {
            //MethodHelper.Action(GetType().Name, () => checkForUpdate(dwCount, pErrors,
            //    phClientItems, pvValues, pwQualities, nameof(OnReadComplete)));
        }

        public void OnWriteComplete(int dwTransid, int hGroup, int hrMastererr, int dwCount, int[] pClienthandles, int[] pErrors)
        {
            MethodHelper.Action(GetType().Name, () =>
            {
                initializeDataChangeList();
                checkForIllegalCrossThreadCalls(false);
                iOPCServer.GetErrorString(pErrors[0], dwLCID, out string ppString);
                if (pErrors[0] != 0)
                {
                    dataChangeList[pClienthandles[0] - 1] += "@";
                }
                setAcknowledge(pClienthandles[0] - 1);
                checkForIllegalCrossThreadCalls(true);
            });
        }

        public void OnCancelComplete(int dwTransid, int hGroup)
        {
        }
        #endregion

        #region PublicMethods
        public void Dispose(bool dispose)
        {
            MethodHelper.Action(GetType().Name, nameof(Dispose), () =>
            {
                LogHelper.KeyValue(GetType().Name, nameof(dispose), dispose);

                if (dispose)
                {
                    if (pdwCookie != 0)
                    {
                        iConnectionPoint.Unadvise(pdwCookie);
                        pdwCookie = 0;
                    }
                    if (iConnectionPoint != null)
                    {
                        Marshal.ReleaseComObject(iConnectionPoint);
                        iConnectionPoint = null;
                    }
                    if (iConnectionPointContainer != null)
                    {
                        Marshal.ReleaseComObject(iConnectionPointContainer);
                        iConnectionPointContainer = null;
                    }
                    if (iOPCAsyncIO2 != null)
                    {
                        Marshal.ReleaseComObject(iOPCAsyncIO2);
                        iOPCAsyncIO2 = null;
                    }
                    if (iOPCGroupStateMgt != null)
                    {
                        Marshal.ReleaseComObject(iOPCGroupStateMgt);
                        iOPCGroupStateMgt = null;
                    }
                    if (ppUnk != null)
                    {
                        Marshal.ReleaseComObject(ppUnk);
                        ppUnk = null;
                    }
                    if (iOPCServer != null)
                    {
                        Marshal.ReleaseComObject(iOPCServer);
                        iOPCServer = null;
                    }
                    if (siemensOPCTagList != null)
                    {
                        siemensOPCTagList = null;
                    }
                }
                else
                {
                    if (pdwCookie != 0)
                    {
                        iConnectionPoint.Unadvise(pdwCookie);
                        pdwCookie = 0;
                    }

                    Marshal.ReleaseComObject(iConnectionPoint);
                    iConnectionPoint = null;

                    Marshal.ReleaseComObject(iConnectionPointContainer);
                    iConnectionPointContainer = null;

                    if (iOPCAsyncIO2 != null)
                    {
                        Marshal.ReleaseComObject(iOPCAsyncIO2);
                        iOPCAsyncIO2 = null;
                    }
                    if (iOPCGroupStateMgt != null)
                    {
                        Marshal.ReleaseComObject(iOPCGroupStateMgt);
                        iOPCGroupStateMgt = null;
                    }
                    if (ppUnk != null)
                    {
                        Marshal.ReleaseComObject(ppUnk);
                        ppUnk = null;
                    }
                    if (iOPCServer != null)
                    {
                        Marshal.ReleaseComObject(iOPCServer);
                        iOPCServer = null;
                    }
                    if (siemensOPCTagList != null)
                    {
                        siemensOPCTagList = null;
                    }
                }
            });
        }

        public void ReadOPC()
        {
            MethodHelper.Action(GetType().Name, nameof(ReadOPC), () =>
            {
                if (iOPCAsyncIO2 != null)
                {
                    IntPtr ppErrors = IntPtr.Zero;

                    try
                    {
                        iOPCAsyncIO2.Read(phServer.Length, phServer,
                            dwTransactionID + 1, out int pdwCancelID, out ppErrors);

                        int[] dwError = new int[1];
                        Marshal.Copy(ppErrors, dwError, 0, 1);
                        if (dwError[0] != 0)
                        {
                            iOPCServer.GetErrorString(dwError[0], dwLCID, out string ppString);
                            throw new Exception($"Result-Async Read. {ppString}.");
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new Exception($"Error-Async Read. {exception.Message}.");
                    }
                    finally
                    {
                        if (ppErrors != IntPtr.Zero)
                        {
                            Marshal.FreeCoTaskMem(ppErrors);
                            ppErrors = IntPtr.Zero;
                        }
                    }
                }
            });
        }

        public T ReadSiemensOPCTag<T>(string siemensOPCTag)
        {
            var value = default(T);

            MethodHelper.Action(GetType().Name, () =>
            {
                Thread.Sleep(1);

                if (siemensOPCValueList.Exists(x => x.SiemensOPCTag == siemensOPCTag))
                {
                    string _value = siemensOPCValueList.Find(
                        x => x.SiemensOPCTag == siemensOPCTag).SiemensOPCValue;

                    if (!string.IsNullOrEmpty(_value))
                    {
                        value = (T)Convert.ChangeType(_value, typeof(T));
                    }
                }
            });

            MethodHelper.Action(GetType().Name, () =>
            {
                if (value == null && typeof(T) == typeof(string))
                {
                    value = (T)Convert.ChangeType(string.Empty, typeof(T));
                }
            });

            LogHelper.SiemensOPCTrace(GetType().Name, MessageHelper.Read(
                MessageHelper.KeyValue(siemensOPCTag, value)));

            return value;
        }

        public void WriteSiemensOPCTag(string siemensOPCTag, object value)
        {
            MethodHelper.Action(GetType().Name, () =>
            {
                Thread.Sleep(1);

                if (iOPCAsyncIO2 != null)
                {
                    IntPtr ppErrors = IntPtr.Zero;

                    try
                    {
                        object[] pItemValues = new object[1] { value };

                        iOPCAsyncIO2.Write(1, new int[1] { siemensOPCTagList.IndexOf(siemensOPCTag) + 1 },
                            pItemValues, dwTransactionID + 1, out int pdwCancelID, out ppErrors);

                        int[] dwError = new int[1];
                        Marshal.Copy(ppErrors, dwError, 0, 1);
                        if (dwError[0] != 0)
                        {
                            throw new Exception($"{nameof(WriteSiemensOPCTag)} error." +
                                $" Siemens OPC Tag({siemensOPCTag}), Value({value}).");
                        }

                        LogHelper.SiemensOPCTrace(GetType().Name, MessageHelper.Write(
                            MessageHelper.KeyValue(siemensOPCTag, value)));
                    }
                    catch (Exception exception)
                    {
                        LogHelper.Error(GetType().Name, exception);
                    }
                    finally
                    {
                        if (ppErrors != IntPtr.Zero)
                        {
                            Marshal.FreeCoTaskMem(ppErrors);
                            ppErrors = IntPtr.Zero;
                        }
                    }
                }
            });
        }
        #endregion
    }
}