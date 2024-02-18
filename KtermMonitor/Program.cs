using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using KtermMonitor.IO.NampedPipe;

namespace KtermMonitor
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            const string MutexNamke = "KtermMonitor"; // 二重起動防止用のMutex名
            var Mutex = new Mutex(true, MutexNamke, out var createdNew);
            if (!createdNew)
            {
                findKShellProcess();

                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainAppForm());
        }


        static void findKShellProcess()
        {
            var ps = System.Diagnostics.Process.GetProcessesByName("KtermMonitor");
            if (0 < ps.Length)
            {
                var args = Environment.GetCommandLineArgs();
                if (args.Length < 2) return;

                int ii = 0;
                foreach (var arg in args)
                {
                    if (arg.Contains("-c"))
                    {
                        var nextIndex = ((ii + 1) + 1);
                        if (args.Length < nextIndex) return;

                        StringBuilder sb = new StringBuilder();
                        sb.Append(arg);
                        sb.Append(":");
                        sb.Append("\"");
                        sb.Append(args[nextIndex-1]);
                        sb.Append("\"");

                        var comandLibePipeClient = new NampedPipeClient("COMMANDLINE");
                        comandLibePipeClient.Send(sb.ToString());
                        return;
                    }

                    ii++;
                }

                return;
            }
        }

    }



}
