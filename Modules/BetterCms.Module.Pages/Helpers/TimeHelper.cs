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
    }
}