using KtermMonitor.IO.NampedPipe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KtermMonitor
{
    public partial class MainAppForm : Form
    {
        public MainAppForm()
        {
            InitializeComponent();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (e.KeyChar == (char)Keys.Escape) return;

            if (e.KeyChar == (char)Keys.Enter)
            {
                _monitorView.Text += Environment.NewLine;
            }
            else
            {
                _monitorView.Text += e.KeyChar;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            using (var pipe = new NampedPipeClient("Send"))
            {

            }
        }
    }
}
