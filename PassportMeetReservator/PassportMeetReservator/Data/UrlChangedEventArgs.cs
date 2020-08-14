using System;

namespace PassportMeetReservator.Data
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
