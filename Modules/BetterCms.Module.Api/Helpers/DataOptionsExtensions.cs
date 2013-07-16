using System.Linq;

using BetterCms.Module.Api.Operations;
using BetterCms.Module.Api.Operations.Enums;

namespace BetterCms.Module.Api.Helpers
{
    /// <summary>
    /// DataOptions class extensions
    /// </summary>
    public static class DataOptionsExtensions
    {
        /// <summary>
        /// Sets the default order.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        public static void SetDefaultOrder(this DataOptions options, string column, OrderDirection direction = OrderDirection.Asc)
        {
            if (!options.Order.By.Any())
            {
                options.Order.Add(column, direction);
            }
        }

        /// <summary>
        /// Determines whether the specified options has paging.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>
        ///   <c>true</c> if the specified options has paging; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasPaging(this DataOptions options)
        {
            return options != null && options.Take > 0;
        }
    }
}