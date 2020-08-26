using System;

namespace PassportMeetReservator.Data
{
    public class LogEventArgs : EventArgs
    {
        public string LogText { get; private set; }

        public LogEventArgs(string text)
        {
            LogText = text;
        }
    }
}
