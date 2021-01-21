using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

using System.Threading;
using System.Threading.Tasks;

using CefSharp;
using CefSharp.WinForms;

using Common;
using Common.Data;
using Common.Extensions;

using PassportMeetReservator.Data;
using PassportMeetReservator.Data.CustomEventArgs;

using PassportMeetReservator.Extensions;
using PassportMeetReservator.Strategies.TimeSelectStrategies;

namespace PassportMeetReservator.Controls
{
    public class ReserverWebView : ChromiumWebBrowser, IDisposable
    {
        public event EventHandler<OrderEventArgs> OnOrderChanged;
        public event EventHandler<EventArgs> OnProxyChanged;

        public event EventHandler<ReservedEventArgs> OnReserved;
        public event EventHandler<ReservedEventArgs> OnReservedManually;

        public event EventHandler<UrlChangedEventArgs> OnUrlChanged;
        public event EventHandler<BrowserPausedChangedEventArgs> OnPausedChanged;

        public event EventHandler<EventArgs> OnManualReactionWaiting;
        public event EventHandler<DateTimeEventArgs> OnDateTimeSelected;

        public event EventHandler<LogEventArgs> OnIterationSkipped;
        public event EventHandler<LogEventArgs> OnIterationFailure;

        public event EventHandler<LogEventArgs> OnIterationLogRequired;

        //MOVE ALL IT TO CSS INFO
        private const string RESERVATION_TYPE_BUTTON_CLASS = "operation-button";

        private const string NEXT_STEP_BUTTON_CLASS = "btn footer-btn btn-secondary btn-lg btn-block";
        private const string NEXT_STEP_BUTTON_TEXT = "Dalej";

        private const string CALENDAR_CLASS = "vc-container vc-reset vc-text-gray-900 vc-bg-white vc-border vc-border-gray-400 vc-rounded-lg";

        private const string TIME_SELECTOR_ID = "selectTime";
        private const string TIME_SELECTOR_CLASS = "text-center form-control custom-select";
        private const string INPUT_CLASS = "property-input form-control";

        private const string ACCEPT_TICK_CLASS = "custom-control-label";
        private const string ACCEPT_TICK_TEXT = "Akceptuję regulamin";

        private const string ACCEPT_BUTTON_CLASS = "btn footer-btn btn-secondary btn-lg btn-block";
        private const string ACCEPT_BUTTON_TEXT = "Rezerwuję";

        private const string NEXT_MONTH_BUTTON_CLASS = "vc-svg-icon";
        private const int NEXT_MONTH_BUTTON_NUM = 1;

        private const string FAILED_RESERVE_TIME_CLASS = "alert alert-danger";
        private const string FAILED_RESERVE_TIME_TEXT = "Niestety wybrana data jest już zarezerwowana, prosimy wybrać inną.";

        private const string CONTINUE_RESERVATION_BUTTON_CLASS = "btn btn-success btn-lg btn-block";
        private const string CONTINUE_RESERVATION_BUTTON_TEXT = "Tak, chcę kontynuować";

        private const string LOADER_CLASS = "vld-overlay is-active is-full-page";

        private const string OPERATION_CIRCLE_ID = "step-Operacja0";
        private const string DONE_CIRCLE_ID = "step-Dane2";

        private const string DATE_CLASS = "vc-day-content vc-focusable";
        private const string DATE_INACTIVE_CLASS = "vc-text-gray-400";

        private const string URL_CHANGE_SUCCESS_PATH = "Info";

        private const string BASE_ADDRESS_LOAD = "https://whatismyipaddress.com";

        private Task Loop { get; set; }
        private CancellationToken Token { get; set; }
        private CancellationTokenSource TokenSource { get; set; }

        public bool IsBusy { get; private set; }
        public bool Selected { get; private set; }

        private string proxy;
        public string Proxy
        {
            get => proxy;
            set
            {
                proxy = value;

                if (IsBrowserInitialized)
                    ApplyProxy();
                else
                    IsBrowserInitializedChanged += ReserverWebView_IsBrowserInitializedChanged;

                OnProxyChanged?.Invoke(this, new EventArgs());
            }
        }

        private bool paused = true;
        public bool Paused
        {
            get => paused;
            set
            {
                if(paused != value)
                {
                    paused = value;

                    if (OnPausedChanged != null)
                        Invoke(OnPausedChanged, this, new BrowserPausedChangedEventArgs(RealBrowserNumber, paused));

                    if (Checker == null)
                        return;

                    if (paused)
                        Checker.PausedFollowersCount++;
                    else
                        Checker.PausedFollowersCount--;
                }
            }
        }

