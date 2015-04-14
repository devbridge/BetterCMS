using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

using BetterCms.Core.Security;

using BetterCms.Module.MediaManager.ViewModels;

using BetterCms.Module.Pages.Command.Layout.GetLayoutOptions;
using BetterCms.Module.Pages.Command.Layout.GetLayoutUserAccess;
using BetterCms.Module.Pages.Command.Page.AddNewPage;
using BetterCms.Module.Pages.Command.Page.ClonePage;
using BetterCms.Module.Pages.Command.Page.ClonePageWithLanguage;
using BetterCms.Module.Pages.Command.Page.CreatePage;
using BetterCms.Module.Pages.Command.Page.DeletePage;
using BetterCms.Module.Pages.Command.Page.GetPageForCloning;
using BetterCms.Module.Pages.Command.Page.GetPageForCloningWithLanguage;
using BetterCms.Module.Pages.Command.Page.GetPageForDelete;
using BetterCms.Module.Pages.Command.Page.GetPageProperties;
using BetterCms.Module.Pages.Command.Page.GetPagesList;
using BetterCms.Module.Pages.Command.Page.GetUntranslatedPagesList;
using BetterCms.Module.Pages.Command.Page.SavePageProperties;
using BetterCms.Module.Pages.Command.Page.SavePagePublishStatus;
using BetterCms.Module.Pages.Command.Page.SuggestPages;
using BetterCms.Module.Pages.Content.Resources;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;

