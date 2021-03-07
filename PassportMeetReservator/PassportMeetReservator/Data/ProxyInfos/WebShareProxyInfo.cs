using Newtonsoft.Json;

namespace PassportMeetReservator.Data.ProxyInfos
{
    public class WebShareProxyInfo : ProxyInfo
    {
        [JsonProperty("username")]
        public override string Username { get; set; }

        [JsonProperty("password")]
        public override string Password { get; set; }

        [JsonProperty("proxy_address")]
        public override string Address { get; set; }

        public override string Port
        {
            get => Ports.HttpPort;
            set => Ports.HttpPort = value;
        }
        
        [JsonProperty("ports")]
        public PortsInfo Ports { get; set; }
    }
}
