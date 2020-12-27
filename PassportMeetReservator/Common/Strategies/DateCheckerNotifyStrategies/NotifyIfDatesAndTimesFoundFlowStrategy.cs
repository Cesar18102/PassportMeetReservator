using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Strategies.DateCheckerNotifyStrategies
{
    public class NotifyIfDatesAndTimesFoundFlowStrategy : DateCheckerFlowStrategyBase
    {
        public override async Task DateCheckFlow(Func<Task<DateTime[]>> datesGetter, Func<DateTime, Task<DateTime[]>> timeGetter, Dictionary<DateTime, DateTime[]> slots, Action foundHandler)
        {
            DateTime[] dates = await datesGetter();

            slots.Clear();

            IEnumerable<Task<DateTime[]>> slotTasks = dates.Select(date => timeGetter(date));
            IEnumerable<DateTime[]> slotsUpdated = await Task.WhenAll(slotTasks);

            for (int i = 0; i < dates.Length; ++i)
                slots.Add(dates[i], slotsUpdated.ElementAt(i));

            if (dates != null && dates.Length != 0 && slots.Any(slot => slot.Value != null && slot.Value.Length != 0))
                foundHandler?.Invoke();
        }
    }
}
