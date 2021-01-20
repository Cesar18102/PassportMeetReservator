using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using Autofac;

using PassportMeetReservator.Data;
using PassportMeetReservator.Forms;
using PassportMeetReservator.Controls;
using PassportMeetReservator.Telegram;
using PassportMeetReservator.Data.CustomEventArgs;
using PassportMeetReservator.Extensions;

using Common;
using Common.Data;
using Common.Data.Forms;
using Common.Data.Platforms;
using Common.Data.CustomEventArgs;

using Common.Extensions;

using Common.Services;
using Common.Strategies.DateCheckerNotifyStrategies;
using Common.Forms;

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

        private static Dictionary<string, int> Proxies = new Dictionary<string, int>()
        {
            //{ "46.238.230.4:8080", 0 },
            //{ "85.198.250.135:3128", 0 },
            //{ "62.87.151.138:35116", 0 },
            //{ "85.221.247.234:8080", 0 },
            //{ "80.53.233.124:80", 0 },
            //{ "146.120.214.62:8080", 0 }
            //{ "91.228.89.29:3128", 0 },
            //{ "62.133.130.206:3128", 0 }
        };

        private static Logger Logger = DependencyHolder.ServiceDependencies.Resolve<Logger>();
        private static FileService FileService = DependencyHolder.ServiceDependencies.Resolve<FileService>();

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

        private TelegramNotifier Notifier { get; set; } = new TelegramNotifier();

        private NotifyAlwaysFlowStrategy NOTIFY_ALWAYS_DATE_CHECKER_FLOW_STRATEGY = new NotifyAlwaysFlowStrategy();
        private NotifyIfDatesFoundFlowStrategy NOTIFY_IF_DATES_FOUND_DATE_CHECKER_FLOW_STRATEGY = new NotifyIfDatesFoundFlowStrategy();
        private NotifyIfDatesAndTimesFoundFlowStrategy NOTIFY_IF_DATES_AND_TIMES_FOUND_DATE_CHECKER_STRATEGY = new NotifyIfDatesAndTimesFoundFlowStrategy();

        public MainForm()
        {
            InitializeComponent();
            Logger.CreateCommonLogFile();

            KeyPreview = true;

            DelayInfo = FileService.LoadData<DelayInfo>(DELAY_SETTINGS_FILE_PATH);

            DateCheckers = DateChecker.CreateFromPlatformInfos<DateChecker>(
                Platforms, DelayInfo,
                Checker_OnRequestError,
                Checker_OnRequestOk
            );

            InitBrowsers();

            PlatformSelector.Items.AddRange(Platforms);
            PlatformSelector.SelectedIndex = 0;

            Orders = FileService.LoadData<List<ReservationOrder>>(ORDERS_FILE_PATH);
            Reserved = FileService.LoadData<List<ReservedInfo>>(OUTPUT_FILE_PATH);
            Schedule = FileService.LoadData<BootSchedule>(SCHEDULE_FILE_PATH);

            Profile = FileService.LoadData<Profile>(PROFILE_FILE_PATH);
            LogChatId.Text = Profile?.TelegramChatId;

            CommonSettings commonSettings = FileService.LoadData<CommonSettings>(COMMON_SETTINGS_FILE_PATH);
            ApplyCommonSettings(commonSettings);

            SavedBrowserSettings = FileService.LoadData<List<BrowserSettings>>(BROWSER_SETTINGS_FILE_PATH);
            foreach (BrowserSettings settings in SavedBrowserSettings)
            {
                if (settings.BrowserNumber < 0 || settings.BrowserNumber >= Reservers.Count)
                    continue;

                Reservers[settings.BrowserNumber].ApplySettings(settings);
            }

            UpdateDateCheckersFlowStrategy();

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

        private void InitBrowsers()
        {
            int count = (int)ReserversCount.Value;

            for (int i = 0; i < count; ++i)
                CreateReserver(i, false);
        }

        private void CreateReserver(int number, bool runtime)
        {
            ReserverView reserver = new ReserverView();

            reserver.RealBrowserNumber = number;
            reserver.Browser.DelayInfo = DelayInfo;
            reserver.Platforms = Platforms;
            reserver.DateCheckers = DateCheckers;

            reserver.Browser.Proxy = Proxies.OrderBy(prx => prx.Value).FirstOrDefault().Key;

            if (reserver.Browser.Proxy != null)
                Proxies[reserver.Browser.Proxy]++;

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

                    reserver.ZoomOut();
                    Reservers.RemoveAt(Reservers.Count - 1);
                    ReserversPanel.Controls.Remove(reserver);
                    reserver.Dispose();
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
            string failureLog = $"Iteration failure: {e.LogText}";

            Log(failureLog, e.BrowserNumber);
            LogIteration(failureLog, e.BrowserNumber);
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

            FileService.SaveData(ORDERS_FILE_PATH, Orders);

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

            string path = Path.Combine(SCREENS_FOLDER_PATH, $"{e.Url.Replace(URL_TO_SCREEN_FILENAME_TRASH_PREFIX, "")}.png");
            Bitmap screen = await Reservers[view.RealBrowserNumber].GetCapture();
            bool saved = screen != null && screen.TrySave(path);

            if (saved)
                await Notifier.NotifyPhoto(path, $"(FROM {Profile.Login}) {e.Url}", FixChatId(LogChatId.Text));
            else
                await Notifier.NotifyMessage($"(FROM {Profile.Login}) {e.Url}", FixChatId(LogChatId.Text));

            Reserved.Add(new ReservedInfo(e.Url));
            FileService.SaveData(OUTPUT_FILE_PATH, Reserved);

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

            FileService.SaveData(OUTPUT_FILE_PATH, Reserved);

            Log("Reserved list saved", null);
        }

        private void AddOrderButton_Click(object sender, EventArgs e)
        {
            AddOrderForm addOrderForm = new AddOrderForm(Platforms, Orders);
            addOrderForm.ShowDialog();

            FileService.SaveData(ORDERS_FILE_PATH, Orders);

            foreach (ReservationOrder found in Orders.Where(order => !order.Doing && !order.Done))
                PutOrderToBrowser(found);
        }

        private void OrderListButton_Click(object sender, EventArgs e)
        {
            OrderListForm orderListForm = new OrderListForm(Platforms, Orders);
            orderListForm.ShowDialog();

            FileService.SaveData(ORDERS_FILE_PATH, Orders);
        }

        private void ScheduleButton_Click(object sender, EventArgs e)
        {
            BootScheduleForm scheduleForm = new BootScheduleForm(Schedule);
            scheduleForm.ShowDialog();

            FileService.SaveData(SCHEDULE_FILE_PATH, Schedule);

            Log("Schedule saved", null);
        }

        private void StartScheduledButton_Click(object sender, EventArgs e)
        {
            foreach (ReserverView reserver in Reservers)
            {
                reserver.Browser.Schedule = Schedule;
                reserver.Browser.Paused = false;
            }

            DateCheckers.ApplyToDateCheckers(checker => checker.Schedule = Schedule);

            Log("All browsers started scheduled", null);
        }

        private void UnbindScheduleButton_Click(object sender, EventArgs e)
        {
            foreach (ReserverView reserver in Reservers)
                reserver.Browser.Schedule = null;

            DateCheckers.ApplyToDateCheckers(checker => checker.Schedule = null);

            Log("Schedule detached. Manual control", null);
        }

        private void DelaySettings_Click(object sender, EventArgs e)
        {
            DelayInfoForm delayInfoForm = new DelayInfoForm(DelayInfo);
            delayInfoForm.ShowDialog();

            FileService.SaveData(DELAY_SETTINGS_FILE_PATH, DelayInfo);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FileService.SaveData(ORDERS_FILE_PATH, Orders);
            FileService.SaveData(OUTPUT_FILE_PATH, Reserved);
            FileService.SaveData(SCHEDULE_FILE_PATH, Schedule);

            FileService.SaveData(COMMON_SETTINGS_FILE_PATH, ExportCommonSettings());

            BrowserSettings[] browserSettings = Reservers.Select(
                reserver => reserver.ExportSettings()
            ).ToArray();
            FileService.SaveData(BROWSER_SETTINGS_FILE_PATH, browserSettings);

            Log("ALL Saved", null);
        }

        private void BotNumber_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < Reservers.Count; ++i)
                Reservers[i].Browser.BotNumber = (int)BotNumber.Value;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode >= Keys.D1 && e.KeyCode < Keys.D1 + Reservers.Count)
                Reservers[e.KeyCode - Keys.D1].ZoomIn();
        }

        private void UnzoomAllBrowsers_Click(object sender, EventArgs e)
        {
            foreach (ReserverView reserver in Reservers)
                reserver.ZoomOut();
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
            FileService.SaveData(PROFILE_FILE_PATH, Profile);
        }

        private void NotifyStrategy_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDateCheckersFlowStrategy();
        }

        private void UpdateDateCheckersFlowStrategy()
        {
            if(NotifyAlwaysStrategyChecker.Checked)
                DateCheckers.ApplyToDateCheckers(checker => checker.FlowStrategy = NOTIFY_ALWAYS_DATE_CHECKER_FLOW_STRATEGY);
            else if (NotifyIfDatesFoundStrategyChecker.Checked)
                DateCheckers.ApplyToDateCheckers(checker => checker.FlowStrategy = NOTIFY_IF_DATES_FOUND_DATE_CHECKER_FLOW_STRATEGY);
            else if (NotifyIfDatesAndTimesFoundStrategyChecker.Checked)
                DateCheckers.ApplyToDateCheckers(checker => checker.FlowStrategy = NOTIFY_IF_DATES_AND_TIMES_FOUND_DATE_CHECKER_STRATEGY);
        }
    }
}
