using System;
using System.Windows.Forms;

using PassportMeetReservator.Data;

namespace PassportMeetReservator.Forms
{
    public partial class DelayInfoForm : Form
    {
        private DelayInfo DelayInfo { get; set; }

        public DelayInfoForm(DelayInfo delayInfo)
        {
            DelayInfo = delayInfo;

            InitializeComponent();

            OrderLoadingDelay.Value = DelayInfo.OrderLoadingIterationDelay;
            BrowserIterationDelay.Value = DelayInfo.BrowserIterationDelay;
            ActionResultDelay.Value = DelayInfo.ActionResultDelay;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            DelayInfo.OrderLoadingIterationDelay = (int)OrderLoadingDelay.Value;
            DelayInfo.BrowserIterationDelay = (int)BrowserIterationDelay.Value;
            DelayInfo.ActionResultDelay = (int)ActionResultDelay.Value;

            this.Close();
        }
    }
}
