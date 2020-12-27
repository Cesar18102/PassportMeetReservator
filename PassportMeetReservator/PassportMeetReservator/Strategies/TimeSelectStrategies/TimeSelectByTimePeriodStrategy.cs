using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Common.Extensions;

using PassportMeetReservator.Controls;
using PassportMeetReservator.Extensions;

namespace PassportMeetReservator.Strategies.TimeSelectStrategies
{
    public class TimeSelectByTimePeriodStrategy : TimeSelectStrategyBase
    {
        public TimeSelectByTimePeriodStrategy(ReserverWebView browser) : base(browser) { }

        public override DateTime ChooseTimeSlotToReserve(List<DateTime> timeSlots)
        {
            return timeSlots.First();
        }

        public override List<DateTime> FilterTimeSlots(List<DateTime> timeSlots)
        {
            return timeSlots.Where(
                time => Browser.ReserveTimePeriod.IsIside(time.TimeOfDay)
            ).ToList();
        }

        public override async Task<DateTime?> SelectTimeFromList(DateTime date, CancellationToken token)
        {
            bool timeFound = await WaitForOptions(token);

            if (timeFound && await Browser.SelectByValue(TIME_SELECTOR_CLASS, date.GetFormattedTime()))
                return date;

            return null;
        }
    }
}
