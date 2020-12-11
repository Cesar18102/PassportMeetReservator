using System;
using System.Collections.Generic;

namespace PassportMeetReservator.Extensions
{
    public static class DateTimeExtensions
    {
        public static string GetFormattedDate(this DateTime date)
        {
            string month = (date.Month < 10 ? "0" : "") + date.Month.ToString();
            string day = (date.Day < 10 ? "0" : "") + date.Day.ToString();

            return $"{date.Year}-{month}-{day}";
        }

        public static string GetFormattedTime(this DateTime date)
        {
            string hour = (date.TimeOfDay.Hours < 10 ? "0" : "") + date.TimeOfDay.Hours.ToString();
            string minute = (date.TimeOfDay.Minutes < 10 ? "0" : "") + date.TimeOfDay.Minutes.ToString();

            return $"{hour}:{minute}";
        }

        public static string GetFormattedDateForFileName(this DateTime date)
        {
            return date.ToString().Replace(":", "_");
        }

        public static TimeSpan ParseTime(this string timeString)
        {
            string[] items = timeString.Split(':');

            int hour = Convert.ToInt32(items[0]);
            int minute = Convert.ToInt32(items[1]);

            return new TimeSpan(hour, minute, 0);
        }

        public static List<DateTime> DatesUntil(this DateTime min, DateTime max)
        {
            if (min.Date > max.Date)
                return new List<DateTime>();

            List<DateTime> dates = new List<DateTime>();
            for(DateTime current = min; current != max; current = current.AddDays(1))
                dates.Add(current);
            dates.Add(max);

            return dates;
        }
    }
}
