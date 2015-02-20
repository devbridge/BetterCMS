using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module database structure update.
    /// </summary>
    [Migration(201307220940)]
    public class Migration201307220940: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201307220940"/> class.
        /// </summary>
        public Migration201307220940()
            : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Update
                .Table("Pages")
                .InSchema(SchemaName)
                .Set(new { UseCanonicalUrl = true })
                .AllRows();

            Alter
                .Table("Pages")
                .InSchema(SchemaName)
                .AlterColumn("UseCanonicalUrl")
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(true);
        }
    }
}