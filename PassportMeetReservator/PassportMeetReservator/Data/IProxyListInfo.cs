using PassportMeetReservator.Data.ProxyInfos;

namespace PassportMeetReservator.Data
{
    public interface IProxyListInfo<TProxyInfo> where TProxyInfo : ProxyInfo
    {
        TProxyInfo[] Proxies { get; set; }
    }
}
