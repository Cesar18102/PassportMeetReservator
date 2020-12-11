using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PassportMeetReservator.Strategies.DateCheckerNotifyStrategies
{
    public class NotifyIfDatesAndTimesFoundFlowStrategy : DateCheckerFlowStrategyBase
    {
        public override async Task DateCheckFlow(Func<Task<DateTime[]>> datesGetter, Func<DateTime, Task<DateTime[]>> timeGetter, Dictionary<DateTime, DateTime[]> slots, Action foundHandler)
        {
            DateTime[] dates = await datesGetter();

            slots.Clear();
            foreach (DateTime date in dates)
                slots.Add(date, await timeGetter(date)); //parallel?

            if (dates != null && dates.Length != 0 && slots.Any(slot => slot.Value != null && slot.Value.Length != 0))
                foundHandler?.Invoke();
        }
    }
}
