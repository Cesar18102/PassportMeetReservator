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

        public EditOrderForm(PlatformApiInfo[] platforms, ReservationOrder order)
        {
            Order = order;

            InitializeComponent();

            PlatformSelector.Items.AddRange(platforms);
            PlatformSelector.SelectedIndexChanged += (sender, e) =>
            {
                CitySelector.Items.Clear();
                CitySelector.Items.AddRange((PlatformSelector.SelectedItem as PlatformApiInfo).CityPlatforms);
            };

            CitySelector.SelectedIndexChanged += (sender, e) =>
            {
                OperationSelector.Items.Clear();
                OperationSelector.Items.AddRange((CitySelector.SelectedItem as CityPlatformInfo).Operations);
            };

            SurnameInput.InputText = Order.Surname;
            NameInput.InputText = Order.Name;
            EmailInput.InputText = Order.Email;

            PlatformSelector.SelectedIndex = PlatformSelector.Items.Cast<PlatformApiInfo>().ToList().FindIndex(
                platform => platform.Name == Order.Platform
            );

            CitySelector.SelectedIndex = CitySelector.Items.Cast<CityPlatformInfo>().ToList().FindIndex(
                city => city.Name == Order.City
            );

            if (Order.Operation != null)
                OperationSelector.SelectedIndex = Order.Operation.Position;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Order.Surname = SurnameInput.InputText;
            Order.Name = NameInput.InputText;
            Order.Email = EmailInput.InputText;

            PlatformApiInfo selectedPlatform = PlatformSelector.SelectedItem as PlatformApiInfo;
            CityPlatformInfo selectedCity = CitySelector.SelectedItem as CityPlatformInfo;

            Order.Platform = selectedPlatform.Name;
            Order.City = selectedCity.Name;
            Order.CityUrl = selectedCity.BaseUrl;

            Order.Operation = OperationSelector.SelectedItem as OperationInfo;

            this.Close();
        }
    }
}
