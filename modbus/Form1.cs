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
using rs232.Services.Model;

namespace rs232
{
    public partial class Form1 : Form
    {
        private readonly IRs232Service service;

        public Form1()
        {
            service = new Rs232Service();

            var tmp = new System.IO.Ports.SerialPort();
            //tmp.PortName = 
            //tmp.BaudRate

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = new List<int> { 100, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 56000, 57600, 115200 }.Select(it => new KeyValuePair<int, int>(it, it)).ToList();
            comboBox2.DataSource = new List<int> { 1, 2 }.Select(it => new KeyValuePair<int, int>(it, it)).ToList();
            comboBox3.DataSource = new List<int> { 5, 6, 7, 8 }.Select(it => new KeyValuePair<int, int>(it, it)).ToList();
            comboBox4.DataSource = Enum.GetNames(typeof(Terminator)).Select(it => new KeyValuePair<Terminator, string>((Terminator)Enum.Parse(typeof(Terminator), it), it)).ToList();
            comboBox5.DataSource = service.GetPortNames().Select(it => new KeyValuePair<string, string>(it, it)).ToList();
            comboBox6.DataSource = Enum.GetNames(typeof(FlowControl)).Select(it => new KeyValuePair<FlowControl, string>((FlowControl)Enum.Parse(typeof(FlowControl), it), it.Replace('_', '/'))).ToList();
            comboBox7.DataSource = Enum.GetNames(typeof(Parity)).Select(it => new KeyValuePair<Parity, string>((Parity)Enum.Parse(typeof(Parity), it), it)).ToList();
            comboBox8.DataSource = Enumerable.Range(1,1000).Select(it => ((double)it)/100).Select(it => new KeyValuePair<double, double>(it, it)).ToList();
            comboBox9.DataSource = Enum.GetNames(typeof(DataType)).Select(it => new KeyValuePair<DataType, string>((DataType)Enum.Parse(typeof(DataType),it), it)).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var control in tableLayoutPanel1.Controls)
            {
                if (control is Control)
                    ((Control)control).Enabled = false;
            }
            button1.Enabled = false;
            button2.Enabled = true;

            var portParameters = new PortParameters
            {
                Speed = Int32.Parse(this.comboBox1.Text),
                StopBits = Int32.Parse(this.comboBox2.Text),
                DataBits = Int32.Parse(this.comboBox3.Text),
                Terminator = (Terminator)Enum.Parse(typeof(Terminator), this.comboBox4.Text),
                MyTerminator = (Terminator)Enum.Parse(typeof(Terminator), this.comboBox4.Text)
                    == Terminator.WŁASNY ? this.textBox1.Text : string.Empty,
                PortName = this.comboBox5.Text,
                FlowControl = (FlowControl)Enum.Parse(typeof(FlowControl), this.comboBox6.Text),
                Parity = (Parity)Enum.Parse(typeof(Parity), this.comboBox7.Text),
                Timeout = Int32.Parse(this.comboBox8.Text),
                DataType = (DataType)Enum.Parse(typeof(DataType), this.comboBox9.Text)
            };

            service.SetParameters(portParameters);
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
            foreach (var control in tableLayoutPanel1.Controls)
            {
                if (control is Control)
                    ((Control)control).Enabled = true;
            }
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Terminator terminator = (Terminator)comboBox4.SelectedValue;
            SetOwnTerminatorVisible(terminator == Terminator.WŁASNY);
        }
    }
}
