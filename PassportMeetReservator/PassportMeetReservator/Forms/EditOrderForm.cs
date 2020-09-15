using System;
using System.Windows.Forms;

using PassportMeetReservator.Data;

namespace PassportMeetReservator.Forms
{
    public partial class EditOrderForm : Form
    {
        private ReservationOrder Order { get; set; }

        public EditOrderForm(ReservationOrder order)
        {
            Order = order;

            InitializeComponent();

            SurnameInput.InputText = Order.Surname;
            NameInput.InputText = Order.Name;
            EmailInput.InputText = Order.Email;
            OrderTypeSelector.SelectedItem = Order.Operation;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Order.Surname = SurnameInput.InputText;
            Order.Name = NameInput.InputText;
            Order.Email = EmailInput.InputText;
            Order.Operation = OrderTypeSelector.SelectedItem.ToString();

            this.Close();
        }
    }
}