using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.Page;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Pages.Controllers
{
    /// <summary>
    /// Controller for CMS pages: create / edit / delete pages.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(PagesModuleDescriptor.PagesAreaName)]
    public class PageController : CmsControllerBase
    {
        /// <summary>
        /// The page service.
        /// </summary>
        private readonly IPageService pageService;

        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageController"/> class.
        /// </summary>
        /// <param name="pageService">The page service.</param>
        public PageController(IPageService pageService)
        {
            this.pageService = pageService;
        }

        /// <summary>
        /// Renders a page list for the site settings dialog.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Rendered pages list.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.DeleteContent)]
        public ActionResult Pages(PagesFilter request)
        {
            request.SetDefaultPaging();

            var model = GetCommand<GetPagesListCommand>().ExecuteCommand(request);
            var success = model != null;
            var view = RenderView("Pages", model);

            return ComboWireJson(success, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Opens dialog for selecting the page.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Rendered pages list</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.DeleteContent)]
        public ActionResult SelectPage(PagesFilter request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetPagesListCommand>().ExecuteCommand(request);
            var success = model != null;

            var view = RenderView("SelectPage", model);

            return ComboWireJson(success, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Creates add new page modal dialog.
        /// </summary>
        /// <param name="parentPageUrl">The parent page URL.</param>
        /// <returns>
        /// ViewResult to render add new page modal dialog.
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult AddNewPage(string parentPageUrl, string addMaster)
        {
            var request = new AddNewPageCommandRequest
                {
                    ParentPageUrl = parentPageUrl,
                    CreateMasterPage = !string.IsNullOrEmpty(addMaster) && addMaster == "true"
                };
            var model = GetCommand<AddNewPageCommand>().ExecuteCommand(request);
            var view = RenderView("AddNewPage", model);

            return ComboWireJson(true, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validates and creates new page.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json with result status and redirect url.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult AddNewPage(AddNewPageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<CreatePageCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    response.PageUrl = HttpUtility.UrlDecode(Http.GetAbsolutePath(response.PageUrl));
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
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult EditPageProperties(string pageId)
        {
            var model = GetCommand<GetPagePropertiesCommand>().ExecuteCommand(pageId.ToGuidOrDefault());
            var success = model != null;

            var view = RenderView("EditPageProperties", model);
            var json = new {
                               PageId = success ? model.Id : (System.Guid?)null,
                               Tags = success ? model.Tags : null,
                               Categories = success ? model.Categories : null,
                               Image = success ? model.Image : new ImageSelectorViewModel(),
                               SecondaryImage = success ? model.SecondaryImage : new ImageSelectorViewModel(),
                               FeaturedImage = success ? model.FeaturedImage : new ImageSelectorViewModel(),
                               OptionValues = success ? model.OptionValues : null,
                               CustomOptions = success ? model.CustomOptions : null,
                               UserAccessList = success ? model.UserAccessList : new List<UserAccessViewModel>(),
                               IsMasterPage = success && model.IsMasterPage,
                               Languages = success ? model.Languages : null,
                               LanguageId = success ? model.LanguageId : null,
                               Translations = success ? model.Translations : null,
                               ShowTranslationsTab = success && model.ShowTranslationsTab,
                               CategoriesFilterKey = success ? model.CategoriesFilterKey : PageProperties.CategorizableItemKeyForPages
                           };

            return ComboWireJson(success, view, json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validates and saves page properties.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json with result status and redirect url.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult EditPageProperties(EditPagePropertiesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SavePagePropertiesCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    response.PageUrl = HttpUtility.UrlDecode(Http.GetAbsolutePath(response.PageUrl));
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
        [BcmsAuthorize(RootModuleConstants.UserRoles.DeleteContent)]
        public ActionResult DeletePageConfirmation(string id)
        {
            var model = GetCommand<GetPageForDeleteCommand>().ExecuteCommand(id.ToGuidOrDefault());
            if (model != null && model.ValidationMessage != null)
            {
                Messages.AddInfo(model.ValidationMessage);
            }
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
        [BcmsAuthorize(RootModuleConstants.UserRoles.DeleteContent)]
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
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult ClonePage(string id)
        {
            var model = GetCommand<GetPageForCloningCommand>().ExecuteCommand(id.ToGuidOrDefault());
            var view = RenderView("ClonePage", model ?? new ClonePageViewModel());
            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Clones the page with language id.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>
        /// Json result status.
        /// </returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult ClonePageWithLanguage(string pageId)
        {
            var request = new GetPageForCloningWithLanguageCommandRequest
                              {
                                  PageId = pageId.ToGuidOrDefault()
                              };
            var model = GetCommand<GetPageForCloningWithLanguageCommand>().ExecuteCommand(request);
            if (model != null && model.Languages.Count == 0)
            {
                if (model.ShowWarningAboutNoCultures)
                {
                    Messages.AddInfo(PagesGlobalization.ClonePageWithLanguage_NoLanguagesCreated_Message);
                }
                else
                {
                    Messages.AddInfo(PagesGlobalization.ClonePageWithLanguage_PageHasAllTranslations_Message);
                }
            }

            var view = RenderView("ClonePageWithLanguage", model ?? new ClonePageWithLanguageViewModel());

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
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
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
        /// Clones the page with language.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Json result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult ClonePageWithLanguage(ClonePageWithLanguageViewModel model)
        {
            model = GetCommand<ClonePageWithLanguageCommand>().ExecuteCommand(model);
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
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.PublishContent)]
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
        /// <param name="parentPageUrl">The parent page URL.</param>
        /// <param name="parentPageId">The parent page identifier.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <returns>
        /// URL, created from text.
        /// </returns>
        [BcmsAuthorize]
        public ActionResult ConvertStringToSlug(string text, string senderId, string parentPageUrl, string parentPageId, string languageId, string categoryId)
        {
            var category = categoryId.ToGuidOrNull();
            List<Guid> categories = null;

            if (category != null)
            {
                categories = new List<Guid>() { category.GetValueOrDefault() };
            }


            var slug = pageService.CreatePagePermalink(text, HttpUtility.UrlDecode(parentPageUrl), parentPageId.ToGuidOrNull(), languageId.ToGuidOrNull(), categories);

            return Json(new { Text = text, Url = slug, SenderId = senderId }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Loads the layout options.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="isMasterPage">if set to <c>true</c> layout is master page.</param>
        /// <returns></returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult LoadLayoutOptions(string id, string isMasterPage)
        {
            var model = GetCommand<GetLayoutOptionsCommand>().ExecuteCommand(new GetLayoutOptionsCommandRequest {
                Id = id.ToGuidOrDefault(),
                IsMasterPage = isMasterPage.ToBoolOrDefault()
            });

            return WireJson(model != null, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Loads the layout user access.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="isMasterPage">The is master page.</param>
        /// <returns></returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult LoadLayoutUserAccess(string id, string isMasterPage)
        {
            var model = GetCommand<GetLayoutUserAccessCommand>().ExecuteCommand(new GetLayoutUserAccessCommandRequest
            {
                Id = id.ToGuidOrDefault(),
                IsMasterPage = isMasterPage.ToBoolOrDefault()
            });

            return WireJson(model != null, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Suggests untranslated pages.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Suggested untranslated pages list</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult SuggestUntranslatedPages(PageSuggestionViewModel model)
        {
            model.OnlyUntranslatedPages = true;
            var suggestedPages = GetCommand<SuggestPagesCommand>().ExecuteCommand(model);

            return Json(new { suggestions = suggestedPages });
        }

        /// <summary>
        /// Searches within untranslated pages.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Search result with untranslated pages list
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult SearchUntranslatedPages(UntranslatedPagesFilter request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetUntranslatedPagesListCommand>().ExecuteCommand(request);

            var view = RenderView("SearchUntranslatedPages", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }
    }
}