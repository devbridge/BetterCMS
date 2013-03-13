using System.Linq;
using System.Web.Mvc;

using BetterCms.Core.Models;
using BetterCms.Core.Security;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Pages.Command.Page.ClonePage;
using BetterCms.Module.Pages.Command.Page.CreatePage;
using BetterCms.Module.Pages.Command.Page.DeletePage;
using BetterCms.Module.Pages.Command.Page.GetPageForCloning;
using BetterCms.Module.Pages.Command.Page.GetPageForDelete;
using BetterCms.Module.Pages.Command.Page.GetPageProperties;
using BetterCms.Module.Pages.Command.Page.GetPagesList;
using BetterCms.Module.Pages.Command.Page.SavePageProperties;
using BetterCms.Module.Pages.Command.Page.SavePagePublishStatus;
using BetterCms.Module.Pages.Commands.GetTemplates;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.Mvc.Helpers;

namespace BetterCms.Module.Pages.Controllers
{
    /// <summary>
    /// Controller for CMS pages: create / edit / delete pages.
    /// </summary>
    public class PageController : CmsControllerBase
    {
        /// <summary>
        /// Renders a page list for the site settings dialog.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Rendered pages list.
        /// </returns>
        public ActionResult Pages(SearchableGridOptions request)
        {
            var model = GetCommand<GetPagesListCommand>().ExecuteCommand(request);
            return View(model);
        }

        /// <summary>
        /// Creates add new page modal dialog.
        /// </summary>
        /// <param name="parentPageUrl">The parent page URL.</param>
        /// <returns>
        /// ViewResult to render add new page modal dialog.
        /// </returns>
        [HttpGet]
        public ActionResult AddNewPage(string parentPageUrl)
        {
            AddNewPageViewModel model = new AddNewPageViewModel { ParentPageUrl = parentPageUrl };
            model.Templates = GetCommand<GetTemplatesCommand>().ExecuteCommand(new GetTemplatesRequest()).Templates;

            // Select first template as active
            if (model.Templates.Count > 0)
            {
                model.Templates.ToList().ForEach(x => x.IsActive = false);
                model.Templates.First().IsActive = true;
                model.TemplateId = model.Templates.First().TemplateId;
            }

            return View(model);
        }

        /// <summary>
        /// Validates and creates new page.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json with result status and redirect url.</returns>
        [HttpPost]
        public ActionResult AddNewPage(AddNewPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<CreatePageCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    response.PageUrl = Http.GetAbsolutePath(response.PageUrl);
                    Messages.AddSuccess(PagesGlobalization.SavePage_CreatedSuccessfully_Message);
                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson(false));
        }

        /// <summary>
        /// Creates edit page properties modal dialog for given page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>
        /// ViewResult to render edit page properties modal dialog.
        /// </returns>
        [HttpGet]
        public ActionResult EditPageProperties(string pageId)
        {
            var model = GetCommand<GetPagePropertiesCommand>().ExecuteCommand(pageId.ToGuidOrDefault());
            var success = model != null;

            if (success)
            {
                model.Templates = GetCommand<GetTemplatesCommand>().ExecuteCommand(new GetTemplatesRequest()).Templates;
                if (!model.TemplateId.HasDefaultValue())
                {
                    model.Templates.Where(x => x.TemplateId == model.TemplateId).ToList().ForEach(x => x.IsActive = true);
                }
            }

            var view = RenderView("EditPageProperties", model);
            var json = new
                           {
                               Tags = success ? model.Tags : null,
                               Image = success ? model.Image : new ImageSelectorViewModel()
                           };

            return ComboWireJson(success, view, json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validates and saves page properties.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json with result status and redirect url.</returns>
        [HttpPost]
        public ActionResult EditPageProperties(EditPagePropertiesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SavePagePropertiesCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    response.PageUrl = Http.GetAbsolutePath(response.PageUrl);
                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// Creates delete page confirmation dialog.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// ViewResult to render delete page confirmation modal dialog.
        /// </returns>
        [HttpGet]
        public ActionResult DeletePageConfirmation(string id)
        {
            var model = GetCommand<GetPageForDeleteCommand>().ExecuteCommand(id.ToGuidOrDefault());
            var view = RenderView("DeletePageConfirmation", model ?? new DeletePageViewModel());
            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes CMS page.
        /// </summary>
        /// <param name="model">The view model.</param>
        /// <returns>
        /// Json with delete result status.
        /// </returns>
        [HttpPost]
        public ActionResult DeletePage(DeletePageViewModel model)
        {
            if (GetCommand<DeletePageCommand>().ExecuteCommand(model))
            {
                Messages.AddSuccess(PagesGlobalization.DeletePage_DeletedSuccessfully_Message);
                return Json(new WireJson { Success = true });
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// Clones the page.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// Json result status.
        /// </returns>
        [HttpGet]
        public ActionResult ClonePage(string id)
        {
            var model = GetCommand<GetPageForCloningCommand>().ExecuteCommand(id.ToGuidOrDefault());
            var view = RenderView("ClonePage", model ?? new ClonePageViewModel());
            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Clones the page.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Json result status.
        /// </returns>
        [HttpPost]
        public ActionResult ClonePage(ClonePageViewModel model)
        {
            model = GetCommand<ClonePageCommand>().ExecuteCommand(model);
            if (model != null)
            {
                Messages.AddSuccess(string.Format(PagesGlobalization.ClonePage_Dialog_Success, model.PageUrl));
                return Json(new WireJson { Success = true, Data = model });
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// Changes CMS page IsPublished status.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Json with delete result status.
        /// </returns>
        [BcmsAuthorize(Roles = RootModuleConstants.UserRoles.PublishContent)]
        [HttpPost]
        public ActionResult ChangePublishStatus(SavePagePublishStatusRequest request)
        {
            var success = GetCommand<SavePagePublishStatusCommand>().ExecuteCommand(request);
            if (success)
            {
                var message = request.IsPublished 
                    ? PagesGlobalization.PublishPage_PagePublishedSuccessfully_Message
                    : PagesGlobalization.PublishPage_PageUnpublishedSuccessfully_Message;
                Messages.AddSuccess(message);
            }
            else
            {
                var message = request.IsPublished
                    ? PagesGlobalization.PublishPage_FailedToPublishPage_Message
                    : PagesGlobalization.PublishPage_FailedToUnpublishPage_Message;
                Messages.AddError(message);
            }

            return Json(new WireJson { Success = success });
        }

        /// <summary>
        /// Converts the string to slug.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="senderId">The sender id.</param>
        /// <returns>
        /// URL, created from text.
        /// </returns>
        public ActionResult ConvertStringToSlug(string text, string senderId)
        {
            const int maxLength = MaxLength.Url - 5;
            
            var slug = text.Transliterate();
            if (string.IsNullOrWhiteSpace(slug))
            {
                slug = "-";
            }
            if (slug.Length >= maxLength)
            {
                slug = slug.Substring(0, maxLength);
            }

            return Json(new { Text = text, Url = slug, SenderId = senderId }, JsonRequestBehavior.AllowGet);
        }
    }
}