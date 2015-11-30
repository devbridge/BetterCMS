// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserProfileController.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Commands.User.GetUserProfile;
using BetterCms.Module.Users.Commands.User.SaveUserProfile;
using BetterCms.Module.Users.ViewModels.User;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Users.Controllers
{
    /// <summary>
    /// User profile management.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(UsersModuleDescriptor.UsersAreaName)]
    public class UserProfileController : CmsControllerBase
    {
        /// <summary>
        /// Edit the user profile.
        /// </summary>
        /// <returns>
        /// Edit user profile view.
        /// </returns>
        [HttpGet]
        public ActionResult EditProfile()
        {
            var model = GetCommand<GetUserProfileCommand>().ExecuteCommand(User.Identity.Name);
            var view = RenderView("EditUserProfile", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the user profile.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Json status result
        /// </returns>
        [HttpPost]
        public ActionResult SaveUserProfile(EditUserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var success = GetCommand<SaveUserProfileCommand>().ExecuteCommand(model);

                return WireJson(success);
            }

            return WireJson(false);
        }
    }
}