namespace PassportMeetReservator.Data
{
    public class DelayInfo
    {
        public int ActionResultDelay { get; set; } = 100;
        public int DiscreteWaitDelay { get; set; } = 25;
        public int ManualReactionWaitDelay { get; set; } = 500 * 1000;
        public int RefreshSessionUpdateDelay { get; set; } = 1000;
        public int PostInputDelay { get; set; } = 10000;
        public int DateCheckDelay { get; set; } = 200;
    }
}
