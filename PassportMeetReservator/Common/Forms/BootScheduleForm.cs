using System;
using System.Linq;
using System.Windows.Forms;

using Common.Data;
using Common.Data.CustomEventArgs;

using PassportMeetReservator.Controls;

namespace Common.Forms
{
    public partial class BootScheduleForm : Form
    {
        private BootSchedule Schedule { get; set; }

        public BootScheduleForm(BootSchedule schedule)
        {
            Schedule = schedule;

            InitializeComponent();

            InitList();
        }

        private const int ROW_HEIGHT = 30;

        public void InitList()
        {
            BootPeriodsListWrapper.Controls.Clear();

            for (int i = 0; i < Schedule.BootPeriods.Count; ++i)
            {
                BootPeriodView bootPeriodView = new BootPeriodView(i, Schedule.BootPeriods.ElementAt(i));

                bootPeriodView.OnPeriodDeleted += BootPeriodView_OnPeriodDeleted;
                bootPeriodView.OnPeriodSplitted += PeriodView_OnPeriodSplitted;

                bootPeriodView.Top = ROW_HEIGHT * i;

                BootPeriodsListWrapper.Controls.Add(bootPeriodView);
            }
        }

        private void AddPeriodButton_Click(object sender, EventArgs e)
        {
            BootPeriod period = new BootPeriod();
            BootPeriodView periodView = new BootPeriodView(Schedule.BootPeriods.Count, period);

            periodView.OnPeriodDeleted += BootPeriodView_OnPeriodDeleted;
            periodView.OnPeriodSplitted += PeriodView_OnPeriodSplitted;

            periodView.Top = ROW_HEIGHT * Schedule.BootPeriods.Count;

            BootPeriodsListWrapper.Controls.Add(periodView);
            Schedule.BootPeriods.Add(period);
        }

        private void PeriodView_OnPeriodSplitted(object sender, NumberEventArgs e)
        {
            BootPeriodSplitForm periodSplitForm = new BootPeriodSplitForm(Schedule.BootPeriods[e.Number]);
            periodSplitForm.ShowDialog();

            Schedule.BootPeriods.RemoveAt(e.Number);
            Schedule.BootPeriods.InsertRange(e.Number, periodSplitForm.Splitted);

            InitList();
        }

        private void BootPeriodView_OnPeriodDeleted(object sender, NumberEventArgs e)
        {
            Schedule.BootPeriods.RemoveAt(e.Number);
            InitList();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            Schedule.BootPeriods.Clear();
            InitList();
        }
    }
}
