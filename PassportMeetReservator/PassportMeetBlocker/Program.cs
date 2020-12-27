using System;
using System.Threading;
using System.Windows.Forms;

using Autofac;

using Common;
using Common.Services;

namespace PassportMeetBlocker
{
    static class Program
    {
        private static Logger Logger = CommonDependencyHolder.ServiceDependencies.Resolve<Logger>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            Application.Run(new MainForm());
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Logger.LogCriticalError(e.Exception);
        }
    }
}
