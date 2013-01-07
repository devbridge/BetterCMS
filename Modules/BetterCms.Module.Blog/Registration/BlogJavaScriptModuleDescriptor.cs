using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Blog.Controllers;

namespace BetterCms.Module.Blog.Registration
{
    /// <summary>
    /// bcms.blog.js module descriptor.
    /// </summary>
    public class BlogJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public BlogJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.blog", "/file/bcms-blog/scripts/bcms.blog")
        {
            Links = new IActionProjection[]
                        {
                            new JavaScriptModuleLinkTo<BlogController>(this, "loadSiteSettingsBlogsUrl", c => c.Index())
                        };

            Globalization = new IActionProjection[] { };
        }
    }
}