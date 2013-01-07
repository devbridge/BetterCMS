using System;
using System.Web.Mvc;

using BetterCms.Module.Blog.Commands.GetBlogPost;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Blog.Controllers
{
    public class BlogController : CmsControllerBase
    {
        public virtual ActionResult Index()
        {
            var model = new BlogPostViewModel();
            return View();
        }

        [HttpGet]
        public virtual ActionResult CreatePost()
        {
            var model = GetCommand<GetBlogPostCommand>().ExecuteCommand(Guid.Empty);

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult CreatePost(BlogPostViewModel model)
        {
            return View();
        }
        
        [HttpGet]
        public virtual ActionResult EditPost(string id)
        {
            var model = GetCommand<GetBlogPostCommand>().ExecuteCommand(id.ToGuidOrDefault());

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult EditPost(BlogPostViewModel model)
        {
            return View();
        }
    }
}
