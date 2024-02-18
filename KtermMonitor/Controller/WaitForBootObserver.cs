using KtermMonitor.Controller.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace KtermMonitor.Controller
{
    internal class WaitForBootObserver
        : IObserver
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WaitForBootObserver()
        {
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Execution(ISubject subject)
        {
            const string compareString = "enter ready";

            if (!subject.Message.Contains(compareString)) return;

            Console.WriteLine("Boot Complete");

            subject.State = 1;
        }

    }
}
