// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagesLanguagesJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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
    /// bcms.pages.languages.js module descriptor.
    /// </summary>
    public class PagesLanguagesJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagesLanguagesJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public PagesLanguagesJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages.languages")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<PageController>(this, "suggestUntranslatedPagesUrl", c => c.SuggestUntranslatedPages(null)),
                    new JavaScriptModuleLinkTo<PageController>(this, "searchUntranslatedPagesUrl", c => c.SearchUntranslatedPages(null))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "unassignTranslationConfirmation", () => PagesGlobalization.EditPageTranslations_UnassignTranslation_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "invariantLanguage", () => RootGlobalization.InvariantLanguage_Title),
                    new JavaScriptModuleGlobalization(this, "replaceItemWithCurrentLanguageConfirmation", () => PagesGlobalization.EditPageTranslations_ReplaceTranslationWithCurrentLanguage_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "replaceItemWithLanguageConfirmation", () => PagesGlobalization.EditPageTranslations_ReplaceTranslationWithLanguage_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "assigningPageHasSameCultureAsCurrentPageMessage", () => PagesGlobalization.EditPageTranslations_PageHasSameCulture_Message),
                };
        }
    }
}