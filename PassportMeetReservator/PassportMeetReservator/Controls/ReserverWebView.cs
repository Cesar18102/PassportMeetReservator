using System;
using System.Threading.Tasks;

using CefSharp;
using CefSharp.WinForms;

using PassportMeetReservator.Data;

namespace PassportMeetReservator.Controls
{
    public class ReserverWebView : ChromiumWebBrowser
    {
        private Random Random = new Random();

        public event EventHandler<ReservedEventArgs> OnReserved;
        public event EventHandler<UrlChangedEventArgs> OnUrlChanged;
        public event EventHandler<OrderEventArgs> OnOrderChanged;

        private const int WAIT_DELAY = 100;//700
        private const int UPDATE_DELAY = 100;
        private const string INIT_URL = "https://rejestracjapoznan.poznan.uw.gov.pl/";

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

        public int Number { get; set; }
        public bool Paused { get; set; } = true;
        public bool IsBusy { get; private set; }

        private ReservationOrder order = null;
        public ReservationOrder Order
        {
            get => order; 
            set
            {
                order = value;
                OnOrderChanged?.Invoke(this, new OrderEventArgs(Order));
            }
        }

        [Obsolete]
        public ReserverWebView()
        {
            this.AddressChanged += (sender, e) => Invoke(OnUrlChanged, this, new UrlChangedEventArgs(Address));
        }

        public async void Start()
        {
            if (Order == null)
                return;

            IsBusy = true;
            Order.Doing = true;
            
            while (true)
            {
                await Task.Delay(UPDATE_DELAY);

                if (Paused)
                    continue;

                if (await Iteration())
                    break;
            }

            Order.Done = true;
            Order.Doing = false;
            IsBusy = false;
        }

        private async Task<bool> Iteration()
        {
            Load(INIT_URL);

            await ClickViewOfClassWithText(RESERVATION_TYPE_BUTTON_CLASS, Order.ReservationTypeText);
            await ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT);
            //SELECT ORDER TYPE

            await WaitForView(CALENDAR_CLASS);
            await Task.Delay(WAIT_DELAY);

            int activeDatesCount = await GetCountOfViewsOfClass(DATE_CLASS, INACTIVE_DATE_CLASS);

            if (activeDatesCount == 0)
            {
                await ClickViewOfClassWithNumber(NEXT_MONTH_BUTTON_CLASS, NEXT_MONTH_BUTTON_NUM, "");
                //await Task.Delay(WAIT_DELAY);

                int activeDatesNextMonthCount = await GetCountOfViewsOfClass(DATE_CLASS, INACTIVE_DATE_CLASS);

                if (activeDatesNextMonthCount == 0)
                    return false;
                else if (!await TryClickViewOfClassWithNumber(DATE_CLASS, Random.Next(0, activeDatesNextMonthCount), INACTIVE_DATE_CLASS))
                    return false;
            }
            else if (!await TryClickViewOfClassWithNumber(DATE_CLASS, Random.Next(0, activeDatesCount), INACTIVE_DATE_CLASS))
                return false;
            //SELECT DATE

            await WaitForView(TIME_SELECTOR_CLASS);
            await Task.Delay(WAIT_DELAY);

            if (!await SelectValue(TIME_SELECTOR_CLASS, Number))
                return false;

            await ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT);
            //SELECT TIME

            if (await TryFindViewOfClassWithText(FAILED_RESERVE_TIME_CLASS, FAILED_RESERVE_TIME_TEXT))
                return false;
            //CHECK TIME RESERVED ERROR

            await WaitForView(INPUT_CLASS);
            //await Task.Delay(WAIT_DELAY);

            await ClickViewOfClassWithNumber(INPUT_CLASS, 0, "");
            if (!await SetTextToViewOfClassWithNumber(INPUT_CLASS, 0, Order.Surname)) //DO NOT CRASH
                return false;

            await ClickViewOfClassWithNumber(INPUT_CLASS, 1, "");
            if (!await SetTextToViewOfClassWithNumber(INPUT_CLASS, 1, Order.Name)) //DO NOT CRASH
                return false;

            await ClickViewOfClassWithNumber(INPUT_CLASS, 2, "");
            if (!await SetTextToViewOfClassWithNumber(INPUT_CLASS, 2, Order.Email)) //DO NOT CRASH
                return false;

            await ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT);
            //FILL FORM

            this.AddressChanged += ReserverWebView_UrlChanged;

            await WaitForView(ACCEPT_TICK_CLASS);
            //await Task.Delay(WAIT_DELAY);

            await ClickViewOfClassWithText(ACCEPT_TICK_CLASS, ACCEPT_TICK_TEXT);
            await ClickViewOfClassWithText(ACCEPT_BUTTON_CLASS, ACCEPT_BUTTON_TEXT);
            //ACCEPT

            return true;
        }

        private void ReserverWebView_UrlChanged(object sender, EventArgs e)
        {
            Invoke(OnReserved, this, new ReservedEventArgs(Address, Order));
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

        private async Task ClickViewOfClassWithText(string className, string text)
        {
            while (!await TryClickViewOfClassWithText(className, text)) ;
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

            return (bool)result.Result;
        }

        private async Task ClickViewOfClassWithNumber(string className, int number, string forbiddenClass)
        {
            while (!await TryClickViewOfClassWithNumber(className, number, forbiddenClass)) ;
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

        private async Task WaitForView(string className)
        {
            while (!await TryFindView(className)) ;
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
