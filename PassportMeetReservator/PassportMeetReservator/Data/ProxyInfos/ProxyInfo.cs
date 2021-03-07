using Newtonsoft.Json;

namespace PassportMeetReservator.Data.ProxyInfos
{
    public abstract class ProxyInfo
    {
        public abstract string Username { get; set; }
        public abstract string Password { get; set; }
        public abstract string Address { get; set; }
        public abstract string Port { get; set; }
    }
}
