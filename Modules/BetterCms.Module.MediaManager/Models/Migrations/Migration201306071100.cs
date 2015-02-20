using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201306071100)]
    public class Migration201306071100: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201306071100"/> class.
        /// </summary>
        public Migration201306071100()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Column("ContentType")
                .OnTable("Medias").InSchema(SchemaName)
                .AsInt32().NotNullable()
                .WithDefaultValue(1);
        }
    }
}