        private bool auto = false;
        public bool Auto
        {
            get => auto;
            set
            {
                if (value == auto)
                    return;

                auto = value;

                if (!auto && order != null)
                    Order = null;
            }
        }

        private ReservationOrder order = null;
        public ReservationOrder Order
        {
            get => order;
            set
            {
                if (order == null && value == null)
                    return;

                if (order != null)
                    order.Doing = false;

                order = value;

                if (order != null)
                    order.Doing = true;
                else
                    Checker = initChecker;

                if (OnOrderChanged != null)
                    Invoke(OnOrderChanged, this, new OrderEventArgs(order));
            }
        }

        private DateChecker initChecker;
        public DateChecker InitChecker
        {
            get => initChecker;
            set 
            {
                initChecker = value;

                if (!Auto || Order == null)
                    Checker = initChecker;
            }
        }

        private DateChecker checker;
        public DateChecker Checker
        {
            get => checker;
            set
            {
                if (checker == value)
                    return;

                if (checker != null)
                {
                    checker.FollowersCount--;

                    if (Paused)
                        checker.PausedFollowersCount--;

                    checker.OnDatesFound -= Checker_OnDatesFound;
                }

                checker = value;

                if (checker != null)
                {
                    checker.FollowersCount++;

                    if (Paused)
                        checker.PausedFollowersCount++;

                    checker.OnDatesFound += Checker_OnDatesFound;
                }

                Reset();
            }
        }

        public int BotNumber { get; set; }
        public int RealBrowserNumber { get; set; }

        private int virtualBrowserNumber;
        public int VirtualBrowserNumber
        {
            get => virtualBrowserNumber;
            set
            {
                virtualBrowserNumber = value;
                Reset();
            }
        }

        public DateTime ReserveDateMin { get; set; } = DateTime.Now.Date;
        public DateTime ReserveDateMax { get; set; } = DateTime.Now.Date;
        public BootPeriod ReserveTimePeriod { get; set; } = new BootPeriod();

        public DelayInfo DelayInfo { get; set; }
        public BootSchedule Schedule { get; set; }

        public TimeSelectStrategyBase TimeSelectStrategy { get; set; }

        private const int DATE_WAIT_ITERATION_COUNT = 20;
        private const int SITE_FALL_WAIT_ATTEMPTS = 40;

        private bool IsRefreshed { get; set; }

        [Obsolete]
        public ReserverWebView() : base(BASE_ADDRESS_LOAD, new RequestContext())
        {
            this.AddressChanged += (sender, e) => Invoke(OnUrlChanged, this, new UrlChangedEventArgs(Address));
            Load(BASE_ADDRESS_LOAD);
        }

        public new void Dispose()
        {
            CancelTask();
            Checker = null;

            base.Dispose();
        }

        public void Free()
        {
            IsBusy = false;
        }

        private void CancelTask()
        {
            try { TokenSource?.Cancel(); }
            catch { TokenSource = null; }
        }

        private bool CanPerformReset()
        {
            return !Selected || MessageBox.Show("This browser has selected a slot. Are you sure to reset it?", "Reset", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        public void Reset()
        {
            if (CanPerformReset())
            {
                CancelTask();
                UpdateBrowser();

                Selected = false;
                IsBusy = false;

                Paused = true;
            }
        }

        public void ResetScroll()
        {
            this.GetMainFrame().ExecuteJavaScriptAsync("window.scrollTo(0, 0);");
        }

        public void Done()
        {
            Invoke(OnReservedManually, this, new ReservedEventArgs(Address, Order));

            Selected = false;
            IsBusy = false;

            TokenSource?.Cancel();
        }

        private void ReserverWebView_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            ApplyProxy();
            IsBrowserInitializedChanged -= ReserverWebView_IsBrowserInitializedChanged;
        }

        private async void ApplyProxy()
        {
            await SetProxy();
        }

        private async Task SetProxy()
        {
            if (Proxy == null)
                return;

            await Cef.UIThreadTaskFactory.StartNew(() =>
            {
                Dictionary<string, object> pref = new Dictionary<string, object>()
                {
                    { "mode", "fixed_servers" },
                    { "server", Proxy }
                };
                bool success = GetBrowser().GetHost().RequestContext.SetPreference("proxy", pref, out string error);
                object p = GetBrowser().GetHost().RequestContext.GetPreference("proxy");
            });
        }

        private async void Checker_OnDatesFound(object sender, EventArgs e)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            using (TokenSource = new CancellationTokenSource())
            {
                Token = TokenSource.Token;

                Loop = TryReserveDates();

                try { await Loop; }
                catch (OperationCanceledException ex) { Loop.Dispose(); }
                catch (ObjectDisposedException ex) { }
            }

            IsBusy = false;
        }

