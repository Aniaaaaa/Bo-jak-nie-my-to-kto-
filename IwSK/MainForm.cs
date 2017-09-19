using rs232.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IwSK
{
    public partial class MainForm : Form
    {
        private readonly IRs232Service service;
        public MainForm()
        {
            service = new Rs232Service();

            InitializeComponent();
        }
    }
}
