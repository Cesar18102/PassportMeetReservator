using System.Collections.Generic;

namespace Common.Services
{
    public class ProxyProvider
    {
        private int currentIndex = 0;

        public List<string> Proxies { get; private set; } = new List<string>();

        public string GetNextProxy()
        {
            if (Proxies == null || Proxies.Count == 0)
                return null;

            string proxy = Proxies[currentIndex];
            currentIndex = (currentIndex + 1) % Proxies.Count;

            return proxy;
        }
    }
}
