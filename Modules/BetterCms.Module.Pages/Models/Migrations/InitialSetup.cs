using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.Pages.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201301151849)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string rootModuleSchemaName;

        /// <summary>
        /// The media manager schema name.
        /// </summary>
        private readonly string mediaManagerSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(PagesModuleDescriptor.ModuleName)
        {
            rootModuleSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
            mediaManagerSchemaName = (new MediaManager.Models.Migrations.MediaManagerVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateRedirectsTable();
                
            CreateHtmlContentsTable();
            CreateServerControlWidgetsTable();
            CreateHtmlContentWidgetsTable();

            CreatePagesTable();

            CreatePageTagsTable();
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the redirects table.
        /// </summary>
        private void CreateRedirectsTable()
        {
            Create
                .Table("Redirects")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("PageUrl").AsString(MaxLength.Url).NotNullable()
                .WithColumn("RedirectUrl").AsString(MaxLength.Url).NotNullable();                
        }

        /// <summary>
        /// Creates the HTML contents table.
        /// </summary>
        private void CreateHtmlContentsTable()
        {
            Create
               .Table("HtmlContents")
               .InSchema(SchemaName)
               .WithColumn("Id").AsGuid().PrimaryKey()
               .WithColumn("ActivationDate").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime)
               .WithColumn("ExpirationDate").AsDateTime().Nullable()
               .WithColumn("CustomCss").AsString(MaxLength.Max).Nullable()
               .WithColumn("UseCustomCss").AsBoolean().NotNullable().WithDefaultValue(false)
               .WithColumn("CustomJs").AsString(MaxLength.Max).Nullable()
               .WithColumn("UseCustomJs").AsBoolean().NotNullable().WithDefaultValue(false)
               .WithColumn("Html").AsString(int.MaxValue).NotNullable();               
               
            Create
                .ForeignKey("FK_Cms_HtmlContents_Id_Contents_Id")
                .FromTable("HtmlContents").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Contents").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
        }

        /// <summary>
        /// Creates the server control widgets table.
        /// </summary>
        private void CreateServerControlWidgetsTable()
        {
            Create
                .Table("ServerControlWidgets")
                .InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey()                
                .WithColumn("Url").AsAnsiString(MaxLength.Url).NotNullable();

            Create
                .ForeignKey("FK_Cms_ServerControlWidgets_Id_Widgets_Id")
                .FromTable("ServerControlWidgets").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Widgets").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
        }

        /// <summary>
        /// Creates the HTML content widgets table.
        /// </summary>
        private void CreateHtmlContentWidgetsTable()
        {
            Create
                .Table("HtmlContentWidgets")
                .InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("CustomCss").AsString(MaxLength.Max).Nullable()
                .WithColumn("UseCustomCss").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("CustomJs").AsString(MaxLength.Max).Nullable()
                .WithColumn("UseCustomJs").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("Html").AsString(MaxLength.Max).NotNullable()
                .WithColumn("UseHtml").AsBoolean().NotNullable().WithDefaultValue(false);

            Create
                .ForeignKey("FK_Cms_HtmlContentWidgets_Id_Widgets_Id")
                .FromTable("HtmlContentWidgets").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Widgets").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
        }

        /// <summary>
        /// Creates the pages table.
        /// </summary>
        private void CreatePagesTable()
        {
            Create
                .Table("Pages")
                .InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Description").AsString(MaxLength.Text).Nullable()
                .WithColumn("ImageId").AsGuid().Nullable()
                .WithColumn("CanonicalUrl").AsAnsiString(MaxLength.Url).Nullable()
                .WithColumn("CustomCss").AsString(MaxLength.Max).Nullable()
                .WithColumn("CustomJS").AsString(MaxLength.Max).Nullable()
                .WithColumn("UseCanonicalUrl").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("IsPublic").AsBoolean().NotNullable().WithDefaultValue(true)                
                .WithColumn("UseNoFollow").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("UseNoIndex").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("PublishedOn").AsDateTime().Nullable()                
                .WithColumn("CategoryId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_PagesPages_Cms_RootPages")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Pages").InSchema(rootModuleSchemaName).PrimaryColumn("Id");            
                
            Create.ForeignKey("FK_Cms_Pages_CategoryId_Categories_Id")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("CategoryId")
                .ToTable("Categories").InSchema(rootModuleSchemaName).PrimaryColumn("Id");

            Create.ForeignKey("FK_Cms_Pages_ImageId_MediaImages_Id")
                .FromTable("Pages").InSchema(SchemaName).ForeignColumn("ImageId")
                .ToTable("MediaImages").InSchema(mediaManagerSchemaName).PrimaryColumn("Id");
        }

        /// <summary>
        /// Creates the page tags table.
        /// </summary>
        private void CreatePageTagsTable()
        {
            Create
                .Table("PageTags")
                .InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("PageId").AsGuid().NotNullable()
                .WithColumn("TagId").AsGuid().NotNullable();

            Create
                .ForeignKey("FK_Cms_PageTags_Cms_Pages")
                .FromTable("PageTags").InSchema(SchemaName).ForeignColumn("PageId")
                .ToTable("Pages").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_PageTags_Cms_Tags")
                .FromTable("PageTags").InSchema(SchemaName).ForeignColumn("TagId")
                .ToTable("Tags").InSchema(rootModuleSchemaName).PrimaryColumn("Id");
        }
    }
}