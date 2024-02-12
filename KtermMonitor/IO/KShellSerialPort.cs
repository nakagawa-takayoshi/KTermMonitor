using KtermMonitor.Controller;
using KtermMonitor.IO.EventArgs;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KtermMonitor.IO
{
    internal class KShellSerialPort : IDisposable
    {
        #region 静的メンバ

        public static bool _isOpen = false;

        public static bool IsOpen => _isOpen;

        #endregion

        private SerialPort _serialPort;

        private Action<ReceivedDataEventArgs> _onReceivedHandler = null;

        private CompositeDisposable _disposables = new CompositeDisposable();

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private Task _dataRecivedTask;

        /// <summary>
        /// プロパティ
        /// </summary>

        public string PortName { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KShellSerialPort(string portName, Action<ReceivedDataEventArgs> receivedHandler)
        {
            PortName = portName;

            _serialPort = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
            _serialPort.Open();

            _isOpen = _serialPort.IsOpen;

            if (!_serialPort.IsOpen) return;

            _disposables.Add(Observable.FromEvent<EventHandler<ReceivedDataEventArgs>, ReceivedDataEventArgs>(
                                                h => (s, e) => h(e),
                                                h => _onReceivedHandler += receivedHandler,
                                                h => _onReceivedHandler -= receivedHandler)
                                                .Subscribe(e => receivedHandler(e)));
            _dataRecivedTask = Task.Run(() => _dataRecivedTaskProc());
        }

        
        /// <summary>
        /// データ受信処理
        /// </summary>
        void _dataRecivedTaskProc()
        {
            if (_onReceivedHandler is null) return; 
            var cancelToken = _cancellationTokenSource.Token;

            while(!cancelToken.IsCancellationRequested)
            {
                if (!_serialPort.IsOpen) return;

                _serialPort.ReadTimeout = 1000;
                if (0 >= _serialPort.BytesToRead) continue;
                var readLineText = _serialPort.ReadLine();

                if (readLineText.Length <= 0) continue;

                _onReceivedHandler(new ReceivedDataEventArgs(readLineText));
            }
        }

        /// <summary>
        /// ポートクローズ
        /// </summary>
        public void Close()
        {
            if (!_serialPort.IsOpen) return;

            var taskCancelController = new TaskCancelController(_dataRecivedTask);
            taskCancelController.WaitForCancelTask(_cancellationTokenSource);

            _serialPort.Close();
        }

        public void Dispose()
        {
            if (_dataRecivedTask.IsCanceled) return;

            Close();
        }

    }
}
