using CefSharp;

using PassportMeetReservator.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PassportMeetReservator.Strategies.TimeSelectStrategies
{
    public abstract class TimeSelectStrategyBase
    {
        protected const int TIME_WAIT_ITERATION_COUNT = 20;
        protected const string TIME_SELECTOR_CLASS = "text-center form-control custom-select";

        protected ReserverWebView Browser { get; set; }

        public TimeSelectStrategyBase(ReserverWebView browser)
        {
            Browser = browser;
        }

        public abstract List<DateTime> FilterTimeSlots(List<DateTime> timeSlots);
        public abstract DateTime ChooseTimeSlotToReserve(List<DateTime> timeSlots);
        public abstract Task<DateTime?> SelectTimeFromList(DateTime date, CancellationToken token);

        public virtual List<DateTime> FilterDateTimeSlots(Dictionary<DateTime, DateTime[]> dateTimeSlots)
        {
            return dateTimeSlots.Where(slot => slot.Key >= Browser.ReserveDateMin && slot.Key <= Browser.ReserveDateMax)
                .SelectMany(slot => slot.Value)
                .ToList();
        }

        protected async Task<bool> WaitForOptions(CancellationToken token)
        {
            for (int j = 0; j < TIME_WAIT_ITERATION_COUNT; ++j)
            {
                JavascriptResponse jsTimesCount = await Browser.GetMainFrame().EvaluateScriptAsync(
                    $"document.getElementsByClassName('{TIME_SELECTOR_CLASS}')[0].options.length"
                );

                if (jsTimesCount.Success && jsTimesCount.Result != null && (int)jsTimesCount.Result != 0)
                    return true;

                await Task.Delay(Browser.DelayInfo.DiscreteWaitDelay, token);
            } //WAIT FOR TIME FOR SEVERAL TIMES - WAIT FOR LOADING

            return false;
        }
    }
}
