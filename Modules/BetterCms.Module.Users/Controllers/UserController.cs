// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserController.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using BetterCms.Module.Users.Commands.User.DeleteUser;
using BetterCms.Module.Users.Commands.User.GetUser;
using BetterCms.Module.Users.Commands.User.GetUsers;
using BetterCms.Module.Users.Commands.User.SaveUser;
using BetterCms.Module.Users.Content.Resources;

using BetterCms.Module.Users.ViewModels.User;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Users.Controllers
{
    /// <summary>
    /// User management.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.ManageUsers)]
    [ActionLinkArea(UsersModuleDescriptor.UsersAreaName)]
    public class UserController : CmsControllerBase
    {
        /// <summary>
        /// User list for Site Settings.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>User list view.</returns>
        public ActionResult Index(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetUsersCommand>().ExecuteCommand(request);

            return View(model);
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <returns>
        /// Create user view
        /// </returns>
        [HttpGet]
        public ActionResult CreateUser()
        {
            var model = GetCommand<GetUserCommand>().ExecuteCommand(System.Guid.Empty);
            var view = RenderView("EditUser", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edits the user.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>User edit view.</returns>
        [HttpGet]
        public ActionResult EditUser(string id)
        {
            var model = GetCommand<GetUserCommand>().ExecuteCommand(id.ToGuidOrDefault());
            var view = RenderView("EditUser", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json status result.</returns>
        [HttpPost]
        public ActionResult SaveUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveUserCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(UsersGlobalization.SaveUser_CreatedSuccessfully_Message);
                    }
                    return WireJson(true, response);
                }
            }

            return WireJson(false);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <returns>Json status result.</returns>
        [HttpPost]
        public ActionResult DeleteUser(string id, string version)
        {
            var success = GetCommand<DeleteUserCommand>().ExecuteCommand(
                new DeleteUserCommandRequest
                    {
                        UserId = id.ToGuidOrDefault(),
                        Version = version.ToIntOrDefault()
                    });

            if (success)
            {
                Messages.AddSuccess(UsersGlobalization.DeleteUser_DeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }
    }
}
