using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.ViewModels.Cms;
using System.Web.Mvc.Html;

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
                        using (new RegionContentWrapper(contentsBuilder, projection, model.CanManageContent))
                        {
                            var content = projection.GetHtml(htmlHelper);
                            contentsBuilder.Append(content);
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

        /// <summary>
        /// Renders the section as layout region.
        /// </summary>
        /// <param name="webPage">The web page.</param>
        /// <param name="partialViewHtml">The partial view HTML.</param>
        /// <param name="sectionName">Name of the section.</param>
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
        /// Renders the page custom JavaScript.
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
        
        /// <summary>
        /// Renders the page custom JavaScript.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="scripts">The scripts.</param>
        /// <returns></returns>
        public static IHtmlString RenderPageCustomJavaScript(this HtmlHelper htmlHelper, IEnumerable<IJavaScriptAccessor> scripts)
        {
            if (scripts != null)
            {
                var jsBuilder = new StringBuilder();

                foreach (var content in scripts)
                {
                    var contentJs = content.GetCustomJavaScript(htmlHelper);
                    if (!string.IsNullOrWhiteSpace(contentJs))
                    {
                        jsBuilder.Append(@"<script type=""text/javascript"" language=""javascript"">");
                        jsBuilder.Append(contentJs);
                        jsBuilder.AppendLine(@"</script>");
                    }
                }

                var js = jsBuilder.ToString();
                if (!string.IsNullOrWhiteSpace(js))
                {
                    return new HtmlString(js);
                }
            }

            return null;
        }

        /// <summary>
        /// The render style sheets.
        /// </summary>
        /// <param name="htmlHelper">The html helper.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The <see cref="IHtmlString"/>.</returns>
        public static IHtmlString RenderStyleSheets<T>(this HtmlHelper htmlHelper) where T : ModuleDescriptor
        {
            return htmlHelper.Action("RenderModuleStyleSheetIncludes", "Rendering", new { moduleDescriptorType = typeof(T) });
        }

        /// <summary>
        /// Renders the body attributes.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <returns>Rendered body attributes, required for page work with CMS panel</returns>
        public static IHtmlString RenderBodyAttributes(this HtmlHelper htmlHelper)
        {
            var attributes = string.Empty;
            var model = htmlHelper.ViewContext.ViewData.Model as RenderPageViewModel;

            if (model != null && model.CanManageContent)
            {
                attributes = string.Format(@" data-page-id = ""{0}""", model.Id);
            }

            return new MvcHtmlString(attributes);
        }
    }
}