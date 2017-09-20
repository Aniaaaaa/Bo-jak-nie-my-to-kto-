using System;
using static rs232.Services.Enums.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using rs232.Services.Model;

namespace rs232.Services
{
    public class Rs232Service : IRs232Service
    {
        private readonly SerialPort _serialPort = new SerialPort();
        private DataType dataType;

        public List<string> GetPortNames()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public string HexToString(String input)
        {
            input = input.ToLower();
            byte[] bytes = new byte[input.Length / 2];
            for (int i = 0; i < input.Length; i += 2)
                bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);
            string str = "";
            new List<byte>(bytes).ForEach(b => str += (char)b);
            return str;
        }
        public string StringToHex(String input)
        {
            string hex = "0123456789abcdef "; /*na końcu spacja jako hack na linq*/
            string str = String.Concat(
                input
                    .Select(ch => (int)ch)
                    .SelectMany(ch => new List<int> { ch / 16, ch % 16, 16 /*spacja*/ })
                    .Select(ch => hex[ch])
            );
            str = str.Substring(0, str.Length - 1); // obcięcie ostatniej spacji
            return str;
            //input.Select(ch => )
        }
        public byte LRC(String input)
        {
            int LRC = 0;
            foreach(byte b in input)
                LRC = (LRC + b) & 0xFF;
            LRC = (((LRC ^ 0xFF) + 1) & 0xFF);
            return (byte)LRC;
        }

        public bool OpenPort(PortParameters portParameters)
        {
            dataType = portParameters.DataType;

            _serialPort.BaudRate = portParameters.Speed;
            _serialPort.StopBits = (System.IO.Ports.StopBits)portParameters.StopBits;
            _serialPort.DataBits = portParameters.DataBits;
            _serialPort.PortName = portParameters.PortName;
            _serialPort.Handshake = (Handshake)portParameters.FlowControl;
            _serialPort.Parity = (System.IO.Ports.Parity)portParameters.Parity;
            _serialPort.ReadTimeout = (int)(portParameters.Timeout * 100);
            _serialPort.WriteTimeout = (int)(portParameters.Timeout * 100);
            _serialPort.NewLine = portParameters.Terminator != Terminator.WŁASNY
                ? TerminatorToAscii(portParameters.Terminator)
                : HexToString(portParameters.MyTerminator);

            try
            {
                _serialPort.Open();
                return _serialPort.IsOpen;
            }
            catch
            {
                _serialPort.Close();
                return false;
            }
        }

        public void ClosePort()
        {
            _serialPort.Close();
        }

        public void SendMessage(string message)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.WriteLine(message);
            }
        }

        public string ReceiveMessage()
        {
            if (_serialPort.IsOpen)
            {
                try
                {
                    if (dataType == DataType.ASCII)
                        return $"[in] {_serialPort.ReadLine()}";
                    else if (dataType == DataType.HEX)
                        return $"[in] {StringToHex(_serialPort.ReadLine())}";
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }
    }
}
