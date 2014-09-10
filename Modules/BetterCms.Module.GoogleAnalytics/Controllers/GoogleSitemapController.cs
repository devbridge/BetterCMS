using System;
using System.Web.Mvc;

using BetterCms.Core.ActionResults;
using BetterCms.Module.GoogleAnalytics.Command.Sitemap;
using BetterCms.Module.GoogleAnalytics.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.GoogleAnalytics.Controllers
{
    public class GoogleSitemapController : CmsControllerBase
    {
        [HttpGet]
        public XmlResult Index(string sitemapId)
        {
            var getSitemapModel = new GetSitemapModel
            {
                SitemapId = !string.IsNullOrEmpty(sitemapId) ? sitemapId.ToGuidOrDefault() : Guid.Empty
            };

            var sitemap = GetCommand<GetSitemapCommand>().ExecuteCommand(getSitemapModel);
            
            return new XmlResult(sitemap);
        }
    }
}
