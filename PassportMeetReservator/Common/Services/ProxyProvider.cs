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

            List<Proxy> unusedProxies = Proxies.Where(p => !p.IsInUse && !p.IsBlocked).ToList();
            Proxy neverUsedProxy = unusedProxies.FirstOrDefault(p => !p.LastUsed.HasValue);

            Proxy newProxy = neverUsedProxy ?? unusedProxies.OrderBy(p => p.LastUsed).FirstOrDefault();

            if (newProxy == null)
                return null;

            if (oldProxy != null)
                oldProxy.IsInUse = false;

            newProxy.IsInUse = true;

            return newProxy;
        }
    }
}
