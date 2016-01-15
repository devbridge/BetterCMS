// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedirectsJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Pages.Registration
{
    public class RedirectsJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectsJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public RedirectsJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages.redirects")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<RedirectController>(this, "loadSiteSettingsRedirectListUrl", c => c.Redirects(null)),
                    new JavaScriptModuleLinkTo<RedirectController>(this, "deleteRedirectUrl", c => c.DeleteRedirect(null)),
                    new JavaScriptModuleLinkTo<RedirectController>(this, "saveRedirectUrl", c => c.SaveRedirect(null))
                };

            Globalization = new IActionProjection[]
                {      
                    new JavaScriptModuleGlobalization(this, "deleteRedirectMessage", () => PagesGlobalization.DeleteRedirect_Confirmation_Message)
                };
        }
    }
}