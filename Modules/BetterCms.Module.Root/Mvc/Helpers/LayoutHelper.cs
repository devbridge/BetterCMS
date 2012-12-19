using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    /// <summary>
    /// A helper to render CMS page content.
    /// </summary>
    public static class LayoutHelper
    {
        /// <summary>
        /// Renders the section contents.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="webPage">The web page.</param>
        /// <param name="model">The model.</param>
        public static void RenderSectionContents(this HtmlHelper htmlHelper, WebPageBase webPage, RenderPageViewModel model)
        {
            foreach (var region in model.Regions)
            {
                var contentsBuilder = new StringBuilder();
                var projections = model.Contents.Where(c => c.RegionId == region.RegionId).OrderBy(c => c.Order).ToList();

                using (new LayoutRegionWrapper(contentsBuilder, region, model.CanManageContent))
                {
                    foreach (var projection in projections)
                    {
                        // Add Html
                        using (new RegionContentWrapper(contentsBuilder, htmlHelper,  projection, model.CanManageContent))
                        {
                            contentsBuilder.Append(projection.GetHtml(htmlHelper));
                        }
                    }
                }

                var html = contentsBuilder.ToString();

                if (!string.IsNullOrWhiteSpace(html))
                {
                    RenderSectionAsLayoutRegion(webPage, html, region.RegionIdentifier);
                }                
            }
        }

        private static void RenderSectionAsLayoutRegion(WebPageBase webPage, string partialViewHtml, string sectionName)
        {
            webPage.DefineSection(
                sectionName,
                () =>
                {
                    Action<TextWriter> writerAction = tw => tw.Write(partialViewHtml);
                    var result = new HelperResult(writerAction);
                    webPage.Write(result);
                });
        }

        /// <summary>
        /// Renders the page custom CSS.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="styles">The styles.</param>
        /// <returns></returns>
        public static IHtmlString RenderPageCustomCss(this HtmlHelper htmlHelper, IEnumerable<IStylesheetAccessor> styles)
        {
            if (styles != null)
            {
                var cssBuilder = new StringBuilder();

                foreach (var content in styles)
                {
                    var contentCss = content.GetCustomStyles(htmlHelper);
                    if (!string.IsNullOrWhiteSpace(contentCss))
                    {
                        cssBuilder.Append(contentCss);
                    }
                }

                var css = cssBuilder.ToString();
                if (!string.IsNullOrWhiteSpace(css))
                {
                    return new HtmlString(string.Format(@"<style type=""text/css"">{0}</style>", css));
                }
            }

            return null;
        }
    }
}