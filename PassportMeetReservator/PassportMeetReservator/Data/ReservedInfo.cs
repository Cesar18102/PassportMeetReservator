namespace PassportMeetReservator.Data
{
    public class ReservedInfo
    {
       // public ReservationOrder Order { get; private set; }
        public string Url { get; private set; }

        public ReservedInfo( string url)
        {
           // Order = order;
            Url = url;
        }
    }
}
