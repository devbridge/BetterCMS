// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedirectController.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Pages.Command.Redirect.DeleteRedirect;
using BetterCms.Module.Pages.Command.Redirect.GetRedirectsList;
using BetterCms.Module.Pages.Command.Redirect.SaveRedirect;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Pages.Controllers
{
    [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
    [ActionLinkArea(PagesModuleDescriptor.PagesAreaName)]
    public class RedirectController : CmsControllerBase
    {
        /// <summary>
        /// Renders a redirects list for the site settings dialog.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Rendered redirects list.
        /// </returns>
        public ActionResult Redirects(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetRedirectsListCommand>().ExecuteCommand(request);
            return View(model);
        }

        /// <summary>
        /// Deletes redirect.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        public ActionResult DeleteRedirect(SiteSettingRedirectViewModel model)
        {
            var success = GetCommand<DeleteRedirectCommand>().ExecuteCommand(model);
            if (success)
            {
                Messages.AddSuccess(PagesGlobalization.DeleteRedirect_DeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }

        /// <summary>
        /// Saves redirect.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json with result  status.</returns>
        [HttpPost]
        public ActionResult SaveRedirect(SiteSettingRedirectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveRedirectCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(PagesGlobalization.CreateRedirect_CreatedSuccessfully_Message);
                    }
                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }
    }
}
