using System;

namespace BetterCms.Module.Root.Mvc
{
    /// <summary>
    /// Converter helpers.
    /// </summary>
    public static class Converters
    {
        /// <summary>
        /// Converts string source to the value of Guid type or Guid.Empty.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <returns>A value or Guid.Empty.</returns>
        public static Guid ToGuidOrDefault(this string source)
        {
            Guid result;
            if (Guid.TryParse(source, out result))
            {
                return result;
            }
            return Guid.Empty;
        }

        public static Guid? ToGuidOrNull(this string source)
        {
            Guid result;
            if (Guid.TryParse(source, out result))
            {
                return result;
            }
            return null;
        }
        
        /// <summary>
        /// Converts string source to the value of Boolean type.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <returns>A Boolean value.</returns>
        public static bool ToBoolOrDefault(this string source)
        {
            if (source == "1")
            {
                return true;
            }

            bool result;
            if (bool.TryParse(source, out result))
            {
                return result;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the given Guid has default value.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the specified source has default value; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasDefaultValue(this Guid source)
        {
            return source == default(Guid);
        }

        /// <summary>
        /// Gets the value or null.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns><c>null</c> if is null or has empty value; otherwise, value.</returns>
        public static Guid? ToNullOrValue(this Guid? source)
        {
            if (source.HasValue && source.Value.HasDefaultValue())
            {
                return null;
            }

            return source;
        }

        /// <summary>
        /// Converts string source to the value on integer type or zero.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A value or default integer value (zero).</returns>
        public static int ToIntOrDefault(this string source)
        {
            int result;
            if (int.TryParse(source, out result))
            {
                return result;
            }
            return default(int);
        }

        /// <summary>
        /// Converts date source to formatted date string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// Formatted date.
        /// </returns>
        public static string ToFormattedDateString(this DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
            {
                return string.Empty;
            }
            return dateTime.ToString("MMM dd, yyyy");            
        }

        public static string ToFormatedTimeString(this TimeSpan timeSpan)
        {
            string format = timeSpan.Days >= 1 ? "d'.'hh':'mm" : "hh':'mm";
            return timeSpan.ToString(format);
        }

        /// <summary>
        /// Converts date source to formatted date string.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// Formatted date.
        /// </returns>
        public static string ToFormattedDateString(this DateTime? dateTime)
        {
            return dateTime != null ? dateTime.Value.ToFormattedDateString() : string.Empty;
        }

        public static string ToFormatedTimeString(this TimeSpan? timeSpan)
        {
            return timeSpan != null ? timeSpan.Value.ToFormatedTimeString() : null;
        }
    }
}