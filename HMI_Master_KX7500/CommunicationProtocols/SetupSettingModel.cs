using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMLine
{
    public class SetupSettingModel : ViewModelBase
    {
        #region For TCPIP Setup
        private string serverIP = string.Empty;
        public string ServerIP
        {
            get { return serverIP; }
            set
            {
                if (serverIP != value)
                {
                    serverIP = value;
                    RaisePropertyChanged(nameof(ServerIP));
                }
            }
        }

        private string serverPort;
        public string ServerPort
        {
            get { return serverPort; }
            set
            {
                if (serverPort != value)
                {
                    serverPort = value;
                    RaisePropertyChanged(nameof(ServerPort));
                }
            }
        }

        private string equipID;
        public string EquipID
        {
            get { return equipID; }
            set
            {
                if (equipID != value)
                {
                    equipID = value;
                    RaisePropertyChanged(nameof(EquipID));
                }
            }
        }

        #endregion
    }
}
