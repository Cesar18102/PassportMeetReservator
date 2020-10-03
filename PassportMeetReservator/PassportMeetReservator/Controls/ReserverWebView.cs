using System;
using System.Drawing;

using System.Threading;
using System.Threading.Tasks;

using CefSharp;
using CefSharp.WinForms;

using PassportMeetReservator.Data;

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

        private const string RESERVATION_TYPE_BUTTON_CLASS = "operation-button";

        private const string NEXT_STEP_BUTTON_CLASS = "btn footer-btn btn-secondary btn-lg btn-block";
        private const string NEXT_STEP_BUTTON_TEXT = "Dalej";

        private const string CALENDAR_CLASS = "vc-container vc-reset vc-text-gray-900 vc-bg-white vc-border vc-border-gray-400 vc-rounded-lg";
        private const string DATE_CLASS = "vc-day-content vc-focusable vc-font-medium vc-text-sm vc-cursor-pointer focus:vc-font-bold vc-rounded-full";
        private const string INACTIVE_DATE_CLASS = "vc-text-gray-400";

        private const string TIME_SELECTOR_CLASS = "text-center form-control custom-select";
        private const string INPUT_CLASS = "property-input form-control";

        private const string ACCEPT_TICK_CLASS = "custom-control-label";
        private const string ACCEPT_TICK_TEXT = "Akceptuję regulamin";

        private const string ACCEPT_BUTTON_CLASS = "btn footer-btn btn-secondary btn-lg btn-block";
        private const string ACCEPT_BUTTON_TEXT = "Rezerwuję";

        private const string NEXT_MONTH_BUTTON_CLASS = "vc-flex vc-justify-center vc-items-center vc-cursor-pointer vc-select-none vc-pointer-events-auto vc-text-gray-600 vc-rounded vc-border-2 vc-border-transparent hover:vc-opacity-50 hover:vc-bg-gray-300 focus:vc-border-gray-300";
        private const int NEXT_MONTH_BUTTON_NUM = 1;

        private const string FAILED_RESERVE_TIME_CLASS = "alert alert-danger";
        private const string FAILED_RESERVE_TIME_TEXT = "Niestety wybrana data jest już zarezerwowana, prosimy wybrać inną.";

        private const string CONTINUE_RESERVATION_BUTTON_CLASS = "btn btn-success btn-lg btn-block";
        private const string CONTINUE_RESERVATION_BUTTON_TEXT = "Tak, chce kontynuować";

        private const string DATES_ABSENT_ID = "Dataiczas1";
        private const string DATES_ABSENT_TEXT = "Brak dostępnych rezerwacji dla tej operacji";

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

                if (OnOrderChanged != null)
                    Invoke(OnOrderChanged, this, new OrderEventArgs(order));
            }
        }

        public int BotNumber { get; set; }

        public int RealBrowserNumber { get; set; }
        public int BrowserNumber { get; set; }
        public int BrowsersCount { get; set; }

        public string Operation { get; set; }
        public string InitUrl { get; set; } = "https://rejestracjapoznan.poznan.uw.gov.pl/";

        public DateTime ReserveDateMin { get; set; } = DateTime.Now;
        public DateTime ReserveDateMax { get; set; } = DateTime.Now;

        public DelayInfo DelayInfo { get; set; }
        public BootSchedule Schedule { get; set; }

        private const int DATE_WAIT_ITERATION_COUNT = 4;
        private const int TIME_WAIT_ITERATION_COUNT = 4;
        private const int SITE_FALL_WAIT_ATTEMPTS = 40;

        [Obsolete]
        public ReserverWebView()
        {
            this.AddressChanged += (sender, e) => Invoke(OnUrlChanged, this, new UrlChangedEventArgs(Address));
            Scale(new SizeF(0.5f, 0.5f));
        }

        public void Free()
        {
            IsBusy = false;
        }

        public void Reset()
        {
            TokenSource.Cancel();
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

            TokenSource.Cancel();
        }

        public async void Start()
        {
            using (TokenSource = new CancellationTokenSource())
            {
                Token = TokenSource.Token;

                Loop = Init();

                try { await Loop; }
                catch (OperationCanceledException ex) { Loop.Dispose(); Start(); }
            }
        }

        private async Task Init()
        {
            IsBusy = true;
            Load(InitUrl);

            while (true)
            {
                await Task.Delay(DelayInfo.BrowserIterationDelay, Token);

                if (Paused)
                    continue;

                if (Schedule != null && !Schedule.IsInside(DateTime.Now.TimeOfDay))
                    continue;

                if (await Iteration() && !Auto)
                    await WaitForManualReaction();
            }
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

        private string GetFormattedDate(DateTime date)
        {
            string month = (date.Month < 10 ? "0" : "") + date.Month.ToString();
            string day = (date.Day < 10 ? "0" : "") + date.Day.ToString();

            return $"{date.Year}-{month}-{day}";
        }

        private async Task<bool> Iteration()
        {
            Request request = new Request()
            {
                Url = InitUrl,
                Flags = UrlRequestFlags.DisableCache
            };

            this.GetMainFrame().LoadRequest(request);
            //Load(InitUrl);

            await Task.Delay(DelayInfo.ActionResultDelay, Token);

            string operation = Auto && Order != null ? Order.Operation : Operation;
            if (!await ClickViewOfClassWithText(RESERVATION_TYPE_BUTTON_CLASS, operation, SITE_FALL_WAIT_ATTEMPTS))
                return false;

            if (!await ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT, SITE_FALL_WAIT_ATTEMPTS))
                return false;
            //SELECT ORDER TYPE

            if (!await WaitForView(CALENDAR_CLASS, SITE_FALL_WAIT_ATTEMPTS))
                return false;

            await Task.Delay(DelayInfo.ActionResultDelay, Token);

            if (DateTime.Now.Month != ReserveDateMin.Month)
            {
                await TryClickViewOfClassWithNumber(NEXT_MONTH_BUTTON_CLASS, NEXT_MONTH_BUTTON_NUM, "");
                await Task.Delay(DelayInfo.ActionResultDelay, Token);
            }
            //PUSH CALENDAR FIRST TIME IF NEEDED

            DateTime? selectedDate = null;
            DateTime currentScanDate = ReserveDateMin;

            while (currentScanDate >= ReserveDateMin && currentScanDate <= ReserveDateMax)
            {
                bool isStartMonth = currentScanDate.Year == ReserveDateMin.Year && currentScanDate.Month == ReserveDateMin.Month;
                bool isEndMonth = currentScanDate.Year == ReserveDateMax.Year && currentScanDate.Month == ReserveDateMax.Month;

                int dayStart = isStartMonth ? ReserveDateMin.Day : 1;
                int dayEnd = isEndMonth ? ReserveDateMax.Day : DateTime.DaysInMonth(currentScanDate.Year, currentScanDate.Month);

                selectedDate = await ScanCalendar(dayStart, dayEnd, currentScanDate.Month, currentScanDate.Year); //TRY SELECT DATE AND TIME

                if (selectedDate.HasValue || isEndMonth)
                    break;

                DateTime temp = currentScanDate.AddMonths(1);
                currentScanDate = new DateTime(temp.Year, temp.Month, 1);

                await TryClickViewOfClassWithNumber(NEXT_MONTH_BUTTON_CLASS, NEXT_MONTH_BUTTON_NUM, "");
                await Task.Delay(DelayInfo.ActionResultDelay, Token);
                //PUSH CALENDAR IF NO ACTIVE DATES FOUND
            }

            if (!selectedDate.HasValue)
                return false;

            await ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT, SITE_FALL_WAIT_ATTEMPTS);
            //CONFIRM SELECTED DATE

            await Task.Delay(DelayInfo.ActionResultDelay, Token);
            await Task.Delay(DelayInfo.ActionResultDelay, Token);//double delay
            await Task.Delay(DelayInfo.ActionResultDelay, Token);//triple delay
            if (await TryFindViewOfClassWithText(FAILED_RESERVE_TIME_CLASS, FAILED_RESERVE_TIME_TEXT))
                return false;
            //CHECK TIME RESERVED ERROR

            Selected = true;
            OnDateTimeSelected?.Invoke(this, new DateTimeEventArgs(selectedDate.Value));

            if (!Auto || Order == null) //manual
                return true;

            await FillForm();

            return true;
        } 

        private async Task FillForm()
        {
            await WaitForView(INPUT_CLASS, SITE_FALL_WAIT_ATTEMPTS);
            await Task.Delay(DelayInfo.ActionResultDelay);

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

        private async Task<DateTime?> ScanCalendar(int dayStart, int dayEnd, int month, int year)
        {
            for (int i = dayStart; i <= dayEnd; ++i)
            {
                DateTime date = new DateTime(year, month, i);
                string formattedDate = GetFormattedDate(date);

                bool dayFound = false;
                for (int j = 0; j < DATE_WAIT_ITERATION_COUNT; ++j)
                {
                    dayFound = await TryFindView($"vc-day id-{formattedDate}");

                    if(dayFound)
                        break;

                    await Task.Delay(DelayInfo.DiscreteWaitDelay, Token);
                } //WAIT FOR DAY FOR SEVERAL TIMES - WAIT FOR LOADING

                if (!dayFound)
                    continue;

                await this.GetMainFrame().EvaluateScriptAsync($"document.getElementsByClassName('vc-day id-{formattedDate}')[0].children[0].children[0].click()");

                await WaitForView(TIME_SELECTOR_CLASS, SITE_FALL_WAIT_ATTEMPTS);
                await Task.Delay(DelayInfo.ActionResultDelay, Token);

                bool timeFound = false;
                for (int j = 0; j < TIME_WAIT_ITERATION_COUNT; ++j)
                {
                    JavascriptResponse jsTimesCount = await this.GetMainFrame().EvaluateScriptAsync($"document.getElementsByClassName('{TIME_SELECTOR_CLASS}')[0].options.length");
                    timeFound = jsTimesCount.Success && jsTimesCount.Result != null && (int)jsTimesCount.Result != 0;

                    if (timeFound)
                        break;

                    await Task.Delay(DelayInfo.DiscreteWaitDelay, Token);
                } //WAIT FOR TIME FOR SEVERAL TIMES - WAIT FOR LOADING

                if (timeFound && await SelectValue(TIME_SELECTOR_CLASS, BotNumber * BrowsersCount + BrowserNumber))
                {
                    JavascriptResponse jsTime = await this.GetMainFrame().EvaluateScriptAsync(
                        $"document.getElementsByClassName('{TIME_SELECTOR_CLASS}')[0].options[{BotNumber * BrowsersCount + BrowserNumber}].text"
                    );

                    TimeSpan time = TimeSpan.Parse(jsTime.Result.ToString());

                    return date + time;
                }
            }

            return null;
        }

        private void ReserverWebView_UrlChanged(object sender, EventArgs e)
        {
            if (Address == InitUrl || Order == null)
                return;

            Order.Done = true;
            Order.Doing = false;

            Invoke(OnReserved, this, new ReservedEventArgs(Address, Order));
            this.AddressChanged -= ReserverWebView_UrlChanged;

            Order = null;
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

        private async Task<bool> SelectValue(string selectorClassName, int number)
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
                    $"let found = Array.prototype.filter.call(views, view => view.textContent === '{text}');" +
                    "if(found.length != 0)" +
                        "found[0].click();" +
                    "found.length != 0;" +
                "}"
            );
            //return true;
            return (bool)result.Result;
        }

        private async Task ClickViewOfClassWithNumber(string className, int number, string forbiddenClass)
        {
            while (!await TryClickViewOfClassWithNumber(className, number, forbiddenClass))
                Token.ThrowIfCancellationRequested();
        }

        private async Task <bool> TryClickViewOfClassWithNumber(string className, int number, string forbiddenClass)
        {
            JavascriptResponse result = await this.GetMainFrame().EvaluateScriptAsync(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"let found = Array.prototype.filter.call(views, view => Array.prototype.indexOf.call(view.classList, '{forbiddenClass}') == -1);" +
                    $"if(found.length > {number})" +
                        $"found[{number}].click();" +
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
