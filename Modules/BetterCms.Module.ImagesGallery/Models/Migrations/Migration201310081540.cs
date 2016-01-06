using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.ImagesGallery.Models.Migrations
{
    /// <summary>
    /// Creates new widget options
    /// </summary>
    [Migration(201310081540)]
    public class Migration201310081540: DefaultMigration
    {
        /// <summary>
        /// The root schema name
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201310081540" /> class.
        /// </summary>
        public Migration201310081540()
            : base(ImagesGalleryModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Migrates UP.
        /// </summary>
        public override void Up()
        {
            CreateWidgetOptions();
        }

        /// <summary>
        /// Creates the widget options, reuired for widgets rendering.
        /// </summary>
        private void CreateWidgetOptions()
        {
            const string galleryWidgetId = "8BD1E830-6C2D-4AF1-BD5C-A24400A83DBC";
            const string albumWidgetId = "F67BC85F-83A7-427E-85C5-A24D008B32E1";

            var options = new
            {
                Option1 = new
                {
                    Id = "0F9FD6D8-8D7C-4759-A1A2-169659E7B719",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = galleryWidgetId,
                    Key = "Albums folder",
                    Type = 99, // Custom
                    IsDeletable = false,
                    CustomOptionId = "FB118858-CD1F-4CC6-8C22-177652EEB2A7"
                },

                Option2 = new
                {
                    Id = "633D1FA2-1C3C-44D8-BC47-A8A0DCE4B563",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = albumWidgetId,
                    Key = "Album folder",
                    Type = 99, // Custom
                    IsDeletable = false,
                    CustomOptionId = "FB118858-CD1F-4CC6-8C22-177652EEB2A7"
                },

                Option3 = new
                {
                    Id = "1D7D6FC7-2E0C-4151-AEEA-B7265ED04459",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = galleryWidgetId,
                    Key = "Album Images Url",
                    Type = 1, // String
                    IsDeletable = false
                },

                Option4 = new
                {
                    Id = "90A97911-17DE-4460-ABD9-A2D03530F93D",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = galleryWidgetId,
                    Key = "Images per section",
                    Type = 2, // Integer
                    IsDeletable = false,
                    DefaultValue = 3
                },

                Option5 = new
                {
                    Id = "BFCBCCFD-EADC-413D-B749-BB0A190E2FA2",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = albumWidgetId,
                    Key = "Images per section",
                    Type = 2, // Integer
                    IsDeletable = false,
                    DefaultValue = 3
                }
            };

            // Add widget options.
            Insert.IntoTable("ContentOptions")
                  .InSchema(rootSchemaName)
                  .Row(options.Option1)
                  .Row(options.Option2)
                  .Row(options.Option3)
                  .Row(options.Option4)
                  .Row(options.Option5);
        }
    }
}