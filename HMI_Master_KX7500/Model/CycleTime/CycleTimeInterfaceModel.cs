using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HMI_Master_KX7500.Helpers;
using HMI_Master_KX7500.Model.Language;
using HMI_Master_KX7500.Services;
using System;
using System.Linq;
using System.Windows.Input;

namespace HMI_Master_KX7500.Models
{
    public enum CycleTimeUpdate
    {
        Enable,
        Name,
        Timer
    }

    public class CycleTimeInterfaceModel : ViewModelBase
    {
        #region PrivateFields
        private readonly CycleTimeService cycleTimeService = new CycleTimeService();
        //private readonly CycleTimeEditorView cycleTimeEditorView = new CycleTimeEditorView();
        #endregion

        #region PublicCommandProperties
        public ICommand EditCycleTimeCommand { get; set; }
        #endregion

        #region PublicProperties
        private bool allowEditCycleTime = false;
        public bool AllowEditCycleTime
        {
            get { return allowEditCycleTime; }
            set
            {
                if (allowEditCycleTime != value)
                {
                    allowEditCycleTime = value;
                    RaisePropertyChanged(nameof(AllowEditCycleTime));
                }
            }
        }

        private CycleTimeMainModel cycleTimeMainModel = new CycleTimeMainModel();
        public CycleTimeMainModel CycleTimeMainModel
        {
            get { return cycleTimeMainModel; }
            set
            {
                if (cycleTimeMainModel != value)
                {
                    cycleTimeMainModel = value;
                    RaisePropertyChanged(nameof(CycleTimeMainModel));
                    updateLanguage();
                }
            }
        }
        #endregion

        #region Constructor
        public CycleTimeInterfaceModel()
        {
            initializeCommand();
            initializeMessenger();
        }
        #endregion

        #region PrivateInitializeMethods
        private void initializeCommand()
        {
            EditCycleTimeCommand = new RelayCommand(editCycleTimeCommand);
        }

        private void initializeMessenger()
        {
            //Messenger.Default.Register<CycleTimeInitializeModel>(this, initializeCycleTime);
            initializeCycleTime();
            Messenger.Default.Register<CycleTimeUpdate>(this, updateCycleTime);
            Messenger.Default.Register<CycleTimeHistoryTypeTimerModel>(this, updateCycleTimeHistory);
            //Messenger.Default.Register<UserAccountModel>(this, updateUserAccount);
            Messenger.Default.Register<LanguageModel>(this, updateLanguage);
            //updateLanguage();
        }
        #endregion

        #region PrivateCommandMethods
        private void editCycleTimeCommand()
        {
            //cycleTimeEditorView.ShowDialog(),
            //    MessageHelper.UserClickedButton("EditCycleTime");
        }
        #endregion

        #region PrivateMessengerMethods
        private void initializeCycleTime(/*CycleTimeInitializeModel cycleTimeInitializeModel*/)
        {
            CycleTimeMainModel = CycleTimeService.DeserializeCycleTime();
        }

        private void updateCycleTimeHistory(CycleTimeHistoryTypeTimerModel cycleTimeHistoryTypeTimerModel)
        {
            CycleTimeHistoryTypeModel historyTypeModel = null;
            CycleTimeHistoryModel historyModel = null;
            var timerTypeModel = CycleTimeMainModel.CycleTimeList.Select(
                x => x.CycleTimeHistoryModel).SelectMany(
                x => x.CycleTimeHistoryTypeList).SelectMany(
                x => x.CycleTimeHistoryTypeTimerList).Where(
                x => x.CycleTimeHistoryTypeTimerTagModel.CycleTimeHistoryTypeTimerTag
                    == cycleTimeHistoryTypeTimerModel.CycleTimeHistoryTypeTimerTagModel.CycleTimeHistoryTypeTimerTag).First();

            if (timerTypeModel != null)
            {
                historyTypeModel = CycleTimeMainModel.CycleTimeList.Select(
                    x => x.CycleTimeHistoryModel).SelectMany(
                    x => x.CycleTimeHistoryTypeList).Where(x => x.CycleTimeHistoryTypeTimerList.Contains(timerTypeModel)).First();
            }

            if (historyTypeModel != null)
            {
                historyModel = CycleTimeMainModel.CycleTimeList.Select(
                x => x.CycleTimeHistoryModel).Where(x => x.CycleTimeHistoryTypeList.Contains(historyTypeModel)).First();
            }

            if (historyModel != null)
                historyModel.RaisePropertyChanged(nameof(CycleTimeHistoryModel.CycleTimeHistorySeries));
        }

        private void updateCycleTime(CycleTimeUpdate cycleTimeUpdate)
        {
            switch (cycleTimeUpdate)
            {
                case CycleTimeUpdate.Name:
                    updateLanguage();
                    break;
                default:
                    break;
            }
        }

        //private void updateUserAccount(UserAccountModel userAccountModel)
        //{
        //    MethodHelper.Action(GetType().Name, nameof(updateUserAccount), () =>
        //    {
        //        AllowEditCycleTime = UserAccountService.IsUserAccountGroupAccessAllow(
        //            nameof(UserAccountGroupAccess.CycleTimeEditor));
        //    });
        //}

        private void updateLanguage(LanguageModel model = null)
        {
            CycleTimeMainModel.CycleTimeList.ForEach(x =>
            {
                x.CycleTimeName = HmiHelper.GetResource<string>(x.CycleTimeKey);

                x.CycleTimeNumericStatusList.ForEach(y =>
                {
                    y.CycleTimeNumericStatusName = HmiHelper.GetResource<string>(y.CycleTimeNumericStatusKey);
                });

                x.CycleTimeHistoryModel.CycleTimeHistoryTypeList.ForEach(y =>
                {
                    y.CycleTimeHistoryTypeName = HmiHelper.GetResource<string>(y.CycleTimeHistoryTypeKey);
                });

                x.CycleTimeHistoryModel.CycleTimeHistoryLabelList.ForEach(y =>
                {
                    y.CycleTimeHistoryLabelName = HmiHelper.GetResource<string>(y.CycleTimeHistoryLabelKey);
                });
            });
        }
        #endregion
    }
}