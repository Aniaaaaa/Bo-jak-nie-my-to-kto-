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
    public partial class Form1 : Form
    {
        private readonly IRs232Service service;

        public Form1()
        {
            service = new Rs232Service();

            InitializeComponent();
        }
    }
}
