using System;
using System.IO;
using System.Linq;

namespace Common.Services
{
    public abstract class LoggerBase
    {
        #region Properties

        protected abstract string CommonMainLogFile { get; }
        protected abstract string CriticalsLogFile { get; }

        #endregion

        #region Methods
        #region Public

        public void CreateCommonLogFile()
        {
            CreateFile(CommonMainLogFile);
            CreateFile(CriticalsLogFile);
        }

        public void LogMain(string text)
        {
            string fullLog = GetLogWithMeta(text);
            WriteCommonLog(fullLog);
        }

        public void LogCriticalError(Exception ex)
        {
            string log = $"{GetExceptionLog(ex)}\n\n***********************************************\n\n";
            WriteLog(log, CriticalsLogFile, this);
        }

        public string GetLogWithMeta(string text)
        {
            DateTime now = DateTime.Now;
            return $"[TIME = {now.ToString()}:{now.Millisecond}]: {text}\n";
        }

        #endregion
        #region Protected

        protected void CreateFile(string path)
        {
            string dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (File.Create(path)) ;
        }

        protected string GetExceptionLog(Exception ex)
        {
            string innerLog = "";

            if (ex.InnerException != null)
            {
                string rawInnerLog = GetExceptionLog(ex.InnerException);
                innerLog = string.Join("\n", rawInnerLog.Split('\n').Select(line => '\t' + line));
            }

            return $"MESSAGE: {ex.Message}\n" +
                   $"STACK TRACE: {ex.StackTrace}\n" +
                   $"INNER: \n{innerLog}";
        }

        protected void WriteLog(string log, string file, object locker)
        {
            lock (locker)
            {
                using (StreamWriter strw = File.AppendText(file))
                    strw.Write(log);
            }
        }

        protected void WriteCommonLog(string log)
        {
            WriteLog(log, CommonMainLogFile, this);
        }

        #endregion
        #endregion
    }
}
