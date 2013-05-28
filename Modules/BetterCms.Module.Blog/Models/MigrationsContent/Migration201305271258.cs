using System.Linq;
using System.Transactions;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Pages.Api.DataContracts;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models.MigrationsContent;

namespace BetterCms.Module.Blog.Models.MigrationsContent
{
    [ContentMigration(201305271258)]
    public class Migration201305271258 : BaseContentMigration
    {
        /// <summary>
        /// Ups the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public override void Up(ICmsConfiguration configuration)
        {
            using (var blogsApi = CmsContext.CreateApiContextOf<BlogsApiContext>())
            {
                var blogs = blogsApi.GetBlogPosts(new GetBlogPostsRequest(includePrivate: true, includeUnpublished: true, includeNotActive: true)).Items;
                if (!blogs.Any())
                {
                    return;
                }

                using (var pagesApi = CmsContext.CreateApiContextOf<PagesApiContext>())
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        foreach (var blog in blogs)
                        {
                            var pageContent = pagesApi.GetPageContents(new GetPageContentsRequest(blog.Id, e => e.Content is BlogPostContent)).FirstOrDefault();
                            if (pageContent == null)
                            {
                                continue;
                            }

                            var content = pageContent.Content as HtmlContent;
                            if (content == null)
                            {
                                continue;
                            }

                            var request = new UpdateBlogPostRequest
                                              {
                                                  ActivationDate = content.ActivationDate,
                                                  ExpirationDate = content.ExpirationDate,
                                                  Id = blog.Id
                                              };
                            // TODO: Uncomment and fix dstributed transactions bug
                            // blogsApi.UpdateBlogPost(request);
                        }

                        transactionScope.Complete();
                    }
                }
            }
        }
    }
}