using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Models;

using FluentMigrator;

namespace BetterCms.Module.MediaManager.Models.Migrations
{
    /// <summary>
    /// Module initial database structure creation.
    /// </summary>
    [Migration(201301151845)]
    public class InitialSetup : DefaultMigration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSetup"/> class.
        /// </summary>
        public InitialSetup()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
        }

        /// <summary>
        /// Ups this instance.
        /// </summary>
        public override void Up()
        {
            CreateMediaImageAlignTable();
            CreateMediaTypesTable();
            CreateMediasTable();
            CreateMediaFolderTable();
            CreateMediaFilesTable();
            CreateMediaImagesTable();
        }

        /// <summary>
        /// Downs this instance.
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the media types table.
        /// </summary>
        private void CreateMediaTypesTable()
        {
            Create
                .Table("MediaTypes").InSchema(SchemaName)
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

            Insert
                .IntoTable("MediaTypes").InSchema(SchemaName)
                .Row(new { Id = 1, Name = "Image" })
                .Row(new { Id = 2, Name = "Video" })
                .Row(new { Id = 3, Name = "Audio" })
                .Row(new { Id = 4, Name = "File" });
        }

        /// <summary>
        /// Creates the medias table.
        /// </summary>
        private void CreateMediasTable()
        {
            Create
                .Table("Medias").InSchema(SchemaName)
                .WithBaseColumns()
                .WithColumn("FolderId").AsGuid().Nullable()
                .WithColumn("Title").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Type").AsInt32().NotNullable();

            Create
                .Index("IX_Cms_Medias_FolderId")
                .OnTable("Medias").InSchema(SchemaName).OnColumn("FolderId").Ascending();

            Create
                .ForeignKey("FK_Cms_Medias_Type_MediaTypes_Id")
                .FromTable("Medias").InSchema(SchemaName).ForeignColumn("Type")
                .ToTable("MediaTypes").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_Medias_Title")
                .OnTable("Medias").InSchema(SchemaName).OnColumn("Title");

            Create
                .Index("IX_Cms_Medias_Type")
                .OnTable("Medias").InSchema(SchemaName).OnColumn("Type");
        }

        /// <summary>
        /// Creates the media folder table.
        /// </summary>
        private void CreateMediaFolderTable()
        {
            Create
                .Table("MediaFolders").InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("ParentFolderId").AsGuid().Nullable();

            Create
                .ForeignKey("FK_Cms_Medias_FolderId_MediaFolders_Id")
                .FromTable("Medias").InSchema(SchemaName).ForeignColumn("FolderId")
                .ToTable("MediaFolders").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_MediaFolders_ParentId_MediaFolders_Id")
                .FromTable("MediaFolders").InSchema(SchemaName).ForeignColumn("ParentFolderId")
                .ToTable("MediaFolders").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .ForeignKey("FK_Cms_MediaFolders_Id_Medias_Id")
                .FromTable("MediaFolders").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Medias").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_MediaFolders_ParentId")
                .OnTable("MediaFolders").InSchema(SchemaName).OnColumn("ParentFolderId").Ascending();
        }

        /// <summary>
        /// Creates the media files table.
        /// </summary>
        private void CreateMediaFilesTable()
        {
            Create
                .Table("MediaFiles").InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("OriginalFileName").AsAnsiString(MaxLength.Name).NotNullable()
                .WithColumn("OriginalFileExtension").AsAnsiString(MaxLength.Name).Nullable()
                .WithColumn("FileUri").AsString(MaxLength.Uri).NotNullable()
                .WithColumn("PublicUrl").AsString(MaxLength.Url).NotNullable()
                .WithColumn("Size").AsInt64().NotNullable()
                .WithColumn("IsTemporary").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("IsUploaded").AsBoolean().NotNullable().WithDefaultValue(0)
                .WithColumn("IsCanceled").AsBoolean().NotNullable().WithDefaultValue(0);

            Create.ForeignKey("FK_MediaFiles_Id_Medias_Id")
                .FromTable("MediaFiles").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("Medias").InSchema(SchemaName).PrimaryColumn("Id");

            Create
                .Index("IX_Cms_MediaFiles_OriginalFileName")
                .OnTable("MediaFiles").InSchema(SchemaName).OnColumn("OriginalFileName").Ascending();

            Create
                .Index("IX_Cms_MediaFiles_Size")
                .OnTable("MediaFiles").InSchema(SchemaName).OnColumn("Size").Ascending();
        }

        /// <summary>
        /// Creates the media images table.
        /// </summary>
        private void CreateMediaImagesTable()
        {
            Create
                .Table("MediaImages").InSchema(SchemaName)
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Caption").AsString(MaxLength.Text).Nullable()
                .WithColumn("ImageAlign").AsInt32().Nullable()
                .WithColumn("Width").AsInt32().NotNullable()
                .WithColumn("Height").AsInt32().NotNullable()
                .WithColumn("CropCoordX1").AsInt32().Nullable()
                .WithColumn("CropCoordY1").AsInt32().Nullable()
                .WithColumn("CropCoordX2").AsInt32().Nullable()
                .WithColumn("CropCoordY2").AsInt32().Nullable()
                .WithColumn("OriginalWidth").AsInt32().NotNullable()
                .WithColumn("OriginalHeight").AsInt32().NotNullable()
                .WithColumn("OriginalSize").AsInt64().NotNullable()
                .WithColumn("OriginalUri").AsString(MaxLength.Uri).NotNullable()
                .WithColumn("PublicOriginallUrl").AsString(MaxLength.Url).NotNullable().WithDefaultValue(string.Empty)
                .WithColumn("IsOriginalUploaded").AsBoolean().NotNullable().WithDefaultValue(0)
                .WithColumn("ThumbnailWidth").AsInt32().NotNullable()
                .WithColumn("ThumbnailHeight").AsInt32().NotNullable()
                .WithColumn("ThumbnailSize").AsInt64().NotNullable()
                .WithColumn("ThumbnailUri").AsString(MaxLength.Uri).NotNullable()
                .WithColumn("PublicThumbnailUrl").AsString(MaxLength.Url).NotNullable().WithDefaultValue(string.Empty)
                .WithColumn("IsThumbnailUploaded").AsBoolean().NotNullable().WithDefaultValue(0);

            Create.ForeignKey("FK_MediaImages_Id_MediaFiles_Id")
                .FromTable("MediaImages").InSchema(SchemaName).ForeignColumn("Id")
                .ToTable("MediaFiles").InSchema(SchemaName).PrimaryColumn("Id");

            Create.ForeignKey("FK_MediaImages_ImageAlign_MediaImageAlignTypes_Id")
                .FromTable("MediaImages").InSchema(SchemaName).ForeignColumn("ImageAlign")
                .ToTable("MediaImageAlignTypes").InSchema(SchemaName).PrimaryColumn("Id");
        }

        /// <summary>
        /// Creates the media image align table.
        /// </summary>
        private void CreateMediaImageAlignTable()
        {
            Create
                .Table("MediaImageAlignTypes").InSchema(SchemaName)
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable();

            Insert
                .IntoTable("MediaImageAlignTypes").InSchema(SchemaName)
                .Row(new { Id = 1, Name = "Left" })
                .Row(new { Id = 2, Name = "Center" })
                .Row(new { Id = 3, Name = "Right" });
        }
    }
}