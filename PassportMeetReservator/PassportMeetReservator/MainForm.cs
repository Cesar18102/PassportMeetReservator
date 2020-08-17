using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using PassportMeetReservator.Data;
using PassportMeetReservator.Forms;
using PassportMeetReservator.Controls;

namespace PassportMeetReservator
{
    public partial class MainForm : Form
    {
        private static string DATA_FILE_PATH = Path.Combine(
            Environment.CurrentDirectory, "Data", "orders.json"
        );

        private static string OUTPUT_FILE_PATH = Path.Combine(
            Environment.CurrentDirectory, "Data", "reserved.txt"
        );

        private const int FREE_BROWSER_SCAN_DELAY = 100;

        private const int BROWSERS_COUNT = 5;
        private ReserverWebView[] Browsers { get; set; }
        private ReserverInfoView[] Infos { get; set; }
        private Panel[] BrowserWrappers { get; set; }

        private List<ReservationOrder> Orders { get; set; }

        public MainForm()
        {
            InitializeComponent();
            InitBrowsers();

            Orders = LoadData();
            StartReserving(Orders);
        }

        private void InitBrowsers()
        {
            Browsers = new ReserverWebView[BROWSERS_COUNT]
            {
                Browser1, Browser2, Browser3, 
                Browser4, Browser5
            };

            Infos = new ReserverInfoView[BROWSERS_COUNT]
            {
                BrowserInfo1, BrowserInfo2, BrowserInfo3,
                BrowserInfo4, BrowserInfo5
            };

            BrowserWrappers = new Panel[BROWSERS_COUNT]
            {
                BrowserPanel1, BrowserPanel2, BrowserPanel3,
                BrowserPanel4, BrowserPanel5
            };

            for (int i = 0; i < BROWSERS_COUNT; ++i)
            {
                Browsers[i].Number = i;
                Browsers[i].OnUrlChanged += Browser_OnUrlChanged;
                Browsers[i].OnOrderChanged += Browser_OnOrderChanged;

                Browsers[i].Size = BrowserWrappers[i].Size;
                BrowserWrappers[i].Controls.Add(Browsers[i]);
            }
        }

        private void Browser_OnOrderChanged(object sender, OrderEventArgs e)
        {
            ReserverWebView browser = sender as ReserverWebView;
            ReserverInfoView info = Infos[browser.Number];

            info.SurnameInput.InputText = e.Order.Surname;
            info.NameInput.InputText = e.Order.Name;
            info.EmailInput.InputText = e.Order.Email;
        }

        private void Browser_OnUrlChanged(object sender, UrlChangedEventArgs e)
        {
            ReserverWebView browser = sender as ReserverWebView;
            Infos[browser.Number].UrlInput.InputText = e.Url;
        }

        private List<ReservationOrder> LoadData()
        {
            if (!File.Exists(DATA_FILE_PATH))
                return new List<ReservationOrder>();

            using (StreamReader str = new StreamReader(DATA_FILE_PATH))
            {
                string json = str.ReadToEnd();

                if (string.IsNullOrEmpty(json))
                    return new List<ReservationOrder>();

                return JsonConvert.DeserializeObject<List<ReservationOrder>>(json);
            }
        }

        private void SaveData()
        {
            using (StreamWriter strw = File.CreateText(DATA_FILE_PATH))
            {
                string orders = JsonConvert.SerializeObject(Orders);
                strw.Write(orders);
            }
        }

        private async void StartReserving(List<ReservationOrder> orders)
        {
            while(true)
            {
                await Task.Delay(FREE_BROWSER_SCAN_DELAY);
                ReserveIteration(orders);
            }
        }

        private void ReserveIteration(List<ReservationOrder> orders)
        {
            foreach (ReservationOrder order in orders)
            {
                if (order.Done || order.Doing)
                    continue;

                ReserverWebView browser = GetFirstFreeBrowser();

                if (browser == null)
                    return;

                OrdersInfo.Text += $"{order.ToString()} is on the {browser.Number} browser\n";

                browser.Order = order;
                browser.OnReserved += (sender, e) =>
                {
                    using (StreamWriter str = new StreamWriter(OUTPUT_FILE_PATH, true))
                        str.WriteLine($"{order.ToString()}: {e.Url}");
                };

                browser.Start();
            }
        }

        private ReserverWebView GetFirstFreeBrowser()
        {
            return Browsers.FirstOrDefault(browser => !browser.IsBusy);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            ObserveOrdersListButton.Enabled = false;
            AddOrderButton.Enabled = false;

            foreach (ReserverWebView browser in Browsers)
                browser.Paused = false;
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            ObserveOrdersListButton.Enabled = true;
            AddOrderButton.Enabled = true;

            foreach (ReserverWebView browser in Browsers)
                browser.Paused = true;
        }

        private void ObserveOrdersListButton_Click(object sender, EventArgs e)
        {
            OrderListObserverForm observerForm = new OrderListObserverForm(Orders);
            observerForm.ShowDialog();
            SaveData();
        }

        private void AddOrderButton_Click(object sender, EventArgs e)
        {
            AddOrderForm addOrderForm = new AddOrderForm(Orders);
            addOrderForm.ShowDialog();
            SaveData();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Orders.RemoveAll(order => order.Done);
            SaveData();
        }
    }
}
