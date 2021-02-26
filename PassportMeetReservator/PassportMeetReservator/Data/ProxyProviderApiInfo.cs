using Newtonsoft.Json;

namespace PassportMeetReservator.Data
{
    public class ProxyProviderApiInfo
    {
        [JsonProperty("url")]
        public string ProxyListEndpoint { get; set; }

        [JsonProperty("key_header")]
        public string AuthHeader { get; set; }

        [JsonProperty("key")]
        public string ApiKey { get; set; }
    }
}
