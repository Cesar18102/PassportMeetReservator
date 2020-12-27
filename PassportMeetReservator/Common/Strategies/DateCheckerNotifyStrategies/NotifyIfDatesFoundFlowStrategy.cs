using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Strategies.DateCheckerNotifyStrategies
{
    public class NotifyIfDatesFoundFlowStrategy : DateCheckerFlowStrategyBase
    {
        public override async Task DateCheckFlow(Func<Task<DateTime[]>> datesGetter, Func<DateTime, Task<DateTime[]>> timeGetter, Dictionary<DateTime, DateTime[]> slots, Action foundHandler)
        {
            DateTime[] dates = await datesGetter();

            slots.Clear();
            foreach (DateTime date in dates)
                slots.Add(date, new DateTime[] { date });

            if (dates != null && dates.Length != 0)
                foundHandler?.Invoke();
        }
    }
}
