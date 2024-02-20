using KtermMonitor.IO.EventArgs;
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

        private void _onCommandLineReceived(ReceivedDataEventArgs e)
        {
            var cmdLines = e.ReceivedData.Split(new string[] { @":" }, StringSplitOptions.RemoveEmptyEntries);
            if (cmdLines.Length < 2) return;
            if (cmdLines[0] == "-c")
            {
                var kShellCommand = cmdLines[1];
                kShellCommand = kShellCommand.Trim(new char[] { '\"' });
            }

            if (InvokeRequired)
            {
                Invoke(new Action(() => { this.Activate(); }));
            }
            
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

            _pipeServer = new NampedPipeServer("COMMANDLINE", _onCommandLineReceived);

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
