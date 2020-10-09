﻿using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Newtonsoft.Json;

using PassportMeetReservator.Data;
using PassportMeetReservator.Forms;
using PassportMeetReservator.Controls;
using PassportMeetReservator.Telegram;
using PassportMeetReservator.Data.Platforms;
using PassportMeetReservator.Data.CustomEventArgs;

namespace PassportMeetReservator
{
    public partial class MainForm : Form
    {
        private static string ORDERS_FILE_PATH = Path.Combine(
            Environment.CurrentDirectory, "Data", "orders.json"
        );

        private static string OUTPUT_FILE_PATH = Path.Combine(
            Environment.CurrentDirectory, "Data", "reserved.txt"
        );

        private static string SCHEDULE_FILE_PATH = Path.Combine(
            Environment.CurrentDirectory, "Data", "schedule.json"
        );

        private static string DELAY_SETTINGS_FILE_PATH = Path.Combine(
            Environment.CurrentDirectory, "Data", "delay_settings.json"
        );

        private static string PROFILE_FILE_PATH = Path.Combine(
            Environment.CurrentDirectory, "Data", "profile.json"
        );

        private static string SCREENS_FOLDER_PATH = Path.Combine(
            Environment.CurrentDirectory, "Data", "Screens"
        );

        private static CityPlatformInfo[] Platforms = new CityPlatformInfo[]
        {
            new PoznanInfo(), new KaliszuInfo(), new KoninInfo(),
            new LesznieInfo(), new PileInfo()
        };

        private const string CHECK_DATES_API_AUTH_TOKEN = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjb21wYW55TmFtZSI6InV3cG96bmFuIiwiY29tcGFueUlkIjoiMSIsIm5iZiI6MTYwMTgwNzgyOSwiZXhwIjoyNTM0MDIyOTcyMDAsImlzcyI6IlFNU1dlYlJlc2VydmF0aW9uLkFQSSIsImF1ZCI6IlFNU1dlYlJlc2VydmF0aW9uLkNMSUVOVCJ9.IwDI0942FsfeN2Vm-0tFrg_3aKHU9dsouPpxRYl1OAw";
        private Dictionary<string, DateChecker[]> DateCheckers { get; set; }

        private const string URL_TO_SCREEN_FILENAME_TRASH_PREFIX = "https://rejestracjapoznan.poznan.uw.gov.pl/Info/";

        private const int BROWSERS_COUNT = 5;
        private ReserverWebView[] Browsers { get; set; }
        private ReserverInfoView[] Infos { get; set; }
        private Panel[] BrowserWrappers { get; set; }

        private Button[] PausedChangeButtons { get; set; }
        private Button[] ResetButtons { get; set; }
        private Button[] DoneButtons { get; set; }

        private CheckBox[] AutoCheckers { get; set; }

        private ComboBox[] OperationSelectors { get; set; }
        private ComboBox[] CitySelectors { get; set; }

        private NumericUpDown[] BrowserNumbers { get; set; }

        private DateTimePicker[] ReserveDatesMin { get; set; }
        private DateTimePicker[] ReserveDatesMax { get; set; }

        private List<ReservationOrder> Orders { get; set; }
        private List<ReservedInfo> Reserved { get; set; }
        private BootSchedule Schedule { get; set; }
        private DelayInfo DelayInfo { get; set; }
        private Profile Profile { get; set; }

        private Size NonZoomedSize { get; set; }
        private Point NonZoomedLocation { get; set; }

        private Panel ZoomedBrowserWrapper { get; set; }
        private ReserverWebView ZoomedBrowser { get; set; }

        private TelegramNotifier Notifier { get; set; } = new TelegramNotifier();

