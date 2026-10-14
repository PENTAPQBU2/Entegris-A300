using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.MachinePath;
using HMI_Master_KX7500.Model;
using HMI_Master_KX7500.Models;
using HMI_Master_KX7500.Services;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HMI_Master_KX7500.ModulesView.EngMode
{
    public class EngMode_OPCUA_ViewModel : ViewModelBase
    {
        #region PrivateFields
        private readonly EngineeringService engineeringService = new EngineeringService();
        private List<string> engineeringTagList = new List<string>();
        private KeyenceDll keyenceDll = null;
        #endregion

        #region PublicProperties
        private EngineeringInterfaceModel engineeringInterfaceModel = new EngineeringInterfaceModel();
        public EngineeringInterfaceModel EngineeringInterfaceModel
        {
            get { return engineeringInterfaceModel; }
            set
            {
                if (engineeringInterfaceModel != value)
                {
                    engineeringInterfaceModel = value;
                    RaisePropertyChanged(nameof(EngineeringInterfaceModel));
                }
            }
        }
        #endregion

        #region Constructor
        public EngMode_OPCUA_ViewModel()
        {
            initializeMessenger();

            Messenger.Default.Send(new EngineeringInitializeModel
            {
                EngineeringFilePath = FilePath.EngineeringMode
            });

            keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, Global.PLCPort, DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());

            Event.UpdateEvent += Update;
        }

        ~EngMode_OPCUA_ViewModel()
        {
            Event.UpdateEvent -= Update;
        }

        private void Update()
        {
            if (Global.PageList["Engineering Mode"])
            {
                foreach (var x in EngineeringInterfaceModel.EngineeringMainModel.EngineeringGroupModel)
                {
                    foreach (var y in x.EngineeringGroupReadOnlyList)
                    {
                        y.EngineeringGroupReadOnlyValue = keyenceDll.Read<int>(y.EngineeringGroupReadOnlyTagModel.EngineeringGroupReadOnlyTag).ToString();
                    }

                    foreach (var y in x.EngineeringGroupToggleList)
                    {
                        y.EngineeringGroupToggleValue = keyenceDll.Read<bool>(y.EngineeringGroupToggleTagModel.EngineeringGroupToggleTag);
                    }
                }
            }
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeMessenger()
        {
            Messenger.Default.Register<EngineeringInitializeModel>(this, initializeEngineering);
            //Messenger.Default.Register<_OPC>(this, opcUpdate);
        }

        //private void initializeopcUa(OPCUA_ opcUa_)
        //{
        //    opcUa = opcUa_;
        //    engineeringTagList = opcUa.GetAddresses();
        //    //engineeringTagList = EngineeringService.GetEngineeringTagList(EngineeringInterfaceModel.EngineeringMainModel);

        //    //if(Global.MachineConfigModel.InitializeOPCUA)
        //    //{
        //    //    opcUa.InitializeUAOPC(Global.OPCUA_Url, engineeringTagList);
        //    //}
        //}
        #endregion

        #region PrivateMessengerMethods
        private void initializeEngineering(EngineeringInitializeModel engineeringInitializeModel)
        {
            EngineeringInterfaceModel.EngineeringMainModel = EngineeringService.DeserializeEngineering();
            EngineeringInterfaceModel.EngineeringMainModel.EngineeringGroupModel.ForEach(x => {
                x.EngineeringGroupToggleList.ForEach(y =>
                {
                    y.onEngineeringGroupToggleCommand = new RelayCommand<EngineeringGroupToggleModel>(onEngineeringToggleCommand);
                    y.offEngineeringGroupToggleCommand = new RelayCommand<EngineeringGroupToggleModel>(offEngineeringToggleCommand);
                });
                x.EngineeringGroupNumericList.ForEach(y =>
                {
                    y.SetEngineeringGroupNumericCommand = new RelayCommand<NumericUpDown>(setEngineeringNumericCommand);
                });
                x.EngineeringGroupTextList.ForEach(y =>
                {
                    y.sendEngineeringGroupTextCommand = new RelayCommand<EngineeringGroupTextModel>(sendEngineeringTextCommand);
                });
            });
            //initializeopcUa();
            //Messenger.Default.Register<OPCUA_>(this, initializeopcUa);
        }

        //private void opcUpdate(_OPC opc_)
        //{
        //    if (opc_.Module.Contains(OPCModules.Engineering))
        //    {
        //        opc_.Datachange.ToList().ForEach(x =>
        //        {
        //            Task.Run(() =>
        //            {
        //                if (EngineeringInterfaceModel.EngineeringMainModel.EngineeringGroupModel.SelectMany(
        //                       y => y.EngineeringGroupToggleList).ToList().Exists(
        //                       y => y.EngineeringGroupToggleTagModel.EngineeringGroupToggleTag == x.Key
        //                   ))
        //                {
        //                    EngineeringInterfaceModel.EngineeringMainModel.EngineeringGroupModel.SelectMany(
        //                        y => y.EngineeringGroupToggleList).ToList().Find(
        //                        y => y.EngineeringGroupToggleTagModel.EngineeringGroupToggleTag == x.Key)
        //                        .EngineeringGroupToggleValue = Convert.ToBoolean(x.Value);
        //                }
        //                else if (EngineeringInterfaceModel.EngineeringMainModel.EngineeringGroupModel.SelectMany(
        //                    y => y.EngineeringGroupReadOnlyList).ToList().Exists(
        //                    y => y.EngineeringGroupReadOnlyTagModel.EngineeringGroupReadOnlyTag == x.Key
        //                ))
        //                {
        //                    EngineeringInterfaceModel.EngineeringMainModel.EngineeringGroupModel.SelectMany(
        //                        y => y.EngineeringGroupReadOnlyList).ToList().Find(
        //                        y => y.EngineeringGroupReadOnlyTagModel.EngineeringGroupReadOnlyTag == x.Key)
        //                        .EngineeringGroupReadOnlyValue = x.Value;
        //                }
        //                else if (EngineeringInterfaceModel.EngineeringMainModel.EngineeringGroupModel.SelectMany(
        //                    y => y.EngineeringGroupNumericList).ToList().Exists(
        //                    y => y.EngineeringGroupNumericTagModel.EngineeringGroupNumericTag == x.Key
        //                ))
        //                {
        //                    EngineeringInterfaceModel.EngineeringMainModel.EngineeringGroupModel.SelectMany(
        //                        y => y.EngineeringGroupNumericList).ToList().Find(
        //                        y => y.EngineeringGroupNumericTagModel.EngineeringGroupNumericTag == x.Key)
        //                        .EngineeringGroupNumericValue = Convert.ToDouble(x.Value);
        //                }
        //            });
        //        });
        //    }
        //}
        #endregion

        #region PrivateCommandMethods

        private void onEngineeringToggleCommand(EngineeringGroupToggleModel engineeringGroupToggleModel)
        {
            if(MessageBox.Show($"Turn On {engineeringGroupToggleModel.EngineeringGroupToggleName} ?", "Confirmation", MessageBoxButton.YesNo, 
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                keyenceDll.Write<bool>(engineeringGroupToggleModel.EngineeringGroupToggleTagModel.EngineeringGroupToggleTag, true);
            }
        }

        private void offEngineeringToggleCommand(EngineeringGroupToggleModel engineeringGroupToggleModel)
        {
            if (MessageBox.Show($"Turn Off {engineeringGroupToggleModel.EngineeringGroupToggleName} ?", "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                keyenceDll.Write<bool>(engineeringGroupToggleModel.EngineeringGroupToggleTagModel.EngineeringGroupToggleTag, false);
            }
        }

        private void setEngineeringNumericCommand(NumericUpDown numericUpDown)
        {
            EngineeringGroupNumericModel EngineeringGroupNumericModel = numericUpDown.Tag as EngineeringGroupNumericModel;

            if (MessageBox.Show($"Set Value {EngineeringGroupNumericModel.EngineeringGroupNumericName} ?", "Confirmation", MessageBoxButton.YesNo,
               MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                keyenceDll.Write<int>(EngineeringGroupNumericModel.EngineeringGroupNumericTagModel.EngineeringGroupNumericTag, (int)numericUpDown.Value);
            }
        }

        private void sendEngineeringTextCommand(EngineeringGroupTextModel engineeringGroupTextModel)
        {
            if (MessageBox.Show($"Set Value {engineeringGroupTextModel.EngineeringGroupTextName} ?", "Confirmation", MessageBoxButton.YesNo,
               MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                //opcUa.Write(engineeringGroupTextModel.EngineeringGroupTextTagModel.EngineeringGroupTextTag,engineeringGroupTextModel.EngineeringGroupTextValue);
                //String length unknown
                keyenceDll.Write<string>(engineeringGroupTextModel.EngineeringGroupTextTagModel.EngineeringGroupTextTag, engineeringGroupTextModel.EngineeringGroupTextValue);
            }
        }
        #endregion
    }
}
