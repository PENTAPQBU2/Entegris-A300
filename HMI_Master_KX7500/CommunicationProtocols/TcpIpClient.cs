using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CAMLine
{
    #region PublicEnums
    public enum LineTerminator
    {
        NA,
        CR,
        LF,
        CRLF
    }
    #endregion

    public class TcpIpClient : IDisposable
    {
        #region PrivateFields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Socket socket = null;
        private IPEndPoint serverEndPoint = null;
        private string serverIp = string.Empty;
        private string serverPort = string.Empty;
        private bool checkConnectionFlag = false;
        private bool autoReconnect = false;
        private bool enableHeartBeat = true;
        private bool runHeartBeat = true;
        private bool receivedHeartBeat = false;
        //private byte[] byteBuffer = new byte[1024]; //one byte
        private byte[] byteBuffer = new byte[2048]; // two byte
        private LineTerminator lineTerminator;
        #endregion

        #region PublicEvents
        public delegate void DataReceivedEvent(string ip, string message);
        public event DataReceivedEvent DataReceived;
        protected virtual void onDataReceived(string message)
        {
            if (DataReceived != null)
            {
                DataReceived(serverEndPoint.ToString(), message);
                receivedHeartBeat = true;
            }
        }

        public delegate void StatusUpdatedEvent(string ip, string status);
        public event StatusUpdatedEvent StatusUpdated;
        protected virtual void onStatusUpdated(string status)
        {
            if (StatusUpdated != null)
            {
                StatusUpdated(serverEndPoint.ToString(), status);
                if (status.Equals("Connected")) { runHeartBeat = true; }
                else if (status.Equals("Disconnected")) { runHeartBeat = false; }
            }
        }
        #endregion

        #region Constructor
        public TcpIpClient(string _serverIp, string _serverPort, bool _enableHearBeat = false, LineTerminator _lineTerminator = LineTerminator.CR)
        {
            if (String.IsNullOrEmpty(_serverIp)) { throw new ArgumentException("Value cannot be null or empty.", "serverIP"); }
            if (String.IsNullOrEmpty(_serverPort)) { throw new ArgumentException("Value cannot be null or empty.", "serverPort"); }

            serverIp = _serverIp;
            serverPort = _serverPort;
            enableHeartBeat = _enableHearBeat;
            lineTerminator = _lineTerminator;
            checkConnectionFlag = false;
            autoReconnect = true;

            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), Convert.ToInt32(serverPort));
            Task.Factory.StartNew(() => checkConnection());
            //Task.Factory.StartNew(() => heartBeat());
        }
        #endregion

        #region PrivateMethods
        private void checkConnection()
        {
            while (true)
            {
                if (checkConnectionFlag && autoReconnect)
                    Connect();
                Thread.Sleep(1000);
            }
        }

        private void setTcpKeepAlive(Socket _socket, uint _keepAliveTime, uint _keepAliveInterval)
        {
            // Marshal the equivalent of the native structure into a byte array.
            int size = Marshal.SizeOf(new uint());
            byte[] _inOptionValue = new byte[size * 3];
            BitConverter.GetBytes((uint)_keepAliveTime).CopyTo(_inOptionValue, 0);
            BitConverter.GetBytes((uint)_keepAliveTime).CopyTo(_inOptionValue, Marshal.SizeOf(size));
            BitConverter.GetBytes((uint)_keepAliveInterval).CopyTo(_inOptionValue, (Marshal.SizeOf(size) * 2));

            try
            {
                // write SIO_VALS to Socket IOControl
                socket.IOControl(IOControlCode.KeepAliveValues, _inOptionValue, null);
            }
            catch (NotSupportedException)
            {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, _inOptionValue);
            }
            catch (NotImplementedException)
            {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, _inOptionValue);
            }
            catch (Exception)
            {

            }
        }

        private void onConnect(IAsyncResult _asyncResult)
        {
            // Socket was the passed in object
            Socket _socket = (Socket)_asyncResult.AsyncState;

            // Check if we were sucessfull
            try
            {
                _socket.EndConnect(_asyncResult);
                if (_socket.Connected)
                {
                    setupReceiveCallBack(_socket);
                    onStatusUpdated("Connected");
                }
                else
                {
                    throw new Exception("Unable to connect to remote machine.");
                }
            }
            catch (ObjectDisposedException)
            {
                onStatusUpdated("Disconnected");
            }
            catch (Exception ex)
            {
                //logger.Error(ex.Message);
                checkConnectionFlag = true;
                if (autoReconnect)
                {
                    onStatusUpdated(ex.Message);
                    onStatusUpdated("Reconnecting");
                    Connect();
                }
                else
                {
                    onStatusUpdated(ex.Message);
                    onStatusUpdated("Disconnected");
                }
            }
        }

        /// <summary>
        /// Setup the callback for received data and loss of connection.
        /// </summary>
        /// <param name="_socket"></param>
        private void setupReceiveCallBack(Socket _socket)
        {
            try
            {
                _socket.BeginReceive(byteBuffer, 0, byteBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceivedData), _socket);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void heartBeat()
        {
            Stopwatch _sw = new Stopwatch();
            _sw.Reset();
            _sw.Start();
            while (true)
            {
                if (runHeartBeat && enableHeartBeat)
                {
                    if (receivedHeartBeat)
                    {
                        _sw.Reset();
                        _sw.Start();
                        receivedHeartBeat = false;
                    }
                    if (_sw.ElapsedMilliseconds >= 5000)
                    {
                        _sw.Reset();
                        _sw.Start();
                        receivedHeartBeat = false;
                    }
                }
                //Thread.Sleep(1000);
            }
        }
        #endregion

        #region PublicMethods
        /// <summary>
        /// Connect to server
        /// </summary>
        public void Connect()
        {
            try
            {
                //if (socket != null && socket.Connected) { return; }
                using (Ping _ping = new Ping())
                {
                    PingReply _pingResult = _ping.Send(serverIp);
                    if (_pingResult.Status != IPStatus.Success) { throw new PingException($"Failed to ping {serverIp}:{serverPort}."); }
                }
                checkConnectionFlag = false;
                // Update status.
                onStatusUpdated("Connecting");

                // Create the socket object.
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                socket.LingerState.Enabled = false;
                setTcpKeepAlive(socket, 20, 20);
                // Connect to server non-blocking method
                socket.BeginConnect(serverEndPoint, new AsyncCallback(onConnect), socket);
            }
            catch (Exception ex)
            {
                //logger.Error(ex.Message);
                checkConnectionFlag = true;
                if (autoReconnect)
                {
                    onStatusUpdated(ex.Message);
                    onStatusUpdated("Reconnecting");
                    Connect();
                }
            }
        }

        /// <summary>
        /// Close the socket connection.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (socket != null)
                {
                    socket.Close();
                    onStatusUpdated("Disconnected");
                    socket = null;
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        /// <summary>
        /// Get the new data and send it out to all other connections.
        /// Note: If not data was received the connection has probably died.
        /// </summary>
        /// <param name="_asyncResult"></param>
        public void OnReceivedData(IAsyncResult _asyncResult)
        {
            // Socket was the passed in object
            Socket _socket = (Socket)_asyncResult.AsyncState;

            // Check if we got any data
            try
            {
                int _bytesRec = 0;
                if (_socket != null && _socket.Connected) { _bytesRec = _socket.EndReceive(_asyncResult); }
                if (_bytesRec > 0)
                {
                    // Wrote the data to the List
                    string _received = Encoding.ASCII.GetString(byteBuffer, 0, _bytesRec);
                    if (byteBuffer != null) { byteBuffer = null; }
                    //byteBuffer = new byte[1024]; //one byte
                    byteBuffer = new byte[2048]; //two byte
                    // If the connection is still usable restablish the callback.
                    setupReceiveCallBack(_socket);
                    // Raise OnDataReceived event.
                    DataReceived(serverEndPoint.ToString(), _received);
                }
                else if (_bytesRec == 0) { checkConnectionFlag = true; }
            }
            catch (Exception ex)
            {
                //logger.Error(ex.Message);
                checkConnectionFlag = true;
                if (autoReconnect) { onStatusUpdated(ex.Message); }
            }
        }

        /// <summary>
        /// Send the message in the message area. Only do this if we are connected.
        /// </summary>
        /// <param name="_message"></param>
        /// <returns></returns>
        public bool Send(string _message)
        {
            bool _sent = false;
            try
            {
                switch (lineTerminator)
                {
                    case LineTerminator.CR: _message = $"{_message}\r"; break;
                    case LineTerminator.LF: _message = $"{_message}\n"; break;
                    case LineTerminator.CRLF: _message = $"{_message}\r\n"; break;
                }
                // Check we are connected.
                if (socket == null || !socket.Connected)
                {
                    FileLogger_Utilities.TCPIP_Log("Must be connected to send a message");
                    throw new Exception("Must be connected to send a message");
                }

                // Read the message from the text box and send it
                try
                {
                    // Convert to byte array and send.
                    Byte[] _byteDateLine = Encoding.ASCII.GetBytes(_message.ToCharArray());
                    socket.Send(_byteDateLine, _byteDateLine.Length, 0);
                    _sent = true;
                    FileLogger_Utilities.TCPIP_Log("_sent is true");
                    FileLogger_Utilities.TCPIP_Log($"{_message}");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex.Message);
                checkConnectionFlag = true;
                if (autoReconnect) { onStatusUpdated(ex.Message); }
            }
            return _sent;
        }
        #endregion

        #region IDisposableMembers
        public void Dispose()
        {
            autoReconnect = false;
            Disconnect();
        }
        #endregion
    }
}
