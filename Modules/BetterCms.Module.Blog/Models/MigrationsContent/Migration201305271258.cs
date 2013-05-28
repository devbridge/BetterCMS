using System.Collections.Generic;
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

                var updateRequests = new List<UpdateBlogPostRequest>();

                using (var pagesApi = CmsContext.CreateApiContextOf<PagesApiContext>())
                {
                    foreach (var blog in blogs)
                    {
                        var requestToGet = new GetPageContentsRequest(blog.Id, e => e.Content is BlogPostContent, includeUnpublished: true, includeNotActive: true);
                        var pageContent = pagesApi.GetPageContents(requestToGet).FirstOrDefault();
                        if (pageContent == null)
                        {
                            continue;
                        }

                        var content = pageContent.Content as HtmlContent;
                        if (content == null)
                        {
                            continue;
                        }

                        updateRequests.Add(new UpdateBlogPostRequest { Id = blog.Id, ActivationDate = content.ActivationDate, ExpirationDate = content.ExpirationDate, });
                    }
                }

                using (var transactionScope = new TransactionScope())
                {
                    foreach (var request in updateRequests)
                    {
                        blogsApi.UpdateBlogPost(request);
                    }

                    transactionScope.Complete();
                }
            }
        }
    }
}