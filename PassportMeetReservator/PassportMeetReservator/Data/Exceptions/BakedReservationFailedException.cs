using System;

namespace PassportMeetReservator.Data.Exceptions
{
    public class BakedReservationFailedException : Exception
    {
        public string Reason { get; set; }

        public BakedReservationFailedException(string reason)
        {
            Reason = reason;
        }
    }
}
