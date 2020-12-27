using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Strategies.DateCheckerNotifyStrategies
{
    public class NotifyAlwaysFlowStrategy : DateCheckerFlowStrategyBase
    {
        public override async Task DateCheckFlow(Func<Task<DateTime[]>> datesGetter, Func<DateTime, Task<DateTime[]>> timeGetter, Dictionary<DateTime, DateTime[]> slots, Action foundHandler)
        {
            foundHandler?.Invoke();
        }
    }
}
