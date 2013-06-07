using System;
using System.Linq;

using Autofac;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Exceptions.Api;
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
            using (var api = CmsContext.CreateApiContextOf<MigrationApiContext>())
            {
                api.UpdateFolderContentTypes();
            }
        }

        private class MigrationApiContext : DataApiContext
        {
            public MigrationApiContext(ILifetimeScope lifetimeScope, IRepository repository = null, IUnitOfWork unitOfWork = null)
                : base(lifetimeScope, repository, unitOfWork)
            {
            }

            /// <summary>
            /// Updates the folder content types.
            /// </summary>
            internal void UpdateFolderContentTypes()
            {
                try
                {
                    UnitOfWork.BeginTransaction();

                    var medias = Repository
                        .AsQueryable<MediaFolder>()
                        .Where(m => m.ContentType != MediaContentType.Folder)
                        .ToList();

                    foreach (var media in medias)
                    {
                        media.ContentType = MediaContentType.Folder;
                        Repository.Save(media);
                    }

                    UnitOfWork.Commit();
                }
                catch (Exception inner)
                {
                    var message = string.Format("Failed to update medias content types.");
                    Logger.Error(message, inner);
                    throw new CmsApiException(message, inner);
                }
            }
        }
    }
}