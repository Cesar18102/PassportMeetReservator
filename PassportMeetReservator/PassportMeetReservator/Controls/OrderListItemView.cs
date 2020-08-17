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

        public int Number { get; private set; }
        public ReservationOrder Order { get; private set; }

        public OrderListItemView(int number, ReservationOrder order)
        {
            Number = number;
            Order = order;

            InitializeComponent();

            EditButton.Click += (sender, e) => OnOrderEdited?.Invoke(
                this,  new NumberEventArgs(Number)
            );

            DeleteButton.Click += (sender, e) => OnOrderDeleted?.Invoke(
                this, new NumberEventArgs(Number)
            );

            DownButton.Click += (sender, e) => OnOrderDown?.Invoke(
                this, new NumberEventArgs(Number)
            );

            UpButton.Click += (sender, e) => OnOrderUp?.Invoke(
                this, new NumberEventArgs(Number)
            );

            OrderInfoLabel.Text = $"{Number + 1}) {Order.Surname} {Order.Name}; {Order.Email}";
        }
    }
}
