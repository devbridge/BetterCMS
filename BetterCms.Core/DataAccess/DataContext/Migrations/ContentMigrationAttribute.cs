using System;

namespace BetterCms.Core.DataAccess.DataContext.Migrations
{
    /// <summary>
    /// Attribute for content migration.
    /// </summary>
    public class ContentMigrationAttribute : Attribute
    {
        public long Version { get; private set; }

        public ContentMigrationAttribute(long version)
        {
            Version = version;
        }
    }
}