        public MainForm()
        {
            InitializeComponent();

            KeyPreview = true;

            DelayInfo = LoadData<DelayInfo>(DELAY_SETTINGS_FILE_PATH);

            InitBrowsers();

            Orders = LoadData<List<ReservationOrder>>(ORDERS_FILE_PATH);
            Reserved = LoadData<List<ReservedInfo>>(OUTPUT_FILE_PATH);
            Schedule = LoadData<BootSchedule>(SCHEDULE_FILE_PATH);

            Profile = LoadData<Profile>(PROFILE_FILE_PATH);
            LogChatId.Text = Profile?.TelegramChatId;

            DateCheckers = DateChecker.CreateFromPlatformInfos(
                Platforms, CHECK_DATES_API_AUTH_TOKEN,
                Checker_OnRequestError, Checker_OnRequestOk,
                DelayInfo
            );

            StartReserving();
        }

        private void ApplyToDateCheckers(Action<DateChecker> action)
        {
            foreach (DateChecker[] checkers in DateCheckers.Values)
                foreach (DateChecker checker in checkers)
                    action(checker);
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

            AutoCheckers = new CheckBox[BROWSERS_COUNT]
            {
                Auto1, Auto2, Auto3, Auto4, Auto5
            };

            CitySelectors = new ComboBox[BROWSERS_COUNT]
            {
                CityChecker1, CityChecker2, CityChecker3,
                CityChecker4, CityChecker5
            };

            OperationSelectors = new ComboBox[BROWSERS_COUNT]
            {
                OrderTypeSelector1, OrderTypeSelector2, OrderTypeSelector3,
                OrderTypeSelector4, OrderTypeSelector5
            };

            BrowserNumbers = new NumericUpDown[BROWSERS_COUNT]
            {
                BrowserNumber1, BrowserNumber2, BrowserNumber3,
                BrowserNumber4, BrowserNumber5
            };

            ReserveDatesMin = new DateTimePicker[BROWSERS_COUNT]
            {
                ReserveDateMin1, ReserveDateMin2, ReserveDateMin3,
                ReserveDateMin4, ReserveDateMin5
            };

            ReserveDatesMax = new DateTimePicker[BROWSERS_COUNT]
            {
                ReserveDateMax1, ReserveDateMax2, ReserveDateMax3,
                ReserveDateMax4, ReserveDateMax5
            };

            for (int i = 0; i < BROWSERS_COUNT; ++i)
            {
                BrowserNumbers[i].Value = i;
                Browsers[i].BrowserNumber = i;
                Browsers[i].RealBrowserNumber = i;
                Browsers[i].OnUrlChanged += Browser_OnUrlChanged;
                Browsers[i].OnPausedChanged += Browser_OnPausedChanged;
                Browsers[i].OnReservedManually += Browser_OnReservedManually;
                Browsers[i].OnReserved += Browser_OnReserved;
                Browsers[i].OnDateTimeSelected += MainForm_OnDateTimeSelected;
                Browsers[i].OnManualReactionWaiting += MainForm_OnManualReactionWaiting;
                Browsers[i].OnOrderChanged += Browser_OnOrderChanged;
                Browsers[i].OnIterationSkipped += Browser_OnIterationSkipped;
                Browsers[i].OnIterationFailure += Browser_OnIterationFailure;
                Browsers[i].DelayInfo = DelayInfo;

                Browsers[i].Size = BrowserWrappers[i].Size;
                Browsers[i].BrowsersCount = BROWSERS_COUNT;

                BrowserWrappers[i].Controls.Add(Browsers[i]);

                PausedChangeButtons[i].Click += BrowserContinue_Click;
                ResetButtons[i].Click += ResetButton_Click;
                DoneButtons[i].Click += DoneButton_Click;

                CitySelectors[i].Items.AddRange(Platforms);
            }

            CityChecker.Items.AddRange(Platforms);
            CityChecker.SelectedIndex = 0;
        }

        private async void Checker_OnRequestError(object sender, DateCheckerErrorEventArgs e)
        {
            DateChecker checker = sender as DateChecker;
            Log($"Date check error at checker {checker.City} : {checker.Operation}; Code: {e.ErrorCode}; Check your VPN and internet connection!");

            await Notifier.NotifyMessage("Check your VPN and internet connection!", LogChatId.Text);
        }

