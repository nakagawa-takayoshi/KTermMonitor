using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KtermMonitor.Controller.Interface
{
    internal interface ISubject
    {
        int State { get; set; }

        string Message { get; }
    }
}
