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
            RefreshSessionUpdateDelay.Value = DelayInfo.RefreshSessionUpdateDelay;
            DiscreteWaitDelay.Value = DelayInfo.DiscreteWaitDelay;
            ManualReactionWaitDelay.Value = DelayInfo.ManualReactionWaitDelay;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            DelayInfo.OrderLoadingIterationDelay = (int)OrderLoadingDelay.Value;
            DelayInfo.BrowserIterationDelay = (int)BrowserIterationDelay.Value;
            DelayInfo.ActionResultDelay = (int)ActionResultDelay.Value;
            DelayInfo.RefreshSessionUpdateDelay = (int)RefreshSessionUpdateDelay.Value;
            DelayInfo.DiscreteWaitDelay = (int)DiscreteWaitDelay.Value;
            DelayInfo.ManualReactionWaitDelay = (int)ManualReactionWaitDelay.Value;

            this.Close();
        }
    }
}
