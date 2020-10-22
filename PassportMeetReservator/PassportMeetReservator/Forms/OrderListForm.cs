using System.Windows.Forms;
using System.Collections.Generic;

using PassportMeetReservator.Data;
using PassportMeetReservator.Controls;
using PassportMeetReservator.Data.Platforms;

namespace PassportMeetReservator.Forms
{
    public partial class OrderListForm : Form
    {
        private List<ReservationOrder> Orders { get; set; }
        private CityPlatformInfo[] Platforms { get; set; }

        public OrderListForm(CityPlatformInfo[] platforms, List<ReservationOrder> orders)
        {
            InitializeComponent();

            Orders = orders;
            Platforms = platforms;

            InitList();
        }

        private const int ROW_HEIGHT = 80;

        public void InitList()
        {
            OrderListWrapper.Controls.Clear();

            for (int i = 0; i < Orders.Count; ++i)
            {
                OrderListItemView orderInfoView = new OrderListItemView(i, Orders[i]);
                orderInfoView.Top = ROW_HEIGHT * i;

                orderInfoView.OnOrderDeleted += (sender, e) =>
                {
                    Orders.RemoveAt(e.Number);
                    InitList();
                };

                orderInfoView.OnOrderEdited += (sender, e) =>
                {
                    EditOrderForm editForm = new EditOrderForm(Platforms, Orders[e.Number]);
                    editForm.ShowDialog();
                    InitList();
                };

                orderInfoView.OnOrderUp += (sender, e) =>
                {
                    if (MoveOrder(e.Number, e.Number - 1))
                        InitList();
                };

                orderInfoView.OnOrderDown += (sender, e) =>
                {
                    if (MoveOrder(e.Number, e.Number + 1))
                        InitList();
                };

                OrderListWrapper.Controls.Add(orderInfoView);
            }
        }

        private bool MoveOrder(int from, int to)
        {
            if (to < 0 || to >= Orders.Count)
                return false;

            ReservationOrder order = Orders[from];
            Orders.RemoveAt(from);
            Orders.Insert(to, order);

            return true;
        }
    }
}
