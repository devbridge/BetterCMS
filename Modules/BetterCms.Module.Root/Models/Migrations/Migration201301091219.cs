using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201301091219)]
    public class Migration201301091219 : DefaultMigration
    {
        public Migration201301091219()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            CreateContentHistoryTable();
            CreateContentOptionHistoryTable();
            CreateWidgetHistoryTable();

            CreatePageContentHistoryTable();
            CreatePageContentOptionHistoryTable();                        
        }

        public override void Down()
        {
            RemovePageContentOptionHistoryTable();
            RemovePageContentHistoryTable();

            RemoveWidgetHistoryTable();
            RemoveContentOptionHistoryTable();
            RemoveContentHistoryTable();
        }

        private void CreateContentHistoryTable()
        {
            Create
                .Table("ContentHistory").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
                .WithColumn("PreviewUrl").AsAnsiString(MaxLength.Url).Nullable();

            Create
                .Index("IX_Cms_Contents_Name")
                .OnTable("ContentHistory").InSchema(SchemaName)
                .OnColumn("Name");
        }

        private void RemoveContentHistoryTable()
        {
            Delete.Index("IX_Cms_Contents_Name").OnTable("ContentHistory").InSchema(SchemaName);
            Delete.Table("ContentHistory").InSchema(SchemaName);
        }

        private void CreateContentOptionHistoryTable()
        {
            Create
                .Table("ContentOptionHistory")
                .InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("ContentHistoryId").AsGuid().NotNullable()
                .WithColumn("Key").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("DefaultValue").AsString(MaxLength.Max).Nullable();

            Create
                .ForeignKey("FK_Cms_ContentOptionHistory_Type_Cms_ContentOptionTypes_Id")
                .FromTable("ContentOptionHistory").InSchema(SchemaName).ForeignColumn("Type")
                .ToTable("ContentOptionTypes").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_ContentOptionHistory_ContentHistoryId_Cms_ContentHistory_Id")
                .FromTable("ContentOptionHistory").InSchema(SchemaName).ForeignColumn("ContentHistoryId")
                .ToTable("ContentHistory").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .UniqueConstraint("UX_Cms_ContentOptionHistory_ContentId_Key")
                .OnTable("ContentOptionHistory").WithSchema(SchemaName)
                .Columns(new[] { "ContentHistoryId", "Key", "DeletedOn" });
        }

        private void RemoveContentOptionHistoryTable()
        {
            Delete.UniqueConstraint("UX_Cms_ContentOptionHistory_ContentId_Key").FromTable("ContentOptionHistory").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_ContentOptionHistory_ContentHistoryId_Cms_ContentHistory_Id").OnTable("ContentOptionHistory").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_ContentOptionHistory_Type_Cms_ContentOptionTypes_Id").OnTable("ContentOptionHistory").InSchema(SchemaName);
            Delete.Table("ContentOptionHistory").InSchema(SchemaName);
        }

        private void CreateWidgetHistoryTable()
        {
            Create
                .Table("WidgetHistory")
                .InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("CategoryId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_WidgetHistory_Id_ContentHistory_Id")
                .FromTable("WidgetHistory").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("ContentHistory").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_WidgetHistory_Id_Category_Id")
                .FromTable("WidgetHistory").InSchema(SchemaName).ForeignColumn("CategoryId")
                .ToTable("Categories").InSchema(SchemaName).PrimaryColumn("Id");
        }

        private void RemoveWidgetHistoryTable()
        {
            Delete.ForeignKey("FK_Cms_WidgetHistory_Id_Category_Id").OnTable("WidgetHistory").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_WidgetHistory_Id_ContentHistory_Id").OnTable("WidgetHistory").InSchema(SchemaName);
            Delete.Table("WidgetHistory").InSchema(SchemaName);
        }
      
        private void CreatePageContentHistoryTable()
        {
            Create
                .Table("PageContentHistory").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("PageContentId").AsGuid().NotNullable()
                .WithColumn("PageId").AsGuid().NotNullable()
                .WithColumn("ContentHistoryId").AsGuid().NotNullable()
                .WithColumn("RegionId").AsGuid().NotNullable()
                .WithColumn("Order").AsInt32().NotNullable().WithDefaultValue(0);

            Create
                .ForeignKey("FK_Cms_PageContentHistory_PageContentId_PageContents_Id")
                .FromTable("PageContentHistory").InSchema(SchemaName).ForeignColumn("PageContentId")
                .ToTable("PageContents").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageContentHistory_PageId_Pages_Id")
                .FromTable("PageContentHistory").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageContentHistory_ContentHistoryId_ContentHistory_Id")
                .FromTable("PageContentHistory").InSchema(SchemaName).ForeignColumn("ContentHistoryId")
                .ToTable("ContentHistory").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageContentHistory_RegionId_Regions_Id")
                .FromTable("PageContentHistory").InSchema(SchemaName).ForeignColumn("RegionId")
                .ToTable("Regions").InSchema(SchemaName).PrimaryColumn("Id");            
        }

        private void RemovePageContentHistoryTable()
        {
            Delete.ForeignKey("FK_Cms_PageContentHistory_PageContentId_PageContents_Id").OnTable("PageContentHistory").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_PageContentHistory_PageId_Pages_Id").OnTable("PageContentHistory").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_PageContentHistory_ContentHistoryId_ContentHistory_Id").OnTable("PageContentHistory").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_PageContentHistory_RegionId_Regions_Id").OnTable("PageContentHistory").InSchema(SchemaName);
            Delete.Table("PageContentHistory").InSchema(SchemaName);
        }

        private void CreatePageContentOptionHistoryTable()
        {
            Create
                .Table("PageContentOptionHistory")
                .InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("PageContentHistoryId").AsGuid().NotNullable()
                .WithColumn("ContentOptionHistoryId").AsGuid().NotNullable()
                .WithColumn("Value").AsString(MaxLength.Max).Nullable();

            Create
                .ForeignKey("FK_Cms_PageContentOptionHistory_PageContentHistoryId_PageContentHistory_Id")
                .FromTable("PageContentOptionHistory").InSchema(SchemaName).ForeignColumn("PageContentHistoryId")
                .ToTable("PageContentHistory").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageContentOptionHistory_ContentOptionHistoryId_ContentOptionHistory_Id")
                .FromTable("PageContentOptionHistory").InSchema(SchemaName).ForeignColumn("ContentOptionHistoryId")
                .ToTable("ContentOptionHistory").InSchema(SchemaName).PrimaryColumn("Id");
        }

        private void RemovePageContentOptionHistoryTable()
        {
            Delete.ForeignKey("FK_Cms_PageContentOptionHistory_PageContentHistoryId_PageContentHistory_Id").OnTable("PageContentOptionHistory").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_PageContentOptionHistory_ContentOptionHistoryId_ContentOptionHistory_Id").OnTable("PageContentOptionHistory").InSchema(SchemaName);
            Delete.Table("PageContentOptionHistory").InSchema(SchemaName);
        }
    }
}