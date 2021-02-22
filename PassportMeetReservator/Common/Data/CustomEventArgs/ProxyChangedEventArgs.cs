using System;

namespace Common.Data.CustomEventArgs
{
    public class ProxyChangedEventArgs : EventArgs
    {
        public string NewProxy { get; private set; }

        public ProxyChangedEventArgs(string newProxy)
        {
            NewProxy = newProxy;
        }
    }
}
