using System;
using System.IO;

using Common.Services;
using Common.Extensions;

namespace PassportMeetBlocker.Services
{
    public class BlockerLogger : LoggerBase
    {
        private static DateTime RUN_TIME = DateTime.Now;
        private static string RUN_TIME_STRING = RUN_TIME.GetFormattedDateForFileName();

        public string Modifier { get; set; }
        private string ModifiedLogFilename => Modifier == null ? $"Log_{RUN_TIME_STRING}.txt" : $"Log_{RUN_TIME_STRING}_{Modifier}.txt";

        protected override string CommonMainLogFile => Path.Combine(
            Environment.CurrentDirectory, "Log", "Blocker", ModifiedLogFilename
        );

        protected override string CriticalsLogFile => Path.Combine(
            Environment.CurrentDirectory, "Log", "Blocker", "Criticals", $"{RUN_TIME_STRING}_CRITICALS.txt"
        );
    }
}
