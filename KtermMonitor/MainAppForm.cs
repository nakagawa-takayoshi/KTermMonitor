using KtermMonitor.IO.NampedPipe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KtermMonitor
{
    public partial class MainAppForm : Form
    {

        private NampedPipeServer _pipeServer;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainAppForm()
        {
            InitializeComponent();
        }

        private void _onReceived(EventArgs e)
        {
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

        /// <summary>
        /// フォームロードのハンドラ
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _pipeServer = new NampedPipeServer("Recv", _onReceived);

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _pipeServer.Dispose();
        }

        private void _settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PortSettingsForm dlg = new PortSettingsForm();

            var dr = dlg.ShowDialog();

            if (DialogResult.Cancel == dr) return;


        }
    }
}
