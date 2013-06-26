using System;
using System.Data.SqlTypes;
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
    [ContentMigration(201306201216)]
    public class Migration201306201216 : BaseContentMigration
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
                        .AsQueryable<Media>()
                        .Where(media => media.PublishedOn == (DateTime)SqlDateTime.MinValue)
                        .ToList();

                    foreach (var media in medias)
                    {
                        media.PublishedOn = media.CreatedOn;
                        Repository.Save(media);
                    }

                    UnitOfWork.Commit();
                }
                catch (Exception inner)
                {
                    var message = string.Format("Failed to update medias PublishedOn date.");
                    Logger.Error(message, inner);
                    throw new CmsApiException(message, inner);
                }
            }
        }
    }
}