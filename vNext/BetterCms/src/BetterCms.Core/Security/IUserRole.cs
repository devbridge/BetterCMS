using System;

namespace BetterCms.Core.Security
{
    /// <summary>
    /// Defines interface to access entity properties.
    /// </summary>
    public interface IUserRole
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the localized name.
        /// </summary>
        /// <value>
        /// The name of the localized.
        /// </value>
        Func<string> LocalizedName { get; }
    }
}
