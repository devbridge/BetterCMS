using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Pages.Command.Sitemap.DeleteSitemapNode;
using BetterCms.Module.Pages.Command.Sitemap.GetPageLinks;
using BetterCms.Module.Pages.Command.Sitemap.GetSitemap;
using BetterCms.Module.Pages.Command.Sitemap.GetSitemapsList;
using BetterCms.Module.Pages.Command.Sitemap.SaveSitemap;
using BetterCms.Module.Pages.Command.Sitemap.SaveSitemapNode;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

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
        /// <returns>Rendered sitemap container.</returns>
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
        /// Adds the new page.
        /// </summary>
        /// <returns>Rendered sitemap container.</returns>
        [HttpGet]
        public ActionResult AddNewPage()
        {
            var sitemap = GetCommand<GetSitemapCommand>().ExecuteCommand(string.Empty.ToGuidOrDefault()); // TODO: update.
            var success = sitemap != null;
            var view = RenderView("NewPage", new SitemapViewModel());

            return ComboWireJson(success, view, sitemap, JsonRequestBehavior.AllowGet);
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
            bool success = GetCommand<DeleteSitemapNodeCommand>().ExecuteCommand(node);

            if (success)
            {
                Messages.AddSuccess(NavigationGlobalization.Sitemap_NodeDeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }
    }
}