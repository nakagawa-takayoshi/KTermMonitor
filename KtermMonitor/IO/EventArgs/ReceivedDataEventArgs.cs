using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KtermMonitor.IO.EventArgs
{
    internal class ReceivedDataEventArgs: System.EventArgs
    {
        public string ReceivedData { get; }

        public ReceivedDataEventArgs(string data)
        {
            ReceivedData = data;
        }
    }
}
