// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SitemapJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.sitemap.js module descriptor.
    /// </summary>
    public class SitemapJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public SitemapJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages.sitemap")
        {            
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<SitemapController>(this, "loadSiteSettingsSitemapsListUrl", c => c.Sitemaps(null)),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "saveSitemapUrl", c => c.SaveSitemap(null)),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "saveSitemapNodeUrl", c => c.SaveSitemapNode(null)),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "deleteSitemapUrl", c => c.DeleteSitemap("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "deleteSitemapNodeUrl", c => c.DeleteSitemapNode(null)),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "sitemapEditDialogUrl", c => c.EditSitemap("{0}")),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "sitemapAddNewPageDialogUrl", c => c.AddNewPage()),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "saveMultipleSitemapsUrl", c => c.SaveMultipleSitemaps(null)),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "sitemapHistoryDialogUrl", c => c.ShowSitemapHistory("{0}")),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "loadSitemapVersionPreviewUrl", c => c.SitemapVersion("{0}")),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "restoreSitemapVersionUrl", c => c.RestoreSitemapVersion("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<SitemapController>(this, "getPageTranslations", c => c.GetPageTranslations("{0}")),
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "sitemapCreatorDialogTitle", () => NavigationGlobalization.Sitemap_CreatorDialog_Title),
                    new JavaScriptModuleGlobalization(this, "sitemapEditorDialogTitle", () => NavigationGlobalization.Sitemap_EditorDialog_Title),
                    new JavaScriptModuleGlobalization(this, "sitemapEditorDialogCustomLinkTitle", () => NavigationGlobalization.Sitemap_EditorDialog_CustomLinkTitle),
                    new JavaScriptModuleGlobalization(this, "sitemapAddNewPageDialogTitle", () => NavigationGlobalization.Sitemap_AddNewPageDialog_Title),
                    new JavaScriptModuleGlobalization(this, "sitemapDeleteNodeConfirmationMessage", () => NavigationGlobalization.Sitemap_DeleteNode_Confirmation_Message),
                    new JavaScriptModuleGlobalization(this, "sitemapDeleteConfirmMessage", () => NavigationGlobalization.Sitemap_Delete_Confirmation_Message),
                    new JavaScriptModuleGlobalization(this, "sitemapSomeNodesAreInEditingState", () => NavigationGlobalization.Sitemap_EditDialog_SomeNodesAreInEditingState),
                    new JavaScriptModuleGlobalization(this, "sitemapNodeSaveButton", () => RootGlobalization.Button_Save),
                    new JavaScriptModuleGlobalization(this, "sitemapNodeOkButton", () => RootGlobalization.Button_Ok),

                    new JavaScriptModuleGlobalization(this, "sitemapIsEmpty", () => NavigationGlobalization.Sitemap_SitemapIsEmpty_Message),
                    new JavaScriptModuleGlobalization(this, "sitemapPlaceLinkHere", () => NavigationGlobalization.Sitemap_NodeEdit_PlaceLinkHere),

                    new JavaScriptModuleGlobalization(this, "sitemapHistoryDialogTitle", () => NavigationGlobalization.Sitemap_HistoryDialog_Title),
                    new JavaScriptModuleGlobalization(this, "sitemapVersionRestoreConfirmation", () => NavigationGlobalization.Sitemap_HistoryDialog_RestoreConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "restoreButtonTitle", () => RootGlobalization.Button_Restore),
                    new JavaScriptModuleGlobalization(this, "closeButtonTitle", () => RootGlobalization.Button_Close),

                    new JavaScriptModuleGlobalization(this, "invariantLanguage", () => RootGlobalization.InvariantLanguage_Title)
                };
        }
    }
}