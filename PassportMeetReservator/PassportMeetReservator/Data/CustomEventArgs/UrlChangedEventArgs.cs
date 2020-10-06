using System;

namespace PassportMeetReservator.Data.CustomEventArgs
{
    public class UrlChangedEventArgs : EventArgs
    {
        public string Url { get; private set; }

        public UrlChangedEventArgs(string url)
        {
            Url = url;
        }
    }
}
