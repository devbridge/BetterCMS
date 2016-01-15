// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorController.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Blog.Commands.DeleteAuthor;
using BetterCms.Module.Blog.Commands.GetAuthorList;
using BetterCms.Module.Blog.Commands.SaveAuthor;
using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.ViewModels.Author;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Blog.Controllers
{
    /// <summary>
    /// Blog authors controller.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
    [ActionLinkArea(BlogModuleDescriptor.BlogAreaName)]
    public class AuthorController : CmsControllerBase
    {
        /// <summary>
        /// Lists the template.
        /// </summary>
        /// <returns>Json result.</returns>
        public ActionResult ListTemplate()
        {
            var view = RenderView("Partial/ListTemplate", null);
            var request = new SearchableGridOptions();
            request.SetDefaultPaging();

            var authors = GetCommand<GetAuthorListCommand>().ExecuteCommand(request);

            return ComboWireJson(authors != null, view, authors, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lists the authors.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Json result.</returns>
        public ActionResult AuthorsList(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetAuthorListCommand>().ExecuteCommand(request);
            return WireJson(model != null, model);
        }

        /// <summary>
        /// Saves the author.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        public ActionResult SaveAuthor(AuthorViewModel model)
        {
            var success = false;
            AuthorViewModel response = null;
            if (ModelState.IsValid)
            {
                response = GetCommand<SaveAuthorCommand>().ExecuteCommand(model);
                if (response != null)
                {
                    if (model.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(BlogGlobalization.CreateAuthor_CreatedSuccessfully_Message);
                    }

                    success = true;
                }
            }

            return WireJson(success, response);
        }

        /// <summary>
        /// Deletes the author.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <returns>Json result.</returns>
        [HttpPost]
        public ActionResult DeleteAuthor(string id, string version)
        {
            var request = new AuthorViewModel { Id = id.ToGuidOrDefault(), Version = version.ToIntOrDefault() };
            var success = GetCommand<DeleteAuthorCommand>().ExecuteCommand(request);
            if (success)
            {
                if (!request.Id.HasDefaultValue())
                {
                    Messages.AddSuccess(BlogGlobalization.DeleteAuthor_DeletedSuccessfully_Message);
                }
            }

            return WireJson(success);
        }
    }
}