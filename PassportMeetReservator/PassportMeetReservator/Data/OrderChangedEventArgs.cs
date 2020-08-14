using System;

namespace PassportMeetReservator.Data
{
    public class OrderChangedEventArgs : EventArgs
    {
        public ReservationOrder Order { get; private set; }

        public OrderChangedEventArgs(ReservationOrder order)
        {
            Order = order;
        }
    }
}
