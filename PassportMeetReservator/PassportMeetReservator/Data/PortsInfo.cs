using Newtonsoft.Json;

namespace PassportMeetReservator.Data
{
    public class PortsInfo
    {
        [JsonProperty("http")]
        public string HttpPort { get; set; }
    }
}
