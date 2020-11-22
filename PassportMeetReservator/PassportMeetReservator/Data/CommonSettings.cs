using System;

namespace PassportMeetReservator.Data
{
    public class CommonSettings
    {
        public string Platform { get; set; }
        public string City { get; set; }
        public int? Operation { get; set; }
        public DateTime ReservationDateStart { get; set; }
        public DateTime ReservationDateEnd { get; set; }
    }
}
