using rs232.Services;
using static rs232.Services.Enums.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rs232
{
    public partial class Form1 : Form
    {
        private readonly IRs232Service service;
        public enum Signal { OB, DA, DTR, DSR, RTS, CTS, CD, RI };
        public enum SignalState { Unknown = 0, Inactive = 1, Active = 2 };

        public Form1()
        {
            service = new Rs232Service();

            InitializeComponent();
        }
        private Dictionary<Signal, Button> signals;

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = new List<int> { 150, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 56000, 57600, 115200 }.Select(it => new KeyValuePair<int, int>(it, it)).ToList();
            comboBox2.DataSource = new List<int> { 1, 2 }.Select(it => new KeyValuePair<int, int>(it, it)).ToList();
            comboBox3.DataSource = new List<int> { 5, 6, 7, 8 }.Select(it => new KeyValuePair<int, int>(it, it)).ToList();
            comboBox4.DataSource = Enum.GetNames(typeof(Terminator)).Select(it => new KeyValuePair<Terminator, string>((Terminator)Enum.Parse(typeof(Terminator), it), it)).ToList();
            comboBox5.DataSource = new List<string> { "DUPA" }.Select(it => new KeyValuePair<string, string>(it, it)).ToList();
            comboBox6.DataSource = Enum.GetNames(typeof(FlowControl)).Select(it => new KeyValuePair<FlowControl, string>((FlowControl)Enum.Parse(typeof(FlowControl), it), it.Replace('_', '/'))).ToList();
            comboBox7.DataSource = Enum.GetNames(typeof(Parity)).Select(it => new KeyValuePair<Parity, string>((Parity)Enum.Parse(typeof(Parity), it), it)).ToList();
            comboBox8.DataSource = Enumerable.Range(1,1000).Select(it => ((double)it)/100).Select(it => new KeyValuePair<double, double>(it, it)).ToList();
            comboBox9.DataSource = Enum.GetNames(typeof(DataType)).Select(it => new KeyValuePair<DataType, string>((DataType)Enum.Parse(typeof(DataType),it), it)).ToList();
            signals = new Dictionary<Signal, Button> { { Signal.OB, button4}, { Signal.DA, button5 }, { Signal.DTR, button6 },
                {Signal.DSR, button7 }, { Signal.RTS, button8 }, {Signal.CTS, button9 }, {Signal.CD, button10 }, {Signal.RI, button11 } };
        }

        public void SetSignalState(Signal signal, SignalState state)
        {
            signals[signal].ImageIndex = (int)state;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetAllEnabled(false);
        }
        private void SetAllEnabled(bool enabled)
        { 
            foreach(var control in tableLayoutPanel1.Controls)
            {
                if (control is Control)
                    ((Control)control).Enabled = enabled;
            }
            button1.Enabled = enabled;
            button2.Enabled = !enabled;
            button3.Enabled = !enabled;
        }
        private void SetOwnTerminatorVisible(bool visible)
        {
            if (visible)
                tableLayoutPanel1.RowStyles[4].Height = 25;
            else
                tableLayoutPanel1.RowStyles[4].Height = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetAllEnabled(true);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Terminator terminator = (Terminator)comboBox4.SelectedValue;
            SetOwnTerminatorVisible(terminator == Terminator.WŁASNY);
        }
    }
}
