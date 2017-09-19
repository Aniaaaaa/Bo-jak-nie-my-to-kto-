﻿using System;
using static rs232.Services.Enums.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rs232.Services.Model;

namespace rs232.Services
{
    public interface IRs232Service
    {
        List<string> GetPortNames();

        bool SetParameters(PortParameters portParameters);

        void Send(string message);

        void ClosePort();
    }
}
