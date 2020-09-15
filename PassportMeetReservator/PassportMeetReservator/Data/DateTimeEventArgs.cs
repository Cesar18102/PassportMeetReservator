using System;

namespace PassportMeetReservator.Data
{
    public class DateTimeEventArgs : EventArgs
    {
        public DateTime Date { get; private set; }

        public DateTimeEventArgs(DateTime date)
        {
            Date = date;
        }
    }
}
