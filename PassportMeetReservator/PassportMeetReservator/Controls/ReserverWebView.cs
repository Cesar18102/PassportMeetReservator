using System;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

using CefSharp;
using CefSharp.WinForms;

using PassportMeetReservator.Data;
using PassportMeetReservator.Data.CustomEventArgs;
using System.Collections.Generic;
using PassportMeetReservator.Extensions;

namespace PassportMeetReservator.Controls
{
    public class ReserverWebView : ChromiumWebBrowser
    {
        public event EventHandler<OrderEventArgs> OnOrderChanged;

        public event EventHandler<ReservedEventArgs> OnReserved;
        public event EventHandler<ReservedEventArgs> OnReservedManually;

        public event EventHandler<UrlChangedEventArgs> OnUrlChanged;
        public event EventHandler<BrowserPausedChangedEventArgs> OnPausedChanged;

        public event EventHandler<EventArgs> OnManualReactionWaiting;
        public event EventHandler<DateTimeEventArgs> OnDateTimeSelected;

        public event EventHandler<LogEventArgs> OnIterationSkipped;
        public event EventHandler<LogEventArgs> OnIterationFailure;

        private const string RESERVATION_TYPE_BUTTON_CLASS = "operation-button";

        private const string NEXT_STEP_BUTTON_CLASS = "btn footer-btn btn-secondary btn-lg btn-block";
        private const string NEXT_STEP_BUTTON_TEXT = "Dalej";

        private const string CALENDAR_CLASS = "vc-container vc-reset vc-text-gray-900 vc-bg-white vc-border vc-border-gray-400 vc-rounded-lg";

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
        private const string CONTINUE_RESERVATION_BUTTON_TEXT = "Tak, chce kontynuować";

        private const string LOADER_CLASS = "vld-overlay is-active is-full-page";

        private const string STATUS_CIRCLE_DONE_ID = "step-Dane2";

        private const string URL_CHANGE_SUCCESS_PATH = "Info";

        private Task Loop { get; set; }
        private CancellationToken Token { get; set; }
        private CancellationTokenSource TokenSource { get; set; }

        public bool IsBusy { get; private set; }
        public bool Selected { get; private set; }

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

        public DateTime ReserveDateMin { get; set; } = DateTime.Now.Date;
        public DateTime ReserveDateMax { get; set; } = DateTime.Now.Date;
        public BootPeriod ReserveTimePeriod { get; set; } = new BootPeriod();

        public DelayInfo DelayInfo { get; set; }
        public BootSchedule Schedule { get; set; }

        private const int DATE_WAIT_ITERATION_COUNT = 4;
        private const int TIME_WAIT_ITERATION_COUNT = 4;
        private const int SITE_FALL_WAIT_ATTEMPTS = 40;

        [Obsolete]
        public ReserverWebView()
        {
            this.AddressChanged += (sender, e) => Invoke(OnUrlChanged, this, new UrlChangedEventArgs(Address));
            Load("https://google.com");
            //Scale(new SizeF(0.5f, 0.5f));
        }

        public void Free()
        {
            IsBusy = false;
        }

        public void Reset()
        {
            try { TokenSource?.Cancel(); }
            catch { TokenSource = null; }

            UpdateBrowser();
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
            }

            IsBusy = false;
        }

