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

        public List<string> GetPortNames()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public bool SetParameters(PortParameters portParameters)
        {
            _serialPort.BaudRate = portParameters.Speed;
            _serialPort.StopBits = (System.IO.Ports.StopBits)portParameters.StopBits;
            _serialPort.DataBits = portParameters.DataBits;
            //_serialPort.Terminator = portParameters.DataBits;   // check for WŁASNY
            _serialPort.PortName = portParameters.PortName;
            //_serialPort.FlowControl
            _serialPort.Parity = (System.IO.Ports.Parity)portParameters.Parity;
            _serialPort.ReadTimeout = (int)(portParameters.Timeout * 100);
            _serialPort.WriteTimeout = (int)(portParameters.Timeout * 100);
            //_serialPort.DataType

            try
            {
                _serialPort.Open();
                return true;
            }
            catch
            {
                _serialPort.Close();
                return false;
            }
        }

        public void Send(string message)
        {
            _serialPort.WriteLine($"[out] {message}");
        }

        public void ClosePort()
        {
            _serialPort.Close();
        }
    }
}
