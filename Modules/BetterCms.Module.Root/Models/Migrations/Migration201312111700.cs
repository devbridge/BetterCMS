using BetterCms.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201312111700)]
    public class Migration201312111700 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201312111700"/> class.
        /// </summary>
        public Migration201312111700()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Create
                .Column("CultureId")
                .OnTable("Pages").InSchema(SchemaName)
                .AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_Pages_Cms_Cultures")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("CultureId")
                .ToTable("Cultures").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Column("MainCulturePageId")
                .OnTable("Pages").InSchema(SchemaName)
                .AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_Pages_MainCulturePageId_Cms_Pages_Id")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("MainCulturePageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");
        }       
    }
}