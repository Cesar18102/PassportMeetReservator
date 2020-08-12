using System;

namespace PassportMeetReservator.Data
{
    public class ReservedEventArgs : EventArgs
    {
        public string Url { get; private set; }

        public ReservedEventArgs(string url)
        {
            Url = url;
        }
    }
}
