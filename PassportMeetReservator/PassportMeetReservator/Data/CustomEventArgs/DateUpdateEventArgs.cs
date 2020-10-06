using System;

namespace PassportMeetReservator.Data.CustomEventArgs
{
    public class DateUpdateEventArgs
    {
        public DateTime[] Dates { get; private set; }

        public DateUpdateEventArgs(DateTime[] dates)
        {
            Dates = dates;
        }
    }
}
