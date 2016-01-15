// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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
    /// bcms.user.js module descriptor.
    /// </summary>
    public class UserJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public UserJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.user")
        {
            Links = new IActionProjection[]
                        {
                            new JavaScriptModuleLinkTo<UserController>(this, "loadSiteSettingsUsersUrl", c => c.Index(null)),
                            new JavaScriptModuleLinkTo<UserController>(this, "loadEditUserUrl", c=> c.EditUser("{0}")), 
                            new JavaScriptModuleLinkTo<UserController>(this, "loadCreateUserUrl", c=> c.CreateUser()), 
                            new JavaScriptModuleLinkTo<UserController>(this, "deleteUserUrl", c => c.DeleteUser("{0}", "{1}"))
                        };

            Globalization = new IActionProjection[]
                        {
                            new JavaScriptModuleGlobalization(this, "usersListTabTitle", () => UsersGlobalization.SiteSettings_Users_ListTab_Title), 
                            new JavaScriptModuleGlobalization(this, "usersAddNewTitle", () => UsersGlobalization.CreateUser_Window_Title),
                            new JavaScriptModuleGlobalization(this, "deleteUserConfirmMessage", () => UsersGlobalization.DeleteUser_Confirmation_Message),
                            new JavaScriptModuleGlobalization(this, "editUserTitle", () => UsersGlobalization.EditUser_Window_Title),
                            new JavaScriptModuleGlobalization(this, "editUserProfileTitle", () => UsersGlobalization.EditUserProfile_Window_Title)
                        };
        }
    }
}