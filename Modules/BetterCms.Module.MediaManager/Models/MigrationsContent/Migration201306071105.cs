using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Module.Root.Models.MigrationsContent;

namespace BetterCms.Module.MediaManager.Models.MigrationsContent
{
    [ContentMigration(201306071105)]
    public class Migration201306071105 : BaseContentMigration
    {
        /// <summary>
        /// Ups the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public override void Up(ICmsConfiguration configuration)
        {
            using (var api = CmsContext.CreateApiContextOf<MediaManagerApiContext>())
            {
                api.UpdateFolderContentTypes();
            }
        }
    }
}