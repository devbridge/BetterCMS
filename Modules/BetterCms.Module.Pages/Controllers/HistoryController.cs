// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryController.cs" company="Devbridge Group LLC">
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
using System;
using System.Web.Mvc;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Command.History.DestroyContentDraft;
using BetterCms.Module.Pages.Command.History.GetContentHistory;
using BetterCms.Module.Pages.Command.History.GetContentVersion;
using BetterCms.Module.Pages.Command.History.GetContentVersionProperties;
using BetterCms.Module.Pages.Command.History.RestoreContentVersion;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

using NHibernate.Hql.Ast.ANTLR.Tree;

namespace BetterCms.Module.Pages.Controllers
{
    /// <summary>
    /// Content history management.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(PagesModuleDescriptor.PagesAreaName)]
    public class HistoryController : CmsControllerBase
    {
        /// <summary>
        /// Contents the history.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <returns>Content history view html.</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult ContentHistory(string contentId)
        {
            var model = GetCommand<GetContentHistoryCommand>().ExecuteCommand(new GetContentHistoryRequest
                                                                                      {
                                                                                          ContentId = contentId.ToGuidOrDefault(),
                                                                                      });
            return View(model);
        }

        /// <summary>
        /// Contents the history.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Content history view html.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent)]
        public ActionResult ContentHistory(GetContentHistoryRequest request)
        {
            var model = GetCommand<GetContentHistoryCommand>().ExecuteCommand(request);

            return View("ContentHistoryTable", model);
        }

        /// <summary>
        /// Contents the version.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Content preview html.</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult ContentVersion(string id)
        {
            var model = GetCommand<GetContentVersionCommand>().ExecuteCommand(id.ToGuidOrDefault());

            return View(model);
        }

        /// <summary>
        /// Shows content properties preview.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Content properties preview.</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult ContentVersionProperties(string id)
        {
            var result = GetCommand<GetContentVersionPropertiesCommand>().ExecuteCommand(id.ToGuidOrDefault());
            if (result != null)
            {
                return View(result.ViewName, result.ViewModel);
            }
            return null;
        }

        /// <summary>
        /// Restores the page content version.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="isUserConfirmed">Determines, if user is confirmed the restoring of a content version.</param>
        /// <param name="includeChildRegions">Determines, if child regions should be included to the results.</param>
        /// <returns>
        /// Json result.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.PublishContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult RestorePageContentVersion(string id, string isUserConfirmed, string includeChildRegions)
        {
            try
            {
                var request = new RestorePageContentViewModel
                {
                    PageContentId = id.ToGuidOrDefault(), 
                    IsUserConfirmed = isUserConfirmed.ToBoolOrDefault(),
                    IncludeChildRegions = includeChildRegions.ToBoolOrDefault()
                };
                var response = GetCommand<RestoreContentVersionCommand>().ExecuteCommand(request);

                return WireJson(response != null, response);
            }
            catch (ConfirmationRequestException exc)
            {
                return Json(new WireJson { Success = false, Data = new { ConfirmationMessage = exc.Resource() } });
            }
        }

        /// <summary>
        /// Destroys the content draft.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <param name="includeChildRegions">Determines, if child regions should be included to the results.</param>
        /// <returns>
        /// Json result.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult DestroyContentDraft(string id, string version, string includeChildRegions)
        {
            var request = new DestroyContentDraftCommandRequest
                              {
                                  Id = id.ToGuidOrDefault(),
                                  Version = version.ToIntOrDefault(),
                                  IncludeChildRegions = includeChildRegions.ToBoolOrDefault()
                              };
            var response = GetCommand<DestroyContentDraftCommand>().ExecuteCommand(request);

            return WireJson(response != null, response);
        }
    }
}
