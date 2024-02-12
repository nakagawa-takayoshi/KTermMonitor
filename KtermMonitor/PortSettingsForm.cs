using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KtermMonitor
{
    /// <summary>
    /// ポート設定フォーム
    /// </summary>
    public partial class PortSettingsForm : Form
    {
        string _selectedPortName = string.Empty;

        public string SelectedPortName => _selectedPortName;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PortSettingsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームロードのハンドラ
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var portNames = SerialPort.GetPortNames();
            _portNamesCb.Items.AddRange(portNames);

            _portNamesCb.Items.Add("デバッグポート");

            if (_portNamesCb.Items.Count > 0)
            {
                _portNamesCb.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// "OK"ボタンのクリックハンドラ
        /// </summary>
        private void _okButton_Click(object sender, EventArgs e)
        {
            _selectedPortName = _portNamesCb.SelectedItem.ToString();

            DialogResult = DialogResult.OK;

            Close();
        }
    }
}
