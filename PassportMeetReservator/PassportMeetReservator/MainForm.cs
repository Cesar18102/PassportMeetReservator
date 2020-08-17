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

        private static string SCHEDULE_FILE_PATH = Path.Combine(
            Environment.CurrentDirectory, "Data", "schedule.json"
        );

        private const int FREE_BROWSER_SCAN_DELAY = 100;

        private const int BROWSERS_COUNT = 5;
        private ReserverWebView[] Browsers { get; set; }
        private ReserverInfoView[] Infos { get; set; }
        private Panel[] BrowserWrappers { get; set; }

        private Button[] PausedChangeButtons { get; set; }
        private Button[] ResetButtons { get; set; }
        private Button[] DoneButtons { get; set; }

        private List<ReservationOrder> Orders { get; set; }
        private List<ReservedInfo> Reserved { get; set; }
        private BootSchedule Schedule { get; set; }

        public MainForm()
        {
            InitializeComponent();
            InitBrowsers();

            Orders = LoadData<List<ReservationOrder>>(DATA_FILE_PATH);
            Reserved = LoadData<List<ReservedInfo>>(OUTPUT_FILE_PATH);
            Schedule = LoadData<BootSchedule>(SCHEDULE_FILE_PATH);

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

            PausedChangeButtons = new Button[BROWSERS_COUNT]
            {
                PauseChangeButton1, PauseChangeButton2, PauseChangeButton3,
                PauseChangeButton4, PauseChangeButton5
            };

            ResetButtons = new Button[BROWSERS_COUNT]
            {
                ResetButton1, ResetButton2, ResetButton3,
                ResetButton4, ResetButton5
            };

            DoneButtons = new Button[BROWSERS_COUNT]
            {
                DoneButton1, DoneButton2, DoneButton3,
                DoneButton4, DoneButton5
            };

            for (int i = 0; i < BROWSERS_COUNT; ++i)
            {
                Browsers[i].Number = i;
                Browsers[i].OnUrlChanged += Browser_OnUrlChanged;
                Browsers[i].OnOrderChanged += Browser_OnOrderChanged;
                Browsers[i].OnPausedChanged += Browser_OnPausedChanged;
                Browsers[i].OnReserved += Browser_OnReserved;

                Browsers[i].Size = BrowserWrappers[i].Size;
                BrowserWrappers[i].Controls.Add(Browsers[i]);

                PausedChangeButtons[i].Click += BrowserContinue_Click;
                ResetButtons[i].Click += ResetButton_Click;
                DoneButtons[i].Click += DoneButton_Click;
            }
        }

        private void Log(string text)
        {
            OrdersInfo.Text += $"{DateTime.Now.ToLongTimeString()}: {text}\n";
        }

        private int FindBrowserNumberByOrder(ReservationOrder order)
        {
            if (order == null)
                return -1;

            for (int i = 0; i < Browsers.Length; ++i)
                if (Browsers[i].Order.Equals(order))
                    return i;

            return -1;
        }

        private int FindBrowserNumberByButton(Button[] buttons, Button find)
        {
            if (find == null)
                return -1;

            for (int i = 0; i < buttons.Length; ++i)
                if (buttons[i].Equals(find))
                    return i;

            return -1;
        }

        private void HandlePausedChangeButtonClick(bool paused, Button sender)
        {
            int browser = FindBrowserNumberByButton(PausedChangeButtons, sender);

            if (browser == -1)
                return;

            Browsers[browser].Paused = paused;
        }

        private void Browser_OnReserved(object sender, ReservedEventArgs e)
        {
            Reserved.Add(new ReservedInfo(e.Order, e.Url));
            SaveData(OUTPUT_FILE_PATH, Reserved);

            Log($"{e.Order.Surname} {e.Order.Name} was registered; Link: {e.Url}");
            HandleBusyChange();
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            int browser = FindBrowserNumberByButton(DoneButtons, sender as Button);

            if (browser == -1)
                return;

            Browsers[browser].Done();
            HandleBusyChange();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            int browser = FindBrowserNumberByButton(ResetButtons, sender as Button);

            if (browser == -1)
                return;

            Browsers[browser].Reset();
            HandleBusyChange();
        }

        private void BrowserContinue_Click(object sender, EventArgs e)
        {
            HandlePausedChangeButtonClick(false, sender as Button);
        }

        private void BrowserPause_Click(object sender, EventArgs e)
        {
            HandlePausedChangeButtonClick(true, sender as Button);
        }

        private void Browser_OnPausedChanged(object sender, BrowserPausedChangedEventArgs e)
        {
            if(e.Paused)
            {
                HandleBusyChange();

                PausedChangeButtons[e.BrowserNumber].Text = "Continue";
                PausedChangeButtons[e.BrowserNumber].Click -= BrowserPause_Click;
                PausedChangeButtons[e.BrowserNumber].Click += BrowserContinue_Click;

                Log($"Browser {e.BrowserNumber} paused");
            }
            else
            {
                HandleBusyChange();

                PausedChangeButtons[e.BrowserNumber].Text = "Pause";
                PausedChangeButtons[e.BrowserNumber].Click -= BrowserContinue_Click;
                PausedChangeButtons[e.BrowserNumber].Click += BrowserPause_Click;

                Log($"Browser {e.BrowserNumber} resumed");
            }
        }

        private void Browser_OnOrderChanged(object sender, OrderEventArgs e)
        {
            ReserverWebView browser = sender as ReserverWebView;
            ReserverInfoView info = Infos[browser.Number];

            info.SurnameInput.InputText = e.Order?.Surname;
            info.NameInput.InputText = e.Order?.Name;
            info.EmailInput.InputText = e.Order?.Email;
        }

        private void Browser_OnUrlChanged(object sender, UrlChangedEventArgs e)
        {
            ReserverWebView browser = sender as ReserverWebView;
            Infos[browser.Number].UrlInput.InputText = e.Url;
        }

        private T LoadData<T>(string filename) where T : new()
        {
            if (!File.Exists(filename))
                return new T();

            using (StreamReader str = new StreamReader(filename))
            {
                string json = str.ReadToEnd();

                if (string.IsNullOrEmpty(json))
                    return new T();

                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        private void SaveData(string filename, object data)
        {
            using (StreamWriter strw = File.CreateText(filename))
                strw.Write(JsonConvert.SerializeObject(data));
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

                Log($"{order.ToString()} is on the {browser.Number} browser");

                browser.Order = order;
                browser.Start();
            }
        }

        private ReserverWebView GetFirstFreeBrowser()
        {
            return Browsers.FirstOrDefault(browser => !browser.IsBusy);
        }

        private void HandleBusyChange()
        {
            bool changesAllowed = Browsers.All(browser => !browser.IsBusy || browser.Paused);

            ObserveOrdersListButton.Enabled = changesAllowed;
            AddOrderButton.Enabled = changesAllowed;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            foreach (ReserverWebView browser in Browsers)
                browser.Paused = false;

            HandleBusyChange();

            Log("All browsers started");
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            foreach (ReserverWebView browser in Browsers)
                browser.Paused = true;

            HandleBusyChange();

            Log("All browsers paused");
        }

        private void ObserveOrdersListButton_Click(object sender, EventArgs e)
        {
            OrderListObserverForm observerForm = new OrderListObserverForm(Orders);
            observerForm.ShowDialog();

            SaveData(DATA_FILE_PATH, Orders);
            ResetBrowserOrderBinding();

            Log("Orders updated and saved");
        }

        private void AddOrderButton_Click(object sender, EventArgs e)
        {
            AddOrderForm addOrderForm = new AddOrderForm(Orders);
            addOrderForm.ShowDialog();

            SaveData(DATA_FILE_PATH, Orders);

            Log("Order added and saved");
        }

        private void ReservedListButton_Click(object sender, EventArgs e)
        {
            ReservedListForm reservedListForm = new ReservedListForm(Reserved);
            reservedListForm.ShowDialog();

            SaveData(OUTPUT_FILE_PATH, Reserved);

            Log("Reserved list saved");
        }

        private void ScheduleButton_Click(object sender, EventArgs e)
        {
            BootScheduleForm scheduleForm = new BootScheduleForm(Schedule);
            scheduleForm.ShowDialog();

            SaveData(SCHEDULE_FILE_PATH, Schedule);

            Log("Schedule saved");
        }

        private void StartScheduledButton_Click(object sender, EventArgs e)
        {
            foreach (ReserverWebView browser in Browsers)
            {
                browser.Schedule = Schedule;
                browser.Paused = false;
            }

            HandleBusyChange();

            Log("All browsers started scheduled");
        }

        private void UnbindScheduleButton_Click(object sender, EventArgs e)
        {
            foreach (ReserverWebView browser in Browsers)
                browser.Schedule = null;

            Log("Schedule detached. Manual control");
        }

        private void ResetBrowserOrderBinding()
        {
            foreach (ReserverWebView browser in Browsers)
                browser.Free();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ask for removing?
            Orders.RemoveAll(order => order.Done);

            SaveData(DATA_FILE_PATH, Orders);
            SaveData(OUTPUT_FILE_PATH, Reserved);
            SaveData(SCHEDULE_FILE_PATH, Schedule);

            Log("ALL Saved");
        }
    }
}
