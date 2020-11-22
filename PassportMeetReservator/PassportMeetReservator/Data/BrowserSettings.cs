using System;

namespace PassportMeetReservator.Data
{
    public class BrowserSettings : CommonSettings
    {
        public int BrowserNumber { get; set; }
        public DateTime ReservationTimeStart { get; set; }
        public DateTime ReservationTimeEnd { get; set; }
    }
}
