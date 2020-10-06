using System;
using System.Windows.Forms;
using System.Collections.Generic;

using PassportMeetReservator.Data;
using PassportMeetReservator.Data.Platforms;

namespace PassportMeetReservator.Forms
{
    public partial class AddOrderForm : Form
    {
        private List<ReservationOrder> Orders { get; set; }

        public AddOrderForm(CityPlatformInfo[] platforms, List<ReservationOrder> orders)
        {
            InitializeComponent();

            CitySelector.Items.AddRange(platforms);
            CitySelector.SelectedIndexChanged += (sender, e) =>
            {
                OperationSelector.Items.Clear();
                OperationSelector.Items.AddRange(platforms[OperationSelector.SelectedIndex].Operations);
            };

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
            bool typeInvalid = OperationSelector.SelectedIndex == -1;

            if (surnameInvalid || nameInvalid || emailInvalid || typeInvalid)
            {
                MessageBox.Show("Invalid data");
                return null;
            }

            ReservationOrder order = new ReservationOrder(
                SurnameInput.InputText, NameInput.InputText, EmailInput.InputText, 
                (CitySelector.SelectedItem as CityPlatformInfo).BaseUrl, 
                OperationSelector.SelectedItem.ToString(), 
                OperationSelector.SelectedIndex + 1
            );

            return order;
        }
    }
}
