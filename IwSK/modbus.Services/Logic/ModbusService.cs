using modbus.Services.Model;
using System;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modbus.Data;
using Modbus.Device;
using Modbus.Utility;
using Modbus.Serial;
using Modbus.IO;

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
                string message = _serialPort.ReadLine();
                   try
                   {
                    if (!message.Contains("*&*"))
                    {
                        return $"[in] {message}";
                    }
                    else
                    {
                        message.Replace("*&*", "");
                        SendMessage(message);
                        return $"[in] {message}";
                    }
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

    /// <summary>
    ///     Demonstration of NModbus
    /// </summary>
   /* public class Driver
    {

        public List<string> GetPortNames()
        {
            return SerialPort.GetPortNames().ToList();
        }

        /// <summary>
        ///     Simple Modbus serial ASCII master read holding registers example.
        /// </summary>
        public static void ModbusSerialAsciiMasterReadRegisters(PortParameters portParameters)
        {
            using (SerialPort port = new SerialPort())
            {
                // configure serial port
                port.PortName = portParameters.PortName;
                port.BaudRate = portParameters.Speed;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Open();

                var adapter = new SerialPortAdapter(port);
                // create modbus master
                IModbusSerialMaster master = ModbusSerialMaster.CreateAscii(adapter);

                byte slaveId = 1;
                ushort startAddress = 1;
                ushort numRegisters = 5;

                // read five registers		
                ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                for (int i = 0; i < numRegisters; i++)
                {
                    Console.WriteLine($"Register {startAddress + i}={registers[i]}");
                }
            }

            // output: 
            // Register 1=0
            // Register 2=0
            // Register 3=0
            // Register 4=0
            // Register 5=0
        }


        /// <summary>
        ///     Simple Modbus serial ASCII slave example.
        /// </summary>
        public static void StartModbusSerialAsciiSlave()
        {
            using (SerialPort slavePort = new SerialPort("COM2"))
            {
                // configure serial port
                slavePort.BaudRate = 9600;
                slavePort.DataBits = 8;
                slavePort.Parity = Parity.None;
                slavePort.StopBits = StopBits.One;
                slavePort.Open();

                byte unitId = 1;

                var adapter = new SerialPortAdapter(slavePort);
                // create modbus slave
                ModbusSlave slave = ModbusSerialSlave.CreateAscii(unitId, adapter);
                slave.DataStore = DataStoreFactory.CreateDefaultDataStore();

                slave.ListenAsync().GetAwaiter().GetResult();
            }
        }

       

        /// <summary>
        ///     Modbus serial ASCII master and slave example.
        /// </summary>
        public static void ModbusSerialAsciiMasterReadRegistersFromModbusSlave()
        {
            using (SerialPort masterPort = new SerialPort("COM1"))
            using (SerialPort slavePort = new SerialPort("COM2"))
            {
                // configure serial ports
                masterPort.BaudRate = slavePort.BaudRate = 9600;
                masterPort.DataBits = slavePort.DataBits = 8;
                masterPort.Parity = slavePort.Parity = Parity.None;
                masterPort.StopBits = slavePort.StopBits = StopBits.One;
                masterPort.Open();
                slavePort.Open();

                var slaveAdapter = new SerialPortAdapter(slavePort);
                // create modbus slave on seperate thread
                byte slaveId = 1;
                ModbusSlave slave = ModbusSerialSlave.CreateAscii(slaveId, slaveAdapter);
                var listenTask = slave.ListenAsync();

                //var masterAdapter = new SerialPortAdapter(masterPort);
                // create modbus master
                ModbusSerialMaster master = ModbusSerialMaster.CreateAscii(masterAdapter);

                master.Transport.Retries = 5;
                ushort startAddress = 100;
                ushort numRegisters = 5;

                // read five register values
                ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                for (int i = 0; i < numRegisters; i++)
                {
                    Console.WriteLine($"Register {(startAddress + i)}={registers[i]}");
                }
            }

            // output
            // Register 100=0
            // Register 101=0
            // Register 102=0
            // Register 103=0
            // Register 104=0
        }

        /// <summary>
        ///     Write a 32 bit value.
        /// </summary>
        public static void ReadWrite32BitValue()
        {
            using (SerialPort port = new SerialPort("COM1"))
            {
                // configure serial port
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Open();

                var adapter = new SerialPortAdapter(port);
                // create modbus master
                ModbusSerialMaster master = ModbusSerialMaster.CreateRtu(adapter);

                byte slaveId = 1;
                ushort startAddress = 1008;
                uint largeValue = UInt16.MaxValue + 5;

                ushort lowOrderValue = BitConverter.ToUInt16(BitConverter.GetBytes(largeValue), 0);
                ushort highOrderValue = BitConverter.ToUInt16(BitConverter.GetBytes(largeValue), 2);

                // write large value in two 16 bit chunks
                master.WriteMultipleRegisters(slaveId, startAddress, new ushort[] { lowOrderValue, highOrderValue });

                // read large value in two 16 bit chunks and perform conversion
                ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, 2);
                uint value = ModbusUtility.GetUInt32(registers[1], registers[0]);
            }
        }
    }*/
}
