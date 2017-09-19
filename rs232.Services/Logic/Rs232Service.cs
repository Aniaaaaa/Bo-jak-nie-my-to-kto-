﻿using System;
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
        private string terminator;
        private DataType dataType;

        public List<string> GetPortNames()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public bool OpenPort(PortParameters portParameters)
        {
            terminator = portParameters.Terminator != Terminator.WŁASNY
                ? Enum.GetName(typeof(Terminator), portParameters.Terminator)
                : portParameters.MyTerminator;

            dataType = portParameters.DataType;

            _serialPort.BaudRate = portParameters.Speed;
            _serialPort.StopBits = (System.IO.Ports.StopBits)portParameters.StopBits;
            _serialPort.DataBits = portParameters.DataBits;
            _serialPort.PortName = portParameters.PortName;
            _serialPort.Handshake = (Handshake)portParameters.FlowControl;
            _serialPort.Parity = (System.IO.Ports.Parity)portParameters.Parity;
            _serialPort.ReadTimeout = (int)(portParameters.Timeout * 100);
            _serialPort.WriteTimeout = (int)(portParameters.Timeout * 100);

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
                _serialPort.WriteLine($"{message}{terminator}");
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
    }
}
