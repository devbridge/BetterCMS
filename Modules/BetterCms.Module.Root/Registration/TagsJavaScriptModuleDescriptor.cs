using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;

using BetterModules.Core.Web.Modules;

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
        public TagsJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.tags")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<TagsController>(this, "loadSiteSettingsTagListUrl", c => c.ListTags(null)),
                    new JavaScriptModuleLinkTo<TagsController>(this, "saveTagUrl", c => c.SaveTag(null)),
                    new JavaScriptModuleLinkTo<TagsController>(this, "deleteTagUrl", c => c.DeleteTag(null)),
                    new JavaScriptModuleLinkTo<TagsController>(this, "tagSuggestionServiceUrl", c => c.SuggestTags(null))
                };

            Globalization = new IActionProjection[]
                {      
                    new JavaScriptModuleGlobalization(this, "confirmDeleteTagMessage", () => RootGlobalization.SiteSettings_Tags_DeleteTagMessage), 
                    new JavaScriptModuleGlobalization(this, "confirmDeleteCategoryMessage", () => RootGlobalization.SiteSettings_Categories_DeleteCategoryMessage), 
                };
        }
    }
}