        [Obsolete]
        private async Task TryReserveDates()
        {
            if (Paused || Selected)
                return;

            if (Checker == null)
            {
                RaiseIterationSkipped("No operation set");
                return;
            }

            if (Schedule != null && !Schedule.IsInside(DateTime.Now.TimeOfDay))
            {
                RaiseIterationSkipped("No schedule match");
                IsRefreshed = false;
                return;
            }

            if (!IsRefreshed)
            {
                this.Reset();
                this.IsRefreshed = true;
                this.Paused = false;
                return;
            }

            List<DateTime> slotsAtDatePeriod = TimeSelectStrategy.FilterDateTimeSlots(Checker.Slots);

            if (slotsAtDatePeriod.Count() == 0)
            {
                RaiseIterationSkipped("Reservation date period mismatch");
                return;
            }

            List<DateTime> slotsAtTimePeriod = TimeSelectStrategy.FilterTimeSlots(slotsAtDatePeriod);

            if (slotsAtTimePeriod.Count() == 0)
            {
                RaiseIterationSkipped("Reservation time period mismatch");
                return;
            }

            DateTime reservedDateTime = TimeSelectStrategy.ChooseTimeSlotToReserve(slotsAtTimePeriod);

            if (await Iteration(reservedDateTime))
            {
                if (!Auto || Order == null)
                    await WaitForManualReaction();
                else
                {
                    await FillForm();
                    UpdateBrowser();
                }
            }
            else
                UpdateBrowser();
        }

        private void RaiseIterationSkipped(string message)
        {
            LogEventArgs logEventArgs = new LogEventArgs(message, RealBrowserNumber);
            OnIterationSkipped?.Invoke(this, logEventArgs);
        }

        private void RaiseIteraionFailure(string message)
        {
            LogEventArgs logEventArgs = new LogEventArgs(message, RealBrowserNumber);
            OnIterationFailure?.Invoke(this, logEventArgs);
        }

        private void RaiseIterationLog(string message)
        {
            LogEventArgs logEventArgs = new LogEventArgs(message, RealBrowserNumber);
            OnIterationLogRequired?.Invoke(this, logEventArgs);
        }

        private async Task WaitForManualReaction()
        {
            OnManualReactionWaiting?.Invoke(this, new EventArgs());
            await Task.Delay(DelayInfo.ManualReactionWaitDelay, Token);

            this.UpdateBrowser();
            await Task.Delay(DelayInfo.RefreshSessionUpdateDelay);

            await WaitForView(CONTINUE_RESERVATION_BUTTON_CLASS, SITE_FALL_WAIT_ATTEMPTS);
            await Task.Delay(DelayInfo.ActionResultDelay, Token);

            await ClickViewOfClassWithText(CONTINUE_RESERVATION_BUTTON_CLASS, CONTINUE_RESERVATION_BUTTON_TEXT, SITE_FALL_WAIT_ATTEMPTS);

            await WaitForManualReaction();
        }

        private void UpdateBrowser()
        {
            string url = Checker?.CityInfo?.BaseUrl ?? BASE_ADDRESS_LOAD;

            Request request = new Request()
            { 
                Url = url,
                Flags = UrlRequestFlags.DisableCache
            };

            try
            {
                if (this.CanGoBack)
                    this.GetMainFrame().LoadRequest(request);
                else
                    this.Load(url);
            }
            catch { }
        }

