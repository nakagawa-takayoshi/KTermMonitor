using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KtermMonitor.Controller
{
    internal class TaskCancelController
    {
        private Task _cancelWaitTask;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TaskCancelController(Task cencelTask)
        {
            _cancelWaitTask = cencelTask;
        }


        public void WaitForCancelTask(CancellationTokenSource cancelTokenSource)
        {
            if (_cancelWaitTask.IsCompleted) return;

            cancelTokenSource.Cancel();
            var task = Task.FromCanceled(cancelTokenSource.Token);
        }
    }
}
