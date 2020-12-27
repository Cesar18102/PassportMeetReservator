using System;
using System.Threading;
using System.Windows.Forms;

using Autofac;

using CefSharp;
using CefSharp.WinForms;

using Common.Services;

namespace PassportMeetReservator
{
    public static class Program
    {
        private static Logger Logger = DependencyHolder.ServiceDependencies.Resolve<Logger>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            var settings = new CefSettings();
            settings.BrowserSubprocessPath = Environment.CurrentDirectory + "/x86/CefSharp.BrowserSubprocess.exe";

            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);

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
