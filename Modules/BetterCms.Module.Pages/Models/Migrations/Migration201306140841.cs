using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Migration for IsInSitemap.
    /// </summary>
    [Migration(201306140841)]
    public class Migration201306140841: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201306140841"/> class.
        /// </summary>
        public Migration201306140841()
            : base(PagesModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Migrate up.
        /// </summary>
        public override void Up()
        {
            Alter.Table("Pages").InSchema(SchemaName)
                .AddColumn("SecondaryImageId").AsGuid().Nullable()
                .AddColumn("FeaturedImageId").AsGuid().Nullable();
        }
    }
}