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
using PassportMeetReservator.Data.Exceptions;
using System.Security.Cryptography.X509Certificates;
using CefSharp.Handler;

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
        public bool CalendarPushed { get; private set; }
        public bool DateSelected { get; private set; }

        private bool isUsingBakedReservations;
        public bool IsUsingBakedReservations
        {
            get => isUsingBakedReservations;
            set
            {
                isUsingBakedReservations = value;

                if (isUsingBakedReservations && !string.IsNullOrEmpty(BakedReservationToken))
                    CompleteBakedReservation();
            }
        }

        private string bakedReservationToken;
        public string BakedReservationToken
        {
            get => bakedReservationToken;
            set
            {
                bakedReservationToken = value;

                if (isUsingBakedReservations && !string.IsNullOrEmpty(BakedReservationToken))
                    CompleteBakedReservation();
            }
        }

        private Proxy proxy;
        public Proxy Proxy
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
                    checker.RemoveFollower(Paused);
                    checker.OnDatesFound -= Checker_OnDatesFound;
                }

                checker = value;

                if (checker != null)
                {
                    checker.AddFollower(Paused);
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

            if (Proxy != null)
            {
                Proxy.IsInUse = false;
                Proxy = null;
            }

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

        private class ProxyRequestHandler : RequestHandler
        {
            private Proxy Proxy { get; set; }

            public ProxyRequestHandler(Proxy proxy)
            {
                Proxy = proxy;
            }

            protected override bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
            {
                if(isProxy && Proxy != null && !string.IsNullOrEmpty(Proxy.Username) && !string.IsNullOrEmpty(Proxy.Password))
                    callback.Continue(Proxy.Username, Proxy.Password);

                return isProxy;
            }
        }

        private async Task SetProxy()
        {
            if (Proxy == null)
                return;

            await Cef.UIThreadTaskFactory.StartNew(() =>
            {
                this.RequestHandler = new ProxyRequestHandler(Proxy);

                Dictionary<string, object> pref = new Dictionary<string, object>()
                {
                    { "mode", "fixed_servers" },
                    { "server", Proxy.Address }
                };
                bool success = GetBrowser().GetHost().RequestContext.SetPreference("proxy", pref, out string error);
                object p = GetBrowser().GetHost().RequestContext.GetPreference("proxy");
            });
        }

        private async void Checker_OnDatesFound(object sender, EventArgs e)
        {
            if (IsBusy || IsUsingBakedReservations)
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

        public async Task CompleteReservation()
        {
            if (!Auto || Order == null)
                await WaitForManualReaction();
            else
            {
                await FillForm();
                UpdateBrowser();
            }
        }

        public async void CompleteBakedReservation()
        {
            try
            {
                IsBusy = true;

                UpdateBrowser();

                RaiseIterationLog($"Baked reservation started for token {this.BakedReservationToken}");

                if (!await WaitForLoading(TimeSpan.FromSeconds(1)))
                    throw new BakedReservationFailedException($"Baked reservation error: Page loading failed");

                JavascriptResponse response = await this.GetMainFrame().EvaluateScriptAsync(
                    "{" +
                        $"sessionStorage.setItem('token', '{this.BakedReservationToken}');" +
                        $"let VUE = document.getElementsByClassName('container-fluid')[0].childNodes[0].__vue__;" +
                        $"VUE.continueReservation();" +
                    "}"
                );

                if (!response.Success)
                    throw new BakedReservationFailedException($"Baked reservation error: Script failed due to {response.Message}");

                await CompleteReservation();
            }
            catch(BakedReservationFailedException ex)
            {
                CompleteBakedReservation();

                //RaiseIteraionFailure(ex.Reason);
                //this.BakedReservationToken = null;
            }
            finally
            {
                IsBusy = false;
            }
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
                UpdateBrowser();
                IsRefreshed = true;
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

            try
            {
                if (await Iteration(reservedDateTime))
                    await CompleteReservation();
                else
                    UpdateBrowser();
            }
            catch (PromiseFailedException)
            {
                RaiseIteraionFailure($"Iteration failure: selectDate promise contains error");
                UpdateBrowser();
            }
            catch (GlobalErrorException)
            {
                RaiseIteraionFailure($"Iteration failure: global error occured");
                UpdateBrowser();
            }
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

                CalendarPushed = false;
                DateSelected = false;
            }
            catch { }
        }

        private async Task<bool> Iteration(DateTime date)
        {
            RaiseIterationLog($"Iteration started for {date} started");

            await WaitForSpinner();
            await Task.Delay(DelayInfo.ActionResultDelay, Token);

            if (!await ClickViewOfClassWithText(RESERVATION_TYPE_BUTTON_CLASS, Checker.OperationInfo.Name, SITE_FALL_WAIT_ATTEMPTS))
            {
                RaiseIteraionFailure("Opreration button not found");
                return false;
            }
            RaiseIterationLog("Operation button found");
            //SELECT ORDER TYPE

            await WaitForSpinner();

            if (!await ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT, SITE_FALL_WAIT_ATTEMPTS))
            {
                RaiseIteraionFailure("NEXT button not found");
                return false;
            }
            RaiseIterationLog("NEXT button clicked");
            //CONFIRM ORDER TYPE

            if (!await WaitForView(CALENDAR_CLASS, SITE_FALL_WAIT_ATTEMPTS))
            {
                RaiseIteraionFailure("Calendar button not found");
                return false;
            }
            RaiseIterationLog("Calendar found");

            await Task.Delay(DelayInfo.ActionResultDelay * 2, Token);//duo delay

            if (!CalendarPushed)
            {
                DateTime currentScanDate = DateTime.Now;
                while (currentScanDate.Month != date.Month)
                {
                    currentScanDate = currentScanDate.AddMonths(1);

                    await this.TryClickViewOfClassWithNumber(NEXT_MONTH_BUTTON_CLASS, NEXT_MONTH_BUTTON_NUM, "", true);
                    await Task.Delay(DelayInfo.ActionResultDelay, Token);

                    RaiseIterationLog($"Calendar pushed to month #{currentScanDate.Month + 1}");
                }
                RaiseIterationLog($"Calendar month found");
                CalendarPushed = true;
            }
            //PUSH CALENDAR FIRST TIME IF NEEDED

            DateTime? selectedDate = await ScanTime(date);
            if (!selectedDate.HasValue)
            {
                RaiseIteraionFailure("Available time for selected date not found");
                return false;
            }
            RaiseIterationLog($"Matching time selection success");

            await WaitForSpinner();

            if (await ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT, SITE_FALL_WAIT_ATTEMPTS))
                RaiseIterationLog("NEXT button clicked");
            else
                RaiseIterationLog("NEXT button NOT clicked");
            //CONFIRM SELECTED DATE

            await Task.Delay(DelayInfo.ActionResultDelay * 3, Token); //triple delay

            await WaitForSpinner();
            if (await this.TryFindViewOfClassWithText(FAILED_RESERVE_TIME_CLASS, FAILED_RESERVE_TIME_TEXT))
            {
                RaiseIteraionFailure("Already reserved error");

                selectedDate = await HandleAlreadyReservedError(date);
                if (!selectedDate.HasValue)
                {
                    RaiseIteraionFailure("Already reserved handler failed");
                    return false;
                }

                RaiseIterationLog("Already reserved handler success");
            }
            //CHECK TIME RESERVED ERROR

            JavascriptResponse jsStatusCircleStyle = await this.GetMainFrame().EvaluateScriptAsync(
                $"document.getElementById('{DONE_CIRCLE_ID}').children[0].style.backgroundColor"
            );

            if (jsStatusCircleStyle.Result.ToString() != Checker.CityInfo.CssInfo.StepCircleColor)
            {
                RaiseIteraionFailure("Step circle check failed");
                return false;
            }
            //CURRENT STEP STATUS CHECK

            Selected = true;
            OnDateTimeSelected?.Invoke(this, new DateTimeEventArgs(selectedDate.Value));

            return true;
        }

        private async Task<DateTime?> HandleAlreadyReservedError(DateTime date)
        {
            DateTime? selected = await TimeSelectStrategy.SelectTimeFromList(date, Token);

            if (!selected.HasValue)
                return null;

            for (int i = 0; i < 2; ++i)
            {
                if (!await ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT, SITE_FALL_WAIT_ATTEMPTS))
                {
                    RaiseIteraionFailure("NEXT button not found");
                    return null;
                }
                RaiseIterationLog("NEXT button clicked");
            }

            await WaitForSpinner();
            return selected;
        }

        private async Task<DateTime?> ScanTime(DateTime date)
        {
            string formattedDate = date.GetFormattedDate();

            bool dayFound = false;
            for (int j = 0; j < DATE_WAIT_ITERATION_COUNT; ++j)
            {
                IEnumerable<Task<bool>> conditions = new List<Task<bool>>()
                {
                    this.TryFindView($"vc-day id-{formattedDate}"),
                    this.TryFindViewOfClassWithoutClass(DATE_CLASS, DATE_INACTIVE_CLASS)
                };

                dayFound = (await Task.WhenAll(conditions)).All(cond => cond);

                if (dayFound)
                    break;

                await Task.Delay(DelayInfo.DiscreteWaitDelay, Token);
            } //WAIT FOR DAY FOR SEVERAL TIMES - WAIT FOR LOADING

            if (!dayFound)
            {
                RaiseIterationLog($"Required date NOT FOUND");
                return null;
            }
            RaiseIterationLog($"Required date found");

            await this.GetMainFrame().EvaluateScriptAsync(
                $"document.getElementsByClassName('vc-day id-{formattedDate}')[0].children[0].children[0].click();"
            );
            RaiseIterationLog($"Required date selected");

            await WaitForSpinner();

            RaiseIterationLog($"Waiting for time selector");
            await WaitForView(TIME_SELECTOR_CLASS, SITE_FALL_WAIT_ATTEMPTS);
            await Task.Delay(DelayInfo.ActionResultDelay * 2, Token);//duo delay

            return await TimeSelectStrategy.SelectTimeFromList(date, Token);
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

        private async Task<bool> WaitForLoading(TimeSpan maxWaitTime)
        {
            DateTime start = DateTime.Now;

            for (; ; )
            {
                RaiseIterationLog($"Waiting for browser to load page");
                Token.ThrowIfCancellationRequested();

                JavascriptResponse loadedProbe = await this.GetMainFrame().EvaluateScriptAsync(
                    "{" +
                        "let vueWrapper = document.getElementsByClassName('container-fluid')[0];" +
                        "vueWrapper != undefined && vueWrapper != null && " +
                        "vueWrapper.childNodes[0] != undefined && vueWrapper.childNodes[0] != null && " +
                        "vueWrapper.childNodes[0].__vue__ != undefined && vueWrapper.childNodes[0].__vue__ != null && " +
                        "vueWrapper.childNodes[0].__vue__.isInitialized" +
                    "}"
                );

                if (loadedProbe.Success && loadedProbe.Result != null && (bool)loadedProbe.Result)
                    return true;

                if (DateTime.Now - start > maxWaitTime)
                    return false;

                await Task.Delay(DelayInfo.DiscreteWaitDelay);
            }
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
