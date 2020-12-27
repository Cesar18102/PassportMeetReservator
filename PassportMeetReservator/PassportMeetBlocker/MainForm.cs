using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autofac;

using Common;
using Common.Data;
using Common.Data.CustomEventArgs;
using Common.Data.Platforms;
using Common.Extensions;
using Common.Forms;
using Common.Services;
using Common.Strategies.DateCheckerNotifyStrategies;

namespace PassportMeetBlocker
{
    public partial class MainForm : Form
    {
        private static string DELAY_SETTINGS_FILE_PATH = Path.Combine(
            Environment.CurrentDirectory, "Data", "delay_settings.json"
        );

        private static FileService FileService = CommonDependencyHolder.ServiceDependencies.Resolve<FileService>();
        private static Logger Logger = CommonDependencyHolder.ServiceDependencies.Resolve<Logger>();

        private static PlatformApiInfo[] Platforms = new PlatformApiInfo[]
        {
            new PoznanPlatformInfo(),
            new BezkolejkiPlatformInfo()
        };

        private DelayInfo DelayInfo { get; set; }
        private Dictionary<string, Dictionary<string, DateBlocker[]>> DateBlockers { get; set; }

        private bool IsBlocking { get; set; }


        private bool paused = true;
        public bool Paused
        {
            get => paused;
            set
            {
                if (paused != value)
                {
                    paused = value;

                    if (Blocker == null)
                        return;

                    if (paused)
                        Blocker.PausedFollowersCount++;
                    else
                        Blocker.PausedFollowersCount--;
                }
            }
        }


        private DateBlocker blocker;
        private DateBlocker Blocker
        {
            get => blocker;
            set
            {
                if (blocker == value)
                    return;

                if (blocker != null)
                {
                    blocker.FollowersCount--;

                    if (Paused)
                        blocker.PausedFollowersCount--;

                    blocker.OnDatesFound -= Checker_OnDatesFound;
                    blocker.OnSlotBlocked -= Blocker_OnSlotBlocked;
                }

                blocker = value;

                if (blocker != null)
                {
                    blocker.FollowersCount++;

                    if (Paused)
                        blocker.PausedFollowersCount++;

                    blocker.OnDatesFound += Checker_OnDatesFound;
                    blocker.OnSlotBlocked += Blocker_OnSlotBlocked;
                }
            }
        }

        public MainForm()
        {
            InitializeComponent();
            Logger.CreateCommonLogFile();

            DelayInfo = FileService.LoadData<DelayInfo>(DELAY_SETTINGS_FILE_PATH);

            DateBlockers = DateChecker.CreateFromPlatformInfos<DateBlocker>(
                Platforms, DelayInfo,
                Checker_OnRequestError,
                Checker_OnRequestOk
            );

            DateBlockers.ApplyToDateCheckers(checker => checker.OnBlockRequestError += Checker_OnBlockRequestError);
            DateBlockers.ApplyToDateCheckers(checker => checker.FlowStrategy = new NotifyIfDatesAndTimesFoundFlowStrategy()); 

            PlatformSelector.Items.AddRange(Platforms);
            PlatformSelector.SelectedIndex = 0;
        }

        private void Checker_OnBlockRequestError(object sender, DateCheckerErrorEventArgs e)
        {
            DateChecker checker = sender as DateChecker;
            Log($"BLOCK error at blocker {checker.CityInfo.Name} : {checker.OperationInfo}; Code: {e.ErrorCode}; Check your VPN and internet connection!");
        }

        private void Checker_OnRequestError(object sender, DateCheckerErrorEventArgs e)
        {
            DateChecker checker = sender as DateChecker;
            Log($"Date check error at checker {checker.CityInfo.Name} : {checker.OperationInfo}; Code: {e.ErrorCode}; Check your VPN and internet connection!");
        }

        private void Checker_OnRequestOk(object sender, DateCheckerOkEventArgs e)
        {
            DateChecker checker = sender as DateChecker;
            Log($"Date check ok at checker {checker.CityInfo.Name} : {checker.OperationInfo}; Content: {e.Content}");
        }

        private void Log(string text)
        {
            Logger.LogMain(text, null);

            string log = Logger.GetLogWithMeta(text, null);
            LogWindow.Invoke((MethodInvoker)delegate {
                LogWindow.AppendText(log);
            });
        }

        private void CommonOperationSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlatformApiInfo platform = PlatformSelector.SelectedItem as PlatformApiInfo;
            CityPlatformInfo city = CitySelector.SelectedItem as CityPlatformInfo;
            OperationInfo operation = OperationSelector.SelectedItem as OperationInfo;

            Blocker = DateBlockers[platform.Name][city.Name][operation.Position];
        }

        private void CommonCityChecker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Blocker = null;

            OperationSelector.Items.Clear();
            OperationSelector.Items.AddRange((CitySelector.SelectedItem as CityPlatformInfo).Operations);
        }

        private void CommonPlatformSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Blocker = null;

            OperationSelector.Items.Clear();

            CitySelector.Items.Clear();
            CitySelector.Items.AddRange((PlatformSelector.SelectedItem as PlatformApiInfo).CityPlatforms);
        }

        private async void Checker_OnDatesFound(object sender, EventArgs e)
        {
            if (IsBlocking)
                return;

            IsBlocking = true;

            IEnumerable<Task> tasks = Blocker.TimeCheckDtos.Select(time => Blocker.BlockSlot(time));
            await Task.WhenAll(tasks);

            IsBlocking = false;
        }

        private void Blocker_OnSlotBlocked(object sender, SlotBlockedEventArgs e)
        {
            Log($"{e.Slot.DateTime} was blocked");
        }

        private void DelaySettings_Click(object sender, EventArgs e)
        {
            DelayInfoForm delayInfoForm = new DelayInfoForm(DelayInfo);
            delayInfoForm.ShowDialog();

            FileService.SaveData(DELAY_SETTINGS_FILE_PATH, DelayInfo);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Paused = false;

            StartButton.Text = "Pause";
            StartButton.Click -= StartButton_Click;
            StartButton.Click += PauseButton_Click;
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            Paused = true;

            StartButton.Text = "Continue";
            StartButton.Click -= PauseButton_Click;
            StartButton.Click += StartButton_Click;
        }
    }
}
