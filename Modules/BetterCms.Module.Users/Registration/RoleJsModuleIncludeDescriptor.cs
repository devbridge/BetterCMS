// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.Controllers;

namespace BetterCms.Module.Users.Registration
{
    /// <summary>
    /// bcms.role.js module descriptor.
    /// </summary>
    public class RoleJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public RoleJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.role")
        {
            Links = new IActionProjection[]
                        {
                            new JavaScriptModuleLinkTo<RoleController>(this, "saveRoleUrl", c => c.SaveRole(null)),
                            new JavaScriptModuleLinkTo<RoleController>(this, "deleteRoleUrl", c=> c.DeleteRole("{0}", "{1}")),
                            new JavaScriptModuleLinkTo<RoleController>(this, "loadSiteSettingsRoleUrl", c => c.ListTemplate()),
                            new JavaScriptModuleLinkTo<RoleController>(this, "loadRolesUrl", c => c.RolesList(null)),
                            new JavaScriptModuleLinkTo<RoleController>(this, "roleSuggestionServiceUrl", c=> c.SuggestRoles(null))
                        };

            Globalization = new IActionProjection[]
                        {
                            new JavaScriptModuleGlobalization(this, "rolesListTabTitle", () => UsersGlobalization.SiteSettings_Roles_ListTab_Title),
                            new JavaScriptModuleGlobalization(this, "deleteRoleConfirmMessage" , ()=> UsersGlobalization.DeleteRole_Confirmation_Message), 
                        };
        }
    }
}