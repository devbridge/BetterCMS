using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;

using BetterModules.Core.Web.Modules;
using BetterModules.Core.Web.Mvc.Extensions;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// bcms.tags.js module descriptor.
    /// </summary>
    public class TagsJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagsJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="controllerExtensions">Controller extensions</param>
        public TagsJsModuleIncludeDescriptor(CmsModuleDescriptor module, ILoggerFactory loggerFactory, IControllerExtensions controllerExtensions)
            : base(module, "bcms.tags")
        {

            Links = new IActionUrlProjection[]
                {
                    new JavaScriptModuleLinkTo<TagsController>(this, "loadSiteSettingsTagListUrl", c => c.ListTags(null), loggerFactory, controllerExtensions),
                    new JavaScriptModuleLinkTo<TagsController>(this, "saveTagUrl", c => c.SaveTag(null), loggerFactory, controllerExtensions),
                    new JavaScriptModuleLinkTo<TagsController>(this, "deleteTagUrl", c => c.DeleteTag(null), loggerFactory, controllerExtensions),
                    new JavaScriptModuleLinkTo<TagsController>(this, "tagSuggestionServiceUrl", c => c.SuggestTags(null), loggerFactory, controllerExtensions)
                };

            Globalization = new IActionProjection[]
                {      
                    new JavaScriptModuleGlobalization(this, "confirmDeleteTagMessage", () => RootGlobalization.SiteSettings_Tags_DeleteTagMessage, loggerFactory), 
                    new JavaScriptModuleGlobalization(this, "confirmDeleteCategoryMessage", () => RootGlobalization.SiteSettings_Categories_DeleteCategoryMessage, loggerFactory) 
                };
        }
    }
}