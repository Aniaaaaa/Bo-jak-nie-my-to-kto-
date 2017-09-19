﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rs232.Services.Enums
{
    public static class Enums
    {
        public enum DataType { ASCII, HEX };

        public enum StopBits { ONE = 1, TWO = 2};

        public enum Terminator { CR, LR, CRLF, BRAK, WŁASNY };

        public enum FlowControl { XON_XOFF = 1, RTS_CTS = 2, DTR_DSR = -1, BRAK = 0};

        public enum Parity { ODD = 1, EVEN = 2, BRAK = 0};

        public enum Signal { OB, DA, DTR, DSR, RTS, CTS, CD, RI };

        public enum SignalState { Unknown = 0, Inactive = 1, Active = 2 };
    }
}
