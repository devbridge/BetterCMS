using System;

using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.ImagesGallery.Models.Migrations
{
    /// <summary>
    /// Creates servercontrol widget "Images Gallery Album Widget" with options
    /// </summary>
    [Migration(201310040830)]
    public class Migration201310040830: DefaultMigration
    {
        /// <summary>
        /// The root schema name
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// The pages schema name
        /// </summary>
        private readonly string pagesSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201310040830" /> class.
        /// </summary>
        public Migration201310040830()
            : base(ImagesGalleryModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
            pagesSchemaName = (new Pages.Models.Migrations.PagesVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Migrates UP.
        /// </summary>
        public override void Up()
        {
            var widget = new
            {

                ForRootSchemaContentTable = new
                {
                    Id = "F67BC85F-83A7-427E-85C5-A24D008B32E1",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    Name = "Images Gallery Album Widget",
                    Status = 3,
                    PublishedOn = DateTime.Now,
                    PublishedByUser = "Better CMS"
                },

                ForRootScemaWidgetsTable = new
                {
                    Id = "F67BC85F-83A7-427E-85C5-A24D008B32E1",
                },

                ForPagesSchemaServerControlWidgetsTable = new
                {
                    Id = "F67BC85F-83A7-427E-85C5-A24D008B32E1",
                    Url = "~/Areas/bcms-images-gallery/Views/Widgets/AlbumWidget.cshtml"
                }

            };

            var options = new
            {
                Option1 = new
                {
                    Id = "F1787DD0-249F-4842-8A86-A24D008B32EB",
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
                },

                Option2 = new
                {
                    Id = "7DB0A523-98E6-49EE-977E-A24D008E2125",
                    Version = 1,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedByUser = "Better CMS",
                    ModifiedOn = DateTime.Now,
                    ModifiedByUser = "Better CMS",
                    ContentId = widget.ForRootSchemaContentTable.Id,
                    Key = "RenderAlbumHeader",
                    Type = 5, // Boolean
                    DefaultValue = "false",
                    IsDeletable = false
                }
            };

            // Register server control widget.
            Insert.IntoTable("Contents").InSchema(rootSchemaName).Row(widget.ForRootSchemaContentTable);
            Insert.IntoTable("Widgets").InSchema(rootSchemaName).Row(widget.ForRootScemaWidgetsTable);
            Insert.IntoTable("ServerControlWidgets").InSchema(pagesSchemaName).Row(widget.ForPagesSchemaServerControlWidgetsTable);

            // Add widget options.
            Insert.IntoTable("ContentOptions").InSchema(rootSchemaName)
                .Row(options.Option1)
                .Row(options.Option2);
        }
    }
}