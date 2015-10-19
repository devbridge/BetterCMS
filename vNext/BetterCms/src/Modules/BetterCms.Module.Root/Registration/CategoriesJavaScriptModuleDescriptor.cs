using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;
using BetterModules.Core.Web.Mvc.Extensions;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// bcms.categories.js module descriptor.
    /// </summary>
    public class CategoriesJavaScriptModuleDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="controllerExtensions">Controller extensions</param>
        public CategoriesJavaScriptModuleDescriptor(CmsModuleDescriptor module, ILoggerFactory loggerFactory, IControllerExtensions controllerExtensions)
            : base(module, "bcms.categories")
        {

            Links = new IActionUrlProjection[]
                {
                      new JavaScriptModuleLinkTo<CategoryController>(this, "loadSiteSettingsCategoryTreesListUrl", c => c.CategoryTrees(null), loggerFactory, controllerExtensions),
                      new JavaScriptModuleLinkTo<CategoryController>(this, "categoryTreeEditDialogUrl", c => c.EditCategoryTree("{0}"), loggerFactory, controllerExtensions),
                      new JavaScriptModuleLinkTo<CategoryController>(this, "saveCategoryTreeUrl", c => c.SaveCategoryTree(null), loggerFactory, controllerExtensions),
                      new JavaScriptModuleLinkTo<CategoryController>(this, "deleteCategoryTreeUrl", c => c.DeleteCategoryTree("{0}", "{1}"), loggerFactory, controllerExtensions),
                       new JavaScriptModuleLinkTo<CategoryController>(this, "categoriesSuggestionServiceUrl", c => c.SuggestCategories(null), loggerFactory, controllerExtensions)
                };

            Globalization = new IActionProjection[]
                {      
                    new JavaScriptModuleGlobalization(this, "categoryTreeCreatorDialogTitle", () => RootGlobalization.CategoryTree_CreatorDialog_Title, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "categoryTreeEditorDialogTitle", () => RootGlobalization.CategoryTree_EditorDialog_Title, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "nodeOkButton", () => RootGlobalization.Button_Ok, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "placeNodeHere", () => RootGlobalization.CategoryTree_PlaceLinkHere_Message, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "categoryTreeIsEmpty", () => RootGlobalization.CategoryTree_TreeIsEmpty_Message, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "deleteCategoryNodeConfirmationMessage", () => RootGlobalization.CategoryTree_DeleteNode_Confirmation_Message, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "someCategoryNodesAreInEditingState", () => RootGlobalization.CategoryTree_SomeNodesAreInEditingState, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "categoryTreeDeleteConfirmMessage", () => RootGlobalization.CategoryTree_Delete_Confirmation_Message, loggerFactory),
                };
        }
    }
}