using System;

using Newtonsoft.Json;

namespace Common.Data.Dto
{
    public class DateCheckDto
    {
        [JsonProperty("operationId")]
        public int OperationId { get; private set; }

        [JsonProperty("availableDays")]
        public DateTime[] AvailableDates { get; private set; }

        [JsonProperty("disabledDays")]
        public DateTime[] DisabledDays { get; private set; }

        [JsonProperty("minDate")]
        public DateTime MinDate { get; private set; }

        [JsonProperty("maxDate")]
        public DateTime MaxDate { get; private set; }
    }
}
