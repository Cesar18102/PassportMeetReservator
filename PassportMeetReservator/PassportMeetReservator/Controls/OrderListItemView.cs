using System;
using System.Windows.Forms;

using PassportMeetReservator.Data;

namespace PassportMeetReservator.Controls
{
    public partial class OrderListItemView : UserControl
    {
        public event EventHandler<OrderNumberEventArgs> OnOrderDeleted;
        public event EventHandler<OrderNumberEventArgs> OnOrderUp;
        public event EventHandler<OrderNumberEventArgs> OnOrderDown;

        public int Number { get; private set; }
        public ReservationOrder Order { get; private set; }

        public OrderListItemView(int number, ReservationOrder order)
        {
            Number = number;
            Order = order;

            InitializeComponent();

            DeleteButton.Click += (sender, e) => OnOrderDeleted?.Invoke(
                this, new OrderNumberEventArgs(Number)
            );

            DownButton.Click += (sender, e) => OnOrderDown?.Invoke(
                this, new OrderNumberEventArgs(Number)
            );

            UpButton.Click += (sender, e) => OnOrderUp?.Invoke(
                this, new OrderNumberEventArgs(Number)
            );

            OrderInfoLabel.Text = $"{Number + 1}) {Order.Surname} {Order.Name}; {Order.Email}";
        }
    }
}