        private void Checker_OnRequestOk(object sender, DateCheckerOkEventArgs e)
        {
            DateChecker checker = sender as DateChecker;
            Log($"Date check ok at checker {checker.City} : {checker.Operation}; Content: {e.Content}");
        }

        private void Browser_OnIterationFailure(object sender, LogEventArgs e)
        {
            Log($"Iteration failure at browser {e.BrowserNumber + 1}: {e.LogText}");
        }

        private void Browser_OnIterationSkipped(object sender, LogEventArgs e)
        {
            Log($"Iteration skipped at browser {e.BrowserNumber + 1}: {e.LogText}");
        }

        private void Log(string text)
        { 
            DateTime now = DateTime.Now;
            OrdersInfo.AppendText($"{now.ToString()}:{now.Millisecond}: {text}\n");
        }

        private int FindBrowserNumberByInfoControl(Control[] controls, Control control)
        {
            if (control == null)
                return -1;

            for (int i = 0; i < controls.Length; ++i)
                if (controls[i].Equals(control))
                    return i;

            return -1;
        }

        private void HandlePausedChangeButtonClick(bool paused, Button sender)
        {
            int browser = FindBrowserNumberByInfoControl(PausedChangeButtons, sender);

            if (browser == -1)
                return;

            Browsers[browser].Paused = paused;
        }

        private async void MainForm_OnManualReactionWaiting(object sender, EventArgs e)
        {
            ReserverWebView browser = sender as ReserverWebView;

            if (!string.IsNullOrEmpty(LogChatId.Text))
                await Notifier.NotifyMessage($"Бот #{browser.BotNumber + 1} чекає на заповення форми на браузері #{browser.RealBrowserNumber + 1}", LogChatId.Text);
        }

        private async void MainForm_OnDateTimeSelected(object sender, DateTimeEventArgs e)
        {
            ReserverWebView browser = sender as ReserverWebView;

            if (!string.IsNullOrEmpty(LogChatId.Text))
                await Notifier.NotifyMessage($"Бот #{browser.BotNumber + 1} злапав дату {e.Date.ToString()} на браузері #{browser.RealBrowserNumber + 1}", LogChatId.Text);
        }

        private async void Browser_OnReserved(object sender, ReservedEventArgs e)
        {
            SaveData(ORDERS_FILE_PATH, Orders);

            if (!string.IsNullOrEmpty(LogChatId.Text))
                await Notifier.NotifyMessage($"{e.Order?.Surname} {e.Order?.Name}: {e.Url}", LogChatId.Text);

            Reserved.Add(new ReservedInfo(e.Url));
            Log($"Link {e.Order?.Surname} {e.Order?.Name}: {e.Url}");

            PutOrderToBrowser(sender as ReserverWebView);
            HandleBusyChange();
        }

        private async void Browser_OnReservedManually(object sender, ReservedEventArgs e)
        {
            ReserverWebView view = sender as ReserverWebView;

            bool screenSaved = false;
            string path = Path.Combine(SCREENS_FOLDER_PATH, $"{e.Url.Replace(URL_TO_SCREEN_FILENAME_TRASH_PREFIX, "")}.png");

            ZoomBrowser(view.RealBrowserNumber);
            view.ResetScroll();

            Task.Delay(1000).GetAwaiter().GetResult();

            try
            {
                this.Snapshot(view).Save(path);
                screenSaved = true;
            }
            catch (Exception ex) { }

            ResetZoom();

            if (!string.IsNullOrEmpty(LogChatId.Text))
            {
                if (screenSaved)
                    await Notifier.NotifyPhoto(path, e.Url, LogChatId.Text);
                else
                    await Notifier.NotifyMessage(e.Url, LogChatId.Text);
            }

            Reserved.Add(new ReservedInfo(e.Url));
            SaveData(OUTPUT_FILE_PATH, Reserved);

            Log($"Link: {e.Url}");
            HandleBusyChange();
        }

