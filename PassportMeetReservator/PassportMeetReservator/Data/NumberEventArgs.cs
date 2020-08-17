using System;

namespace PassportMeetReservator.Data
{
    public class NumberEventArgs : EventArgs
    {
        public int Number { get; private set; }

        public NumberEventArgs(int number)
        {
            Number = number;
        }
    }
}
