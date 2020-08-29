namespace PassportMeetReservator.Data
{
    public class ReservedInfo
    {
        public string Url { get; private set; }

        public ReservedInfo( string url)
        {
            Url = url;
        }
    }
}
