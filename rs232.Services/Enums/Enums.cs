using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rs232.Services.Enums
{
    public static class Enums
    {
        public enum DataType { ASCII, HEX };

        public enum Terminator { CR, LR, CRLF, BRAK };

        public enum FlowControl { XON_XOFF, RTS_CTS, BRAK };

        public enum Parity { ODD, EVEN, BRAK };
    }
}
