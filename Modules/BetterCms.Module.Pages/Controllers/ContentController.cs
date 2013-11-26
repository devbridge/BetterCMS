using System.Linq;
using System.Web.Mvc;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Security;

using BetterCms.Module.Pages.Command.Content.DeletePageContent;
using BetterCms.Module.Pages.Command.Content.GetInsertHtmlContent;
using BetterCms.Module.Pages.Command.Content.GetPageContentOptions;
using BetterCms.Module.Pages.Command.Content.GetPageHtmlContent;
using BetterCms.Module.Pages.Command.Content.InsertContent;
using BetterCms.Module.Pages.Command.Content.SavePageContentOptions;
using BetterCms.Module.Pages.Command.Content.SavePageHtmlContent;
using BetterCms.Module.Pages.Command.Content.SortPageContent;
using BetterCms.Module.Pages.Command.Widget.GetWidgetCategory;

using BetterCms.Module.Pages.ViewModels.Content;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Pages.Controllers
{
    /// <summary>
    /// Controller for content management.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(PagesModuleDescriptor.PagesAreaName)]
    public class ContentController : CmsControllerBase
    {
        /// <summary>
        /// Inserts content to given page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="contentId">The widget id.</param>
        /// <param name="regionId">The region id.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult InsertContentToPage(string pageId, string contentId, string regionId)
        {
            var request = new InsertContentToPageRequest
            {
                ContentId = contentId.ToGuidOrDefault(),
                PageId = pageId.ToGuidOrDefault(),
                RegionId = regionId.ToGuidOrDefault()
            };

            if (GetCommand<InsertContentToPageCommand>().ExecuteCommand(request))
            {
                return Json(new WireJson { Success = true });
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// Creates a widget categories partial view for given search query.
        /// </summary>
        /// <param name="query">The widgets search query.</param>
        /// <returns>
        /// ViewResult to render a widget categories partial view.
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult Widgets(string query)
        {
            var request = new GetWidgetCategoryRequest { Filter = query };
            var model = GetCommand<GetWidgetCategoryCommand>().ExecuteCommand(request);

            return PartialView(model.WidgetCategories);
        }

        // TODO: remove action; update command.
        /// <summary>
        /// Creates widget categories partial view for given category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns>
        /// Html with rendered partial view.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        private ActionResult WidgetCategory(string categoryId)
        {
            var result = GetCommand<GetWidgetCategoryCommand>().ExecuteCommand(new GetWidgetCategoryRequest
                                                                            {
                                                                                CategoryId = categoryId.ToGuidOrDefault(),
                                                                                Filter = null
                                                                            });
            var model = result.WidgetCategories.FirstOrDefault();

            return PartialView("Partial/WidgetCategory", model);
        }

        /// <summary>
        /// Creates add page content modal dialog for given page.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="regionId">The region id.</param>
        /// <returns>
        /// ViewResult to render add page content modal dialog.
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult AddPageHtmlContent(string pageId, string regionId)
        {
            var model = GetCommand<GetInsertHtmlContentCommand>().ExecuteCommand(new InsertHtmlContentRequest() { PageId = pageId, RegionId = regionId });

            var request = new GetWidgetCategoryRequest();
            model.WidgetCategories = GetCommand<GetWidgetCategoryCommand>().ExecuteCommand(request).WidgetCategories;

            var view = RenderView("AddPageHtmlContent", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validates and saves page content.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// JSON with result status and redirect URL.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent)]
        public ActionResult SavePageHtmlContent(PageContentViewModel model)
        {
            try
            {
                var result = GetCommand<SavePageHtmlContentCommand>().ExecuteCommand(model);

                if (result != null)
                {
                    return
                        Json(
                            new WireJson
                                {
                                    Success = true,
                                    Data =
                                        new
                                            {
                                                PageContentId = result.PageContentId,
                                                ContentId = result.ContentId,
                                                RegionId = result.RegionId,
                                                PageId = result.PageId,
                                                DesirableStatus = model.DesirableStatus
                                            }
                                });
                }

                return Json(new WireJson { Success = false });
            }
            catch (ConfirmationRequestException exc)
            {
                return Json(new WireJson { Success = false, Data = new { ConfirmationMessage = exc.Resource() } });
            }
        }

        /// <summary>
        /// Creates edit page content modal dialog for given page.
        /// </summary>
        /// <param name="pageContentId">The page content id.</param>
        /// <returns>
        /// ViewResult to render an edit content dialog.
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent)]
        public ActionResult EditPageHtmlContent(string pageContentId)
        {
            var model = GetCommand<GetPageHtmlContentCommand>().ExecuteCommand(pageContentId.ToGuidOrDefault());

            var view = RenderView("EditPageHtmlContent", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Creates modal dialog for editing a page content options.
        /// </summary>
        /// <param name="pageContentId">The page content id.</param>
        /// <returns>
        /// ViewResult to render page content options modal dialog.
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult PageContentOptions(string pageContentId)
        {
            var model = GetCommand<GetPageContentOptionsCommand>().ExecuteCommand(pageContentId.ToGuidOrDefault());
            var view = RenderView("PageContentOptions", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves page content options.
        /// </summary>
        /// <param name="model">The view model.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult PageContentOptions(PageContentOptionsViewModel model)
        {
            bool success = GetCommand<SavePageContentOptionsCommand>().ExecuteCommand(model);

            return Json(new WireJson { Success = success });
        }

        /// <summary>
        /// Deletes page content.
        /// </summary>
        /// <param name="pageContentId">Page content id.</param>
        /// <param name="pageContentVersion">The page content version.</param>
        /// <param name="contentVersion">The content version.</param>
        /// <param name="isUserConfirmed">if set to <c>true</c> user has confirmed the deletion of content with dynamic regions.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult DeletePageContent(string pageContentId, string pageContentVersion, string contentVersion, string isUserConfirmed)
        {
            try
            {
                var request = new DeletePageContentCommandRequest
                                  {
                                      PageContentId = pageContentId.ToGuidOrDefault(),
                                      PageContentVersion = pageContentVersion.ToIntOrDefault(),
                                      ContentVersion = contentVersion.ToIntOrDefault(),
                                      IsUserConfirmed = isUserConfirmed.ToBoolOrDefault()
                                  };

                var success = GetCommand<DeletePageContentCommand>().ExecuteCommand(request);

                return Json(new WireJson(success));
            }
            catch (ConfirmationRequestException exc)
            {
                return Json(new WireJson { Success = false, Data = new { ConfirmationMessage = exc.Resource() } });
            }
        }

        /// <summary>
        /// Sorts page content.
        /// </summary>
        /// <param name="model">The view model.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult SortPageContent(PageContentSortViewModel model)
        {
            var response = GetCommand<SortPageContentCommand>().ExecuteCommand(model);
            return Json(new WireJson { Success = response });
        }
    }
}
