using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Command.History.GetSitemapHistory;
using BetterCms.Module.Pages.Command.History.RestoreSitemapVersion;
using BetterCms.Module.Pages.Command.Sitemap.DeleteSitemap;
using BetterCms.Module.Pages.Command.Sitemap.DeleteSitemapNode;
using BetterCms.Module.Pages.Command.Sitemap.GetPageLinks;
using BetterCms.Module.Pages.Command.Sitemap.GetPageTranslations;
using BetterCms.Module.Pages.Command.Sitemap.GetSitemap;
using BetterCms.Module.Pages.Command.Sitemap.GetSitemapsForNewPage;
using BetterCms.Module.Pages.Command.Sitemap.GetSitemapsList;
using BetterCms.Module.Pages.Command.Sitemap.GetSitemapVersion;
using BetterCms.Module.Pages.Command.Sitemap.SaveMultipleSitemaps;
using BetterCms.Module.Pages.Command.Sitemap.SaveSitemap;
using BetterCms.Module.Pages.Command.Sitemap.SaveSitemapNode;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.History;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Pages.Controllers
{
    /// <summary>
    /// Handles sitemap logic.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
    [ActionLinkArea(PagesModuleDescriptor.PagesAreaName)]
    public class SitemapController : CmsControllerBase
    {
        /// <summary>
        /// Gets sitemaps list for Site Settings.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Sitemaps list.</returns>
        public ActionResult Sitemaps(SitemapsFilter request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetSitemapsListCommand>().ExecuteCommand(request);
            var success = model != null;

            var view = RenderView("Sitemaps", model);
            var json = new
            {
                Tags = request.Tags,
            };

            return ComboWireJson(success, view, json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edits the sitemap.
        /// </summary>
        /// <param name="sitemapId">The sitemap identifier.</param>
        /// <returns>
        /// Rendered sitemap container.
        /// </returns>
        [HttpGet]
        public ActionResult EditSitemap(string sitemapId)
        {
            var model = GetCommand<GetSitemapCommand>().ExecuteCommand(sitemapId.ToGuidOrDefault());
            var pageLinks = GetCommand<GetPageLinksCommand>().ExecuteCommand(string.Empty);
            var success = model != null & pageLinks != null;
            var view = RenderView("Edit", model);

            var data = new SitemapAndPageLinksViewModel();
            if (success)
            {
                data.Sitemap = model;
                data.PageLinks = pageLinks;
            }

            return ComboWireJson(success, view, data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Shows the sitemap history.
        /// </summary>
        /// <param name="sitemapId">The sitemap identifier.</param>
        /// <returns>
        /// Rendered sitemap history view.
        /// </returns>
        [HttpGet]
        public ActionResult ShowSitemapHistory(string sitemapId)
        {
            return ShowSitemapHistory(new GetSitemapHistoryRequest { SitemapId = sitemapId.ToGuidOrDefault() });
        }

        /// <summary>
        /// Shows the sitemap history.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Rendered sitemap history view.
        /// </returns>
        [HttpPost]
        public ActionResult ShowSitemapHistory(GetSitemapHistoryRequest request)
        {
            var model = GetCommand<GetSitemapHistoryCommand>().ExecuteCommand(request);
            var view = RenderView("History", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Sitemaps the version.
        /// </summary>
        /// <param name="versionId">The version identifier.</param>
        /// <returns>
        /// Sitemap preview.
        /// </returns>
        [HttpGet]
        public ActionResult SitemapVersion(string versionId)
        {
            var model = GetCommand<GetSitemapVersionCommand>().ExecuteCommand(versionId.ToGuidOrDefault());
            var view = RenderView("Preview", model);
            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Restores the sitemap version.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="isUserConfirmed">The is user confirmed.</param>
        /// <returns>
        /// Json result.
        /// </returns>
        [HttpPost]
        public ActionResult RestoreSitemapVersion(string id, string isUserConfirmed)
        {
            try
            {
                var model = GetCommand<RestoreSitemapVersionCommand>().ExecuteCommand(new SitemapRestoreViewModel
                    {
                        SitemapVersionId = id.ToGuidOrDefault(),
                        IsUserConfirmed = isUserConfirmed.ToBoolOrDefault()
                    });
                return WireJson(model != null, model);
            }
            catch (ConfirmationRequestException exc)
            {
                return Json(new WireJson { Success = false, Data = new { ConfirmationMessage = exc.Resource() } });
            }
        }

        /// <summary>
        /// Saves the sitemap.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult SaveSitemap(SitemapViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.UserAccessList = model.UserAccessList ?? new List<UserAccessViewModel>();
                var response = GetCommand<SaveSitemapCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(NavigationGlobalization.Sitemap_SitemapCreatedSuccessfully_Message);
                    }

                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// Saves multiple sitemaps at once.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult SaveMultipleSitemaps(List<SitemapViewModel> model)
        {
            if (ModelState.IsValid)
            {
                model.ForEach(svm => svm.UserAccessList = svm.UserAccessList ?? new List<UserAccessViewModel>());
                var success = GetCommand<SaveMultipleSitemapsCommand>().ExecuteCommand(model);
                return Json(new WireJson(success));
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// Adds the new page.
        /// </summary>
        /// <returns>Rendered sitemap container.</returns>
        [HttpGet]
        public ActionResult AddNewPage()
        {
            var model = GetCommand<GetSitemapsForNewPageCommand>().ExecuteCommand();
            var success = model != null;
            var view = RenderView("NewPage", model);

            return ComboWireJson(success, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the page links.
        /// </summary>
        /// <param name="searchQuery">The search query.</param>
        /// <returns>JSON result.</returns>
        public ActionResult GetPageLinks(string searchQuery)
        {
            var response = GetCommand<GetPageLinksCommand>().ExecuteCommand(searchQuery);
            if (response != null)
            {
                var data = new SitemapAndPageLinksViewModel { PageLinks = response };
                return Json(new WireJson { Success = true, Data = data });
            }

            return Json(new WireJson { Success = false });
        }

        [HttpGet]
        public ActionResult GetPageTranslations(string pageId)
        {
            var response = GetCommand<GetPageTranslationsCommand>().ExecuteCommand(pageId.ToGuidOrDefault());
            return ComboWireJson(response != null, null, response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the sitemap node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>JSON result.</returns>
        [HttpPost]
        public ActionResult SaveSitemapNode(SitemapNodeViewModel node)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveSitemapNodeCommand>().ExecuteCommand(node);
                if (response != null)
                {
                    if (node.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(NavigationGlobalization.Sitemap_NodeCreatedSuccessfully_Message);
                    }

                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// Deletes the sitemap node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>JSON result.</returns>
        [HttpPost]
        public ActionResult DeleteSitemapNode(SitemapNodeViewModel node)
        {
            var success = GetCommand<DeleteSitemapNodeCommand>().ExecuteCommand(node);

            if (success)
            {
                Messages.AddSuccess(NavigationGlobalization.Sitemap_NodeDeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }

        /// <summary>
        /// Deletes the sitemap node.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        /// <returns>
        /// JSON result.
        /// </returns>
        [HttpPost]
        public ActionResult DeleteSitemap(string id, string version)
        {
            var success = GetCommand<DeleteSitemapCommand>().ExecuteCommand(new SitemapViewModel
                {
                    Id = id.ToGuidOrDefault(),
                    Version = version.ToIntOrDefault()
                });

            if (success)
            {
                Messages.AddSuccess(NavigationGlobalization.Sitemap_SitemapDeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }
    }
}