using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201306260946)]
    public class Migration201306260946: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201306260946"/> class.
        /// </summary>
        public Migration201306260946()
            : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Column("IsArchived")
                .OnTable("Pages").InSchema(SchemaName)
                .AsBoolean()
                .WithDefaultValue(false);
        }
    }
}