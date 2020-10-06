using System;

namespace PassportMeetReservator.Data.CustomEventArgs
{
    public class ReservedEventArgs : EventArgs
    {
        public string Url { get; private set; }
        public ReservationOrder Order { get; private set; }

        public ReservedEventArgs(string url, ReservationOrder order)
        {
            Url = url;
            Order = order;
        }
    }
}
