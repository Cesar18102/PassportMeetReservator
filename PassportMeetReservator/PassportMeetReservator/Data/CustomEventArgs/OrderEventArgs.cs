namespace PassportMeetReservator.Data.CustomEventArgs
{
    public class OrderEventArgs
    {
        public ReservationOrder Order { get; private set; }

        public OrderEventArgs(ReservationOrder order)
        {
            Order = order;
        }
    }
}
