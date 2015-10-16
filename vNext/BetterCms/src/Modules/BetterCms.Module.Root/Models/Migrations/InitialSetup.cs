using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Root.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201301151829)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(RootModuleDescriptor.ModuleName)
        {            
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
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

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the modules table.
        /// </summary>
        private void CreateModulesTable()
        {
            Create
                .Table("Modules").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Description").AsString(MaxLength.Text).NotNullable()
                .WithColumn("ModuleVersion").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Enabled").AsBoolean().NotNullable();

            Create
                .UniqueConstraint("UX_Cms_Modules_Name")
                .OnTable("Modules").WithSchema(SchemaName)
                .Columns(new[] { "Name", "DeletedOn" });
        }

        /// <summary>
        /// Creates the layouts table.
        /// </summary>
        private void CreateLayoutsTable()
        {
            Create
                .Table("Layouts").InSchema(SchemaName)
                .WithBaseColumns()
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

        /// <summary>
        /// Creates the regions table.
        /// </summary>
        private void CreateRegionsTable()
        {
            Create
                .Table("Regions").InSchema(SchemaName)
                .WithBaseColumns()                
                .WithColumn("RegionIdentifier").AsAnsiString(MaxLength.Name).NotNullable();        
    
            Create
                .UniqueConstraint("UX_Cms_Regions_RegionIdentifier")
                .OnTable("Regions").WithSchema(SchemaName)
                .Columns(new[] { "RegionIdentifier", "DeletedOn" });
        }

        /// <summary>
        /// Creates the layout regions table.
        /// </summary>
        private void CreateLayoutRegionsTable()
        {
            Create
                .Table("LayoutRegions").InSchema(SchemaName)
                .WithBaseColumns()
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

        /// <summary>
        /// Creates the contents table.
        /// </summary>
        private void CreateContentsTable()
        {
            Create
                .Table("Contents").InSchema(SchemaName)
                .WithBaseColumns()
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

        /// <summary>
        /// Creates the content options table.
        /// </summary>
        private void CreateContentOptionsTable()
        {
            Create
                .Table("ContentOptions")
                .InSchema(SchemaName)
                .WithBaseColumns()
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

        /// <summary>
        /// Creates the content options types table.
        /// </summary>
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

        /// <summary>
        /// Creates the widgets table.
        /// </summary>
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

        /// <summary>
        /// Creates the users table.
        /// </summary>
        private void CreateUsersTable()
        {
            Create
                .Table("Users").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("UserName").AsString(MaxLength.Name).NotNullable().Unique()
                .WithColumn("Email").AsString(MaxLength.Email).Nullable()
                .WithColumn("DisplayName").AsString(MaxLength.Name).Nullable();
        }

        /// <summary>
        /// Creates the tags table.
        /// </summary>
        private void CreateTagsTable()
        {
            Create
                .Table("Tags").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

            Create
                .UniqueConstraint("UX_Cms_Tags_Name")
                .OnTable("Tags").WithSchema(SchemaName)
                .Columns(new[] { "Name", "DeletedOn" });
        }

        /// <summary>
        /// Creates the categories table.
        /// </summary>
        private void CreateCategoriesTable()
        {
            Create
                .Table("Categories").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();
                
            Create
                .UniqueConstraint("UX_Cms_Categories_Name")
                .OnTable("Categories").WithSchema(SchemaName)
                .Columns(new[] { "Name", "DeletedOn" });
        }

        /// <summary>
        /// Creates the pages table.
        /// </summary>
        private void CreatePagesTable()
        {
            Create
                .Table("Pages").InSchema(SchemaName)
                .WithBaseColumns()

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

        /// <summary>
        /// Creates the page contents table.
        /// </summary>
        private void CreatePageContentsTable()
        {
            Create
                .Table("PageContents").InSchema(SchemaName)
                .WithBaseColumns()
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

        /// <summary>
        /// Creates the page content options table.
        /// </summary>
        private void CreatePageContentOptionsTable()
        {
            Create
                .Table("PageContentOptions")
                .InSchema(SchemaName)
                .WithBaseColumns()
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

        /// <summary>
        /// Creates the content statuses table.
        /// </summary>
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
    }
}