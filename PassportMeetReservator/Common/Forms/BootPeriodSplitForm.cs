using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

using Common.Data;

namespace Common.Forms
{
    public partial class BootPeriodSplitForm : Form
    {
        private BootPeriod Period { get; set; }
        public ICollection<BootPeriod> Splitted { get; private set; }

        public BootPeriodSplitForm(BootPeriod period)
        {
            InitializeComponent();

            Period = period;
            Splitted = new List<BootPeriod>() { Period };

            FromTime.Value = DateTime.Now.Date + Period.TimeStart;
            ToTime.Value = DateTime.Now.Date + Period.TimeEnd;
        }

        private void SplitButton_Click(object sender, EventArgs e)
        {
            double periodTotal = (Period.TimeEnd - Period.TimeStart).TotalMilliseconds;

            if (periodTotal < 0)
                periodTotal += 86400000;

            int splittedCount = (int)CountSelector.Value;
            int splittedDuration = (int)DurationSelector.Value * 1000;
            int splittedTotal = splittedCount * splittedDuration;

            if (splittedTotal > periodTotal)
            {
                MessageBox.Show("Specified count and duration take more time than the period lasts", "Error");
                return;
            }

            double interval = (periodTotal - splittedTotal) / ((double)CountSelector.Value - 1);

            Splitted.Clear();
            for(int i = 0; i < splittedCount; ++i)
            {
                BootPeriod period = new BootPeriod();

                int dStart = (int)(i * (splittedDuration + interval));

                period.TimeStart = Period.TimeStart + new TimeSpan(0, 0, 0, 0, dStart);
                period.TimeEnd = Period.TimeStart + new TimeSpan(0, 0, 0, 0, dStart + splittedDuration);

                Splitted.Add(period);
            }

            IEnumerable<string> stringPeriods = Splitted.Select(
                date => $"{date.TimeStart.Hours}:{date.TimeStart.Minutes}:{date.TimeStart.Seconds}:{date.TimeStart.Milliseconds} - " +
                        $"{date.TimeEnd.Hours}:{date.TimeEnd.Minutes}:{date.TimeEnd.Seconds}:{date.TimeEnd.Milliseconds}"
            );

            DialogResult result = MessageBox.Show(
                string.Join(", ", stringPeriods),
                "Split result", MessageBoxButtons.YesNo
            );

            if (result == DialogResult.Yes)
                this.Close();
        }
    }
}
