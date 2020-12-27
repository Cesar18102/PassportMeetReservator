using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Strategies.DateCheckerNotifyStrategies
{
    public abstract class DateCheckerFlowStrategyBase
    {
        public abstract Task DateCheckFlow(Func<Task<DateTime[]>> datesGetter, Func<DateTime, Task<DateTime[]>> timeGetter, Dictionary<DateTime, DateTime[]> slots, Action foundHandler);
    }
}
