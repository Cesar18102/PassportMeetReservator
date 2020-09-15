namespace PassportMeetReservator.Data
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
