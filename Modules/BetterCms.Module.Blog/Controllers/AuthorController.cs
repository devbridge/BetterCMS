using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Blog.Commands.DeleteAuthor;
using BetterCms.Module.Blog.Commands.GetAuthorList;
using BetterCms.Module.Blog.Commands.SaveAuthor;
using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.ViewModels.Author;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Blog.Controllers
{
    /// <summary>
    /// Blog authors controller.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
    [ActionLinkArea(BlogModuleDescriptor.BlogAreaName)]
    public class AuthorController : CmsControllerBase
    {
        /// <summary>
        /// Lists the template.
        /// </summary>
        /// <returns>Json result.</returns>
        public ActionResult ListTemplate()
        {
            var view = RenderView("Partial/ListTemplate", null);
            var request = new SearchableGridOptions();
            request.SetDefaultPaging();

            var authors = GetCommand<GetAuthorListCommand>().ExecuteCommand(request);

            return ComboWireJson(authors != null, view, authors, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lists the authors.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Json result.</returns>
        public ActionResult AuthorsList(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetAuthorListCommand>().ExecuteCommand(request);
            return WireJson(model != null, model);
        }

        /// <summary>
        /// Saves the author.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        public ActionResult SaveAuthor(AuthorViewModel model)
        {
            var success = false;
            AuthorViewModel response = null;
            if (ModelState.IsValid)
            {
                response = GetCommand<SaveAuthorCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(BlogGlobalization.CreateAuthor_CreatedSuccessfully_Message);
                    }

                    success = true;
                }
            }

            return WireJson(success, response);
        }

        /// <summary>
        /// Deletes the author.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        public ActionResult DeleteAuthor(string id, string version)
        {
            var request = new AuthorViewModel { Id = id.ToGuidOrDefault(), Version = version.ToIntOrDefault() };
            var success = GetCommand<DeleteAuthorCommand>().ExecuteCommand(request);
            if (success)
            {
                if (!request.Id.HasDefaultValue())
                {
                    Messages.AddSuccess(BlogGlobalization.DeleteAuthor_DeletedSuccessfully_Message);
                }
            }

            return WireJson(success);
        }
    }
}