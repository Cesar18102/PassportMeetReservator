using PassportMeetReservator.Controls;
using PassportMeetReservator.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PassportMeetReservator.Strategies.TimeSelectStrategies
{
    public class TimeSelectByBrowserNumberStrategy : TimeSelectStrategyBase
    {
        public TimeSelectByBrowserNumberStrategy(ReserverWebView browser) : base(browser) { }

        public override DateTime ChooseTimeSlotToReserve(List<DateTime> timeSlots)
        {
            return timeSlots.First();
        }

        public override List<DateTime> FilterDateTimeSlots(Dictionary<DateTime, DateTime[]> dateTimeSlots)
        {
            if (dateTimeSlots.Count != 0)
                return base.FilterDateTimeSlots(dateTimeSlots);

            return Browser.ReserveDateMin.DatesUntil(Browser.ReserveDateMax);
        }

        public override List<DateTime> FilterTimeSlots(List<DateTime> timeSlots)
        {
            return timeSlots;
        }

        public override async Task<DateTime?> SelectTimeFromList(DateTime date, CancellationToken token)
        {
            bool timeFound = await WaitForOptions(token);

            if (timeFound)
            {
                string selectedTime = await Browser.SelectByIndex(TIME_SELECTOR_CLASS, Browser.VirtualBrowserNumber);
                return date.Date + selectedTime.ParseTime();
            }

            return null;
        }
    }
}
