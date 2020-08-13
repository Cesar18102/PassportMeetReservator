using System;
using System.Threading.Tasks;

using EO.WebBrowser;

using PassportMeetReservator.Data;

namespace PassportMeetReservator.Controls
{
    public class ReserverWebView : WebView
    {
        public event EventHandler<ReservedEventArgs> OnReserved;
        private Random Random = new Random();

        private const int WAIT_DELAY = 700;
        private const int UPDATE_DELAY = 500;
        private const string INIT_URL = "https://rejestracjapoznan.poznan.uw.gov.pl/";

        private const string RESERVATION_TYPE_BUTTON_CLASS = "operation-button";
        private const string NEXT_STEP_BUTTON_CLASS = "btn footer-btn btn-secondary btn-lg btn-block";

        private const string CALENDAR_CLASS = "vc-container vc-reset vc-text-gray-900 vc-bg-white vc-border vc-border-gray-400 vc-rounded-lg";
        private const string DATE_CLASS = "vc-day-content vc-focusable vc-font-medium vc-text-sm vc-cursor-pointer focus:vc-font-bold vc-rounded-full";
        private const string INACTIVE_DATE_CLASS = "vc-text-gray-400";

        private const string TIME_SELECTOR_CLASS = "text-center form-control custom-select";
        private const string INPUT_CLASS = "property-input form-control";

        private const string ACCEPT_TICK_CLASS = "custom-control-label";
        private const string ACCEPT_BUTTON_CLASS = "btn footer-btn btn-secondary btn-lg btn-block";

        private const string NEXT_STEP_BUTTON_TEXT = "Dalej";
        private const string ACCEPT_TICK_TEXT = "Akceptuję regulamin";
        private const string ACCEPT_BUTTON_TEXT = "Rezerwuję";

        public IntPtr BodyHandle
        {
            get => Handle;
            set => Create(value);
        }

        public bool IsBusy { get; private set; }

        public int Number { get; set; }
        public ReservationOrder Order { get; set; }

        public async void Start()
        {
            if (Order == null)
                return;

            IsBusy = true;
            Order.Doing = true;
            
            while (true)
            {
                if (await Iteration())
                    break;

                await Task.Delay(UPDATE_DELAY);
            }

            Order.Done = true;
            Order.Doing = false;
            IsBusy = false;
        }

        private async Task<bool> Iteration()
        {
            LoadUrlAndWait(INIT_URL);

            ClickViewOfClassWithText(RESERVATION_TYPE_BUTTON_CLASS, Order.ReservationTypeText);
            ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT);
            //SELECT ORDER TYPE

            WaitForView(CALENDAR_CLASS);
            await Task.Delay(WAIT_DELAY);

            int activeDatesCount = GetCountOfViewsOfClass(DATE_CLASS, INACTIVE_DATE_CLASS);
            if(!TryClickViewOfClassWithNumber(DATE_CLASS, Random.Next(0, activeDatesCount), INACTIVE_DATE_CLASS))
                return false;
            //SELECT DATE

            WaitForView(TIME_SELECTOR_CLASS);
            await Task.Delay(WAIT_DELAY);

            if (!SelectValue(TIME_SELECTOR_CLASS, Number))
                return false;

            ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT);
            //SELECT TIME

            WaitForView(INPUT_CLASS);

            ClickViewOfClassWithNumber(INPUT_CLASS, 0, "");
            if (!SetTextToViewOfClassWithNumber(INPUT_CLASS, 0, Order.Surname))
                return false;

            ClickViewOfClassWithNumber(INPUT_CLASS, 1, "");
            if (!SetTextToViewOfClassWithNumber(INPUT_CLASS, 1, Order.Name))
                return false;

            ClickViewOfClassWithNumber(INPUT_CLASS, 2, "");
            if (!SetTextToViewOfClassWithNumber(INPUT_CLASS, 2, Order.Email))
                return false;

            ClickViewOfClassWithText(NEXT_STEP_BUTTON_CLASS, NEXT_STEP_BUTTON_TEXT);
            //FILL FORM

            this.UrlChanged += ReserverWebView_UrlChanged;

            WaitForView(ACCEPT_TICK_CLASS);
            ClickViewOfClassWithText(ACCEPT_TICK_CLASS, ACCEPT_TICK_TEXT);
            ClickViewOfClassWithText(ACCEPT_BUTTON_CLASS, ACCEPT_BUTTON_TEXT);
            //ACCEPT

            //await Task.Delay(2000);

            return true;
        }

        private void ReserverWebView_UrlChanged(object sender, EventArgs e)
        {
            OnReserved?.Invoke(this, new ReservedEventArgs(Url));
            this.UrlChanged -= ReserverWebView_UrlChanged;
        }

        private bool SetTextToViewOfClassWithNumber(string className, int number, string text)
        {
            return (bool)EvalScript(
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
                "}", true
            );
        }

        private bool SelectValue(string selectorClassName, int number)
        {
            return (bool)EvalScript(
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
                "}", true
            );
        }

        private void ClickViewOfClass(string className)
        {
            ClickViewOfClassWithNumber(className, 0, "");
        }

        private void ClickViewOfClassWithText(string className, string text)
        {
            while (!TryClickViewOfClassWithText(className, text)) ;
        }

        private bool TryClickViewOfClassWithText(string className, string text)
        {
            return (bool)EvalScript(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"let found = Array.prototype.filter.call(views, view => view.textContent === '{text}');" +
                    "if(found.length != 0)" +
                        "found[0].click();" +
                    "found.length != 0;" +
                "}", true
            );
        }

        private void ClickViewOfClassWithNumber(string className, int number, string forbiddenClass)
        {
            while (!TryClickViewOfClassWithNumber(className, number, forbiddenClass)) ;
        }

        private bool TryClickViewOfClassWithNumber(string className, int number, string forbiddenClass)
        {
            return (bool)EvalScript(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"let found = Array.prototype.filter.call(views, view => Array.prototype.indexOf.call(view.classList, '{forbiddenClass}') == -1);" +
                    $"if(found.length > {number})" +
                        $"found[{number}].click();" +
                    $"found.length > {number};" +
                "}", true
            );
        }

        private void WaitForView(string className)
        {
            while (!TryFindView(className)) ;
        }

        private bool TryFindView(string className)
        {
            return (bool)EvalScript(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"views.length != 0;" +
                "}", true
            );
        }

        private int GetCountOfViewsOfClass(string className, string forbiddenClass)
        {
            return (int)EvalScript(
                "{" +
                    $"let views = document.getElementsByClassName('{className}');" +
                    $"let found = Array.prototype.filter.call(views, view => Array.prototype.indexOf.call(view.classList, '{forbiddenClass}') == -1);" +
                    "found.length;" +
                "}", true
            );
        }
    }
}
