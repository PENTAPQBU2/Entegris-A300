using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_SSDC_PIM.CommunicationProtocols.COMPort
{
    public class COMPortSettingModel : ViewModelBase
    {
        #region For COMPort Setup

                //SerialPort1.Parity = Parity.None;
                //SerialPort1.StopBits = StopBits.One;
                //SerialPort1.Handshake = Handshake.None;

        private string portName = string.Empty;
        public string PortName
        {
            get { return portName; }
            set
            {
                if (portName != value)
                {
                    portName = value;
                    RaisePropertyChanged(nameof(PortName));
                }
            }
        }

        private int baudRate;
        public int BaudRate
        {
            get { return baudRate; }
            set
            {
                if (baudRate != value)
                {
                    baudRate = value;
                    RaisePropertyChanged(nameof(BaudRate));
                }
            }
        }

        private int dataBits;
        public int DataBits
        {
            get { return dataBits; }
            set
            {
                if (dataBits != value)
                {
                    dataBits = value;
                    RaisePropertyChanged(nameof(DataBits));
                }
            }
        }

        #endregion
    }
}
