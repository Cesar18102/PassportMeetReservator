using System;
using System.Windows.Forms;
using System.Collections.Generic;

using Common.Data.Platforms;
using PassportMeetReservator.Data;

namespace PassportMeetReservator.Forms
{
    public partial class AddOrderForm : Form
    {
        private List<ReservationOrder> Orders { get; set; }

        public AddOrderForm(PlatformApiInfo[] platforms, List<ReservationOrder> orders)
        {
            InitializeComponent();

            PlatformSelector.Items.AddRange(platforms);
            PlatformSelector.SelectedIndexChanged += (sender, e) =>
            {
                OperationSelector.Items.Clear();

                CitySelector.Items.Clear();
                CitySelector.Items.AddRange((PlatformSelector.SelectedItem as PlatformApiInfo).CityPlatforms);
            };

            CitySelector.SelectedIndexChanged += (sender, e) =>
            {
                OperationSelector.Items.Clear();
                OperationSelector.Items.AddRange((CitySelector.SelectedItem as CityPlatformInfo).Operations);
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

            PlatformApiInfo platform = PlatformSelector.SelectedItem as PlatformApiInfo;
            CityPlatformInfo city = CitySelector.SelectedItem as CityPlatformInfo;
            OperationInfo operation = OperationSelector.SelectedItem as OperationInfo;

            ReservationOrder order = new ReservationOrder(
                SurnameInput.InputText, NameInput.InputText, EmailInput.InputText,
                platform.Name, city.Name, city.BaseUrl, operation
            );

            return order;
        }
    }
}
