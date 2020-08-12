using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using PassportMeetReservator.Data;
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

        private const int FREE_BROWSER_SCAN_DELAY = 1000;

        private const int BROWSERS_COUNT = 5;
        private ReserverWebView[] Browsers = new ReserverWebView[BROWSERS_COUNT];

        public MainForm()
        {
            InitializeComponent();
            InitBrowsers();

            List<ReservationOrder> orders = LoadData();
            StartReserving(orders);
        }

        private void InitBrowsers()
        {
            Browsers = new ReserverWebView[BROWSERS_COUNT]
            {
                Browser1, Browser2, Browser3, 
                Browser4, Browser5
            };

            for (int i = 0; i < BROWSERS_COUNT; ++i)
                Browsers[i].Number = i;

            Browser1.BodyHandle = BrowserPanel1.Handle;
            Browser2.BodyHandle = BrowserPanel2.Handle;
            Browser3.BodyHandle = BrowserPanel3.Handle;
            Browser4.BodyHandle = BrowserPanel4.Handle;
            Browser5.BodyHandle = BrowserPanel5.Handle;
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

        private async void StartReserving(List<ReservationOrder> orders)
        {
            while(true)
            {
                await Task.Delay(FREE_BROWSER_SCAN_DELAY);

                ReserveIteration(orders);
                //GetFirstFreeBrowser().LoadUrl("google.com");
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
    }
}
