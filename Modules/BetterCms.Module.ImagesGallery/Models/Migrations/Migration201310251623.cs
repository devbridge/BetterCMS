using BetterModules.Core.DataAccess.DataContext.Migrations;

using FluentMigrator;

namespace BetterCms.Module.ImagesGallery.Models.Migrations
{
    [Migration(201310251623)]
    public class Migration201310251623: DefaultMigration
    {
        /// <summary>
        /// The root schema name
        /// </summary>
        private readonly string rootSchemaName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Migration201310251623" /> class.
        /// </summary>
        public Migration201310251623()
            : base(ImagesGalleryModuleDescriptor.ModuleName)
        {
            rootSchemaName = (new Root.Models.Migrations.RootVersionTableMetaData()).SchemaName;
        }

        /// <summary>
        /// Migrates UP.
        /// </summary>
        public override void Up()
        {
            Update.Table("Contents")
                  .InSchema(rootSchemaName)
                  .Set(new { PreviewUrl = "/file/bcms-images-gallery/Content/GalleryPreview.png", Name = "Gallery Albums Widget" })
                  .Where(new { Id = "8BD1E830-6C2D-4AF1-BD5C-A24400A83DBC" });

            Update.Table("Contents")
                  .InSchema(rootSchemaName)
                  .Set(new { PreviewUrl = "/file/bcms-images-gallery/Content/GalleryPreview.png", Name = "Gallery Album Images Widget" })
                  .Where(new { Id = "F67BC85F-83A7-427E-85C5-A24D008B32E1" });
        }
    }
}