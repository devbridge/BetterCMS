using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Binders;
using BetterCms.Core.Security;

using BetterCms.Module.Pages.Command.Content.DeletePageContent;
using BetterCms.Module.Pages.Command.Content.GetChildContentOptions;
using BetterCms.Module.Pages.Command.Content.GetContentType;
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
using BetterCms.Module.Root.ViewModels.Option;

using BetterModules.Core.Web.Models;

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
        /// <param name="parentPageContentId">The parent page content identifier.</param>
        /// <param name="includeChildRegions">The include child regions.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult InsertContentToPage(string pageId, string contentId, string regionId, string parentPageContentId, string includeChildRegions)
        {
            var request = new InsertContentToPageRequest
                {
                    ContentId = contentId.ToGuidOrDefault(),
                    PageId = pageId.ToGuidOrDefault(),
                    RegionId = regionId.ToGuidOrDefault(),
                    ParentPageContentId = (!string.IsNullOrWhiteSpace(parentPageContentId)) 
                        ? parentPageContentId.ToGuidOrDefault() 
                        : (System.Guid?) null,
                    IncludeChildRegions = includeChildRegions.ToBoolOrDefault()
                };

            var result = GetCommand<InsertContentToPageCommand>().ExecuteCommand(request);

            return WireJson(result != null, result);
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
            var request = new GetRecentWidgetAndWidgetCategoryRequest { Filter = query };
            var model = GetCommand<GetRecentWidgetAndWidgetCategoryCommand>().ExecuteCommand(request);

            return PartialView(model);
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
            var result = GetCommand<GetRecentWidgetAndWidgetCategoryCommand>().ExecuteCommand(new GetRecentWidgetAndWidgetCategoryRequest
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
        /// <param name="pageIdentifier">The page identifier.</param>
        /// <param name="regionIdentifier">The region identifier.</param>
        /// <param name="parentPageContentIdentifier">The parent page content identifier.</param>
        /// <returns>
        /// ViewResult to render add page content modal dialog.
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult AddPageHtmlContent(string pageIdentifier, string regionIdentifier, string parentPageContentIdentifier)
        {
            var addRequest = new InsertHtmlContentRequest
                    {
                        PageId = pageIdentifier,
                        RegionId = regionIdentifier,
                        ParentPageContentId = parentPageContentIdentifier
                    };
            var model = GetCommand<GetInsertHtmlContentCommand>().ExecuteCommand(addRequest);
            var view = RenderView("AddPageHtmlContent", model ?? new PageContentViewModel());

            var result = ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);

            result.MaxJsonLength = int.MaxValue;
// TODO: very large JSON ~2.1MB on test environment!!!
//            var scriptSerializer = new JavaScriptSerializer();
//            if (result.MaxJsonLength.HasValue)
//            {
//                scriptSerializer.MaxJsonLength = result.MaxJsonLength.Value;
//            }
//            var jsonString = scriptSerializer.Serialize(result.Data);

            return result;
        }

        /// <summary>
        /// Validates and saves page content.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// JSON with result status and redirect URL.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent)]
        public ActionResult SavePageHtmlContent([ModelBinder(typeof(JSONDataBinder))] SavePageHtmlContentCommandRequest request)
        {
            try
            {
                ValidateModelExplicitly(request.Content);

                ChangedContentResultViewModel result = null;
                if (ModelState.IsValid)
                {
                    result = GetCommand<SavePageHtmlContentCommand>().ExecuteCommand(request);
                }

                return WireJson(result != null, result);
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
        /// Creates modal dialog for editing a child content options.
        /// </summary>
        /// <param name="contentId">The content identifier.</param>
        /// <param name="assignmentIdentifier">The assignment identifier.</param>
        /// <param name="widgetId">The widget identifier.</param>
        /// <param name="loadOptions">if set to <c>true</c> [load options].</param>
        /// <returns>
        /// ViewResult to render page content options modal dialog.
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult ChildContentOptions(string contentId, string assignmentIdentifier, string widgetId, string loadOptions)
        {
            var request = new GetChildContentOptionsCommandRequest
                          {
                              ContentId = contentId.ToGuidOrDefault(),
                              AssignmentIdentifier = (assignmentIdentifier ?? "").ToGuidOrDefault(),
                              WidgetId = (widgetId ?? "").ToGuidOrDefault(),
                              LoadOptions = loadOptions.ToBoolOrDefault()
                          };
            var model = GetCommand<GetChildContentOptionsCommand>().ExecuteCommand(request);
            var view = RenderView("ChildContentOptions", model);

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
        public ActionResult PageContentOptions(ContentOptionValuesViewModel model)
        {
            var response = GetCommand<SavePageContentOptionsCommand>().ExecuteCommand(model);

            return WireJson(response != null, response);
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

            return WireJson(response != null, response);
        }

        /// <summary>
        /// Returns the type of the content.
        /// </summary>
        /// <param name="childContentId">The content identifier.</param>
        /// <returns>JSON result with the type of the child content</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public JsonResult GetContentType(string childContentId)
        {
            var response = GetCommand<GetContentTypeCommand>().ExecuteCommand(childContentId.ToGuidOrDefault());

            return WireJson(response != null, response, JsonRequestBehavior.AllowGet);
        }
    }
}
