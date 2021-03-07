using Newtonsoft.Json;
using PassportMeetReservator.Data.ProxyInfos;

namespace PassportMeetReservator.Data
{
    public class ProxyListInfo<TProxyInfo> : IProxyListInfo<TProxyInfo> where TProxyInfo : ProxyInfo
    {
        [JsonProperty("results")]
        public TProxyInfo[] Proxies { get; set; }
    }
}
