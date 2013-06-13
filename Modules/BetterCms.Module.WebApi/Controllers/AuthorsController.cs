using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Query;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Module.Blog.Api.DataModels;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.WebApi.Extensions;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.WebApi.Controllers
{
    /// <summary>
    /// Authors API controller.
    /// </summary>
    [ActionLinkArea(WebApiModuleDescriptor.WebApiAreaName)]
    public class AuthorsController : ApiController
    {
        /// <summary>
        /// Gets the list of blog posts by specified filter.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>
        /// List of blog post service models.
        /// </returns>
        public IList<AuthorModel> Get(ODataQueryOptions<AuthorModel> options)
        {
            using (var api = CmsContext.CreateApiContextOf<BlogsApiContext>())
            {
                var models = api.GetAuthorsAsQueryable();
                var results = options.ApplyToModels(models);
                return results.ToList();
            }
        }

        /// <summary>
        /// Gets the blog post by specified ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Blog post service model</returns>
        public AuthorModel Get(string id)
        {
            using (var api = CmsContext.CreateApiContextOf<BlogsApiContext>())
            {
                return api.GetAuthorsAsQueryable().FirstOrDefault(w => w.Id == id.ToGuidOrDefault());
            }
        }
    }
}
