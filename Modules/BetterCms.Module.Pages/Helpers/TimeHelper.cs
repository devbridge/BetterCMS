using System;

namespace BetterCms.Module.Pages.Helpers
{
    /// <summary>
    /// Helper class for working with date and time.
    /// </summary>
    public static class TimeHelper
    {
        /// <summary>
        /// Formats the end date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime? FormatEndDate(DateTime? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            if (date.Value != new DateTime(date.Value.Year, date.Value.Month, date.Value.Day))
            {
                return date;
            }

            return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 23, 59, 59);
        }

        public static bool IsTheSameDay(DateTime? date1, DateTime? date2)
        {
            return (!date1.HasValue && !date2.HasValue)
                   || (date1.HasValue && date2.HasValue
                       && date1.Value.Year == date2.Value.Year
                       && date1.Value.Month == date2.Value.Month
                       && date1.Value.Day == date2.Value.Day);
        }

        public static DateTime GetFirstIfTheSameDay(DateTime date1, DateTime date2)
        {
            return IsTheSameDay(date1, date2) ? date1 : date2;
        }

        public static DateTime? GetFirstIfTheSameDay(DateTime? date1, DateTime? date2)
        {
            return IsTheSameDay(date1, date2) ? date1 : date2;
        }
    }
}