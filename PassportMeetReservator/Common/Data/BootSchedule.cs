using System;
using System.Linq;
using System.Collections.Generic;

namespace Common.Data
{
    public class BootSchedule
    {
        public List<BootPeriod> BootPeriods { get; private set; } = new List<BootPeriod>();

        public bool IsInside(TimeSpan time)
        {
            return BootPeriods.Any(period => period.IsIside(time));
        }
    }
}
