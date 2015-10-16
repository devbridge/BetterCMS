using BetterModules.Core.DataAccess.DataContext.Migrations;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201310241500)]
    public class Migration201310241500: DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201310241500"/> class.
        /// </summary>
        public Migration201310241500()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            // Set layout id as nullable
            Alter
                .Column("LayoutId")
                .OnTable("Pages").InSchema(SchemaName)
                .AsGuid().Nullable();

            // Create nullable master page id
            Create
                .Column("MasterPageId")
                .OnTable("Pages").InSchema(SchemaName)
                .AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_Pages_Cms_Pages")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("MasterPageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            // Create content regions table
            Create
                .Table("ContentRegions").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("RegionId").AsGuid().NotNullable()
                .WithColumn("ContentId").AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_ContentRegions_Cms_Contents")
                .FromTable("ContentRegions").InSchema(SchemaName).ForeignColumn("ContentId")
                .ToTable("Contents").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_ContentRegions_Cms_Regions")
                .FromTable("ContentRegions").InSchema(SchemaName).ForeignColumn("RegionId")
                .ToTable("Regions").InSchema(SchemaName).PrimaryColumn("Id");

            // Create flag, indicating, that page is master page
            Create
                .Column("IsMasterPage")
                .OnTable("Pages").InSchema(SchemaName)
                .AsBoolean().NotNullable().WithDefaultValue(false);

            // Create master pages table
            Create
                .Table("MasterPages").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("PageId").AsGuid().NotNullable()
                .WithColumn("MasterPageId").AsGuid().NotNullable();

            Create
                .UniqueConstraint("UX_Cms_MasterPages_PageId_MasterPageId")
                .OnTable("MasterPages").WithSchema(SchemaName)
                .Columns(new[] { "PageId", "MasterPageId", "DeletedOn" });

            Create
                .ForeignKey("FK_Cms_MasterPages_PageId_Cms_Pages")
                .FromTable("MasterPages").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_MasterPages_MasterPageId_Cms_Pages")
                .FromTable("MasterPages").InSchema(SchemaName).ForeignColumn("MasterPageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");
        }       
    }
}