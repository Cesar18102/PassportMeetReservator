using System;
using System.Linq;
using System.Windows.Forms;

using Autofac;

using Common;
using Common.Data;
using Common.Services;

namespace PassportMeetReservator
{
    public partial class ProxyListForm : Form
    {
        private ProxyProvider ProxyProvider = CommonDependencyHolder.ServiceDependencies.Resolve<ProxyProvider>();

        public ProxyListForm()
        {
            InitializeComponent();

            ProxyProvider.Proxies.ForEach(p => p.IsInUseChanged += Proxy_IsInUseChanged);

            ProxyList.Text = ProxyProvider.Proxies == null ? "" : string.Join("\r\n", ProxyProvider.Proxies.Select(proxy => proxy.Address));
            UpdateProxiesStatus();
        }

        private void ProxyListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProxyProvider.Proxies.ForEach(p => p.IsInUseChanged -= Proxy_IsInUseChanged);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string[] proxies = ProxyList.Text.Split('\n').Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToArray();

            Proxy[] removedProxies = ProxyProvider.Proxies.Where(proxy => !proxies.Contains(proxy.Address)).ToArray();
            string[] addedProxies = proxies.Except(ProxyProvider.Proxies.Select(proxy => proxy.Address)).ToArray();

            foreach (Proxy removed in removedProxies)
            {
                ProxyProvider.Proxies.Remove(removed);
                removed.IsInUseChanged -= Proxy_IsInUseChanged;
            }

            foreach (string added in addedProxies)
            {
                Proxy newProxy = new Proxy(added);
                ProxyProvider.Proxies.Add(newProxy);
                newProxy.IsInUseChanged += Proxy_IsInUseChanged;
            }

            this.Close();
        }

        private void UpdateProxiesStatus()
        {
            ProxyStatusList.Text = ProxyProvider.Proxies == null ? "" : string.Join("\r\n", ProxyProvider.Proxies.Select(proxy => proxy.IsInUse));
        }

        private void Proxy_IsInUseChanged(object sender, EventArgs e)
        {
            UpdateProxiesStatus();
        }
    }
}
