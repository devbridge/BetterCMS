using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.seo.js module descriptor.
    /// </summary>
    public class TagsJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagsJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public TagsJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.pages.tags")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<CategoryController>(this, "loadSiteSettingsCategoryListUrl", c => c.Categories(null)),
                    new JavaScriptModuleLinkTo<TagsController>(this, "loadSiteSettingsTagListUrl", c => c.ListTags(null)),
                    new JavaScriptModuleLinkTo<TagsController>(this, "saveTagUrl", c => c.SaveTag(null)),
                    new JavaScriptModuleLinkTo<TagsController>(this, "deleteTagUrl", c => c.DeleteTag(null)),
                    new JavaScriptModuleLinkTo<CategoryController>(this, "saveCategoryUrl", c => c.SaveCategory(null)),
                    new JavaScriptModuleLinkTo<CategoryController>(this, "deleteCategoryUrl", c => c.DeleteCategory(null)),
                    new JavaScriptModuleLinkTo<TagsController>(this, "tagSuggestionSeviceUrl", c => c.SuggestTags(null))
                };

            Globalization = new IActionProjection[]
                {      
                    new JavaScriptModuleGlobalization(this, "confirmDeleteTagMessage", () => PagesGlobalization.SiteSettings_Tags_DeleteTagMessage), 
                    new JavaScriptModuleGlobalization(this, "confirmDeleteCategoryMessage", () => PagesGlobalization.SiteSettings_Categories_DeleteCategoryMessage), 
                };
        }
    }
}