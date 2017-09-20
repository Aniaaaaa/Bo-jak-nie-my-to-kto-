using static modbus.Services.Enums.Enums;

namespace modbus.Services.Model
{
    public class Frame
    {
        /// <summary>
        /// Adres.
        /// </summary>
        public byte Address { get; set; }

        /// <summary>
        /// Rozkaz.
        /// </summary>
        public Function Function { get; set; }

        /// <summary>
        /// Dane przesyłane w ramce.
        /// </summary>
        public string Message { get; set; }
    }
}
