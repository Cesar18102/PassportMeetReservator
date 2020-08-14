using System.Windows.Forms;
using System.Collections.Generic;

using PassportMeetReservator.Data;
using System.Drawing;

namespace PassportMeetReservator.Forms
{
    public partial class OrderListObserverForm : Form
    {
        private List<ReservationOrder> Orders { get; set; }

        public OrderListObserverForm(List<ReservationOrder> orders)
        {
            InitializeComponent();

            Orders = orders;
            InitList();
        }

        private const int ROW_HEIGHT = 30;
        private const string DELETE_BUTTON_TEXT = "DELETE";

        private static Size DELETE_BUTTON_SIZE = new Size(100, 25);

        public void InitList()
        {
            OrderListWrapper.Controls.Clear();

            for(int i = 0; i < Orders.Count; ++i)
            {
                Button orderDeleteButton = new Button();

                orderDeleteButton.Top = ROW_HEIGHT * i;
                orderDeleteButton.Left = OrderListWrapper.Width - DELETE_BUTTON_SIZE.Width;
                orderDeleteButton.Size = DELETE_BUTTON_SIZE;
                orderDeleteButton.Text = DELETE_BUTTON_TEXT;

                int temp = i;
                orderDeleteButton.Click += (sender, e) =>
                {
                    Orders.RemoveAt(temp);
                    InitList();
                };

                Label orderInfoLabel = new Label();

                orderInfoLabel.AutoSize = true;
                orderInfoLabel.Text = $"{Orders[i].Surname} {Orders[i].Name}; {Orders[i].Email}";
                orderInfoLabel.Top = ROW_HEIGHT * i + (orderDeleteButton.Height - orderInfoLabel.Height);

                OrderListWrapper.Controls.Add(orderInfoLabel);
                OrderListWrapper.Controls.Add(orderDeleteButton);
            }
        }
    }
}
