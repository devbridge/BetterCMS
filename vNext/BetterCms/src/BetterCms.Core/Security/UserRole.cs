using System;

namespace BetterCms.Core.Security
{
    /// <summary>
    /// Implements permission interface properties.
    /// </summary>
    public class UserRole : IUserRole
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRole" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="localizedName">Name of the localized.</param>
        public UserRole(string name, Func<string> localizedName)
        {
            Name = name;
            LocalizedName = localizedName;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the localized name.
        /// </summary>
        /// <value>
        /// The localized name.
        /// </value>
        public Func<string> LocalizedName { get; private set; }
    }
}
