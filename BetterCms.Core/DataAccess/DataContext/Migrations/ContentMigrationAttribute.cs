using System;

namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    /// <summary>
    /// Attribute for content migration.
    /// </summary>
    public class ContentMigrationAttribute : Attribute
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public long Version { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentMigrationAttribute"/> class.
        /// </summary>
        /// <param name="version">The version.</param>
        public ContentMigrationAttribute(long version)
        {
            Version = version;
        }
    }
}
