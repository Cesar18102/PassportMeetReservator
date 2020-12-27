using System;
using System.Windows.Forms;

using Common.Data;
using Common.Data.CustomEventArgs;

namespace PassportMeetReservator.Controls
{
    public partial class BootPeriodView : UserControl
    {
        public event EventHandler<NumberEventArgs> OnPeriodSplitted;
        public event EventHandler<NumberEventArgs> OnPeriodDeleted;

        private int Number { get; set; }
        private BootPeriod Period { get; set; }

        public BootPeriodView(int nubmer, BootPeriod period)
        {
            Number = nubmer;
            Period = period;

            InitializeComponent();

            FromTime.Value = DateTime.Now.Date + Period.TimeStart;
            ToTime.Value = DateTime.Now.Date + Period.TimeEnd;

            DeleteButton.Click += (sender, e) => OnPeriodDeleted?.Invoke(
                this, new NumberEventArgs(Number)
            );

            SplitButton.Click += (sender, e) => OnPeriodSplitted?.Invoke(
                this, new NumberEventArgs(Number)
            );
        }

        private void FromTime_ValueChanged(object sender, EventArgs e)
        {
            Period.TimeStart = FromTime.Value.TimeOfDay;
        }

        private void ToTime_ValueChanged(object sender, EventArgs e)
        {
            Period.TimeEnd = ToTime.Value.TimeOfDay;
        }
    }
}
