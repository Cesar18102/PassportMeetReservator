using System;

namespace Common.Data.CustomEventArgs
{
    public class DateCheckerErrorEventArgs : EventArgs
    {
        public int ErrorCode { get; private set; }

        public DateCheckerErrorEventArgs(int errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}
