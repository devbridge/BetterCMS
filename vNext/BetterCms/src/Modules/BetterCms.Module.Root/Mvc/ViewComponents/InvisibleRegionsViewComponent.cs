using BetterCms.Configuration;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;
using BetterCms.Module.Root.ViewModels.Cms;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;

namespace BetterCms.Module.Root.Mvc.ViewComponents
{
    public class InvisibleRegionsViewComponent: ViewComponent
    {
        private readonly CmsConfigurationSection configuration;

        public InvisibleRegionsViewComponent(CmsConfigurationSection configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Renders the invisible regions:.
        /// - Layout regions:
        /// -- When switching from layout A to layout B, and layout B has nor regions, which were in layout A
        /// -- When layout regions are deleted in Site Settings -> Page Layouts -> Templates
        /// - Widget regions:
        /// -- When region was deleted from the widget, and page has a content, assigned to that region
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="model">The  model.</param>
        /// <returns></returns>
        public HtmlString Invoke(IHtmlHelper htmlHelper, RenderPageViewModel model)
        {
            var childContentRenderHelper = new PageContentRenderHelper(htmlHelper, configuration);
            var html = childContentRenderHelper.RenderInvisibleRegions(model);
            
            return new HtmlString(html);
        }
    }
}