        private async void Browser_OnOrderChanged(object sender, OrderEventArgs e)
        {
            ReserverWebView browser = sender as ReserverWebView;

            if (browser.Order == null)
            {
                await Notifier.NotifyMessage($"Браузер #{browser.RealBrowserNumber + 1}(бот #{browser.BotNumber + 1}) не має ордера (працює в ручному режимі)", LogChatId.Text);
                Log($"Browser #{browser.RealBrowserNumber + 1} has no order assigned, so working in manual mode");

                if (browser.Auto)
                    PutOrderToBrowser(browser);
            }
            else
            {
                await Notifier.NotifyMessage($"{browser.Order.Surname} {browser.Order.Name} лапається на браузері #{browser.RealBrowserNumber + 1}(бот #{browser.BotNumber})", LogChatId.Text);
                Log($"{browser.Order.Surname} {browser.Order.Name} is reserving on browser #{browser.RealBrowserNumber + 1}");
            }
        }

        private DateChecker FindDateCheckerByOrder(ReservationOrder order)
        {
            return DateCheckers[order.City][order.Operation.Position];
        }

        private void PutOrderToBrowser(ReserverWebView browser)
        {
            ReservationOrder found = Orders.FirstOrDefault(order => !order.Doing && !order.Done);

            if (found != null)
            {
                browser.Order = found;
                browser.Checker = FindDateCheckerByOrder(found);
            }
        }

        private void PutOrderToBrowser(ReservationOrder order)
        {
            ReserverWebView found = Browsers.FirstOrDefault(browser => browser.Auto && browser.Order == null && !browser.Selected);

            if (found != null)
            {
                found.Order = order;
                found.Checker = FindDateCheckerByOrder(order);
            }
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            int browser = FindBrowserNumberByInfoControl(DoneButtons, sender as Button);

            if (browser == -1)
                return;

            Browsers[browser].Done();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            int browser = FindBrowserNumberByInfoControl(ResetButtons, sender as Button);

            if (browser == -1)
                return;

            Browsers[browser].Reset();
        }

        private void OrderTypeSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox operationSelector = sender as ComboBox;
            int browser = FindBrowserNumberByInfoControl(OperationSelectors, operationSelector);

            if (browser == -1)
                return;

            string selectedCity = (CitySelectors[browser].SelectedItem as CityPlatformInfo).Name;
            OperationInfo selectedOperation = operationSelector.SelectedItem as OperationInfo; ;

            Browsers[browser].Operation = selectedOperation;
            Browsers[browser].InitChecker = DateCheckers[selectedCity][selectedOperation.Position];
        }

        private void CityChecker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cityChecker = sender as ComboBox;
            int browser = FindBrowserNumberByInfoControl(CitySelectors, cityChecker);

            if (browser == -1)
                return;

            CityPlatformInfo selectedPlatform = cityChecker.SelectedItem as CityPlatformInfo;

            Browsers[browser].InitUrl = selectedPlatform.BaseUrl;
            Browsers[browser].Operation = null;
            Browsers[browser].InitChecker = null;

