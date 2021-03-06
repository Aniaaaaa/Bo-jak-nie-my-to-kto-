﻿using rs232.Services;
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
using System.Threading;
using System.Text.RegularExpressions;

namespace rs232
{
    public partial class Form1 : Form
    {
        private readonly IRs232Service service;

        public Form1()
        {
            service = new Rs232Service();

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = new List<int> { 150, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 56000, 57600, 115200 }.Select(it => new KeyValuePair<int, int>(it, it)).ToList();
            comboBox2.DataSource = new List<int> { 1, 2 }.Select(it => new KeyValuePair<int, int>(it, it)).ToList();
            comboBox3.DataSource = new List<int> { 7, 8 }.Select(it => new KeyValuePair<int, int>(it, it)).ToList();
            comboBox4.DataSource = Enum.GetNames(typeof(Terminator)).Select(it => new KeyValuePair<Terminator, string>((Terminator)Enum.Parse(typeof(Terminator), it), it)).ToList();
            comboBox5.DataSource = service.GetPortNames().Select(it => new KeyValuePair<string, string>(it, it)).ToList();
            comboBox6.DataSource = Enum.GetNames(typeof(FlowControl)).Select(it => new KeyValuePair<FlowControl, string>((FlowControl)Enum.Parse(typeof(FlowControl), it), it.Replace('_', '/'))).ToList();
            comboBox7.DataSource = Enum.GetNames(typeof(Parity)).Select(it => new KeyValuePair<Parity, string>((Parity)Enum.Parse(typeof(Parity), it), it)).ToList();
            comboBox8.DataSource = Enumerable.Range(1,1000).Select(it => ((double)it)/100).Select(it => new KeyValuePair<double, double>(it, it)).ToList();
            comboBox9.DataSource = Enum.GetNames(typeof(DataType)).Select(it => new KeyValuePair<DataType, string>((DataType)Enum.Parse(typeof(DataType),it), it)).ToList();
            SetAllEnabled(true);

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    var message = service.ReceiveMessage();

                    if (!string.IsNullOrEmpty(message))
                    {
                        try
                        {
                            this.textBox2.AppendText(Environment.NewLine);
                            this.textBox2.AppendText(message);
                        }
                        catch
                        {

                        }
                    }
                }
            }).Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var portParameters = new PortParameters
            {
                Speed = Int32.Parse(this.comboBox1.Text),
                StopBits = (StopBits)Int32.Parse(this.comboBox2.Text),
                DataBits = Int32.Parse(this.comboBox3.Text),
                Terminator = (Terminator)Enum.Parse(typeof(Terminator), this.comboBox4.Text),
                MyTerminator = (Terminator)Enum.Parse(typeof(Terminator), this.comboBox4.Text)
                    == Terminator.WŁASNY ? this.textBox1.Text : string.Empty,
                PortName = this.comboBox5.Text,
                FlowControl = (FlowControl)Enum.Parse(typeof(FlowControl), this.comboBox6.Text.Replace('/', '_')),
                Parity = (Parity)Enum.Parse(typeof(Parity), this.comboBox7.Text),
                Timeout = Double.Parse(this.comboBox8.Text),
                DataType = (DataType)Enum.Parse(typeof(DataType), this.comboBox9.Text)
            };

            this.textBox2.Text = "Konfiguracja portu...";

            var isOpen = service.OpenPort(portParameters);
            SetAllEnabled(!isOpen);

            if (isOpen)
            {
                this.textBox2.AppendText(Environment.NewLine);
                this.textBox2.Text += "Konfiguracja przebiegła pomyślnie.";
            }
            else
            {
                this.textBox2.AppendText(Environment.NewLine);
                this.textBox2.Text += "Konfiguracja NIE przebiegła pomyślnie.";
            }
        }
        private void SetAllEnabled(bool enabled)
        { 
            foreach(var control in tableLayoutPanel1.Controls)
            {
                if (control is Control)
                    ((Control)control).Enabled = enabled;
            }
            if(enabled)
            {
                var value = comboBox5.SelectedValue;
                comboBox5.DataSource = service.GetPortNames().Select(it => new KeyValuePair<string, string>(it, it)).ToList();
                try
                {
                    comboBox5.SelectedValue = value;
                }
                catch (Exception) { }
            }

            button1.Enabled = enabled;
            button2.Enabled = !enabled;
            button3.Enabled = !enabled;
            textBox3.Enabled = !enabled;
            button12.Enabled = !enabled; // "wyślij"
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
            service.ClosePort();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Terminator terminator = (Terminator)comboBox4.SelectedValue;
            SetOwnTerminatorVisible(terminator == Terminator.WŁASNY);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var message = this.textBox3.Text;
            var encodedMessage = message;
            DataType transferType = (DataType)comboBox9.SelectedValue;
            if (transferType == DataType.HEX)
            {
                if (!Regex.IsMatch(textBox3.Text, "^([0-9a-fA-F]{2}( )?)*$"))
                {
                    MessageBox.Show("Nieprawidłowy ciąg bajtów.", "Błąd walidacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                encodedMessage = service.HexToString(encodedMessage.Replace(" ", ""));
            }
            service.SendMessage(encodedMessage);
            this.textBox2.AppendText(Environment.NewLine);
            this.textBox2.AppendText($"[out] {message}");
            this.textBox3.Text = string.Empty;
            this.textBox3.Focus();
        }

        private void hexByte_Validating(object sender, CancelEventArgs e)
        {
            if(!Regex.IsMatch(((TextBox)sender).Text, "^([0-9a-fA-F]{2}( )?)*$"))
            {
                e.Cancel = true;
                MessageBox.Show("Nieprawidłowy ciąg bajtów.", "Błąd walidacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            service.SendPing();
        }
    }
}
