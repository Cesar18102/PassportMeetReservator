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

            ProxyList.Text = ProxyProvider.Proxies == null ? "" : string.Join("\r\n", ProxyProvider.Proxies.Select(proxy => proxy.Address));
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string[] proxies = ProxyList.Text.Split('\n').Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToArray();

            Proxy[] removedProxies = ProxyProvider.Proxies.Where(proxy => !proxies.Contains(proxy.Address)).ToArray();
            string[] addedProxies = proxies.Except(ProxyProvider.Proxies.Select(proxy => proxy.Address)).ToArray();

            foreach (Proxy removed in removedProxies)
                ProxyProvider.Proxies.Remove(removed);

            foreach (string added in addedProxies)
                ProxyProvider.Proxies.Add(new Proxy(added));

            this.Close();
        }
    }
}
