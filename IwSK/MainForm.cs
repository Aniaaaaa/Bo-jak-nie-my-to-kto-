using modbus.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace modbus
{
    public partial class MainForm : Form
    {
        private readonly IModbusService service;
        public MainForm()
        {
            service = new ModbusService();

            InitializeComponent();
        }
    }
}
