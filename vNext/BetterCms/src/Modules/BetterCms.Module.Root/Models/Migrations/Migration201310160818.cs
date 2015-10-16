using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201310160818)]
    public class Migration201310160818: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201310160818"/> class.
        /// </summary>
        public Migration201310160818()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Delete
                .UniqueConstraint("UX_Cms_Pages_PageUrl")
                .FromTable("Pages").InSchema(SchemaName);

            Delete
                .Index("IX_Cms_Pages_PageUrl")
                .OnTable("Pages").InSchema(SchemaName);

            Alter
                .Table("Pages").InSchema(SchemaName)
                .AlterColumn("PageUrl").AsString(MaxLength.Url).NotNullable();

            Alter
                .Table("Pages").InSchema(SchemaName)
                .AlterColumn("PageUrlLowerTrimmed").AsString(MaxLength.Url).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_Pages_PageUrl")
                .OnTable("Pages").WithSchema(SchemaName)
                .Columns(new[] { "PageUrl", "DeletedOn" });

            Create
                .Index("IX_Cms_Pages_PageUrl")
                .OnTable("Pages").InSchema(SchemaName)
                .OnColumn("PageUrl").Ascending();

            Create
                .UniqueConstraint("UX_Cms_Pages_PageUrlLowerTrimmed")
                .OnTable("Pages").WithSchema(SchemaName)
                .Columns(new[] { "PageUrlLowerTrimmed", "DeletedOn" });

            Create
                .Index("IX_Cms_Pages_PageUrlLowerTrimmed")
                .OnTable("Pages").InSchema(SchemaName)
                .OnColumn("PageUrlLowerTrimmed").Ascending();
        }       
    }
}