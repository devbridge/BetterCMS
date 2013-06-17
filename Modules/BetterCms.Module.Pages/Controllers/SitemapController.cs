using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Pages.Command.Sitemap.DeleteSitemapNode;
using BetterCms.Module.Pages.Command.Sitemap.GetPageLinks;
using BetterCms.Module.Pages.Command.Sitemap.GetSitemap;
using BetterCms.Module.Pages.Command.Sitemap.SaveSitemap;
using BetterCms.Module.Pages.Command.Sitemap.SaveSitemapNode;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

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
        /// Renders sitemap container.
        /// </summary>
        /// <param name="searchQuery">The search query.</param>
        /// <returns>
        /// Rendered sitemap container.
        /// </returns>
        public ActionResult Index(string searchQuery)
        {
            var sitemap = GetCommand<GetSitemapCommand>().ExecuteCommand(searchQuery);
            var success = sitemap != null;
            var view = RenderView("Index", new SearchableSitemapViewModel());

            return ComboWireJson(success, view, sitemap, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edits the sitemap.
        /// </summary>
        /// <returns>Rendered sitemap container.</returns>
        [HttpGet]
        public ActionResult EditSitemap()
        {
            var sitemap = GetCommand<GetSitemapCommand>().ExecuteCommand(string.Empty);
            var pageLinks = GetCommand<GetPageLinksCommand>().ExecuteCommand(string.Empty);
            var success = sitemap != null & pageLinks != null;
            var view = RenderView("Edit", null);

            var data = new SitemapAndPageLinksViewModel();
            if (success)
            {
                data.RootNodes = sitemap.RootNodes;
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
        public ActionResult SaveSitemap(List<SitemapNodeViewModel> model)
        {
            return Json(new WireJson { Success = GetCommand<SaveSitemapCommand>().ExecuteCommand(model) });
        }

        /// <summary>
        /// Adds the new page.
        /// </summary>
        /// <returns>Rendered sitemap container.</returns>
        [HttpGet]
        public ActionResult AddNewPage()
        {
            var sitemap = GetCommand<GetSitemapCommand>().ExecuteCommand(string.Empty);
            var success = sitemap != null;
            var view = RenderView("NewPage", new SearchableSitemapViewModel());

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