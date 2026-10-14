using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.CommunicationProtocols;
using HMI_Master_KX7500.Globals;
using HMI_Master_KX7500.Model;
using HMI_Master_KX7500.Models;
using HMI_Master_KX7500.Services;
using MahApps.Metro.Controls;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HMI_Master_KX7500.ModulesView.Settings
{
    public class Settings_OPCUA_ViewModel : ViewModelBase
    {
        #region PrivateFields
        private readonly SettingsGeneralService settingsService = new SettingsGeneralService();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private List<string> settingsTagList = new List<string>();
        private static OPCModules moduleName = OPCModules.Settings;
        #endregion

        #region PublicProperties
        private SettingsGeneralInterfaceModel settingsGeneralInterfaceModel = new SettingsGeneralInterfaceModel();
        public SettingsGeneralInterfaceModel SettingsGeneralInterfaceModel
        {
            get { return settingsGeneralInterfaceModel; }
            set
            {
                if(settingsGeneralInterfaceModel != value)
                {
                    settingsGeneralInterfaceModel = value;
                    RaisePropertyChanged(nameof(SettingsGeneralInterfaceModel));
                }
            }
        }
        #endregion

        private KeyenceDll _keyenceDll;
        #region Constructor
        public Settings_OPCUA_ViewModel()
        {
            try
            {
                _keyenceDll = new KeyenceDll(new Dictionary<string, string>(), Global.OPCUA_Url, Global.PLCPort, DATABUILDERAXLibLB.DBPlcId.DBPLC_DKV7K, new List<OPCModules>());
                initializeSettingsGeneral();
                Event.UpdateEvent += Update;
            }
            catch(Exception ex) { logger.Error(ex); }
        }
        #endregion

        ~Settings_OPCUA_ViewModel()
        {

            Event.UpdateEvent -= Update;
        }

        #region PrivateMessengerMethods
        private void initializeSettingsGeneral()
        {
            SettingsGeneralInterfaceModel.SettingsGeneralModel = SettingsGeneralService.DeserializeSettingsGeneral();
            SettingsGeneralInterfaceModel.SettingsGeneralModel.SettingsGeneralGroupList.ForEach(x =>
            {
                x.SettingsGeneralGroupNumericList.ForEach(y =>
                {
                    y.SetSettingsGeneralGroupNumericCommand = new RelayCommand<NumericUpDown>(
                         setSettingsGeneralGroupNumericCommand);
                });

                x.SettingsGeneralGroupToggleList.ForEach(y =>
                {
                    y.SettingsGeneralGroupToggleCommand = new RelayCommand<SettingsGeneralGroupToggleModel>(
                            SettingsGeneralGroupToggleCommand);
                });
            });
        }


        private void Update()
        {
            if (Global.PageList["Setting"])
            {
                SettingsGeneralInterfaceModel.SettingsGeneralModel.SettingsGeneralGroupList.ForEach(x =>
                {
                    x.SettingsGeneralGroupNumericList.ForEach(y => y.SettingsGeneralGroupNumericValue = _keyenceDll.Read<float>(y.SettingsGeneralGroupNumericTagModel.SettingsGeneralGroupNumericTag));
                    x.SettingsGeneralGroupToggleList.ForEach(y => y.SettingsGeneralGroupToggleValue = _keyenceDll.Read<bool>(y.SettingsGeneralGroupToggleTagModel.SettingsGeneralGroupToggleTag).ToString());
                    x.SettingsGeneralGroupReadOnlyList.ForEach(y => y.SettingsGeneralGroupReadOnlyValue = _keyenceDll.Read<bool>(y.SettingsGeneralGroupReadOnlyTagModel.SettingsGeneralGroupReadOnlyTag).ToString());
                });
            }
        }

        //private void opcUpdate(_OPC obj)
        //{
        //    if(obj.Module.Contains(moduleName))
        //    {
        //        Task.Run(() =>
        //        {
        //            obj.Datachange.ToList().ForEach(node =>
        //            {
        //                if(node.Value != null)
        //                {

        //                    if(SettingsGeneralInterfaceModel.SettingsGeneralModel.SettingsGeneralGroupList.SelectMany(
        //                        x => x.SettingsGeneralGroupNumericList).ToList().Exists(
        //                        x => x.SettingsGeneralGroupNumericTagModel.SettingsGeneralGroupNumericTag ==
        //                        node.Key))
        //                    {
        //                        SettingsGeneralInterfaceModel.SettingsGeneralModel.SettingsGeneralGroupList.SelectMany(
        //                            x => x.SettingsGeneralGroupNumericList).ToList().Find(
        //                            x => x.SettingsGeneralGroupNumericTagModel.SettingsGeneralGroupNumericTag ==
        //                            node.Key).SettingsGeneralGroupNumericValue =
        //                            Convert.ToDouble(node.Value);
        //                    }
        //                    else if(SettingsGeneralInterfaceModel.SettingsGeneralModel.SettingsGeneralGroupList.SelectMany(
        //                        x => x.SettingsGeneralGroupToggleList).ToList().Exists(
        //                        x => x.SettingsGeneralGroupToggleTagModel.SettingsGeneralGroupToggleTag ==
        //                        node.Key))
        //                    {
        //                        SettingsGeneralInterfaceModel.SettingsGeneralModel.SettingsGeneralGroupList.SelectMany(
        //                            x => x.SettingsGeneralGroupToggleList).ToList().Find(
        //                            x => x.SettingsGeneralGroupToggleTagModel.SettingsGeneralGroupToggleTag ==
        //                            node.Key).SettingsGeneralGroupToggleValue =
        //                            node.Value;
        //                    }
        //                }
        //            });
        //        });
        //    }
        //}
        #endregion

        #region PrivateCommandMethod
        private void setSettingsGeneralGroupNumericCommand(NumericUpDown numericUpDown)
        {
            SettingsGeneralGroupNumericModel settingsGeneralGroupNumericModel =
                numericUpDown.Tag as SettingsGeneralGroupNumericModel;
            _keyenceDll.Write<float>(settingsGeneralGroupNumericModel.SettingsGeneralGroupNumericTagModel.SettingsGeneralGroupNumericTag, (float)numericUpDown.Value);
        }

        private void SettingsGeneralGroupToggleCommand(
            SettingsGeneralGroupToggleModel settingsGeneralGroupToggleModel)
        {
            bool val = bool.Parse(settingsGeneralGroupToggleModel.SettingsGeneralGroupToggleValue);
            _keyenceDll.Write<bool>(settingsGeneralGroupToggleModel.SettingsGeneralGroupToggleTagModel.SettingsGeneralGroupToggleTag, val);
        }
        #endregion
    }
}
