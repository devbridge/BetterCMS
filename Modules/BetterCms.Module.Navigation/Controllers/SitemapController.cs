using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Module.Navigation.Command.Sitemap.GetSitemap;
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
        /// <param name="search">Sitemap node search text.</param>
        /// <returns>
        /// Rendered sitemap container.
        /// </returns>
        public ActionResult Index(string search)
        {
            var sitemap = GetCommand<GetSitemapCommand>().ExecuteCommand(search);
            var json = new
                           {
                               Data = new WireJson
                                          {
                                              Success = true,
                                              Data = sitemap
                                          },
                               Html = RenderView("Index", new SearchableSitemapViewModel())
                           };
            return WireJson(true, json, JsonRequestBehavior.AllowGet);
        }
    }
}