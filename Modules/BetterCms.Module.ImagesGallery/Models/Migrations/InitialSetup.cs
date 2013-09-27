using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.ImagesGallery.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201309271415)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string mediaManagerSchemaName;

        /// <summary>
        /// The root schema name
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// The pages schema name
        /// </summary>
        private readonly string pagesSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(ImagesGalleryModuleDescriptor.ModuleName)
        {
            mediaManagerSchemaName = (new MediaManager.Models.Migrations.MediaManagerVersionTableMetaData()).SchemaName;
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
            pagesSchemaName = (new Pages.Models.Migrations.PagesVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateAlbums();
            CreateCustomOption();
            CreateWidget();
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the albums table.
        /// </summary>
        private void CreateAlbums()
        {
            Create
                .Table("Albums").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Title").AsString(MaxLength.Name).NotNullable()
                .WithColumn("FolderId").AsGuid().NotNullable()
                .WithColumn("CoverImageId").AsGuid().Nullable();

            Create.ForeignKey("FK_ImagesGallery_Albums_FolderId_MediaFolders_Id")
                .FromTable("Albums").InSchema(SchemaName).ForeignColumn("FolderId")
                .ToTable("MediaFolders").InSchema(mediaManagerSchemaName).PrimaryColumn("Id");

            Create.ForeignKey("FK_ImagesGallery_Albums_CoverImageId_MediaImages_Id")
                .FromTable("Albums").InSchema(SchemaName).ForeignColumn("CoverImageId")
                .ToTable("MediaImages").InSchema(mediaManagerSchemaName).PrimaryColumn("Id");
        }

        /// <summary>
        /// Creates the custom option.
        /// </summary>
        private void CreateCustomOption()
        {
            Insert
                .IntoTable("CustomOptions").InSchema(rootSchemaName)
                .Row(new
                         {
                             Id = new Guid("9BCDA77D-C900-4AED-96D9-4FE8AD1F4138"),
                             Version = 1,
                             CreatedOn = DateTime.Now,
                             ModifiedOn = DateTime.Now,
                             CreatedByUser = "Admin",
                             ModifiedByUser = "Admin",
                             IsDeleted = 0,
                             Identifier = "images-gallery-album",
                             Title = "Images Gallery Album"
                         });
        }

        /// <summary>
        /// Creates the widget.
        /// </summary>
        private void CreateWidget()
        {
            var widget = new
            {

                ForRootSchemaContentTable = new
                {
                    Id = "8BD1E830-6C2D-4AF1-BD5C-A24400A83DBC",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    Name = "Images Gallery Widget",
                    Status = 3,
                    PublishedOn = DateTime.Now,
                    PublishedByUser = "Better CMS"
                },

                ForRootScemaWidgetsTable = new
                {
                    Id = "8BD1E830-6C2D-4AF1-BD5C-A24400A83DBC",
                },

                ForPagesSchemaServerControlWidgetsTable = new
                {
                    Id = "8BD1E830-6C2D-4AF1-BD5C-A24400A83DBC",
                    Url = "~/Areas/bcms-images-gallery/Views/Widgets/ImagesGalleryWidget.cshtml"
                }

            };

            var options = new
            {
                Option1 = new
                {
                    Id = "309EDA84-6307-4619-8602-A2460080DE27",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = widget.ForRootSchemaContentTable.Id,
                    Key = "LoadCmsStyles",
                    Type = 5, // Boolean
                    DefaultValue = "true",
                    IsDeletable = false
                }
            };

            // Register server control widget.
            Insert.IntoTable("Contents").InSchema(rootSchemaName).Row(widget.ForRootSchemaContentTable);
            Insert.IntoTable("Widgets").InSchema(rootSchemaName).Row(widget.ForRootScemaWidgetsTable);
            Insert.IntoTable("ServerControlWidgets").InSchema(pagesSchemaName).Row(widget.ForPagesSchemaServerControlWidgetsTable);

            // Add widget options.
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.Option1);
        }
    }
}