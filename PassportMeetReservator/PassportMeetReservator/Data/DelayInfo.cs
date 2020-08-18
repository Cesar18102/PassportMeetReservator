namespace PassportMeetReservator.Data
{
    public class DelayInfo
    {
        public int OrderLoadingIterationDelay { get; set; } = 100;
        public int BrowserIterationDelay { get; set; } = 100;
        public int ActionResultDelay { get; set; } = 100;
    }
}
