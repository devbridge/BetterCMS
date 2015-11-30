// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagsController.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Root.Commands.Tag.DeleteTag;
using BetterCms.Module.Root.Commands.Tag.GetTagList;
using BetterCms.Module.Root.Commands.Tag.SaveTag;
using BetterCms.Module.Root.Commands.Tag.SearchTags;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.Autocomplete;
using BetterCms.Module.Root.ViewModels.Tags;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Handles site settings logic for Pages module.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(RootModuleDescriptor.RootAreaName)]
    public class TagsController : CmsControllerBase
    {
        /// <summary>
        /// Renders a tag list for the site settings dialog.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Rendered tag list.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult ListTags(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetTagListCommand>().ExecuteCommand(request);
            return View(model);
        }

        /// <summary>
        /// An action to save the tag.
        /// </summary>
        /// <param name="tag">The tag data.</param>
        /// <returns>Json with status.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult SaveTag(TagItemViewModel tag)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveTagCommand>().ExecuteCommand(tag);
                if (response != null)
                {
                    if (tag.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(RootGlobalization.CreateTag_CreatedSuccessfully_Message);
                    }
                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// An action to delete a given tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>
        /// Json with status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult DeleteTag(TagItemViewModel tag)
        {
            bool success = GetCommand<DeleteTagCommand>().ExecuteCommand(                
                new DeleteTagCommandRequest
                    {
                        TagId = tag.Id,
                        Version = tag.Version
                    });

            if (success)
            {
                Messages.AddSuccess(RootGlobalization.DeleteTag_DeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }

        [HttpPost]
        public ActionResult SuggestTags(SuggestionViewModel model)
        {
            var suggestedTags = GetCommand<SearchTagsCommand>().ExecuteCommand(model);
            return Json(new { suggestions = suggestedTags });
        }
    }
}
