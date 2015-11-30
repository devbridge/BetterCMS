// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleController.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Root.ViewModels.Autocomplete;
using BetterCms.Module.Users.Commands.Role.DeleteRole;
using BetterCms.Module.Users.Commands.Role.GetRoles;
using BetterCms.Module.Users.Commands.Role.SaveRole;
using BetterCms.Module.Users.Commands.Role.SearchRoles;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.ViewModels.Role;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Users.Controllers
{
    /// <summary>
    /// Role management.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.ManageUsers)]
    [ActionLinkArea(UsersModuleDescriptor.UsersAreaName)]
    public class RoleController : CmsControllerBase
    {
        /// <summary>
        /// Lists the template.
        /// </summary>
        /// <returns>Json result.</returns>
        [HttpGet]
        public ActionResult ListTemplate()
        {
            var request = new SearchableGridOptions();
            request.SetDefaultPaging();

            var view = RenderView("Partial/ListTemplate", null);
            var roles = GetCommand<GetRolesCommand>().ExecuteCommand(request);

            return ComboWireJson(roles != null, view, roles, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the list of roles.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Json result.</returns>
        public ActionResult RolesList(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetRolesCommand>().ExecuteCommand(request);

            return WireJson(model != null, model);
        }

        /// <summary>
        /// An action to delete a given role.
        /// </summary>
        /// <param name="id">The role id.</param>
        /// <param name="version">The version.</param>
        /// <returns>Json with status.</returns>
        [HttpPost]
        public ActionResult DeleteRole(string id, string version)
        {
            bool success = GetCommand<DeleteRoleCommand>().ExecuteCommand(
                new DeleteRoleCommandRequest
                {
                    RoleId = id.ToGuidOrDefault(),
                    Version = version.ToIntOrDefault()
                });

            if (success)
            {
                Messages.AddSuccess(UsersGlobalization.DeleteRole_DeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }

        /// <summary>
        /// Saves the role.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.ManageUsers)]
        public ActionResult SaveRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveRoleCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(UsersGlobalization.SaveRole_CreatedSuccessfully_Message);
                    }

                    return WireJson(true, response);
                }
            }

            return WireJson(false);
        }

        /// <summary>
        /// Suggests the tags.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.ManageUsers)]
        public ActionResult SuggestRoles(SuggestionViewModel model)
        {
            var suggestedRoles = GetCommand<SearchRolesCommand>().ExecuteCommand(model);

            return Json(new { suggestions = suggestedRoles });
        }
    }
}
