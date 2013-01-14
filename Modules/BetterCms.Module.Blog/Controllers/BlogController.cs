using System;
using System.Web.Mvc;

using BetterCms.Module.Blog.Commands.GetBlogPost;
using BetterCms.Module.Blog.Commands.GetBlogPostList;
using BetterCms.Module.Blog.Commands.SaveBlogPost;
using BetterCms.Module.Blog.ViewModels.Blog;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Blog.Controllers
{
    public class BlogController : CmsControllerBase
    {
        public virtual ActionResult Index(SearchableGridOptions request)
        {
            var model = GetCommand<GetBlogPostListCommand>().ExecuteCommand(request);
            return View(model);
        }

        [HttpGet]
        public virtual ActionResult CreatePost()
        {
            var model = GetCommand<GetBlogPostCommand>().ExecuteCommand(Guid.Empty);
            var json = new
            {
                Data = new WireJson
                {
                    Success = true,
                    Data = model
                },
                Html = RenderView("CreatePost", model)
            };

            return WireJson(true, json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult CreatePost(BlogPostViewModel model)
        {
            var response = GetCommand<SaveBlogPostCommand>().ExecuteCommand(model);

            return WireJson(response != null, response);
        }
        
        [HttpGet]
        public virtual ActionResult EditPost(string id)
        {
            var model = GetCommand<GetBlogPostCommand>().ExecuteCommand(id.ToGuidOrDefault());
            var json = new
            {
                Data = new WireJson
                {
                    Success = true,
                    Data = model
                },
                Html = RenderView("EditPost", model)
            };

            return WireJson(true, json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult EditPost(BlogPostViewModel model)
        {
            var response = GetCommand<SaveBlogPostCommand>().ExecuteCommand(model);

            return WireJson(response != null, response);
        }
    }
}
