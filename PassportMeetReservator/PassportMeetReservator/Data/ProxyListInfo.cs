using Newtonsoft.Json;

namespace PassportMeetReservator.Data
{
    public class ProxyListInfo
    {
        [JsonProperty("results")]
        public ProxyInfo[] Proxies { get; set; }
    }
}
