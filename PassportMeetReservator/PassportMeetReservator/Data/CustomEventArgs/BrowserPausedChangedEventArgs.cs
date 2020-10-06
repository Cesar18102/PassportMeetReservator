using System;

namespace PassportMeetReservator.Data.CustomEventArgs
{
    public class BrowserPausedChangedEventArgs : EventArgs
    {
        public int BrowserNumber { get; private set; }
        public bool Paused { get; private set; }

        public BrowserPausedChangedEventArgs(int browserNumber, bool paused)
        {
            BrowserNumber = browserNumber;
            Paused = paused;
        }
    }
}
