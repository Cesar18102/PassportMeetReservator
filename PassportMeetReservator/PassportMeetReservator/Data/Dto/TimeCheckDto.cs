using Newtonsoft.Json;

using System;

namespace PassportMeetReservator.Data.Dto
{
    public class TimeCheckDto
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("operationId")]
        public int OperationId { get; private set; }

        [JsonProperty("reservationId")]
        public string ReservationId { get; private set; }

        [JsonProperty("reservationEnd")]
        public DateTime ReservationEnd { get; private set; }

        [JsonProperty("dateTime")]
        public DateTime DateTime { get; private set; }
    }
}
