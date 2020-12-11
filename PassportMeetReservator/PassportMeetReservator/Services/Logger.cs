using PassportMeetReservator.Extensions;

using System;
using System.Collections.Generic;
using System.IO;

namespace PassportMeetReservator.Services
{
    public class Logger
    {
        #region Fields

        private static DateTime RUN_TIME = DateTime.Now;
        private static string RUN_TIME_STRING = RUN_TIME.GetFormattedDateForFileName();

        private static string COMMON_MAIN_LOG_FILE = Path.Combine(
            Environment.CurrentDirectory, "Log", "Main", "Common", $"Log_{RUN_TIME_STRING}.txt"
        );

        private static string MAIN_LOG_BY_BWOSERS_PATH = Path.Combine(
            Environment.CurrentDirectory, "Log", "Main", RUN_TIME_STRING
        );

        private static string ITERATION_LOG_BY_BWOSERS_PATH = Path.Combine(
            Environment.CurrentDirectory, "Log", "Iteration", RUN_TIME_STRING
        );

        private List<object> Lockers = new List<object>();

        #endregion

        #region Methods
        #region Public

        public void CreateCommonLogFile()
        {
            using (File.Create(COMMON_MAIN_LOG_FILE)) ;
        }

        public void CreateLogFilesForBrowser(int browser)
        {
            CreateLogFileForBrowser(MAIN_LOG_BY_BWOSERS_PATH, browser);
            CreateLogFileForBrowser(ITERATION_LOG_BY_BWOSERS_PATH, browser);
        }

        public void LogMain(string text, int? browser)
        {
            string fullLog = GetLogWithMeta(text, browser);
            WriteCommonLog(fullLog);

            if (browser.HasValue)
                WriteBrowserLog(fullLog, MAIN_LOG_BY_BWOSERS_PATH, browser.Value);
        }

        public void LogIteration(string text, int browser)
        {
            string fullLog = GetLogWithMeta(text, browser);
            WriteBrowserLog(fullLog, ITERATION_LOG_BY_BWOSERS_PATH, browser);
        }

        public string GetLogWithMeta(string text, int? browser)
        {
            DateTime now = DateTime.Now;
            string browserText = browser.HasValue ? $"[BROWSER = {browser + 1}]" : "";
            return $"[TIME = {now.ToString()}:{now.Millisecond}] {browserText}: {text}\n";
        }

        #endregion
        #region Private

        private void WriteLog(string log, string file, object locker)
        {
            lock (locker)
            {
                using (StreamWriter strw = File.AppendText(file))
                    strw.Write(log);
            }
        }

        private void WriteCommonLog(string log)
        {
            WriteLog(log, COMMON_MAIN_LOG_FILE, this);
        }

        private void WriteBrowserLog(string log, string baseDir, int browser)
        {
            string logFilePath = Path.Combine(
                baseDir, GetBrowserLogFileName(browser)
            );

            WriteLog(log, logFilePath, Lockers[browser]);
        }

        private string GetBrowserLogFileName(int browser)
        {
            return $"Browser_{browser + 1}_log.txt";
        }

        private void CreateLogFileForBrowser(string baseDir, int browser)
        {
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            string browserLogPath = Path.Combine(
                baseDir, GetBrowserLogFileName(browser)
            );

            if (!File.Exists(browserLogPath))
            {
                using (File.Create(browserLogPath)) ;
                Lockers.Add(new object());
            }
        }

        #endregion
        #endregion
    }
}