        private async Task TryReserveDates()
        {
            if (Paused)
                return;

            if (Checker == null)
            {
                RaiseIterationSkipped("No operation set");
                return;
            }

            if (Schedule != null && !Schedule.IsInside(DateTime.Now.TimeOfDay))
            {
                RaiseIterationSkipped("No schedule match");
                return;
            }

            List<DateTime> slotsAtDatePeriod = Checker.Slots
                .Where(slot => slot.Key >= ReserveDateMin && slot.Key <= ReserveDateMax)
                .SelectMany(slot => slot.Value)
                .ToList();

            if (slotsAtDatePeriod.Count() == 0)
            {
                RaiseIterationSkipped("Reservation date period mismatch");
                return;
            }

            List<DateTime> slotsAtTimePeriod = slotsAtDatePeriod.Where(
                time => ReserveTimePeriod.IsIside(time.TimeOfDay)
            ).ToList();

            if (slotsAtTimePeriod.Count() == 0)
            {
                RaiseIterationSkipped("Reservation time period mismatch");
                return;
            }

            DateTime reservedDateTime = slotsAtTimePeriod.First();

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

        private async Task WaitForManualReaction()
        {
            OnManualReactionWaiting?.Invoke(this, new EventArgs());
            await Task.Delay(DelayInfo.ManualReactionWaitDelay, Token);

            this.Reload();
            await Task.Delay(DelayInfo.RefreshSessionUpdateDelay);

            await WaitForView(CONTINUE_RESERVATION_BUTTON_CLASS, SITE_FALL_WAIT_ATTEMPTS);
            await Task.Delay(DelayInfo.ActionResultDelay, Token);

            await ClickViewOfClassWithText(CONTINUE_RESERVATION_BUTTON_CLASS, CONTINUE_RESERVATION_BUTTON_TEXT, SITE_FALL_WAIT_ATTEMPTS);

            await WaitForManualReaction();
        }

        private void UpdateBrowser()
        {
            if (Checker == null)
                return;

            Request request = new Request()
            {
                Url = Checker.CityInfo.BaseUrl,
                Flags = UrlRequestFlags.DisableCache
            };

            try { this.GetMainFrame().LoadRequest(request); }
            catch { }
        }

        private async Task<bool> Iteration(DateTime date)
        {
            await WaitForSpinner();
            //await Task.Delay(DelayInfo.ActionResultDelay, Token);

            if (!await ClickViewOfClassWithText(RESERVATION_TYPE_BUTTON_CLASS, Checker.OperationInfo.Name, SITE_FALL_WAIT_ATTEMPTS))
            {
                RaiseIteraionFailure("Opreration button not found");
                return false;
            }

            await WaitForSpinner();

            if (!await ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT, SITE_FALL_WAIT_ATTEMPTS))
            {
                RaiseIteraionFailure("NEXT button not found");
                return false;
            }
            //SELECT ORDER TYPE

            if (!await WaitForView(CALENDAR_CLASS, SITE_FALL_WAIT_ATTEMPTS))
            {
                RaiseIteraionFailure("Calendar button not found");
                return false;
            }

            await Task.Delay(DelayInfo.ActionResultDelay, Token);
            await Task.Delay(DelayInfo.ActionResultDelay, Token);//duo delay

            DateTime currentScanDate = DateTime.Now;
            while (currentScanDate.Month != date.Month)
            {
                currentScanDate = currentScanDate.AddMonths(1);

                await TryClickViewOfClassWithNumber(NEXT_MONTH_BUTTON_CLASS, NEXT_MONTH_BUTTON_NUM, "", true);
                await Task.Delay(DelayInfo.ActionResultDelay, Token);
            }
            //PUSH CALENDAR FIRST TIME IF NEEDED

            DateTime? selectedDate = await ScanTime(date);
            if (!selectedDate.HasValue)
            {
                RaiseIteraionFailure("Available time for selected date not found");
                return false;
            }

            await WaitForSpinner();
            await ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT, SITE_FALL_WAIT_ATTEMPTS);
            //CONFIRM SELECTED DATE

            /*for(int j = 0; j < 3; ++j) //triple delay
                await Task.Delay(DelayInfo.ActionResultDelay, Token);*/

            await WaitForSpinner();
            if (await TryFindViewOfClassWithText(FAILED_RESERVE_TIME_CLASS, FAILED_RESERVE_TIME_TEXT))
            {
                RaiseIteraionFailure("Already reserved error");
                return false;
            }
            //CHECK TIME RESERVED ERROR

