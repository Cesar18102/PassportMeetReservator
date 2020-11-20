using System;

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
    }
}
