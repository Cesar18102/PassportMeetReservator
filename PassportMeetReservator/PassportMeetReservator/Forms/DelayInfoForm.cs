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

            ActionResultDelay.Value = DelayInfo.ActionResultDelay;
            RefreshSessionUpdateDelay.Value = DelayInfo.RefreshSessionUpdateDelay;
            DiscreteWaitDelay.Value = DelayInfo.DiscreteWaitDelay;
            ManualReactionWaitDelay.Value = DelayInfo.ManualReactionWaitDelay;
            PostInputDelay.Value = DelayInfo.PostInputDelay;
            DateCheckDelay.Value = DelayInfo.DateCheckDelay;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            DelayInfo.ActionResultDelay = (int)ActionResultDelay.Value;
            DelayInfo.RefreshSessionUpdateDelay = (int)RefreshSessionUpdateDelay.Value;
            DelayInfo.DiscreteWaitDelay = (int)DiscreteWaitDelay.Value;
            DelayInfo.ManualReactionWaitDelay = (int)ManualReactionWaitDelay.Value;
            DelayInfo.PostInputDelay = (int)PostInputDelay.Value;
            DelayInfo.DateCheckDelay = (int)DateCheckDelay.Value;

            this.Close();
        }
    }
}
