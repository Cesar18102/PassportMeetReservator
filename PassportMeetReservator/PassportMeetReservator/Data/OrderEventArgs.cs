using System;

namespace PassportMeetReservator.Data
{
    public class OrderEventArgs : EventArgs
    {
        public ReservationOrder Order { get; private set; }

        public OrderEventArgs(ReservationOrder order)
        {
            Order = order;
        }
    }
}
