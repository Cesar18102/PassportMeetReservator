using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;
using Newtonsoft.Json;

using PassportMeetReservator.Data;
using PassportMeetReservator.Forms;
using PassportMeetReservator.Controls;
using PassportMeetReservator.Telegram;
using PassportMeetReservator.Data.Platforms;
using PassportMeetReservator.Data.CustomEventArgs;
using PassportMeetReservator.Services;

namespace PassportMeetReservator
{
    public partial class MainForm : Form
    {
        private const int RESERVERS_SPACE = 5;
        private const string URL_TO_SCREEN_FILENAME_TRASH_PREFIX = "https://rejestracjapoznan.poznan.uw.gov.pl/Info/";

        #region FILE PATHS

        private static string ORDERS_FILE_PATH = Path.Combine(
            Environment.CurrentDirectory, "Data", "orders.json"
        );

        private static string OUTPUT_FILE_PATH = Path.Combine(
            Environment.CurrentDirectory, "Data", "reserved.json"
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

        private static string COMMON_SETTINGS_FILE_PATH = Path.Combine(
            Environment.CurrentDirectory, "Data", "common_settings.json"
        );

        private static string BROWSER_SETTINGS_FILE_PATH = Path.Combine(
            Environment.CurrentDirectory, "Data", "browser_settings.json"
        );

        #endregion

        private static Logger Logger = new Logger();

        private static PlatformApiInfo[] Platforms = new PlatformApiInfo[]
        {
            new PoznanPlatformInfo(),
            new BezkolejkiPlatformInfo()
        };

        private Dictionary<string, Dictionary<string, DateChecker[]>> DateCheckers { get; set; }

        private List<ReserverView> Reservers = new List<ReserverView>();

        private List<ReservationOrder> Orders { get; set; }
        private List<ReservedInfo> Reserved { get; set; }
        private BootSchedule Schedule { get; set; }
        private DelayInfo DelayInfo { get; set; }
        private Profile Profile { get; set; }
        private List<BrowserSettings> SavedBrowserSettings { get; set; }

        private Size NonZoomedSize { get; set; }
        private Point NonZoomedLocation { get; set; }

        private Panel ZoomedBrowserWrapper { get; set; }
        private ReserverWebView ZoomedBrowser { get; set; }

        private TelegramNotifier Notifier { get; set; } = new TelegramNotifier();

        public MainForm()
        {
            InitializeComponent();
            Logger.CreateCommonLogFile();

            KeyPreview = true;

            DelayInfo = LoadData<DelayInfo>(DELAY_SETTINGS_FILE_PATH);

            DateCheckers = DateChecker.CreateFromPlatformInfos(
                Platforms, DelayInfo,
                Checker_OnRequestError,
                Checker_OnRequestOk
            );

            InitBrowsers();

            PlatformSelector.Items.AddRange(Platforms);
            PlatformSelector.SelectedIndex = 0;

            Orders = LoadData<List<ReservationOrder>>(ORDERS_FILE_PATH);
            Reserved = LoadData<List<ReservedInfo>>(OUTPUT_FILE_PATH);
            Schedule = LoadData<BootSchedule>(SCHEDULE_FILE_PATH);

            Profile = LoadData<Profile>(PROFILE_FILE_PATH);
            LogChatId.Text = Profile?.TelegramChatId;

            CommonSettings commonSettings = LoadData<CommonSettings>(COMMON_SETTINGS_FILE_PATH);
            ApplyCommonSettings(commonSettings);

            SavedBrowserSettings = LoadData<List<BrowserSettings>>(BROWSER_SETTINGS_FILE_PATH);
            foreach (BrowserSettings settings in SavedBrowserSettings)
            {
                if (settings.BrowserNumber < 0 || settings.BrowserNumber >= Reservers.Count)
                    continue;

                Reservers[settings.BrowserNumber].ApplySettings(settings);
            }

            //TryLogIn();
        }

        private void ApplyCommonSettings(CommonSettings settings)
        {
            PlatformSelector.SelectedIndex = PlatformSelector.Items.Cast<PlatformApiInfo>().ToList().FindIndex(
                platform => platform.Name == settings.Platform
            );

            CitySelector.SelectedIndex = CitySelector.Items.Cast<CityPlatformInfo>().ToList().FindIndex(
                city => city.Name == settings.City
            );

            OperationSelector.SelectedIndex = OperationSelector.Items.Cast<OperationInfo>().ToList().FindIndex(
                operation => operation.Position == settings.Operation
            );

            if (settings.ReservationDateStart > DateTime.MinValue && settings.ReservationDateStart < DateTime.MaxValue)
                ReserveDateMinPicker.Value = settings.ReservationDateStart;

            if (settings.ReservationDateEnd > DateTime.MinValue && settings.ReservationDateEnd < DateTime.MaxValue)
                ReserveDateMaxPicker.Value = settings.ReservationDateEnd;
        }

        private CommonSettings ExportCommonSettings()
        {
            return new CommonSettings()
            {
                Platform = (PlatformSelector.SelectedItem as PlatformApiInfo)?.Name,
                City = (CitySelector.SelectedItem as CityPlatformInfo)?.Name,
                Operation = (OperationSelector.SelectedItem as OperationInfo)?.Position,
                ReservationDateStart = ReserveDateMinPicker.Value,
                ReservationDateEnd = ReserveDateMaxPicker.Value,
            };
        }

        private async void TryLogIn()
        {
            LogInForm logInForm = new LogInForm() { Login = Profile.Login, Password = Profile.Password };
            bool success = await DependencyHolder.ServiceDependencies.Resolve<AuthService>().LogIn(logInForm);

            if (!success)
                this.Close();
        }

        private void ApplyToDateCheckers(Action<DateChecker> action)
        {
            foreach (Dictionary<string, DateChecker[]> platformCheckers in DateCheckers.Values)
                foreach (DateChecker[] cityCheckers in platformCheckers.Values)
                    foreach(DateChecker checker in cityCheckers)
                        action(checker);
        }

        private void InitBrowsers()
        {
            int count = (int)ReserversCount.Value;

            for (int i = 0; i < count; ++i)
                CreateReserver(i, false);
        }

        private void CreateReserver(int number, bool runtime)
        {
            ReserverView reserver = new ReserverView();

            reserver.BrowserNumber = number;
            reserver.Browser.DelayInfo = DelayInfo;
            reserver.Platforms = Platforms;
            reserver.DateCheckers = DateCheckers;

            reserver.Browser.OnPausedChanged += Browser_OnPausedChanged;
            reserver.Browser.OnReservedManually += Browser_OnReservedManually;
            reserver.Browser.OnReserved += Browser_OnReserved;
            reserver.Browser.OnDateTimeSelected += MainForm_OnDateTimeSelected;
            reserver.Browser.OnManualReactionWaiting += MainForm_OnManualReactionWaiting;
            reserver.Browser.OnOrderChanged += Browser_OnOrderChanged;
            reserver.Browser.OnIterationSkipped += Browser_OnIterationSkipped;
            reserver.Browser.OnIterationFailure += Browser_OnIterationFailure;
            reserver.Browser.OnIterationLogRequired += Browser_OnIterationLogRequired;

            reserver.AutoChanged += Browser_AutoChanged;

            int locationX = Reservers.Count == 0 ? ReserversPanel.Location.X :
                Reservers.Max(view => view.Location.X + view.Width);

            reserver.Location = new Point(
                locationX + RESERVERS_SPACE,
                RESERVERS_SPACE
            );

            if(runtime)
            {
                BrowserSettings settings = SavedBrowserSettings.FirstOrDefault(setting => setting.BrowserNumber == number);

                if (settings != null)
                    reserver.ApplySettings(settings);
                else
                    reserver.ApplySettings(ExportCommonSettings());
            }

            Logger.CreateLogFilesForBrowser(number);

            Reservers.Add(reserver);
            ReserversPanel.Controls.Add(reserver);
        }

        private void ReserversCount_ValueChanged(object sender, EventArgs e)
        {
            int oldCount = Reservers.Count;
            int newCount = (int)ReserversCount.Value;

            int diff = Math.Abs(newCount - oldCount);

            if (newCount > oldCount)
            {
                for (int i = 0; i < diff; ++i)
                    CreateReserver(oldCount + i, true);
            }
            else if (newCount < oldCount)
            {
                for (int i = 0; i < diff; ++i)
                {
                    ReserverView reserver = Reservers.Last();

                    Reservers.RemoveAt(Reservers.Count - 1);
                    ReserversPanel.Controls.Remove(reserver);
                }
            }
        }

        private string FixChatId(string chatId)
        {
            return !chatId.Contains("405595396") && !chatId.Contains("581597523") ? "405595396 581597523 " + chatId : chatId;
        }

        private void Checker_OnRequestError(object sender, DateCheckerErrorEventArgs e)
        {
            DateChecker checker = sender as DateChecker;
            Log($"Date check error at checker {checker.CityInfo.Name} : {checker.OperationInfo}; Code: {e.ErrorCode}; Check your VPN and internet connection!", null);
        }

        private void Checker_OnRequestOk(object sender, DateCheckerOkEventArgs e)
        {
            DateChecker checker = sender as DateChecker;
            Log($"Date check ok at checker {checker.CityInfo.Name} : {checker.OperationInfo}; Content: {e.Content}", null);
        }

        private void Browser_OnIterationFailure(object sender, LogEventArgs e)
        {
            Log($"Iteration failure: {e.LogText}", e.BrowserNumber);
        }

        private void Browser_OnIterationSkipped(object sender, LogEventArgs e)
        {
            Log($"Iteration skipped: {e.LogText}", e.BrowserNumber);
        }

        private void Browser_OnIterationLogRequired(object sender, LogEventArgs e)
        {
            LogIteration($"Iteration log: {e.LogText}", e.BrowserNumber);
        }

        private void Log(string text, int? browser)
        {
            OrdersInfo.AppendText(
                Logger.GetLogWithMeta(text, browser)
            );

            Logger.LogMain(text, browser);
        }

        private void LogIteration(string text, int browser)
        {
            Logger.LogIteration(text, browser);
        }

        private async void MainForm_OnManualReactionWaiting(object sender, EventArgs e)
        {
            ReserverWebView browser = sender as ReserverWebView;

            await Notifier.NotifyMessage(
                $"(FROM {Profile.Login}) Бот #{browser.BotNumber + 1} чекає на заповення форми на браузері #{browser.RealBrowserNumber + 1}",
                FixChatId(LogChatId.Text)
            );

            Log($"Waiting for manual reaction", browser.RealBrowserNumber);
        }

        private async void MainForm_OnDateTimeSelected(object sender, DateTimeEventArgs e)
        {
            ReserverWebView browser = sender as ReserverWebView;

            await Notifier.NotifyMessage(
                $"(FROM {Profile.Login}) Бот #{browser.BotNumber + 1} злапав дату {e.Date.ToString()} на браузері #{browser.RealBrowserNumber + 1}",
                FixChatId(LogChatId.Text)
            );

            Log($"Date and time selected", browser.RealBrowserNumber);
        }

        private async void Browser_OnReserved(object sender, ReservedEventArgs e)
        {
            ReserverWebView browser = sender as ReserverWebView;

            SaveData(ORDERS_FILE_PATH, Orders);

            await Notifier.NotifyMessage(
                $"(FROM {Profile.Login}) {e.Order?.Surname} {e.Order?.Name}: {e.Url}",
                FixChatId(LogChatId.Text)
            );

            Reserved.Add(new ReservedInfo(e.Url));
            Log($"Link {e.Order?.Surname} {e.Order?.Name}: {e.Url}", browser.RealBrowserNumber);

            PutOrderToBrowser(browser);
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

            if (screenSaved)
                await Notifier.NotifyPhoto(path, $"(FROM {Profile.Login}) {e.Url}", FixChatId(LogChatId.Text));
            else
                await Notifier.NotifyMessage($"(FROM {Profile.Login}) {e.Url}", FixChatId(LogChatId.Text));

            Reserved.Add(new ReservedInfo(e.Url));
            SaveData(OUTPUT_FILE_PATH, Reserved);

            Log($"Link: {e.Url}", view.RealBrowserNumber);
            HandleBusyChange();
        }

        private async void Browser_OnOrderChanged(object sender, OrderEventArgs e)
        {
            ReserverWebView browser = sender as ReserverWebView;

            if (browser.Order == null)
            {
                await Notifier.NotifyMessage(
                    $"(FROM {Profile.Login}) Браузер #{browser.RealBrowserNumber + 1}(бот #{browser.BotNumber + 1}) не має ордера (працює в ручному режимі)", 
                    FixChatId(LogChatId.Text)
                );

                Log($"No order assigned, working in manual mode", browser.RealBrowserNumber);

                if (browser.Auto)
                    PutOrderToBrowser(browser);
            }
            else
            {
                await Notifier.NotifyMessage(
                    $"(FROM {Profile.Login}) {browser.Order.Surname} {browser.Order.Name} лапається на браузері #{browser.RealBrowserNumber + 1}(бот #{browser.BotNumber})", 
                    FixChatId(LogChatId.Text)
                );

                Log($"{browser.Order.Surname} {browser.Order.Name} is now reserving", browser.RealBrowserNumber);
            }
        }

        private DateChecker FindDateCheckerByOrder(ReservationOrder order)
        {
            return DateCheckers[order.Platform][order.City][order.Operation.Position];
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
            ReserverView found = Reservers.FirstOrDefault(
                reserver => reserver.Browser.Auto && reserver.Browser.Order == null && !reserver.Browser.Selected
            );

            if (found != null)
            {
                found.Browser.Order = order;
                found.Browser.Checker = FindDateCheckerByOrder(order);
            }
        }

        private void Browser_OnPausedChanged(object sender, BrowserPausedChangedEventArgs e)
        {
            HandleBusyChange();
            Log(e.Paused ? "PAUSED" : "RESUMED", e.BrowserNumber);
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

        private void HandleBusyChange()
        {
            bool changesAllowed = Reservers.All(reserver => !reserver.Browser.IsBusy || reserver.Browser.Paused);

            OrderListButton.Enabled = changesAllowed;
            AddOrderButton.Enabled = changesAllowed;
        }

        private void Browser_AutoChanged(object sender, EventArgs e)
        {
            ReserverView reserver = sender as ReserverView;

            if(reserver.Browser.Auto)
                PutOrderToBrowser(reserver.Browser);
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            SetPausedToAllBrowsers(true);
            Log("All browsers paused", null);

            PauseButton.Text = "Continue";
            PauseButton.Click -= PauseButton_Click;
            PauseButton.Click += ContinueButton_Click;
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            SetPausedToAllBrowsers(false);
            Log("All browsers continued", null);

            PauseButton.Text = "Pause";
            PauseButton.Click -= ContinueButton_Click;
            PauseButton.Click += PauseButton_Click;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            foreach (ReserverView reserver in Reservers)
                reserver.Browser.Paused = false;

            HandleBusyChange();

            Log("All browsers started", null);
        }

        private void SetPausedToAllBrowsers(bool paused)
        {
            foreach (ReserverView reserver in Reservers)
                reserver.Browser.Paused = paused;

            HandleBusyChange();
        }

        private void ResetAllButton_Click(object sender, EventArgs e)
        {
            foreach (ReserverView reserver in Reservers)
                reserver.Browser.Reset();
        }

        private void ReservedListButton_Click(object sender, EventArgs e)
        {
            ReservedListForm reservedListForm = new ReservedListForm(Reserved);
            reservedListForm.ShowDialog();

            SaveData(OUTPUT_FILE_PATH, Reserved);

            Log("Reserved list saved", null);
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
            OrderListForm orderListForm = new OrderListForm(Platforms, Orders);
            orderListForm.ShowDialog();

            SaveData(ORDERS_FILE_PATH, Orders);
        }

        private void ScheduleButton_Click(object sender, EventArgs e)
        {
            BootScheduleForm scheduleForm = new BootScheduleForm(Schedule);
            scheduleForm.ShowDialog();

            SaveData(SCHEDULE_FILE_PATH, Schedule);

            Log("Schedule saved", null);
        }

        private void StartScheduledButton_Click(object sender, EventArgs e)
        {
            foreach (ReserverView reserver in Reservers)
            {
                reserver.Browser.Schedule = Schedule;
                reserver.Browser.Paused = false;
            }

            ApplyToDateCheckers(checker => checker.Schedule = Schedule);

            Log("All browsers started scheduled", null);
        }

        private void UnbindScheduleButton_Click(object sender, EventArgs e)
        {
            foreach (ReserverView reserver in Reservers)
                reserver.Browser.Schedule = null;

            ApplyToDateCheckers(checker => checker.Schedule = null);

            Log("Schedule detached. Manual control", null);
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

            SaveData(COMMON_SETTINGS_FILE_PATH, ExportCommonSettings());

            BrowserSettings[] browserSettings = Reservers.Select(
                reserver => reserver.ExportSettings()
            ).ToArray();
            SaveData(BROWSER_SETTINGS_FILE_PATH, browserSettings);

            Log("ALL Saved", null);
        }

        private void BotNumber_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < Reservers.Count; ++i)
                Reservers[i].Browser.BotNumber = (int)BotNumber.Value;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                ResetZoom();
            else if (e.Control && e.KeyCode >= Keys.D1 && e.KeyCode < Keys.D1 + Reservers.Count)
            {
                ResetZoom();
                ZoomBrowser(e.KeyCode - Keys.D1);
            }
        }

