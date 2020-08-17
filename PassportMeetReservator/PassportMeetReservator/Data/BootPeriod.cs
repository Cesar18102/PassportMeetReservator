using System;

namespace PassportMeetReservator.Data
{
    public class BootPeriod
    {
        public TimeSpan TimeStart { get; set; } = DateTime.Now.TimeOfDay;
        public TimeSpan TimeEnd { get; set; } = DateTime.Now.TimeOfDay;

        public bool IsIside(TimeSpan time)
        {
            if(TimeEnd >= TimeStart)
                return time >= TimeStart && time <= TimeEnd;
            return time >= TimeStart || time <= TimeEnd;
        }
    }
}
