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
        public PortParameters PortParameters { get; private set; }

        public List<string> GetPortNames()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public void SetParameters(PortParameters portParameters)
        {
            PortParameters = portParameters;
            _serialPort.Open();
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
