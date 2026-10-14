using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HMI_Master_KX7500.Model.IO
{
    public class IO_OPCUA_ViewModel : ViewModelBase
    {
        #region PrivateFields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private readonly IOService ioService = new IOService();
        //private List<string> ioTagList = new List<string>();
        //private OPCUA_ opcUa = new OPCUA_();

        private KeyenceDll keyenceDll = null;
        #endregion

        #region PublicProperties
        private IOInterfaceModel iOInterfaceModel = new IOInterfaceModel();
        public IOInterfaceModel IOInterfaceModel
        {
            get { return iOInterfaceModel; }
            set
            {
                if (iOInterfaceModel != value)
                {
                    iOInterfaceModel = value;
                    RaisePropertyChanged(nameof(IOInterfaceModel));
                }
            }
        }
        #endregion

        #region Constructor
        public IO_OPCUA_ViewModel()
        {
            initializeMessenger();

            keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, Global.PLCPort, DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());

            Event.UpdateEvent += Update;
        }

        ~IO_OPCUA_ViewModel()
        {
            Event.UpdateEvent -= Update;
        }

        private void Update()
        {
            if (Global.PageList["IO"])
            {
                IOInterfaceModel.IOModel.IOItemsModels.AsParallel().ForAll(x =>
                {
                    foreach (var y in x.IOInputList)
                    {
                        y.IO_InputValue = keyenceDll.Read<bool>(y.IOInputTagModel.IOInputTag).ToString();
                    }

                    foreach (var y in x.IOOutputList)
                    {
                        y.IO_OutputValue = keyenceDll.Read<bool>(y.IOOutputTagModel.IOOutputTag).ToString();
                    }
                });
            }
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeMessenger()
        {
            try
            {
                initializeIO();

                //Messenger.Default.Register<_OPC>(this, opcUaUpdate);
            } catch (Exception ex) { logger.Error(ex); }
        }

        //private void initializeOPCUa(OPCUA_ opcUa_)
        //{
        //    try
        //    {
        //        opcUa = opcUa_;
        //        ioTagList = opcUa.GetAddresses();

        //        //ioTagList = IOService.GetIOTagList(IOInterfaceModel.IOModel);

        //        ////opcUa = new SiemensOPC(tagList);
        //        //if (Global.MachineConfigModel.InitializeOPCUA)
        //        //{
        //        //    opcUa.InitializeUAOPC(Global.OPCUA_Url, ioTagList, new List<OPCModules> { OPCModules.IO });
        //        //}
        //    } catch (Exception ex) { logger.Error(ex); }
        //}
        #endregion

        #region PrivateCommandMethods
        private void ioSelectionChangedCommand(ComboBox comboBox)
        {
            if (comboBox.SelectedItem != null)
            {
                IOModel iOModel = comboBox.SelectedItem as IOModel;
            }
        }

        private void ioOutputCommand(IOOutputModel iOOutputModel)
        {
            try
            {
                keyenceDll.Write<bool>(iOOutputModel.IOOutputTagModel.IOOutputTag, Global.ConvertBool(iOOutputModel.IO_OutputValue));
            } catch (Exception ex) { logger.Error(ex); }
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializeIO(/*IOInitializeModel iOInitializeModel*/)
        {
            try
            {
                IOInterfaceModel.IOModel = IOService.DeserializeIO();
                iOInterfaceModel.IOModel.IOSelectionChangedCommand = new RelayCommand<ComboBox>(ioSelectionChangedCommand);

                iOInterfaceModel.IOModel.IOItemsModels.ForEach(x =>
                {
                    x.IOOutputList.ForEach(y =>
                    {
                        y.IOOutputCommand = new RelayCommand<IOOutputModel>(ioOutputCommand);
                    });
                });
                //Messenger.Default.Register<OPCUA_>(this, initializeOPCUa);

            } catch (Exception ex) { logger.Error(ex); }
        }

        //private void opcUaUpdate(_OPC opcua_)
        //{
        //    if (opcua_.Module.Contains(OPCModules.IO))
        //    {
        //        opcua_.Datachange.ToList().ForEach(x =>
        //        {
        //            Task.Run(() =>
        //            {
        //                if (IOInterfaceModel.IOModel.IOItemsModels.SelectMany(
        //                        y => y.IOInputList).ToList().Exists(
        //                        y => y.IOInputTagModel.IOInputTag == x.Key))
        //                {
        //                    iOInterfaceModel.IOModel.IOItemsModels.SelectMany(
        //                        y => y.IOInputList).ToList().Find(
        //                        y => y.IOInputTagModel.IOInputTag == x.Key).IO_InputValue = x.Value;
        //                }

        //                else if (IOInterfaceModel.IOModel.IOItemsModels.SelectMany(
        //                    y => y.IOOutputList).ToList().Exists(
        //                    y => y.IOOutputTagModel.IOOutputTag == x.Key))
        //                {
        //                    iOInterfaceModel.IOModel.IOItemsModels.SelectMany(
        //                        y => y.IOOutputList).ToList().Find(
        //                        y => y.IOOutputTagModel.IOOutputTag == x.Key).IO_OutputValue = x.Value;
        //                }
        //            });
        //        });
        //    }
        //}
        #endregion
    }
}
