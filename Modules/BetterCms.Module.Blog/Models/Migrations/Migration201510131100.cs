using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Blog.Models.Migrations
{
    [Migration(201510131100)]
    public class Migration201510131100: DefaultMigration
    {
        /// <summary>
        /// The pages module schema name.
        /// </summary>
        private readonly string pagesModuleSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201510131100"/> class.
        /// </summary>
        public Migration201510131100()
            : base(BlogModuleDescriptor.ModuleName)
        {
            pagesModuleSchemaName = (new Pages.Models.Migrations.PagesVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Column("DefaultContentTextMode")
                .OnTable("Options")
                .InSchema(SchemaName)
                .AsInt32().NotNullable().WithDefaultValue("1");

            Create
                .ForeignKey("FK_Cms_Blog_Options_DefaultContentTextMode_Pages_ContentTextModes_Id")
                .FromTable("Options").InSchema(SchemaName).ForeignColumn("DefaultContentTextMode")
                .ToTable("ContentTextModes").InSchema(pagesModuleSchemaName).PrimaryColumn("Id");
        }
    }
}