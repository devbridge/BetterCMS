using System.Web.Mvc;

using BetterCms.Module.Blog.Commands.DeleteAuthor;
using BetterCms.Module.Blog.Commands.GetAuthorList;
using BetterCms.Module.Blog.Commands.SaveAuthor;
using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.ViewModels.Author;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Blog.Controllers
{
    public class AuthorController : CmsControllerBase
    {
        public virtual ActionResult ListTemplate()
        {
            var view = RenderView("Partial/ListTemplate", null);
            var blogPosts = GetCommand<GetAuthorListCommand>().ExecuteCommand(new SearchableGridOptions());

            return ComboWireJson(true, view, blogPosts, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult AuthorsList(SearchableGridOptions request)
        {
            var model = GetCommand<GetAuthorListCommand>().ExecuteCommand(request);
            return WireJson(true, model);
        }

        [HttpPost]
        public virtual ActionResult SaveAuthor(AuthorViewModel model)
        {
            var response = GetCommand<SaveAuthorCommand>().ExecuteCommand(model);
            if (response != null)
            {
                if (model.Id.HasDefaultValue())
                {
                    Messages.AddSuccess(BlogGlobalization.CreateAuthor_CreatedSuccessfully_Message);
                }
            }

            return WireJson(response != null, response);
        }
        
        [HttpPost]
        public virtual ActionResult DeleteAuthor(AuthorViewModel model)
        {
            var success = GetCommand<DeleteAuthorCommand>().ExecuteCommand(model);
            if (success)
            {
                if (!model.Id.HasDefaultValue())
                {
                    Messages.AddSuccess(BlogGlobalization.DeleteAuthor_DeletedSuccessfully_Message);
                }
            }

            return WireJson(success);
        }
    }
}