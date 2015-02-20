using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201306171041)]
    public class Migration201306171041: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201306171041"/> class.
        /// </summary>
        public Migration201306171041()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Column("OriginalId")
                .OnTable("Medias").InSchema(SchemaName)
                .AsGuid().Nullable();
        }
    }
}