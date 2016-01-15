// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoriesJavaScriptModuleDescriptor.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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