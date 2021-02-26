using System.Linq;
using System.Collections.Generic;

using Common.Data;

namespace Common.Services
{
    public class ProxyProvider
    {
        public List<Proxy> Proxies { get; private set; } = new List<Proxy>();

        public Proxy GetNextProxy()
        {
            return this.GetNextProxy(null);
        }

        public Proxy GetNextProxy(Proxy oldProxy)
        {
            if (Proxies == null)
                return null;

            Proxy newProxy = Proxies.FirstOrDefault(p => !p.IsInUse && !p.IsBlocked);

            if (newProxy == null)
                return null;

            if (oldProxy != null)
                oldProxy.IsInUse = false;

            newProxy.IsInUse = true;

            return newProxy;
        }
    }
}
