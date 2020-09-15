using System;
using System.Windows.Forms;

using PassportMeetReservator.Data;

namespace PassportMeetReservator.Controls
{
    public partial class OrderListItemView : UserControl
    {
        public event EventHandler<NumberEventArgs> OnOrderEdited;
        public event EventHandler<NumberEventArgs> OnOrderDeleted;
        public event EventHandler<NumberEventArgs> OnOrderUp;
        public event EventHandler<NumberEventArgs> OnOrderDown;

        private int Number { get; set; }
        private ReservationOrder Order { get; set; }

        public OrderListItemView(int number, ReservationOrder order)
        {
            InitializeComponent();

            Number = number;
            Order = order;

            InfoLabel.Text = $"{Number + 1}) {Order.Surname} {Order.Name}; {Order.Email}; ";

            if (Order.Done)
                InfoLabel.Text += "DONE";
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            OnOrderDeleted?.Invoke(this, new NumberEventArgs(Number));
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            OnOrderEdited?.Invoke(this, new NumberEventArgs(Number));
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            OnOrderUp?.Invoke(this, new NumberEventArgs(Number));
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            OnOrderDown?.Invoke(this, new NumberEventArgs(Number));
        }
    }
}
