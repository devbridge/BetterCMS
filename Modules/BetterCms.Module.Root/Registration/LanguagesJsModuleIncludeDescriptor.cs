// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguagesJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.Web.Modules;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// bcms.languages.js module descriptor.
    /// </summary>
    public class LanguagesJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguagesJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public LanguagesJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.languages")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<LanguageController>(this, "loadSiteSettingsLanguagesUrl", c => c.ListTemplate()),
                    new JavaScriptModuleLinkTo<LanguageController>(this, "loadLanguagesUrl", c => c.LanguagesList(null)),
                    new JavaScriptModuleLinkTo<LanguageController>(this, "saveLanguageUrl", c => c.SaveLanguage(null)),
                    new JavaScriptModuleLinkTo<LanguageController>(this, "deleteLanguageUrl", c => c.DeleteLanguage(null, null)),
                    new JavaScriptModuleLinkTo<LanguageController>(this, "languageSuggestionUrl", c => c.SuggestLanguages(null))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "deleteLanguageConfirmMessage", () => RootGlobalization.DeleteLanguage_Confirmation_Message)
                };
        }
    }
}