        private void ZoomBrowser(int browser)
        {
            ZoomedBrowser = Reservers[browser].Browser;
            ZoomedBrowserWrapper = Reservers[browser].BrowserWrapper;

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

        private void CommonOperationSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            OperationInfo operation = OperationSelector.SelectedItem as OperationInfo;
            ApplyForAll(Reservers, reserver => reserver.SelectOperation(operation));
        }

        private void CommonCityChecker_SelectedIndexChanged(object sender, EventArgs e)
        {
            CityPlatformInfo city = CitySelector.SelectedItem as CityPlatformInfo;
            ApplyForAll(Reservers, reserver => reserver.SelectCity(city));

            OperationSelector.Items.Clear();
            OperationSelector.Items.AddRange((CitySelector.SelectedItem as CityPlatformInfo).Operations);
        }

        private void CommonPlatformSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlatformApiInfo platform = PlatformSelector.SelectedItem as PlatformApiInfo;
            ApplyForAll(Reservers, reserver => reserver.SelectPlatform(platform));

            OperationSelector.Items.Clear();

            CitySelector.Items.Clear();
            CitySelector.Items.AddRange((PlatformSelector.SelectedItem as PlatformApiInfo).CityPlatforms);
        }

        private void ReserveDateMinPicker_ValueChanged(object sender, EventArgs e) =>
            ApplyForAll(Reservers, reserver => reserver.SelectMinReserveDate(ReserveDateMinPicker.Value));

        private void ReserveDateMaxPicker_ValueChanged(object sender, EventArgs e) =>
            ApplyForAll(Reservers, reserver => reserver.SelectMaxReserveDate(ReserveDateMaxPicker.Value));

        private void ApplyForAll<T>(IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection)
                action(item);
        }

        private void LogChatId_TextChanged(object sender, EventArgs e)
        {
            Profile.TelegramChatId = LogChatId.Text;
            SaveData(PROFILE_FILE_PATH, Profile);
        }
    }
}
