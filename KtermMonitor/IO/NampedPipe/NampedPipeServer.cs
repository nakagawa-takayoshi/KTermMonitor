using KtermMonitor.Controller;
using KtermMonitor.IO.EventArgs;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KtermMonitor.IO.NampedPipe
{
    internal class NampedPipeServer : IDisposable
    {
        const string baseName = "KTerm::Monitor::";

        string _pipeName;

        private CancellationTokenSource _cancellationTokenSource;

        private CancellationTokenSource _waitConnectCancellationToken;

        private Task _serverTask;

        private Task _waitConnectTask;

        private Action<ReceivedDataEventArgs> _handler;

        private CompositeDisposable _disposables = new CompositeDisposable();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NampedPipeServer(string pipeName, Action<ReceivedDataEventArgs> handler)
        {
            _pipeName = baseName + pipeName;
            _cancellationTokenSource = new CancellationTokenSource();
            _waitConnectCancellationToken = new CancellationTokenSource();
            _handler = handler;
            _serverTask = Task.Run(() => _startServer(_pipeName, _cancellationTokenSource.Token));

            _disposables.Add(Observable.FromEvent<EventHandler<ReceivedDataEventArgs>, ReceivedDataEventArgs>(
                                 h => (s, e) => h(e),
                                 h => _handler += handler,
                                 h => _handler -= handler)
                .Subscribe(e => _handler(e)));
        }

        /// <summary>
        /// サーバーを開始
        /// </summary>
        private void _startServer(string pipeName, CancellationToken token)
        {
            // クライアントの接続を待機
            using (var server = new NamedPipeServerStream(pipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous))
            {
                _waitConnectTask = server.WaitForConnectionAsync(_waitConnectCancellationToken.Token);

                while (!token.IsCancellationRequested)
                {
                    if (!server.IsConnected)
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    // クライアントからのデータを受信
                    var buffer = new byte[1024];
                    var readSize = server.Read(buffer, 0, buffer.Length);
                    if (0 == readSize) continue;

                    var message = Encoding.ASCII.GetString(buffer, 0, readSize);

                    _handler?.Invoke(new ReceivedDataEventArgs(message));

                    _waitConnectTask = server.WaitForConnectionAsync(_waitConnectCancellationToken.Token);
                }
            }
        }

        /// <summary>
        /// サーバーを閉じる
        /// </summary>
        public void Close()
        {
            var connectWaitTaskCancelController = new TaskCancelController(_waitConnectTask);
            connectWaitTaskCancelController.WaitForCancelTask(_waitConnectCancellationToken);

            var serverTaskCancelController = new TaskCancelController(_serverTask);
            serverTaskCancelController.WaitForCancelTask(_cancellationTokenSource);
        }

        /// <summary>
        /// リソースの破棄
        /// </summary>
        public void Dispose()
        {
            Close();
            _disposables?.Dispose();
        }
    }
}       
