using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Query;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Api.DataContracts.Filters;
using BetterCms.Module.Blog.Api.DataContracts.Models;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Root.Mvc;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.WebApi.Controllers
{
    /// <summary>
    /// Blogs API controller.
    /// </summary>
    [ActionLinkArea(WebApiModuleDescriptor.WebApiAreaName)]
    public class BlogsController : ApiController
    {
        /// <summary>
        /// Gets the list of blog posts by specified filter.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// List of blog post service models.
        /// </returns>
        public IList<BlogPostModel> Get(ODataQueryOptions<BlogPostModel> options, [FromUri] BlogPostFilter filter)
        {
            using (var api = CmsContext.CreateApiContextOf<BlogsApiContext>())
            {
                var entities = api.Queryable<BlogPost>();

                if (!string.IsNullOrWhiteSpace(filter.Like))
                {
                    entities = entities.Where(b => b.Title.Contains(filter.Like));
                }
                if (filter.Tags != null)
                {
                    foreach (var tag in filter.Tags)
                    {
                        entities = entities.Where(b => b.PageTags.Any(pt => pt.Tag.Name == tag));
                    }
                }

                var models = entities.Select(blog => new BlogPostModel { Id = blog.Id, CreatedOn = blog.CreatedOn, Title = blog.Title, Version = blog.Version });

                var results = options.ApplyTo(models);

                return results.Cast<BlogPostModel>().ToList();
            }
        }

        /// <summary>
        /// Gets the blog post by specified ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Blog post service model</returns>
        public BlogPostModel Get(string id)
        {
            using (var api = CmsContext.CreateApiContextOf<BlogsApiContext>())
            {
                return BlogPostModel.FromEntity(api.GetBlogPosts(new GetBlogPostsRequest(w => w.Id == id.ToGuidOrDefault())).Items[0]);
            }
        }
    }
}
