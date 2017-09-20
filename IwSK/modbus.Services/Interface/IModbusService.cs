using modbus.Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modbus.Services
{
    public interface IModbusService
    {
        List<string> GetPortNames();

        bool OpenPort(PortParameters portParameters);

        void ClosePort();

        void SendMessage(string message);

        string ReceiveMessage();

        Frame Send(Frame frame);
    }
}
