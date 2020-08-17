using System;

namespace PassportMeetReservator.Data
{
    public class OrderNumberEventArgs : EventArgs
    {
        public int Number { get; private set; }

        public OrderNumberEventArgs(int number)
        {
            Number = number;
        }
    }
}
