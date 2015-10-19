using System.Linq;
using System.Text;
using BetterCms.Configuration;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;
using BetterCms.Module.Root.ViewModels.Cms;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Rendering;

namespace BetterCms.Module.Root.Mvc.ViewComponents
{
    public class SectionContentsViewComponent : ViewComponent
    {
        private readonly CmsConfigurationSection configuration;

        public SectionContentsViewComponent(CmsConfigurationSection configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Renders the section contents.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="razorPage">The razor page.</param>
        /// <param name="model">The model.</param>
        public void RenderInvoke(IHtmlHelper htmlHelper, RazorPage razorPage, RenderPageViewModel model)
        {
            var contentHtmlHelper = new PageContentRenderHelper(htmlHelper, configuration);

            foreach (var region in model.Regions)
            {
                var contentsBuilder = new StringBuilder();
                var projections =
                    model.Contents.Where(c => c.RegionId == region.RegionId).OrderBy(c => c.Order).ToList();

                using (new LayoutRegionWrapper(contentsBuilder, region, model.AreRegionsEditable))
                {
                    foreach (var projection in projections)
                    {
                        // Add content Html
                        using (
                            new RegionContentWrapper(contentsBuilder, projection, configuration,
                                model.CanManageContent && model.AreRegionsEditable))
                        {
                            contentsBuilder = contentHtmlHelper.AppendHtml(contentsBuilder, projection, model);
                        }
                    }
                }

                var pageHtmlHelper = new PageHtmlRenderer.PageHtmlRenderer(contentsBuilder, model);
                if (model.AreRegionsEditable)
                {
                    pageHtmlHelper.ReplaceRegionRepresentationHtml();
                }
                var html = pageHtmlHelper.GetReplacedHtml().ToString();

                RenderSectionAsLayoutRegion(razorPage, html, region.RegionIdentifier);
            }
        }

        /// <summary>
        /// Renders the section as layout region.
        /// </summary>
        /// <param name="razorPage">The razor page.</param>
        /// <param name="partialViewHtml">The partial view HTML.</param>
        /// <param name="sectionName">Name of the section.</param>
        private void RenderSectionAsLayoutRegion(RazorPage razorPage, string partialViewHtml, string sectionName)
        {
            razorPage.DefineSection(
                sectionName,
                async writer =>
                {
                    //Action<TextWriter> writerAction = tw => tw.Write(partialViewHtml);
                    //var result = new HelperResult(writerAction);
                    await writer.WriteAsync(partialViewHtml);
                });
        }
    }
}