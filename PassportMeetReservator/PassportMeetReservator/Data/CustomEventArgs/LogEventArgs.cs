using System;

namespace PassportMeetReservator.Data.CustomEventArgs
{
    public class LogEventArgs : EventArgs
    {
        public string LogText { get; private set; }
        public int BrowserNumber { get; private set; }

        public LogEventArgs(string text, int browserNumber)
        {
            LogText = text;
            BrowserNumber = browserNumber;
        }
    }
}
