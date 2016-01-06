using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201308021130)]
    public class Migration201308021130 : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201308021130" /> class.
        /// </summary>
        public Migration201308021130()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            Rename
                .Table("ContentOptionTypes").InSchema(SchemaName)
                .To("OptionTypes").InSchema(SchemaName);

            // Create new foreign keys, remove old ones
            Create
                .ForeignKey("FK_Cms_ContentOptions_Type_Cms_OptionTypes_Id")
                .FromTable("ContentOptions").InSchema(SchemaName)
                .ForeignColumn("Type")
                .ToTable("OptionTypes").InSchema(SchemaName)
                .PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageContentOptions_Type_Cms_OptionTypes_Id")
                .FromTable("PageContentOptions").InSchema(SchemaName)
                .ForeignColumn("Type")
                .ToTable("OptionTypes").InSchema(SchemaName)
                .PrimaryColumn("Id");

            Delete
                .ForeignKey("FK_Cms_ContentOptions_Type_Cms_ContentOptionTypes_Id")
                .OnTable("ContentOptions").InSchema(SchemaName);
            
            Delete
                .ForeignKey("FK_Cms_PageContentOptions_Type_Cms_ContentOptionTypes_Id")
                .OnTable("PageContentOptions").InSchema(SchemaName);
        }
    }
}