using System;
using System.Windows.Forms;

using PassportMeetReservator.Data;

namespace PassportMeetReservator.Controls
{
    public partial class ReservedListItemView : UserControl
    {
        public event EventHandler<NumberEventArgs> OnReservedForgotten;

        private int Number { get; set; }
        private ReservedInfo Reserved { get; set; }
        
        public ReservedListItemView(int number, ReservedInfo reserved)
        {
            Number = number;
            Reserved = reserved;

            InitializeComponent();

            InfoLabel.Text = $"{Reserved.Order.Surname} {Reserved.Order.Name}";
            CopyInfoButton.Click += CopyInfoButton_Click;

            ForgetButton.Click += (sender, e) => OnReservedForgotten?.Invoke(
                this, new NumberEventArgs(Number)
            );
        }

        private void CopyInfoButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText($"{Reserved.Order.Surname} {Reserved.Order.Name} {Reserved.Url}");
        }
    }
}
