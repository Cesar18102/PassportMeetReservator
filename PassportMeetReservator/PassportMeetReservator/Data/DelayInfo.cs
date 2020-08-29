namespace PassportMeetReservator.Data
{
    public class DelayInfo
    {
        public int OrderLoadingIterationDelay { get; set; } = 100;
        public int BrowserIterationDelay { get; set; } = 100;
        public int ActionResultDelay { get; set; } = 100;
        public int PostInputDelay { get; set; } = 1000;
        public int DiscreteWaitDelay { get; set; } = 25;
        public int ManualReactionWaitDelay { get; set; } = 500 * 1000;
    }
}
