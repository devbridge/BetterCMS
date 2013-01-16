using System.Web.Mvc;

using BetterCms.Module.Navigation.Command.Sitemap.DeleteSitemapNode;
using BetterCms.Module.Navigation.Command.Sitemap.GetSitemap;
using BetterCms.Module.Navigation.Command.Sitemap.SaveSitemapNode;
using BetterCms.Module.Navigation.Content.Resources;
using BetterCms.Module.Navigation.ViewModels.Sitemap;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Navigation.Controllers
{
    /// <summary>
    /// Handles sitemap logic.
    /// </summary>
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
        public ActionResult EditSitemap()
        {
            var sitemap = GetCommand<GetSitemapCommand>().ExecuteCommand(string.Empty);
            var success = sitemap != null;
            var view = RenderView("Edit", null);

            return ComboWireJson(success, view, sitemap, JsonRequestBehavior.AllowGet);
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
//                        Messages.AddSuccess(NavigationGlobalization.CreateSitemapNode_CreatedSuccessfully_Message);
                    }

                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }

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