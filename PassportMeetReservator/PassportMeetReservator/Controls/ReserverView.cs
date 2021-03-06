﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Common;
using Common.Data.Platforms;

using PassportMeetReservator.Data;
using PassportMeetReservator.Data.CustomEventArgs;
using PassportMeetReservator.Extensions;
using PassportMeetReservator.Forms;
using PassportMeetReservator.Strategies.TimeSelectStrategies;

namespace PassportMeetReservator.Controls
{
    public partial class ReserverView : UserControl, IDisposable
    {
        public event EventHandler<EventArgs> AutoChanged;

        #region Properties

        private TimeSelectByTimePeriodStrategy TIME_SELECT_BY_TIME_PERIOD_STRATEGY { get; set; }
        private TimeSelectByBrowserNumberStrategy TIME_SELECT_BY_BROWSER_NUMBER_STRATEGY { get; set; }

        public ZoomedBrowserForm ZoomedBrowserForm { get; private set; }
        public ReserverWebView Browser { get; private set; }
        public Panel BrowserWrapper => BrowserPanel;

        private int realBrowserNumber;
        public int RealBrowserNumber
        {
            get => realBrowserNumber;
            set
            {
                realBrowserNumber = value;
                VirtualBrowserNumber.Value = realBrowserNumber;
                Browser.RealBrowserNumber = realBrowserNumber;
                BrowserInfo.Text = $"Browser {realBrowserNumber + 1}";
            }
        }

        private PlatformApiInfo[] platforms;
        public PlatformApiInfo[] Platforms
        {
            get => platforms;
            set
            {
                platforms = value;

                OperationSelector.Items.Clear();
                CitySelector.Items.Clear();
                PlatformSelector.Items.Clear();

                if (platforms != null)
                    PlatformSelector.Items.AddRange(platforms);
            }
        }

        public Dictionary<string, Dictionary<string, DateChecker[]>> DateCheckers { get; set; }

        #endregion

        public ReserverView()
        {
            InitializeComponent();

            Browser = new ReserverWebView();
            Browser.Size = BrowserPanel.Size;
            BrowserPanel.Controls.Add(Browser);

            Browser.ReserveDateMin = ReserveDateMin.Value.Date;
            Browser.ReserveDateMax = ReserveDateMax.Value.Date;

            Browser.ReserveTimePeriod.TimeStart = ReserveTimeMin.Value.TimeOfDay;
            Browser.ReserveTimePeriod.TimeEnd = ReserveTimeMax.Value.TimeOfDay;

            Browser.OnUrlChanged += Browser_OnUrlChanged;
            Browser.OnProxyChanged += Browser_OnProxyChanged;
            Browser.OnPausedChanged += Browser_OnPausedChanged;

            TIME_SELECT_BY_TIME_PERIOD_STRATEGY = new TimeSelectByTimePeriodStrategy(Browser);
            TIME_SELECT_BY_BROWSER_NUMBER_STRATEGY = new TimeSelectByBrowserNumberStrategy(Browser);

            UpdateTimeSelectStrategy();
        }

        #region Methods
        #region Public

        public new void Dispose()
        {
            Browser.Dispose();
            base.Dispose();
        }

        public async Task<Bitmap> GetCapture()
        {
            ZoomIn();
            Browser.ResetScroll();

            Bitmap screen = null;
            await Task.Delay(1000);

            try { screen = ZoomedBrowserForm.Snapshot(Browser); }
            catch (Exception ex) { }

            ZoomOut();

            return screen;
        }

        public void ZoomIn()
        {
            ZoomedBrowserForm = new ZoomedBrowserForm();
            ZoomedBrowserForm.FormClosing += (sender, e) => ZoomOutClosing();
            ZoomedBrowserForm.Text = $"{BrowserInfo.Text}; {Browser.Checker.PlatformInfo}; {Browser.Checker.CityInfo}; {Browser.Checker.OperationInfo}; " +
                                     $"{Browser.Order?.Surname} {Browser.Order?.Name}";

            ZoomedBrowserForm.Controls.Add(Browser);
            ZoomedBrowserForm.Show();

            ZoomedBrowserForm.WindowState = FormWindowState.Maximized;
            Browser.Size = ZoomedBrowserForm.Size;
        }

        public void ZoomOut()
        {
            ZoomOutClosing();
            ZoomedBrowserForm?.Close();
        }

        private void ZoomOutClosing()
        {
            BrowserWrapper.Controls.Add(Browser);
            Browser.Size = BrowserWrapper.Size;
        }

        public void ApplySettings(BrowserSettings settings)
        {
            if (settings.BrowserNumber != Browser.RealBrowserNumber)
                return;

            ApplySettings(settings as CommonSettings);

            if (settings.ReservationTimeStart > DateTime.MinValue && settings.ReservationTimeStart < DateTime.MaxValue)
                ReserveTimeMin.Value = settings.ReservationTimeStart;

            if (settings.ReservationTimeEnd > DateTime.MinValue && settings.ReservationTimeEnd < DateTime.MaxValue)
                ReserveTimeMax.Value = settings.ReservationTimeEnd;
        }

        public void ApplySettings(CommonSettings settings)
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
                ReserveDateMin.Value = settings.ReservationDateStart;

            if (settings.ReservationDateEnd > DateTime.MinValue && settings.ReservationDateEnd < DateTime.MaxValue)
                ReserveDateMax.Value = settings.ReservationDateEnd;
        }

