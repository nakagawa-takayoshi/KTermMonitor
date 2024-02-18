using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KtermMonitor.Controller.Interface
{
    internal interface IObserver
    {
        void Execution(ISubject subject);
    }
}
