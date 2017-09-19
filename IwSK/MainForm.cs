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
        public enum Transmission { T_7E1, T_7O1, T_7N2 };
        public enum Station { MASTER, SLAVE };

        private void MainForm_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = new List<int> { 150, 300, 600, 1200, 2400, 4800, 9600 /*, 14400, 19200, 38400, 56000, 57600, 115200*/ }.Select(it => new KeyValuePair<int, int>(it, it)).ToList();
            comboBox2.DataSource = new List<string> { "DUPA" }.Select(it => new KeyValuePair<string, string>(it, it)).ToList();
            comboBox3.DataSource = Enum.GetNames(typeof(Transmission)).Select(it => new KeyValuePair<Transmission, string>((Transmission)Enum.Parse(typeof(Transmission), it), it.Replace("T_",""))).ToList();
            comboBox4.DataSource = Enumerable.Range(1, 1000).Select(it => ((double)it) / 100).Select(it => new KeyValuePair<double, double>(it, it)).ToList();
            comboBox5.DataSource = Enumerable.Range(1, 1000).Select(it => ((double)it) / 100).Select(it => new KeyValuePair<double, double>(it, it)).ToList();
            comboBox6.DataSource = Enum.GetNames(typeof(Station)).Select(it => new KeyValuePair<Station, string>((Station)Enum.Parse(typeof(Station), it), it.Replace('_', '/'))).ToList();
            comboBox7.DataSource = Enumerable.Range(0, 8).Select(it => new KeyValuePair<int, int>(it, it)).ToList();
            comboBox8.DataSource = Enumerable.Range(0, 6).Select(it => new KeyValuePair<int, int>(it, it)).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetAllEnabled(false);
        }
        private void SetAllEnabled(bool enabled)
        {
            foreach (var control in tableLayoutPanel1.Controls)
            {
                if (control is Control)
                    ((Control)control).Enabled = enabled;
            }
            button1.Enabled = enabled;
            button2.Enabled = !enabled;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetAllEnabled(true);
        }
    }
}