            JavascriptResponse jsStatusCircleStyle = await this.GetMainFrame().EvaluateScriptAsync(
                $"document.getElementById('{STATUS_CIRCLE_DONE_ID}').children[0].style.backgroundColor"
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

        private async Task<DateTime?> ScanTime(DateTime date)
        {
            string formattedDate = date.GetFormattedDate();

            bool dayFound = false;
            for (int j = 0; j < DATE_WAIT_ITERATION_COUNT; ++j)
            {
                dayFound = await TryFindView($"vc-day id-{formattedDate}");

                if (dayFound)
                    break;

                await Task.Delay(DelayInfo.DiscreteWaitDelay, Token);
            } //WAIT FOR DAY FOR SEVERAL TIMES - WAIT FOR LOADING

            if (!dayFound)
                return null;

            await this.GetMainFrame().EvaluateScriptAsync($"document.getElementsByClassName('vc-day id-{formattedDate}')[0].children[0].children[0].click()");

            await WaitForSpinner();
            await WaitForView(TIME_SELECTOR_CLASS, SITE_FALL_WAIT_ATTEMPTS);
            //await Task.Delay(DelayInfo.ActionResultDelay, Token);
            //await Task.Delay(DelayInfo.ActionResultDelay, Token);//duo delay

            bool timeFound = false;
            for (int j = 0; j < TIME_WAIT_ITERATION_COUNT; ++j)
            {
                JavascriptResponse jsTimesCount = await this.GetMainFrame().EvaluateScriptAsync($"document.getElementsByClassName('{TIME_SELECTOR_CLASS}')[0].options.length");
                timeFound = jsTimesCount.Success && jsTimesCount.Result != null && (int)jsTimesCount.Result != 0;

                if (timeFound)
                    break;

                await Task.Delay(DelayInfo.DiscreteWaitDelay, Token);
            } //WAIT FOR TIME FOR SEVERAL TIMES - WAIT FOR LOADING

            if (timeFound && await SelectByValue(TIME_SELECTOR_CLASS, date.GetFormattedTime()))
                return date;

            return null;
        }

        private async Task FillForm()
        {
            if (Order == null)
                return;

            await WaitForSpinner();
            await WaitForView(INPUT_CLASS, SITE_FALL_WAIT_ATTEMPTS);
            //await Task.Delay(DelayInfo.ActionResultDelay);

            while (!await SetTextToViewOfClassWithNumber(INPUT_CLASS, 0, Order.Surname)) ;
            while (!await SetTextToViewOfClassWithNumber(INPUT_CLASS, 1, Order.Name)) ;
            while (!await SetTextToViewOfClassWithNumber(INPUT_CLASS, 2, Order.Email)) ;

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

        private async Task<bool> SetTextToViewOfClassWithNumber(string className, int number, string text)
        {
            JavascriptResponse result = await this.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"if(views.length > {number})" +
                    "{" +
                        $"views[{number}].value = '{text}';" +
                        $"let e = document.createEvent('HTMLEvents');" +
                        $"e.initEvent('change', true, true);" +
                        $"views[{number}].dispatchEvent(e);" +
                    "}" +
                    $"views.length > {number};" +
                "}"
            );

            return (bool)result.Result;
        }

        private async Task<bool> SelectByIndex(string selectorClassName, int number)
        {
            JavascriptResponse result = await this.GetMainFrame().EvaluateScriptAsync(
                "{" + 
                    $"let views = document.getElementsByClassName('{selectorClassName}');" +
                    $"if(views.length == 1 && views[0].options.length > {number})" +
                    "{" +
                        $"views[0].selectedIndex = {number};" +
                        $"let e = document.createEvent('HTMLEvents');" +
                        $"e.initEvent('change', true, true);" +
                        $"views[0].dispatchEvent(e);" +
                    "}" +
                    $"views.length == 1 && views[0].options.length > {number}" +
                "}"
            );

            return (bool)result.Result;
        }

        private async Task<bool> SelectByValue(string selectorClassName, string value)
        {
            JavascriptResponse result = await this.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let found = false;" +
                    $"let views = document.getElementsByClassName('{selectorClassName}');" +
                    $"if(views.length == 1)" +
                    "{" +
                        "for(let i = 0; i < views[0].options.length; ++i)" +
                        "{" +
                            $"if(views[0].options[i].text == '{value}')" +
                            "{" +
                                "views[0].selectedIndex = i;" +
                                "let e = document.createEvent('HTMLEvents');" +
                                "e.initEvent('change', true, true);" +
                                "views[0].dispatchEvent(e);" +
                                "found = true;" +
                                "break;" +
                            "}" +
                        "}" +
                    "}" +
                    "found == true" +
                "}"
            );

            return (bool)result.Result;
        }

