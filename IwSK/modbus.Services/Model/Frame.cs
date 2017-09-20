using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modbus.Services.Model
{
    public class Frame
    {
        #region Properties

        /// <summary>
        /// Adres.
        /// </summary>
        public byte Address { get; set; }

        /// <summary>
        /// Rozkaz.
        /// </summary>
        public byte Function { get; set; }

        /// <summary>
        /// Dane przesyłane w ramce.
        /// </summary>
        public string Message { get; set; }

        #endregion

        #region Ctor

        public Frame(int address, int function, string message)
        {
            Address = (byte)address;
            Function = (byte)function;
            Message = message;
        }

        #endregion
    }
}
