using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    [Migration(201301151829)]
    public class InitialSetup : DefaultMigration
    {
        public InitialSetup()
            : base(RootModuleDescriptor.ModuleName)
        {
        }

        public override void Up()
        {
            CreateModulesTable();

            CreateLayoutsTable();            
            CreateRegionsTable();
            CreateLayoutRegionsTable();

            CreateTagsTable();
            CreateCategoriesTable();

            CreateContentStatusesTable();   
            CreateContentsTable();
            CreateContentOptionsTypesTable();
            CreateContentOptionsTable();
            CreateWidgetsTable();

            CreatePagesTable();
            CreatePageContentsTable();
            CreatePageContentOptionsTable();
                        
            CreateUsersTable();                     
        }

        public override void Down()
        {                        
            RemoveUsersTable();
                                    
            RemovePageContentOptionsTable();
            RemovePageContentsTable();
            RemovePagesTable();

            RemoveWidgetsTable();
            RemoveContentOptionsTable();
            RemoveContentOptionsTypesTable();
            RemoveContentsTable();
            RemoveContentStatusesTable();

            RemoveLayoutRegionsTable();
            RemoveLayoutsTable();
            RemoveRegionsTable();            

            RemoveCategoriesTable();
            RemoveTagsTable();
            
            RemoveModulesTable();
        }

        private void CreateModulesTable()
        {
            Create
                .Table("Modules").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Description").AsString(MaxLength.Text).NotNullable()
                .WithColumn("ModuleVersion").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Enabled").AsBoolean().NotNullable();

            Create
                .UniqueConstraint("UX_Cms_Modules_Name")
                .OnTable("Modules").WithSchema(SchemaName)
                .Columns(new[] { "Name", "DeletedOn" });
        }

        private void RemoveModulesTable()
        {
            Delete.UniqueConstraint("UX_Cms_Modules_Name").FromTable("Modules").InSchema(SchemaName);
            Delete.Table("Modules").InSchema(SchemaName);            
        }

        private void CreateLayoutsTable()
        {
            Create
                .Table("Layouts").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
                .WithColumn("LayoutPath").AsAnsiString(MaxLength.Url).NotNullable()
                .WithColumn("ModuleId").AsGuid().Nullable()
                .WithColumn("PreviewUrl").AsAnsiString(MaxLength.Url).Nullable();

            Create
                .ForeignKey("FK_Cms_Layouts_Cms_Modules")
                .FromTable("Layouts").InSchema(SchemaName).ForeignColumn("ModuleId")
                .ToTable("Modules").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_Layouts_ModuleId")
                .OnTable("Layouts").InSchema(SchemaName).OnColumn("ModuleId");
        }

        private void RemoveLayoutsTable()
        {
            Delete.ForeignKey("FK_Cms_Layouts_Cms_Modules").OnTable("Layouts").InSchema(SchemaName);
            Delete.Table("Layouts").InSchema(SchemaName);
        }

        private void CreateRegionsTable()
        {
            Create
                .Table("Regions").InSchema(SchemaName)
                .WithCmsBaseColumns()                
                .WithColumn("RegionIdentifier").AsAnsiString(MaxLength.Name).NotNullable();        
    
            Create
                .UniqueConstraint("UX_Cms_Regions_RegionIdentifier")
                .OnTable("Regions").WithSchema(SchemaName)
                .Columns(new[] { "RegionIdentifier", "DeletedOn" });
        }

        private void RemoveRegionsTable()
        {
            Delete.UniqueConstraint("UX_Cms_Regions_RegionIdentifier").FromTable("Regions").InSchema(SchemaName);
            Delete.Table("Regions").InSchema(SchemaName);
        }

        private void CreateLayoutRegionsTable()
        {
            Create
                .Table("LayoutRegions").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Description").AsAnsiString(MaxLength.Name).Nullable()
                .WithColumn("LayoutId").AsGuid().NotNullable()
                .WithColumn("RegionId").AsGuid().NotNullable();

            Create
                .UniqueConstraint("UX_Cms_LayoutRegions_LayoutId_RegionId")
                .OnTable("LayoutRegions").WithSchema(SchemaName)
                .Columns(new[] { "LayoutId", "RegionId", "DeletedOn" });

            Create
                .ForeignKey("FK_Cms_LayoutRegions_Cms_Layouts")
                .FromTable("LayoutRegions").InSchema(SchemaName).ForeignColumn("LayoutId")
                .ToTable("Layouts").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_LayoutRegions_Cms_Regions")
                .FromTable("LayoutRegions").InSchema(SchemaName).ForeignColumn("RegionId")
                .ToTable("Regions").InSchema(SchemaName).PrimaryColumn("Id");
        }

        private void RemoveLayoutRegionsTable()
        {
            Delete.UniqueConstraint("UX_Cms_LayoutRegions_LayoutId_RegionId").FromTable("LayoutRegions").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_LayoutRegions_Cms_Layouts").OnTable("LayoutRegions").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_LayoutRegions_Cms_Regions").OnTable("LayoutRegions").InSchema(SchemaName);
            Delete.Table("LayoutRegions").InSchema(SchemaName);
        }

        private void CreateContentsTable()
        {
            Create
                .Table("Contents").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
                .WithColumn("PreviewUrl").AsAnsiString(MaxLength.Url).Nullable()
                .WithColumn("Status").AsInt32().NotNullable()
                .WithColumn("PublishedOn").AsDateTime().Nullable()
                .WithColumn("PublishedByUser").AsString(MaxLength.Name).Nullable()
                .WithColumn("OriginalId").AsGuid().Nullable();

            Create
                .Index("IX_Cms_Contents_Name")
                .OnTable("Contents").InSchema(SchemaName)
                .OnColumn("Name");

            Create
                .Index("IX_Cms_Contents_Status")
                .OnTable("Contents").InSchema(SchemaName)
                .OnColumn("Status");

            Create
                .Index("IX_Cms_Contents_OriginalId")
                .OnTable("Contents").InSchema(SchemaName)
                .OnColumn("OriginalId");

            Create
                .ForeignKey("FK_Cms_Contents_Cms_ContentStatuses")
                .FromTable("Contents").InSchema(SchemaName).ForeignColumn("Status")
                .ToTable("ContentStatuses").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_Contents_Cms_Contents_OriginalId")
                .FromTable("Contents").InSchema(SchemaName).ForeignColumn("OriginalId")
                .ToTable("Contents").InSchema(SchemaName).PrimaryColumn("Id");
        }

        private void RemoveContentsTable()
        {
            Delete.ForeignKey("FK_Cms_Contents_Cms_Contents_OriginalId").OnTable("Contents").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_Contents_Cms_ContentStatuses").OnTable("Contents").InSchema(SchemaName);
            Delete.Index("IX_Cms_Contents_OriginalId").OnTable("Contents").InSchema(SchemaName);
            Delete.Index("IX_Cms_Contents_Status").OnTable("Contents").InSchema(SchemaName);
            Delete.Index("IX_Cms_Contents_Name").OnTable("Contents").InSchema(SchemaName);
            Delete.Table("Contents").InSchema(SchemaName);
        }

        private void CreateContentOptionsTable()
        {
            Create
                .Table("ContentOptions")
                .InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("ContentId").AsGuid().NotNullable()
                .WithColumn("Key").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("DefaultValue").AsString(MaxLength.Max).Nullable();

            Create
                .ForeignKey("FK_Cms_ContentOptions_Type_Cms_ContentOptionTypes_Id")
                .FromTable("ContentOptions").InSchema(SchemaName).ForeignColumn("Type")
                .ToTable("ContentOptionTypes").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_ContentOptions_ContentId_Cms_Contents_Id")
                .FromTable("ContentOptions").InSchema(SchemaName).ForeignColumn("ContentId")
                .ToTable("Contents").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .UniqueConstraint("UX_Cms_ContentOptions_ContentId_Key")
                .OnTable("ContentOptions").WithSchema(SchemaName)
                .Columns(new[] { "ContentId", "Key", "DeletedOn" });
        }

        private void RemoveContentOptionsTable()
        {
            Delete.UniqueConstraint("UX_Cms_ContentOptions_ContentId_Key").FromTable("ContentOptions").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_ContentOptions_ContentId_Cms_Contents_Id").OnTable("ContentOptions").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_ContentOptions_Type_Cms_ContentOptionTypes_Id").OnTable("ContentOptions").InSchema(SchemaName);
            Delete.Table("ContentOptions").InSchema(SchemaName);
        }

        private void CreateContentOptionsTypesTable()
        {
            Create
                .Table("ContentOptionTypes")
                .InSchema(SchemaName)
                .WithColumn("Id").AsInt32().PrimaryKey()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_ContentOptionTypes_Name")
                .OnTable("ContentOptionTypes").WithSchema(SchemaName)
                .Column("Name");

            Insert
                .IntoTable("ContentOptionTypes")
                .InSchema(SchemaName)
                .Row(new
                        {
                            Id = 1,
                            Name = "Text"
                        });
        }

        private void RemoveContentOptionsTypesTable()
        {
            Delete.UniqueConstraint("UX_Cms_ContentOptionTypes_Name").FromTable("ContentOptionTypes").InSchema(SchemaName);
            Delete.Table("ContentOptionTypes").InSchema(SchemaName);
        }

        private void CreateWidgetsTable()
        {
            Create
                .Table("Widgets")
                .InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("CategoryId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_Widgets_Id_Content_Id")
                .FromTable("Widgets").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Contents").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_Widgets_Id_Category_Id")
                .FromTable("Widgets").InSchema(SchemaName).ForeignColumn("CategoryId")
                .ToTable("Categories").InSchema(SchemaName).PrimaryColumn("Id");
        }

        private void RemoveWidgetsTable()
        {
            Delete.ForeignKey("FK_Cms_Widgets_Id_Category_Id").OnTable("Widgets").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_Widgets_Id_Content_Id").OnTable("Widgets").InSchema(SchemaName);
            Delete.Table("Widgets").InSchema(SchemaName);
        }

        private void CreateUsersTable()
        {
            Create
                .Table("Users").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("UserName").AsString(MaxLength.Name).NotNullable().Unique()
                .WithColumn("Email").AsString(MaxLength.Email).Nullable()
                .WithColumn("DisplayName").AsString(MaxLength.Name).Nullable();
        }

        private void RemoveUsersTable()
        {
            Delete.Table("Users").InSchema(SchemaName);
        }

        private void CreateTagsTable()
        {
            Create
                .Table("Tags").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_Tags_Name")
                .OnTable("Tags").WithSchema(SchemaName)
                .Columns(new[] { "Name", "DeletedOn" });
        }

        private void RemoveTagsTable()
        {
            Delete.UniqueConstraint("UX_Cms_Tags_Name").FromTable("Tags").InSchema(SchemaName);
            Delete.Table("Tags").InSchema(SchemaName);
        }

        private void CreateCategoriesTable()
        {
            Create
                .Table("Categories").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();
                

            Create
                .UniqueConstraint("UX_Cms_Categories_Name")
                .OnTable("Categories").WithSchema(SchemaName)
                .Columns(new[] { "Name", "DeletedOn" });
        }

        private void RemoveCategoriesTable()
        {
            Delete.UniqueConstraint("UX_Cms_Categories_Name").FromTable("Categories").InSchema(SchemaName);
            Delete.Table("Categories").InSchema(SchemaName);
        }
        
        private void CreatePagesTable()
        {
            Create
                .Table("Pages").InSchema(SchemaName)
                .WithCmsBaseColumns()

                .WithColumn("PageUrl").AsAnsiString(MaxLength.Url).NotNullable()
                .WithColumn("Title").AsString(MaxLength.Name).NotNullable()
                .WithColumn("LayoutId").AsGuid().NotNullable()
                .WithColumn("PublishedOn").AsDateTime().Nullable()
                .WithColumn("IsPublished").AsBoolean().NotNullable().WithDefaultValue(false)                

                .WithColumn("MetaTitle").AsString(MaxLength.Name).Nullable()
                .WithColumn("MetaKeywords").AsString(MaxLength.Max).Nullable()
                .WithColumn("MetaDescription").AsString(MaxLength.Max).Nullable();

            Create
                .UniqueConstraint("UX_Cms_Pages_PageUrl")
                .OnTable("Pages").WithSchema(SchemaName)
                .Columns(new[] { "PageUrl", "DeletedOn" });

            Create
                .Index("IX_Cms_Pages_PageUrl")
                .OnTable("Pages").InSchema(SchemaName)
                .OnColumn("PageUrl").Ascending();

            Create
                .Index("IX_Cms_Pages_LayoutId")
                .OnTable("Pages").InSchema(SchemaName)
                .OnColumn("LayoutId").Ascending();

            Create
                .ForeignKey("FK_Cms_Pages_Cms_Layouts")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("LayoutId")
                .ToTable("Layouts").InSchema(SchemaName).PrimaryColumn("Id");
        }

        private void RemovePagesTable()
        {
            Delete.Index("IX_Cms_Pages_PageUrl").OnTable("Pages").InSchema(SchemaName);
            Delete.Index("IX_Cms_Pages_LayoutId").OnTable("Pages").InSchema(SchemaName);
            Delete.UniqueConstraint("UX_Cms_Pages_PageUrl").FromTable("Pages").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_Pages_Cms_Layouts").OnTable("Pages").InSchema(SchemaName);
            Delete.Table("Pages").InSchema(SchemaName);
        }

        private void CreatePageContentsTable()
        {
            Create
                .Table("PageContents").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("PageId").AsGuid().NotNullable()
                .WithColumn("ContentId").AsGuid().NotNullable()
                .WithColumn("RegionId").AsGuid().NotNullable()
                .WithColumn("Order").AsInt32().NotNullable().WithDefaultValue(0);

            Create
                .ForeignKey("FK_Cms_PageContents_PageId_Pages_Id")
                .FromTable("PageContents").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageContents_ContentId_Contents_Id")
                .FromTable("PageContents").InSchema(SchemaName).ForeignColumn("ContentId")
                .ToTable("Contents").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageContents_RegionId_Regions_Id")
                .FromTable("PageContents").InSchema(SchemaName).ForeignColumn("RegionId")
                .ToTable("Regions").InSchema(SchemaName).PrimaryColumn("Id");            
        }

        private void RemovePageContentsTable()
        {            
            Delete.ForeignKey("FK_Cms_PageContents_PageId_Pages_Id").OnTable("PageContents").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_PageContents_ContentId_Contents_Id").OnTable("PageContents").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_PageContents_RegionId_Regions_Id").OnTable("PageContents").InSchema(SchemaName);
            Delete.Table("PageContents").InSchema(SchemaName);
        }

        private void CreatePageContentOptionsTable()
        {
            Create
                .Table("PageContentOptions")
                .InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("PageContentId").AsGuid().NotNullable()
                .WithColumn("Value").AsString(MaxLength.Max).Nullable()
                .WithColumn("Key").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Type").AsInt32().NotNullable();
                
            Create
                .ForeignKey("FK_Cms_PageContentOptions_PageContentId_Cms_PageContents_Id")
                .FromTable("PageContentOptions").InSchema(SchemaName).ForeignColumn("PageContentId")
                .ToTable("PageContents").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageContentOptions_Type_Cms_ContentOptionTypes_Id")
                .FromTable("PageContentOptions").InSchema(SchemaName).ForeignColumn("Type")
                .ToTable("ContentOptionTypes").InSchema(SchemaName).PrimaryColumn("Id");
            
            Create
                .UniqueConstraint("UX_Cms_PageContentOptions_PageContentId_Key")
                .OnTable("PageContentOptions").WithSchema(SchemaName)
                .Columns(new[] { "PageContentId", "Key", "DeletedOn" });
        }

        private void RemovePageContentOptionsTable()
        {
            Delete.ForeignKey("FK_Cms_PageContentOptions_PageContentId_Cms_PageContents_Id").OnTable("PageContentOptions").InSchema(SchemaName);
            Delete.ForeignKey("FK_Cms_PageContentOptions_Type_Cms_ContentOptionTypes_Id").OnTable("PageContentOptions").InSchema(SchemaName);
            Delete.UniqueConstraint("UX_Cms_PageContentOptions_PageContentId_Key").FromTable("PageContentOptions").InSchema(SchemaName);
            Delete.Table("PageContentOptions").InSchema(SchemaName);
        }
        
        private void CreateContentStatusesTable()
        {
            Create
                .Table("ContentStatuses")
                .InSchema(SchemaName)
                .WithColumn("Id").AsInt32().PrimaryKey()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_ContentStatuses_Name")
                .OnTable("ContentStatuses").WithSchema(SchemaName)
                .Column("Name");

            Insert
                .IntoTable("ContentStatuses")
                .InSchema(SchemaName)
                .Row(new
                {
                    Id = 1,
                    Name = "Preview"
                })
                .Row(new
                {
                    Id = 2,
                    Name = "Draft"
                })
                .Row(new
                {
                    Id = 3,
                    Name = "Published"
                })
                .Row(new
                {
                    Id = 4,
                    Name = "Archived"
                });
        }

        private void RemoveContentStatusesTable()
        {            
            Delete.UniqueConstraint("UX_Cms_ContentStatuses_Name").FromTable("ContentStatuses").InSchema(SchemaName);
            Delete.Table("ContentStatuses").InSchema(SchemaName);
        } 
    }
}