using System;
using System.Collections.Generic;

namespace Common.Extensions
{
    public static class DateCheckerExtensions
    {
        public static void ApplyToDateCheckers<T>(this Dictionary<string, Dictionary<string, T[]>> dateCheckers,  Action<T> action) where T : DateChecker
        {
            foreach (Dictionary<string, T[]> platformCheckers in dateCheckers.Values)
                foreach (T[] cityCheckers in platformCheckers.Values)
                    foreach (T checker in cityCheckers)
                        action(checker);
        }
    }
}
