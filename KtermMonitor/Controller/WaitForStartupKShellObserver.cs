using KtermMonitor.Controller.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KtermMonitor.Controller
{
    internal class WaitForStartupKShellObserver
        : IObserver
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WaitForStartupKShellObserver() 
        { 
        }

        /// <summary>
        /// 実行
        /// </summary>
        public void Execution(ISubject subject)
        {
            const string compareString = "KSHELL: ";

            if (1 != subject.State) return;

            if (!subject.Message.Contains(compareString)) return;

            Console.WriteLine("KShell Start Complete");

            subject.State = 2;
        }
    }
}
