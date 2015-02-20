using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201303081430)]
    public class Migration201303081430: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201303081430"/> class.
        /// </summary>
        public Migration201303081430()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            // Set IsUploaded as nullable
            Alter
                .Column("IsUploaded")
                .OnTable("MediaFiles").InSchema(SchemaName)
                .AsBoolean().Nullable();

            // Set IsOriginalUploaded as nullable
            Alter
                .Column("IsOriginalUploaded")
                .OnTable("MediaImages").InSchema(SchemaName)
                .AsBoolean().Nullable();

            // Set IsThumbnailUploaded as nullable
            Alter
                .Column("IsThumbnailUploaded")
                .OnTable("MediaImages").InSchema(SchemaName)
                .AsBoolean().Nullable();
        }
    }
}