using Newtonsoft.Json;

namespace PassportMeetReservator.Data
{
    public class ProxyInfo
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("proxy_address")]
        public string Address { get; set; }

        [JsonProperty("ports")]
        public PortsInfo Ports { get; set; }
    }
}
