using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201306111530)]
    public class Migration201306111530: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201306111530"/> class.
        /// </summary>
        public Migration201306111530()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Column("IsArchived")
                .OnTable("Medias").InSchema(SchemaName)
                .AsBoolean()
                .WithDefaultValue(false);
        }
    }
}