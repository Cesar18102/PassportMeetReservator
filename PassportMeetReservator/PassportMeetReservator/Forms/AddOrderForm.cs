using System;
using System.Windows.Forms;
using System.Collections.Generic;

using PassportMeetReservator.Data;

namespace PassportMeetReservator.Forms
{
    public partial class AddOrderForm : Form
    {
        private List<ReservationOrder> Orders { get; set; }

        public AddOrderForm(List<ReservationOrder> orders)
        {
            InitializeComponent();

            Orders = orders;
        }

        private void AddFirstButton_Click(object sender, EventArgs e)
        {
            ReservationOrder order = BuildOrder();

            if (order == null)
                return;

            Orders.Insert(0, order);
            this.Close();
        }

        private void AddLastButton_Click(object sender, EventArgs e)
        {
            ReservationOrder order = BuildOrder();

            if (order == null)
                return;

            Orders.Add(order);
            this.Close();
        }

        private ReservationOrder BuildOrder()
        {
            bool surnameInvalid = string.IsNullOrEmpty(SurnameInput.InputText);
            bool nameInvalid = string.IsNullOrEmpty(NameInput.InputText);
            bool emailInvalid = string.IsNullOrEmpty(EmailInput.InputText);
            bool typeInvalid = OrderTypeSelector.SelectedIndex == -1;

            if (surnameInvalid || nameInvalid || emailInvalid || typeInvalid)
            {
                MessageBox.Show("Invalid data");
                return null;
            }

            ReservationOrder order = new ReservationOrder();

            order.ReservationTypeText = OrderTypeSelector.SelectedItem.ToString();
            order.Surname = SurnameInput.InputText;
            order.Name = NameInput.InputText;
            order.Email = EmailInput.InputText;

            return order;
        }
    }
}
