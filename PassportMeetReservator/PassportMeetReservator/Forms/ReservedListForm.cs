using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

using PassportMeetReservator.Data;
using PassportMeetReservator.Controls;

namespace PassportMeetReservator.Forms
{
    public partial class ReservedListForm : Form
    {
        private List<ReservedInfo> Reserved { get; set; }

        public ReservedListForm(List<ReservedInfo> reserved)
        {
            Reserved = reserved;

            InitializeComponent();

            InitList();
        }

        private const int ROW_HEIGHT = 30;

        public void InitList()
        {
            ReservedListWrapper.Controls.Clear();

            for (int i = 0; i < Reserved.Count; ++i)
            {
                ReservedListItemView reservedInfoView = new ReservedListItemView(i, Reserved[i]);
                reservedInfoView.OnReservedForgotten += ReservedInfoView_OnReservedForgotten;
                reservedInfoView.Top = ROW_HEIGHT * i;

                ReservedListWrapper.Controls.Add(reservedInfoView);
            }
        }

        private void ReservedInfoView_OnReservedForgotten(object sender, NumberEventArgs e)
        {
            Reserved.RemoveAt(e.Number);
            InitList();
        }

        private void CopyAllButton_Click(object sender, EventArgs e)
        {
            if (Reserved.Count == 0)
                return;

            IEnumerable<string> infos = Reserved.Select(
                reserved => $"{reserved.Order.Surname} {reserved.Order.Name} {reserved.Url}"
            );

            Clipboard.SetText(string.Join("\n", infos));
        }
    }
}