        private async Task<bool> Iteration(DateTime date)
        {
            RaiseIterationLog($"Iteration started for {date} started");

            string formattedDate = date.GetFormattedDate();

            JavascriptResponse iterationResponse = await this.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    "let VUE = document.getElementsByClassName('container-fluid')[0].childNodes[0].__vue__;" +
                    $"let BROWSER_NUMBER = {VirtualBrowserNumber};" +
                    "async function selectTime() {" +
                        "await VUE.refreshSlots();" +
                        "if (VUE.availableSlots.length != 0 && VUE.availableSlots.length > BROWSER_NUMBER) {" +
                            $"VUE.selectedSlot = VUE.availableSlots[BROWSER_NUMBER];" +
                        "} else { " +
                            $"setTimeout(selectTime, {DelayInfo.DiscreteWaitDelay});" +
                        "}" +
                   "}" +
                   "async function selectDate() {" +
                        "await VUE.getAvailableDates();" +
                        "if (VUE.minAvailableDate.toString() != 'Invalid Date') {" +
                            "VUE.selectedDay = VUE.minAvailableDate;" +
                            "await selectTime();" +
                        "} else {" +
                            $"setTimeout(selectDate, {DelayInfo.DiscreteWaitDelay});" +
                        "}" +
                   "}" +
                   $"VUE.selectedOperation = {Checker.OperationInfo.Number};" +
                   "selectDate();" +
                "}"
            );

            if (!iterationResponse.Success)
            {
                RaiseIteraionFailure($"Iteration failure: {iterationResponse.Message}");
                return false;
            }

            await WaitForSlotSelection();
            await BlockSlot();

            DateTime? taken = await WaitForSlotBlock();
            if (!taken.HasValue)
            {
                RaiseIteraionFailure("Wait reserve iteration fail");
                return false;
            }

            Selected = true;
            RaiseIterationLog($"Reserve iteration success");

            OnDateTimeSelected?.Invoke(this, new DateTimeEventArgs(taken.Value));

            return true;
        }

        private async Task WaitForSlotSelection()
        {
            for (; ; )
            {
                RaiseIterationLog($"Waiting for slot selection");
                Token.ThrowIfCancellationRequested();

                JavascriptResponse slotSelectedProbe = await this.GetMainFrame().EvaluateScriptAsync(
                    "document.getElementsByClassName('container-fluid')[0].childNodes[0].__vue__.selectedSlot != null"
                );

                if (slotSelectedProbe.Success && slotSelectedProbe.Result != null && (bool)slotSelectedProbe.Result)
                    break;

                await Task.Delay(DelayInfo.ActionResultDelay);
            }
        }

        private async Task<DateTime?> WaitForSlotBlock()
        {
            for (; ; )
            {
                RaiseIterationLog($"Waiting for slot block");
                Token.ThrowIfCancellationRequested();

                JavascriptResponse slotBlockedProbe = await this.GetMainFrame().EvaluateScriptAsync(
                    "{" +
                        "let VUE = document.getElementsByClassName('container-fluid')[0].childNodes[0].__vue__;" +
                        "let result = (VUE.reservationBlockEndIn == null ? '0' : '1') + (VUE.slotBlockedError == '' ? '0' : '1');" +
                        "result" +
                    "}"
                );

                if (slotBlockedProbe.Success && slotBlockedProbe.Result != null)
                {
                    string probeResult = slotBlockedProbe.Result.ToString();

                    bool isBlocked = probeResult[0] == '1';
                    bool hasAlreadyReservedError = probeResult[1] == '1';

                    if (isBlocked)
                    {
                        JavascriptResponse selected = await this.GetMainFrame().EvaluateScriptAsync(
                            "{" +
                                "let VUE = document.getElementsByClassName('container-fluid')[0].childNodes[0].__vue__;" +
                                "VUE.selectedSlot.dateTime" +
                            "}"
                        );

                        return DateTime.Parse(selected.Result.ToString());
                    }
                    else if (hasAlreadyReservedError)
                    {
                        RaiseIteraionFailure("Already reserved error");

                        JavascriptResponse alreadyReservedHandlerResult = await this.GetMainFrame().EvaluateScriptAsync(
                            "{" +
                                "let VUE = document.getElementsByClassName('container-fluid')[0].childNodes[0].__vue__;" +
                                $"VUE.selectedSlot = VUE.availableSlots[Math.min({VirtualBrowserNumber}, VUE.availableSlots.length - 1)];" +
                            "}"
                        );

                        if (alreadyReservedHandlerResult.Success)
                        {
                            await BlockSlot();

                            DateTime? selectedDateTime = await WaitForSlotBlock();
                            if (!selectedDateTime.HasValue)
                            {
                                RaiseIteraionFailure("Already reserved handler failed");
                                return null;
                            }

                            RaiseIterationLog("Already reserved handler success");
                            return selectedDateTime;
                        }
                        else
                        {
                            RaiseIteraionFailure("Already reserved handler failed: no more slots available");
                            return null;
                        }
                    }
                }

                await Task.Delay(DelayInfo.ActionResultDelay);
            }
        }

