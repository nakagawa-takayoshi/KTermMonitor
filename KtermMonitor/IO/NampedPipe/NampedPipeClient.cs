using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KtermMonitor.IO.NampedPipe
{
    internal class NampedPipeClient : IDisposable
    {
        const string baseName = "KTerm::Monitor;;";

        string _pipeName;

        PipeStream _pipeClient = null;

        bool IsConnected => _pipeClient.IsConnected;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NampedPipeClient(string pipeName) 
        { 
            _pipeName = baseName + pipeName;

            // パイプの接続
            _connect();
        }

        /// <summary>
        /// リソースの解放
        /// </summary>
        void IDisposable.Dispose()
        {
            // パイプの開放
            _pipeDispose();
        }

        /// <summary>
        /// ハイプの接続
        /// </summary>
        private void _connect()
        {
            _pipeClient = new NamedPipeClientStream(".", _pipeName, PipeDirection.Out, PipeOptions.None);
            if (!_pipeClient.IsConnected) return;
        }

        /// <summary>
        /// パイプの開放
        /// </summary>
        private void _pipeDispose()
        {
            if (_pipeClient == null) return;
            if (!_pipeClient.IsConnected) return;
            _pipeClient.Close();

            _pipeClient.Dispose();
        }
    }
}
