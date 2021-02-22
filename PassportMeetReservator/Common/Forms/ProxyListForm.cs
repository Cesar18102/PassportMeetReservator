using System;
using System.Linq;
using System.Windows.Forms;

using Autofac;

using Common;
using Common.Services;

namespace PassportMeetReservator
{
    public partial class ProxyListForm : Form
    {
        private ProxyProvider ProxyProvider = CommonDependencyHolder.ServiceDependencies.Resolve<ProxyProvider>();

        public ProxyListForm()
        {
            InitializeComponent();

            ProxyList.Text = ProxyProvider.Proxies == null ? "" : string.Join("\n", ProxyProvider.Proxies);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string[] proxies = ProxyList.Text.Split('\n');

            string[] removedProxies = ProxyProvider.Proxies.Except(proxies).ToArray();
            string[] addedProxies = proxies.Except(ProxyProvider.Proxies).ToArray();

            foreach (string removed in removedProxies)
                ProxyProvider.Proxies.Remove(removed);

            foreach (string added in addedProxies)
                ProxyProvider.Proxies.Add(added);

            this.Close();
        }
    }
}
