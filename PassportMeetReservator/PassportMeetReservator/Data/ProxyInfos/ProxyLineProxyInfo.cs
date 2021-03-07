using Newtonsoft.Json;

namespace PassportMeetReservator.Data.ProxyInfos
{
    public class ProxyLineProxyInfo : ProxyInfo
    {
        [JsonProperty("username")]
        public override string Username { get; set; }

        [JsonProperty("password")]
        public override string Password { get; set; }

        [JsonProperty("ip")]
        public override string Address { get; set; }

        [JsonProperty("port_http")]
        public override string Port { get; set; }
    }
}
