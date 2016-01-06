using BetterCms.Module.Root.Models.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    [Migration(201502031000)]
    public class Migration201502031000 : DefaultMigration
    {
        private readonly string rootModuleSchemaName;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="201502031000"/> class.
        /// </summary>
        public Migration201502031000()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            Create
                .Table("MediaCategories")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("MediaId").AsGuid().NotNullable()
                .WithColumn("CategoryId").AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_MediaCategories_MediaId_Cms_Mdedia_Id")
                .FromTable("MediaCategories").InSchema(SchemaName).ForeignColumn("MediaId")
                .ToTable("Medias").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_MediaCategories_CategoryId_Cms_Category_Id")
                .FromTable("MediaCategories").InSchema(SchemaName).ForeignColumn("CategoryId")
                .ToTable("Categories").InSchema(rootModuleSchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_MediaCategories_MediaId")
                .OnTable("MediaCategories").InSchema(SchemaName).OnColumn("MediaId");

            Create
                .Index("IX_Cms_MediaCategories_AccessRuleId")
                .OnTable("MediaCategories").InSchema(SchemaName).OnColumn("CategoryId");
        }        
    }
}