using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    /// <summary>
    /// Migration for IsInSitemap.
    /// </summary>
    [Migration(201306271601)]
    public class Migration201306271601: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201306271601"/> class.
        /// </summary>
        public Migration201306271601()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrate up.
        /// </summary>
        public override void Up()
        {
            Alter.Table("Medias").InSchema(SchemaName)
                .AddColumn("ImageId").AsGuid().Nullable();
        }
    }
}