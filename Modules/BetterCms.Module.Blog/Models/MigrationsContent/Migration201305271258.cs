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

using NHibernate.Linq;

namespace BetterCms.Module.Blog.Models.MigrationsContent
{
    [ContentMigration(201805271258)]
    public class Migration201305271258 : ContentMigrationBase
    {
        public override void Up(ICmsConfiguration configuration)
        {
            using (var api = CmsContext.CreateApiContextOf<MigrationApiContext>())
            {
                api.UpdateBlogPostDates();
            }
        }

        private class MigrationApiContext : DataApiContext
        {
            public MigrationApiContext(ILifetimeScope lifetimeScope, IRepository repository = null, IUnitOfWork unitOfWork = null)
                : base(lifetimeScope, repository, unitOfWork)
            {
            }

            /// <summary>
            /// Updates the blog post dates.
            /// </summary>
            internal void UpdateBlogPostDates()
            {
                try
                {
                    UnitOfWork.BeginTransaction();

                    var blogPostContents = Repository
                        .AsQueryable<BlogPostContent>()
                        .FetchMany(bp => bp.PageContents)
                        .ThenFetch(bp => bp.Page)
                        .ToList();

                    foreach (var content in blogPostContents)
                    {
                        if (content.PageContents != null && content.PageContents.Count > 0)
                        {
                            var page = content.PageContents[0].Page as BlogPost;
                            if (page != null)
                            {
                                if (content.ActivationDate != page.ActivationDate
                                    || content.ExpirationDate != page.ExpirationDate)
                                {
                                    page.ActivationDate = content.ActivationDate;
                                    page.ExpirationDate = content.ExpirationDate;

                                    Repository.Save(page);
                                }
                            }
                        }
                    }

                    UnitOfWork.Commit();
                }
                catch (Exception inner)
                {
                    var message = string.Format("Failed to update blog post dates.");
                    Logger.Error(message, inner);
                    throw new CmsApiException(message, inner);
                }
            }
        }
    }
}