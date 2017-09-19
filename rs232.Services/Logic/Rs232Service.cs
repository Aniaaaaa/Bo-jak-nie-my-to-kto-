using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace rs232.Services
{
    public class Rs232Service : IRs232Service
    {
        public int MyProperty { get; set; }

        public List<string> GetPortNames()
        {
            return SerialPort.GetPortNames().ToList();
        }
    }
}
