using System;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using HMI_SSDC_PIM.CommunicationProtocols.COMPort;
using HMI_Master_KX7500.Helpers;

namespace HMI_SSDC_PIM.CommunicationProtocols
{
    public class SerialListener
    {
        public EventHandler onDataReceived;
        SerialPort SerialPort1;
        //private string PortName = "";
        private bool isRecord = false;
        //private Thread Asker;

        private COMPortSettingModel _comPortSettingModel = new COMPortSettingModel();
        public SerialListener() //int portName
        {
            LoadCOMPortService.OnUpdate_COMPortSetting += new LoadCOMPortService.UpdateCOMPortSetting(LoadCOMPortSetting);
            LoadCOMPortService.LoadCOMPortSetting();

            //UpdatePort(portName);
        }

        //public void UpdatePort(int portName)
        //{
        //    try
        //    {
        //        PortName = "COM" + portName;
        //    }
        //    catch (Exception ex) { ShowErr(ex.Message); }
        //}

        public void Open(bool isAuto)
        {
            try
            {
                Close();
                Thread.Sleep(500);
                //SerialPort1 = new SerialPort(PortName);
                SerialPort1 = new SerialPort(_comPortSettingModel.PortName);
                //SerialPort1.BaudRate = 57600;
                SerialPort1.BaudRate = _comPortSettingModel.BaudRate;
                SerialPort1.Parity = Parity.None;
                SerialPort1.StopBits = StopBits.One;
                //SerialPort1.DataBits = 8;
                SerialPort1.DataBits = _comPortSettingModel.DataBits;
                SerialPort1.Handshake = Handshake.None;
                isRecord = true;
                SerialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                //if (!isAuto)
                //{
                //    if (Asker != null && Asker.IsAlive) Asker.Abort();
                //    Asker = new Thread(ManualRead);
                //    Asker.Start();
                //}
                SerialPort1.Open();
            }
            catch (Exception ex) { ShowErr(ex.Message); LogHelper.Error(GetType().Name, ex.Message); }
        }

        public void Close()
        {
            try
            {
                Stop();
                if (SerialPort1?.IsOpen ?? false)
                    SerialPort1.Close();
            }
            catch (Exception ex) { ShowErr(ex.Message); LogHelper.Error(GetType().Name, ex.Message); }
        }

        public void Stop()
        {
            try
            {
                isRecord = false;
            }
            catch (Exception ex) { ShowErr(ex.Message); LogHelper.Error(GetType().Name, ex.Message); }
        }


        //private void ManualRead()
        //{
        //    try
        //    {
        //        while (isRecord)
        //        {
        //            try
        //            {
        //                if (SerialPort1?.IsOpen ?? false)
        //                {
        //                    SerialPort1?.Write("$81N$04");
        //                }
        //                Thread.Sleep(10);
        //            }
        //            catch (Exception ex) { ShowErr(ex.Message, "Inner ManaulRead Thread"); }
        //        }
        //    }
        //    catch (Exception ex) { ShowErr(ex.Message); }
        //}

        private void DataReceivedHandler(
                            object sender,
                            SerialDataReceivedEventArgs e)
        {
            try
            {
                if (isRecord)
                {
                    SerialPort sp = (SerialPort)sender;
                    string indata = sp.ReadExisting();
                    onDataReceived?.Invoke(indata, e);
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex) { ShowErr(ex.Message); LogHelper.Error(GetType().Name, ex.Message); }
        }

        private void ShowErr(string description, [CallerMemberName] string name = "")
        {
            try
            {
                MessageBox.Show(description, $"Serial Port Error | MethodName {name}");
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void LoadCOMPortSetting(COMPortSettingModel comPortSettingModel)
        {
            this._comPortSettingModel = comPortSettingModel;
        }
    }
}



