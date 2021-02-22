using System;
using System.IO;
using System.Collections.Generic;

using Common.Services;
using Common.Extensions;

namespace PassportMeetReservator.Services
{
    public class ReservatorLogger : LoggerBase
    {
        #region Fields

        private List<object> Lockers = new List<object>();

        private static DateTime RUN_TIME = DateTime.Now;
        private static string RUN_TIME_STRING = RUN_TIME.GetFormattedDateForFileName();

        private static string COMMON_MAIN_LOG_FILE = Path.Combine(
            Environment.CurrentDirectory, "Log", "Main", "Common", $"Log_{RUN_TIME_STRING}.txt"
        );

        private static string CRITICALS_LOG_FILE = Path.Combine(
            Environment.CurrentDirectory, "Log", "Main", "Criticals", $"{RUN_TIME_STRING}_CRITICALS.txt"
        );

        private static string MAIN_LOG_BY_BWOSERS_PATH = Path.Combine(
            Environment.CurrentDirectory, "Log", "Main", "Browsers", RUN_TIME_STRING
        );

        private static string ITERATION_LOG_BY_BWOSERS_PATH = Path.Combine(
            Environment.CurrentDirectory, "Log", "Iteration", RUN_TIME_STRING
        );

        #endregion

        #region Properties

        protected override string CommonMainLogFile => COMMON_MAIN_LOG_FILE;
        protected override string CriticalsLogFile => CRITICALS_LOG_FILE;

        #endregion

        #region Methods
        #region Public

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
            string browserLogPath = Path.Combine(
                baseDir, GetBrowserLogFileName(browser)
            );

            if (!File.Exists(browserLogPath))
            {
                CreateFile(browserLogPath);
                Lockers.Add(new object());
            }
        }

        #endregion
        #endregion
    }
}
