using modbus.Services.Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modbus.Services
{
    public class ModbusService : IModbusService
    {
        private readonly SerialPort _serialPort = new SerialPort();

        public Frame RequestFrame { get; set; }

        public List<string> GetPortNames()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public bool OpenPort(PortParameters portParameters)
        {
            _serialPort.BaudRate = portParameters.Speed;
            _serialPort.PortName = portParameters.PortName;

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
                    return $"[in] {_serialPort.ReadLine()}";
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public Frame Execute(Frame frame)
        {
            if (IsFrameValid(frame))
            {
                if (frame.Function == Enums.Enums.Function.SEND)
                {
                    return SendRequest(frame);
                }
                else if (frame.Function == Enums.Enums.Function.GET)
                {
                    return GetRequest(frame);
                }
            }

            return null;
        }

        #region Private methods

        private bool IsFrameValid(Frame frame)
        {
            return frame.Address == RequestFrame.Address &&
                frame.Function == RequestFrame.Function;
        }

        private Frame SendRequest(Frame frame)
        {
            throw new NotImplementedException();
        }

        private Frame GetRequest(Frame frame)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
