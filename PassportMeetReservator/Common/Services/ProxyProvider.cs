using System.Linq;
using System.Collections.Generic;

using Common.Data;
using System;

namespace Common.Services
{
    public class ProxyProvider
    {
        public List<Proxy> Proxies { get; private set; } = new List<Proxy>();

        public Proxy GetNextProxy(bool byBrowser = false)
        {
            return this.GetNextProxy(null, byBrowser);
        }

        public Proxy GetNextProxy(Proxy oldProxy, bool byBrowser = false)
        {
            if (Proxies == null)
                return null;

            Predicate<Proxy> condition = p => !p.IsInUse;

            if(byBrowser)
                condition = p => !p.IsInUseByBrowser;

            List<Proxy> unusedProxies = Proxies.Where(p => condition(p) && !p.IsBlocked).ToList();
            Proxy neverUsedProxy = unusedProxies.FirstOrDefault(p => !p.LastUsed.HasValue);

            Proxy newProxy = neverUsedProxy ?? unusedProxies.OrderBy(p => p.LastUsed).FirstOrDefault();

            if (newProxy == null)
                return null;

            if (byBrowser)
            {
                if (oldProxy != null)
                    oldProxy.IsInUseByBrowser = false;

                newProxy.IsInUseByBrowser = true;
            }
            else
            {
                if (oldProxy != null)
                    oldProxy.IsInUse = false;

                newProxy.IsInUse = true;
            }

            return newProxy;
        }
    }
}