        public BrowserSettings ExportSettings()
        {
            return new BrowserSettings()
            {
                BrowserNumber = Browser.RealBrowserNumber,
                Platform = (PlatformSelector.SelectedItem as PlatformApiInfo)?.Name,
                City = (CitySelector.SelectedItem as CityPlatformInfo)?.Name,
                Operation = (OperationSelector.SelectedItem as OperationInfo)?.Position,
                ReservationDateStart = ReserveDateMin.Value,
                ReservationDateEnd = ReserveDateMax.Value,
                ReservationTimeStart = ReserveTimeMin.Value,
                ReservationTimeEnd = ReserveTimeMax.Value
            };
        }

        public void SelectPlatform(PlatformApiInfo platform) =>
            PlatformSelector.SelectedItem = platform;

        public void SelectCity(CityPlatformInfo city) =>
            CitySelector.SelectedItem = city;

        public void SelectOperation(OperationInfo operation) =>
            OperationSelector.SelectedItem = operation;

        public void SelectMinReserveDate(DateTime minReserveDate) =>
            ReserveDateMin.Value = minReserveDate;

        public void SelectMaxReserveDate(DateTime maxReserveDate) =>
            ReserveDateMax.Value = maxReserveDate;

        #endregion
        #region Private

        private void Browser_OnUrlChanged(object sender, UrlChangedEventArgs e) =>
            UrlInput.InputText = e.Url;

        private void Browser_OnProxyChanged(object sender, EventArgs e) =>
            ProxyLabel.Text = Browser.Proxy;

        private void ResetButton_Click(object sender, EventArgs e) =>
            Browser.Reset();

        private void DoneButton_Click(object sender, EventArgs e) => 
            Browser.Done();

        private void ReserveDateMin_ValueChanged(object sender, EventArgs e) =>
            Browser.ReserveDateMin = ReserveDateMin.Value.Date;

        private void ReserveDateMax_ValueChanged(object sender, EventArgs e) =>
            Browser.ReserveDateMax = ReserveDateMax.Value.Date;

        private void ReserveTimeMin_ValueChanged(object sender, EventArgs e) =>
            Browser.ReserveTimePeriod.TimeStart = ReserveTimeMin.Value.TimeOfDay;

        private void ReserveTimeMax_ValueChanged(object sender, EventArgs e) =>
            Browser.ReserveTimePeriod.TimeEnd = ReserveTimeMax.Value.TimeOfDay;

        private void Auto_CheckedChanged(object sender, EventArgs e)
        {
            Browser.Auto = Auto.Checked;
            AutoChanged?.Invoke(this, new EventArgs());
        }

        private void Continue_Click(object sender, EventArgs e) =>
            Browser.Paused = false;

        private void Pause_Click(object sender, EventArgs e) =>
            Browser.Paused = true;

        private void Browser_OnPausedChanged(object sender, BrowserPausedChangedEventArgs e)
        {
            if(e.Paused)
            {
                PauseChangeButton.Text = "Continue";
                PauseChangeButton.Click -= Pause_Click;
                PauseChangeButton.Click += Continue_Click;
            }
            else
            {
                PauseChangeButton.Text = "Pause";
                PauseChangeButton.Click -= Continue_Click;
                PauseChangeButton.Click += Pause_Click;
            }
        }

        private void PlatformSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PlatformSelector.SelectedItem == null)
                return;

            PlatformApiInfo selectedPlatform = PlatformSelector.SelectedItem as PlatformApiInfo;
            Browser.InitChecker = null;

            OperationSelector.Items.Clear();

            CitySelector.Items.Clear();
            CitySelector.Items.AddRange(selectedPlatform.CityPlatforms);
        }

        private void CitySelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CitySelector.SelectedItem == null)
                return;

            CityPlatformInfo selectedCity = CitySelector.SelectedItem as CityPlatformInfo;
            Browser.InitChecker = null;

            OperationSelector.Items.Clear();
            OperationSelector.Items.AddRange(selectedCity.Operations);
        }

        private void OperationSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PlatformSelector.SelectedItem == null || CitySelector.SelectedItem == null || OperationSelector.SelectedItem == null)
                return;

            string selectedPlatform = (PlatformSelector.SelectedItem as PlatformApiInfo).Name;
            string selectedCity = (CitySelector.SelectedItem as CityPlatformInfo).Name;
            OperationInfo selectedOperation = OperationSelector.SelectedItem as OperationInfo;

            Browser.InitChecker = DateCheckers[selectedPlatform][selectedCity][selectedOperation.Position];
        }

        private void StrategyChecker_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTimeSelectStrategy();   
        }

        private void VirtualBrowserNumber_ValueChanged(object sender, EventArgs e)
        {
            Browser.VirtualBrowserNumber = (int)VirtualBrowserNumber.Value;
        }

        private void UpdateTimeSelectStrategy()
        {
            if (BrowserNumberStrategyChecker.Checked)
                Browser.TimeSelectStrategy = TIME_SELECT_BY_BROWSER_NUMBER_STRATEGY;
            else if (TimePeriodStrategyChecker.Checked)
                Browser.TimeSelectStrategy = TIME_SELECT_BY_TIME_PERIOD_STRATEGY;
        }

        #endregion

        #endregion
    }
}