        private async Task ClickViewOfClass(string className)
        {
            await ClickViewOfClassWithNumber(className, 0, "");
        }

        private async Task<bool> ClickViewOfClassWithText(string className, string text, int attempts)
        {
            for(int i = 0; i < attempts; ++i)
            {
                if (await TryClickViewOfClassWithText(className, text))
                    return true;

                Token.ThrowIfCancellationRequested();
                await Task.Delay(DelayInfo.DiscreteWaitDelay);
            }

            return false;
        }

        private async Task<bool> TryClickViewOfClassWithText(string className, string text)
        {
            JavascriptResponse result = await this.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"let found = false;" +
                    "for(let view of views) {" +
                        $"if(view.textContent.indexOf('{text}') != -1)" + " {" +
                            "view.click();" +
                            "found = true;" +
                            "break;" +
                         "}" +
                    "}" +
                    "found;" +
                "}"
            );
            return (bool)result.Result;
        }

        private async Task ClickViewOfClassWithNumber(string className, int number, string forbiddenClass, bool parent = false)
        {
            while (!await TryClickViewOfClassWithNumber(className, number, forbiddenClass))
            {
                Token.ThrowIfCancellationRequested();
                await Task.Delay(DelayInfo.DiscreteWaitDelay);
            }
        }

        private async Task<bool> TryClickViewOfClassWithNumber(string className, int number, string forbiddenClass, bool parent = false)
        {
            string parentText = parent ? ".parentNode" : "";
            JavascriptResponse result = await this.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"let found = Array.prototype.filter.call(views, view => Array.prototype.indexOf.call(view.classList, '{forbiddenClass}') == -1);" +
                    $"if(found.length > {number})" +
                        $"found[{number}]{parentText}.click();" +
                    $"found.length > {number};" +
                "}"
            );

            return (bool)result.Result;
        }

        private async Task<bool> WaitForView(string className, int attempts)
        {
            for(int i = 0; i < attempts; ++i)
            {
                if (await TryFindView(className))
                    return true;

                Token.ThrowIfCancellationRequested();
                await Task.Delay(DelayInfo.DiscreteWaitDelay);
            }

            return false;
        }

        private async Task<bool> TryFindView(string className)
        {
            JavascriptResponse result = await this.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"views.length != 0;" +
                "}"
            );

            return (bool)result.Result;
        }

        private async Task<bool> TryFindViewOfClassWithText(string className, string text)
        {
            JavascriptResponse result = await this.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"let found = Array.prototype.filter.call(views, view => view.textContent === '{text}');" +
                    "found.length != 0;" +
                "}"
            );

            return (bool)result.Result;
        }

        private async Task<int> GetCountOfViewsOfClass(string className, string forbiddenClass)
        {
            JavascriptResponse result = await this.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"let found = Array.prototype.filter.call(views, view => Array.prototype.indexOf.call(view.classList, '{forbiddenClass}') == -1);" +
                    "found.length;" +
                "}"
            );

            return (int)result.Result;
        }

        private async Task WaitForSpinner()
        {
            while (await CheckSpinnerVisible())
            {
                Token.ThrowIfCancellationRequested();
                await Task.Delay(DelayInfo.DiscreteWaitDelay);
            }
        }

        private async Task<bool> CheckSpinnerVisible()
        {
            JavascriptResponse result = await this.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{LOADER_CLASS}');" +
                    $"views.length == 1 && views[0].style.display != 'none'" + 
                "}"
            );

            return (bool)result.Result;
        }

        private static class BrowserUtil
        {
            public static string GetViewsOfClassWithText(string className, string text)
            {
                return $"Array.prototype.filter.call({GetViewsOfClass(className)}, view => view.textContent === '{text}')";
            }

            public static string GetViewsOfClass(string className)
            {
                return $"document.getElementsByClassName('{className}')";
            }
        }
    }
}
