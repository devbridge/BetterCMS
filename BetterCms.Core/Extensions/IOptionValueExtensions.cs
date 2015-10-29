using System;

using BetterCms.Core.DataContracts;

using Common.Logging;

namespace BetterCms.Core.Extensions
{
    public static class IOptionValueExtensions
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static T CastValueOrDefault<T>(this IOptionValue optionValue, T defaultValue = default(T))
        {
            try
            {
                return (T)optionValue.Value;
            }
            catch (InvalidCastException ex)
            {
                Log.ErrorFormat("Failed to convert option {0} to type {1}", ex, optionValue.Value.ToString(), typeof(T));
                return defaultValue;
            }
        }
    }
}