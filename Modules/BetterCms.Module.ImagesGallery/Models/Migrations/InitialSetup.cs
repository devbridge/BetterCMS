using System;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.ImagesGallery.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201304221200)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// The root module schema name.
        /// </summary>
        private readonly string mediaManagerSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(ImagesGalleryModuleDescriptor.ModuleName)
        {
            mediaManagerSchemaName = (new MediaManager.Models.Migrations.MediaManagerVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateAlbums();
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the subscribers.
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
    }
}