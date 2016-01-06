using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;

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
        public CategoriesJavaScriptModuleDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.categories")
        {

            Links = new IActionProjection[]
                {
                      new JavaScriptModuleLinkTo<CategoryController>(this, "loadSiteSettingsCategoryTreesListUrl", c => c.CategoryTrees(null)),
                      new JavaScriptModuleLinkTo<CategoryController>(this, "categoryTreeEditDialogUrl", c => c.EditCategoryTree("{0}")),
                      new JavaScriptModuleLinkTo<CategoryController>(this, "saveCategoryTreeUrl", c => c.SaveCategoryTree(null)),
                      new JavaScriptModuleLinkTo<CategoryController>(this, "deleteCategoryTreeUrl", c => c.DeleteCategoryTree("{0}", "{1}")),
                       new JavaScriptModuleLinkTo<CategoryController>(this, "categoriesSuggestionServiceUrl", c => c.SuggestCategories(null))
                };

            Globalization = new IActionProjection[]
                {      
                    new JavaScriptModuleGlobalization(this, "categoryTreeCreatorDialogTitle", () => RootGlobalization.CategoryTree_CreatorDialog_Title),
                    new JavaScriptModuleGlobalization(this, "categoryTreeEditorDialogTitle", () => RootGlobalization.CategoryTree_EditorDialog_Title),
                    new JavaScriptModuleGlobalization(this, "nodeOkButton", () => RootGlobalization.Button_Ok),
                    new JavaScriptModuleGlobalization(this, "placeNodeHere", () => RootGlobalization.CategoryTree_PlaceLinkHere_Message),
                    new JavaScriptModuleGlobalization(this, "categoryTreeIsEmpty", () => RootGlobalization.CategoryTree_TreeIsEmpty_Message),
                    new JavaScriptModuleGlobalization(this, "deleteCategoryNodeConfirmationMessage", () => RootGlobalization.CategoryTree_DeleteNode_Confirmation_Message),
                    new JavaScriptModuleGlobalization(this, "someCategoryNodesAreInEditingState", () => RootGlobalization.CategoryTree_SomeNodesAreInEditingState),
                    new JavaScriptModuleGlobalization(this, "categoryTreeDeleteConfirmMessage", () => RootGlobalization.CategoryTree_Delete_Confirmation_Message),
                };
        }
    }
}