using System;

namespace Common.Data.CustomEventArgs
{
    public class DateCheckerOkEventArgs : EventArgs
    {
        public string Content { get; private set; }

        public DateCheckerOkEventArgs(string content)
        {
            Content = content;
        }
    }
}
