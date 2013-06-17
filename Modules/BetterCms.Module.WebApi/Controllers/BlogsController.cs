using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Query;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Api.DataModels;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.WebApi.Extensions;

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
        public DataListResponse<BlogPostModel> Get(ODataQueryOptions<BlogPostModel> options, [FromUri] GetBlogPostsRequest filter)
        {
            using (var api = CmsContext.CreateApiContextOf<BlogsApiContext>())
            {
                var results = api.GetBlogPostsAsQueryable(filter);
                return results.ToDataListResponse(options);
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
                return api.GetBlogPostsAsQueryable().FirstOrDefault(w => w.Id == id.ToGuidOrDefault());
            }
        }
    }
}
