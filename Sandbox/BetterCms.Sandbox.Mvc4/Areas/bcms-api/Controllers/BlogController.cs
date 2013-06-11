using System;
using System.Linq;
using System.Web;
using System.Web.Http;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Sandbox.Mvc4.Areas.bcms_api.Controllers
{
    /// <summary>
    /// Blogs API controller.
    /// </summary>
    public class BlogController : ApiController
    {
        public System.Collections.Generic.IEnumerable<BloPostModel> Get()
        {
            string[] tags = null;
            var queryParam = HttpContext.Current.Request.QueryString.GetValues("tags[]");
            if (queryParam != null)
            {
                tags = queryParam;
            }

            var api = CmsContext.CreateApiContextOf<BlogsApiContext>();
            var results = api.GetBlogPostsQueryable(GetQueryFilter(), tags);

            return results.Select(Convert);

            /*var query = api.GetBlogPostsQueryable();

            var result = options.ApplyTo(query);

            var aaa = result.Cast<BlogPost>().Select(Convert).ToList();*/
            //return api.GetBlogPosts(new GetBlogPostsRequest()).Items.Select(Convert).AsQueryable();
            /*return aaa;*/
        }

        // GET api/values/5
        public BloPostModel Get(string id)
        {
            using (var api = CmsContext.CreateApiContextOf<BlogsApiContext>())
            {
                return Convert(api.GetBlogPosts(new GetBlogPostsRequest(w => w.Id == id.ToGuidOrDefault())).Items[0]);
            }
        }

        private BloPostModel Convert(BlogPost blog)
        {
            return new BloPostModel
            {
                Id = blog.Id,
                CreatedOn = blog.CreatedOn,
                Title = blog.Title,
                Version = blog.Version
            };
        }

        public class BloPostModel
        {
            public Guid Id { get; set; }
            public int Version { get; set; }
            public string Title { get; set; }
            public DateTime CreatedOn { get; set; }
        }

        private string GetQueryFilter()
        {
            var filter = string.Empty;
            var allowableParameters = new[] { "$filter", "$orderby", "$top", "$skip" };

            foreach (var key in HttpContext.Current.Request.QueryString.AllKeys)
            {
                if (allowableParameters.Contains(key))
                {
                    filter = string.Format("{0}{1}{2}={3}",
                        filter,
                        !string.IsNullOrWhiteSpace(filter) ? "&" : string.Empty,
                        key,
                        HttpContext.Current.Request.QueryString[key]);
                }
            }
            return filter;
        }
    }
}