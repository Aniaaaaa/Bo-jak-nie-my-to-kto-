using static rs232.Services.Enums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rs232.Services.Model
{
    public class PortParameters
    {
        public int Speed { get; set; }
        public int StopBits { get; set; }
        public int DataBits { get; set; }
        public Terminator Terminator { get; set; }
        public string MyTerminator { get; set; }
        public string PortName { get; set; }
        public FlowControl FlowControl { get; set; }
        public Parity Parity { get; set; }
        public int Timeout { get; set; }
        public DataType DataType { get; set; }
    }
}
