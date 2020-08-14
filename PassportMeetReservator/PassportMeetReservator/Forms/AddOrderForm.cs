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

        private void AddOrderButton_Click(object sender, System.EventArgs e)
        {
            bool surnameInvalid = string.IsNullOrEmpty(SurnameInput.InputText);
            bool nameInvalid = string.IsNullOrEmpty(NameInput.InputText);
            bool emailInvalid = string.IsNullOrEmpty(EmailInput.InputText);
            bool typeInvalid = OrderTypeSelector.SelectedIndex == -1;

            if (surnameInvalid || nameInvalid || emailInvalid || typeInvalid)
            {
                MessageBox.Show("Invalid data");
                return;
            }

            ReservationOrder order = new ReservationOrder();

            order.ReservationTypeText = OrderTypeSelector.SelectedItem.ToString();
            order.Surname = SurnameInput.InputText;
            order.Name = NameInput.InputText;
            order.Email = EmailInput.InputText;

            Orders.Add(order);
            this.Close();
        }
    }
}