        private async Task BlockSlot()
        {
            for (int i = 0; i < 2; ++i)
                await PressNextButton();
        }

        private async Task<bool> PressNextButton()
        {
            if (!await ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT, SITE_FALL_WAIT_ATTEMPTS))
            {
                RaiseIteraionFailure("NEXT button not found");
                return false;
            }

            RaiseIterationLog("NEXT button clicked");
            return true;
        }

        private async Task FillForm()
        {
            if (Order == null)
                return;

            await WaitForSpinner();
            await WaitForView(INPUT_CLASS, SITE_FALL_WAIT_ATTEMPTS);
            //await Task.Delay(DelayInfo.ActionResultDelay);

            while (!await this.SetTextToViewOfClassWithNumber(INPUT_CLASS, 0, Order.Surname)) ;
            while (!await this.SetTextToViewOfClassWithNumber(INPUT_CLASS, 1, Order.Name)) ;
            while (!await this.SetTextToViewOfClassWithNumber(INPUT_CLASS, 2, Order.Email)) ;

            await Task.Delay(DelayInfo.PostInputDelay);
            await ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT, SITE_FALL_WAIT_ATTEMPTS);
            //FILL FORM

            this.AddressChanged += ReserverWebView_UrlChanged;

            await WaitForView(ACCEPT_TICK_CLASS, SITE_FALL_WAIT_ATTEMPTS);
            await Task.Delay(DelayInfo.ActionResultDelay);

            await ClickViewOfClassWithText(ACCEPT_TICK_CLASS, ACCEPT_TICK_TEXT, SITE_FALL_WAIT_ATTEMPTS);
            await ClickViewOfClassWithText(ACCEPT_BUTTON_CLASS, ACCEPT_BUTTON_TEXT, SITE_FALL_WAIT_ATTEMPTS);
            //ACCEPT

            if (await WaitForView(CONTINUE_RESERVATION_BUTTON_CLASS, SITE_FALL_WAIT_ATTEMPTS))
            {
                await Task.Delay(DelayInfo.ActionResultDelay, Token);
                await ClickViewOfClassWithText(CONTINUE_RESERVATION_BUTTON_CLASS, CONTINUE_RESERVATION_BUTTON_TEXT, SITE_FALL_WAIT_ATTEMPTS);

                await FillForm();
            }
        }

        private void ReserverWebView_UrlChanged(object sender, EventArgs e)
        {
            if (Address == Checker.CityInfo.BaseUrl || !Address.Contains(URL_CHANGE_SUCCESS_PATH) || Order == null)
                return;

            Order.Done = true;
            Order.Doing = false;

            ReservedEventArgs args = new ReservedEventArgs(Address, Order);

            Order = null;
            Selected = false;

            Invoke(OnReserved, this, args);
            this.AddressChanged -= ReserverWebView_UrlChanged;
        }

        private async Task WaitForSpinner()
        {
            while (await this.CheckItemOfClassVisible(LOADER_CLASS))
            {
                RaiseIterationLog($"Waiting for spinner");

                Token.ThrowIfCancellationRequested();
                await Task.Delay(DelayInfo.DiscreteWaitDelay);
            }

            RaiseIterationLog("Spinner finished");
        }

        private async Task<bool> ClickAnyViewOfClassWithoutText(string className, string text, int attempts)
        {
            return await this.ClickAnyViewOfClassWithoutText(className, text, attempts, DelayInfo.DiscreteWaitDelay, Token);
        }

        private async Task<bool> ClickViewOfClassWithText(string className, string text, int attempts)
        {
            return await this.ClickViewOfClassWithText(className, text, attempts, DelayInfo.DiscreteWaitDelay, Token);
        }

        private async Task ClickViewOfClassWithNumber(string className, int number, string forbiddenClass, bool parent = false)
        {
            await this.ClickViewOfClassWithNumber(className, number, forbiddenClass, DelayInfo.DiscreteWaitDelay, Token, parent);
        }

        private async Task<bool> WaitForView(string className, int attempts)
        {
            return await this.WaitForView(className, attempts, DelayInfo.DiscreteWaitDelay, Token);
        }

        private async Task ClickViewOfClass(string className)
        {
            await this.ClickViewOfClassWithNumber(className, 0, "", DelayInfo.DiscreteWaitDelay, Token);
        }
    }
}