            OperationSelectors[browser].Items.Clear();
            OperationSelectors[browser].Items.AddRange(selectedPlatform.Operations);
        }

        private void ReserveDateMin_ValueChanged(object sender, EventArgs e)
        {
            int browser = FindBrowserNumberByInfoControl(ReserveDatesMin, sender as DateTimePicker);

            if (browser == -1)
                return;

            Browsers[browser].ReserveDateMin = (sender as DateTimePicker).Value.Date;
        }

        private void ReserveDateMax_ValueChanged(object sender, EventArgs e)
        {
            int browser = FindBrowserNumberByInfoControl(ReserveDatesMax, sender as DateTimePicker);

            if (browser == -1)
                return;

            Browsers[browser].ReserveDateMax = (sender as DateTimePicker).Value.Date;
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

                Log($"Browser {e.BrowserNumber + 1} paused");
            }
            else
            {
                HandleBusyChange();

                Browsers[e.BrowserNumber].ReserveDateMin = ReserveDatesMin[e.BrowserNumber].Value.Date;
                Browsers[e.BrowserNumber].ReserveDateMax = ReserveDatesMax[e.BrowserNumber].Value.Date;

                PausedChangeButtons[e.BrowserNumber].Text = "Pause";
                PausedChangeButtons[e.BrowserNumber].Click -= BrowserContinue_Click;
                PausedChangeButtons[e.BrowserNumber].Click += BrowserPause_Click;

                Log($"Browser {e.BrowserNumber + 1} resumed");
            }
        }

        private void Browser_OnUrlChanged(object sender, UrlChangedEventArgs e)
        {
            ReserverWebView browser = sender as ReserverWebView;
            Infos[browser.RealBrowserNumber].UrlInput.InputText = e.Url;
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

        private async void StartReserving()
        {
            while(true)
            {
                await Task.Delay(DelayInfo.OrderLoadingIterationDelay);
                ReserveIteration();
            }
        }

        private void ReserveIteration()
        {
            ReserverWebView browser = GetFirstFreeBrowser();

            if (browser == null)
                return;

            browser.Start();
        }

        private ReserverWebView GetFirstFreeBrowser()
        {
            return Browsers.FirstOrDefault(browser => !browser.IsBusy);
        }

        private void HandleBusyChange()
        {
            bool changesAllowed = Browsers.All(browser => !browser.IsBusy || browser.Paused);

            OrderListButton.Enabled = changesAllowed;
            AddOrderButton.Enabled = changesAllowed;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            foreach (ReserverWebView browser in Browsers)
                browser.Paused = false;

            HandleBusyChange();

            Log("All browsers started");
        }

        private void Auto_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checker = sender as CheckBox;
            int browser = FindBrowserNumberByInfoControl(AutoCheckers, checker);

            if (browser == -1)
                return;

            Browsers[browser].Auto = checker.Checked;

            if(checker.Checked)
                PutOrderToBrowser(Browsers[browser]);
        }

        private void BrowserNumber_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown browserNumber = sender as NumericUpDown;
            int browser = FindBrowserNumberByInfoControl(BrowserNumbers, browserNumber);

            if (browser == -1)
                return;

            Browsers[browser].BrowserNumber = (int)browserNumber.Value;
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            SetPausedToAllBrowsers(true);
            Log("All browsers paused");

            PauseButton.Text = "Continue";
            PauseButton.Click -= PauseButton_Click;
            PauseButton.Click += ContinueButton_Click;
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            SetPausedToAllBrowsers(false);
            Log("All browsers continued");

            PauseButton.Text = "Pause";
            PauseButton.Click -= ContinueButton_Click;
            PauseButton.Click += PauseButton_Click;
        }

        private void SetPausedToAllBrowsers(bool paused)
        {
            foreach (ReserverWebView browser in Browsers)
                browser.Paused = paused;

            HandleBusyChange();
        }

        private void ResetAllButton_Click(object sender, EventArgs e)
        {
            foreach (ReserverWebView browser in Browsers)
                browser.Reset();
        }

        private void ReservedListButton_Click(object sender, EventArgs e)
        {
            ReservedListForm reservedListForm = new ReservedListForm(Reserved);
            reservedListForm.ShowDialog();

            SaveData(OUTPUT_FILE_PATH, Reserved);

            Log("Reserved list saved");
        }

        private void AddOrderButton_Click(object sender, EventArgs e)
        {
            AddOrderForm addOrderForm = new AddOrderForm(Platforms, Orders);
            addOrderForm.ShowDialog();

            SaveData(ORDERS_FILE_PATH, Orders);

            foreach (ReservationOrder found in Orders.Where(order => !order.Doing && !order.Done))
                PutOrderToBrowser(found);
        }

        private void OrderListButton_Click(object sender, EventArgs e)
        {
            OrderListForm orderListForm = new OrderListForm(Orders);
            orderListForm.ShowDialog();

            SaveData(ORDERS_FILE_PATH, Orders);
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

            ApplyToDateCheckers(checker => checker.Schedule = Schedule);

            Log("All browsers started scheduled");
        }

        private void UnbindScheduleButton_Click(object sender, EventArgs e)
        {
            foreach (ReserverWebView browser in Browsers)
                browser.Schedule = null;

            ApplyToDateCheckers(checker => checker.Schedule = null);

            Log("Schedule detached. Manual control");
        }

        private void DelaySettings_Click(object sender, EventArgs e)
        {
            DelayInfoForm delayInfoForm = new DelayInfoForm(DelayInfo);
            delayInfoForm.ShowDialog();

            SaveData(DELAY_SETTINGS_FILE_PATH, DelayInfo);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveData(ORDERS_FILE_PATH, Orders);
            SaveData(OUTPUT_FILE_PATH, Reserved);
            SaveData(SCHEDULE_FILE_PATH, Schedule);

            Log("ALL Saved");
        }

        private void BotNumber_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < Browsers.Length; ++i)
                Browsers[i].BotNumber = (int)BotNumber.Value;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                ResetZoom();
            else if (e.Control && e.KeyCode >= Keys.D1 && e.KeyCode < Keys.D1 + BROWSERS_COUNT)
            {
                ResetZoom();
                ZoomBrowser(e.KeyCode - Keys.D1);
            }
        }

        private void ZoomBrowser(int browser)
        {
            ZoomedBrowser = Browsers[browser];
            ZoomedBrowserWrapper = BrowserWrappers[browser];

            NonZoomedSize = ZoomedBrowserWrapper.Size;
            NonZoomedLocation = ZoomedBrowserWrapper.Location;

            ZoomedBrowser.Size = new Size(this.Width, this.Height);

            ZoomedBrowserWrapper.Location = new Point(0, 0);
            ZoomedBrowserWrapper.Size = new Size(this.Width, this.Height);

            ZoomedBrowserWrapper.BringToFront();
            ForceRollBrowserUp.BringToFront();
        }

        private void ResetZoom()
        {
            if (ZoomedBrowserWrapper != null)
            {
                ZoomedBrowserWrapper.Size = NonZoomedSize;
                ZoomedBrowserWrapper.Location = NonZoomedLocation;
            }

            if (ZoomedBrowser != null)
                ZoomedBrowser.Size = NonZoomedSize;
        }

        private void ForceRollBrowserUp_Click(object sender, EventArgs e)
        {
            ResetZoom();
        }

        private void OrderTypeSelector_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            foreach (ComboBox orderTypeSelector in OperationSelectors)
                orderTypeSelector.SelectedIndex = OrderTypeSelector.SelectedIndex;
        }

        private void CityChecker_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            foreach (ComboBox citySelector in CitySelectors)
                citySelector.SelectedIndex = CityChecker.SelectedIndex;

            OrderTypeSelector.Items.Clear();
            OrderTypeSelector.Items.AddRange((CityChecker.SelectedItem as CityPlatformInfo).Operations);
        }

        private void ReserveDateMinPicker_ValueChanged(object sender, EventArgs e)
        {
            foreach (DateTimePicker reserveDatePicker in ReserveDatesMin)
                reserveDatePicker.Value = ReserveDateMinPicker.Value;
        }

        private void ReserveDateMaxPicker_ValueChanged(object sender, EventArgs e)
        {
            foreach (DateTimePicker reserveDatePicker in ReserveDatesMax)
                reserveDatePicker.Value = ReserveDateMaxPicker.Value;
        }

        private void LogChatId_TextChanged(object sender, EventArgs e)
        {
            Profile.TelegramChatId = LogChatId.Text;
            SaveData(PROFILE_FILE_PATH, Profile);
        }
    }
}
