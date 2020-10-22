using System;
using System.Linq;
using System.Windows.Forms;

using PassportMeetReservator.Data;
using PassportMeetReservator.Data.Platforms;

namespace PassportMeetReservator.Forms
{
    public partial class EditOrderForm : Form
    {
        private ReservationOrder Order { get; set; }

        public EditOrderForm(CityPlatformInfo[] platforms, ReservationOrder order)
        {
            Order = order;

            InitializeComponent();

            CitySelector.Items.AddRange(platforms);
            CitySelector.SelectedIndexChanged += (sender, e) =>
            {
                OperationSelector.Items.Clear();
                OperationSelector.Items.AddRange(platforms[CitySelector.SelectedIndex].Operations);
            };

            SurnameInput.InputText = Order.Surname;
            NameInput.InputText = Order.Name;
            EmailInput.InputText = Order.Email;
            CitySelector.SelectedIndex = CitySelector.Items.Cast<CityPlatformInfo>().ToList().FindIndex(platform => platform.Name == Order.City);

            if (Order.Operation != null)
                OperationSelector.SelectedIndex = Order.Operation.Position;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Order.Surname = SurnameInput.InputText;
            Order.Name = NameInput.InputText;
            Order.Email = EmailInput.InputText;

            CityPlatformInfo selectedCity = CitySelector.SelectedItem as CityPlatformInfo;

            Order.City = selectedCity.Name;
            Order.CityUrl = selectedCity.BaseUrl;

            Order.Operation = OperationSelector.SelectedItem as OperationInfo;

            this.Close();
        }
    }
}
