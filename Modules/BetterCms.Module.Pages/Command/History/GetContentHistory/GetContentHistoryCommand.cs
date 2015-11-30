// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetContentHistoryCommand.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Security;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.History;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.History.GetContentHistory
{
    /// <summary>
    /// Command to load a list of the content history versions.
    /// </summary>
    public class GetContentHistoryCommand : CommandBase, ICommand<GetContentHistoryRequest, ContentHistoryViewModel>
    {
        /// <summary>
        /// The history service
        /// </summary>
        private readonly IHistoryService historyService;

        /// <summary>
        /// The CMS configuration
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContentHistoryCommand" /> class.
        /// </summary>
        /// <param name="historyService">The history service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public GetContentHistoryCommand(IHistoryService historyService, ICmsConfiguration cmsConfiguration)
        {
            this.historyService = historyService;
            this.cmsConfiguration = cmsConfiguration;
        }

        private bool userHasPublishRole { get; set; }
        private bool userHasAdministrationRole { get; set; }
        private bool hasAccessLevel { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The view model with list of history view models</returns>
        public ContentHistoryViewModel Execute(GetContentHistoryRequest request)
        {
            userHasPublishRole = SecurityService.IsAuthorized(RootModuleConstants.UserRoles.PublishContent);
            userHasAdministrationRole = SecurityService.IsAuthorized(RootModuleConstants.UserRoles.Administration);

            var historyEntities = historyService.GetContentHistory(request.ContentId, request);

            hasAccessLevel = HasAccessLevel(historyEntities);

            var history = historyEntities.Select(Convert).ToList();
            return new ContentHistoryViewModel(history, request, history.Count, request.ContentId);
        }

        private ContentHistoryItem Convert(Root.Models.Content content)
        {
            var canRestore = content.Status == ContentStatus.Archived;

            var contentType = content.GetType();
            if (contentType == typeof(HtmlContentWidget) || contentType == typeof(ServerControlWidget))
            {
                canRestore = hasAccessLevel && canRestore && userHasAdministrationRole;
            }
            else
            {
                canRestore = hasAccessLevel && canRestore && userHasPublishRole;
            }            

            return new ContentHistoryItem
                       {
                           Id = content.Id,
                           Version = content.Version,
                           StatusName = historyService.GetStatusName(content.Status),
                           Status = content.Status,
                           ArchivedByUser = content.Status == ContentStatus.Archived ? content.CreatedByUser : null,
                           ArchivedOn = content.Status == ContentStatus.Archived ? content.CreatedOn : (DateTime?)null,
                           DisplayedFor = content.Status == ContentStatus.Archived && content.PublishedOn != null
                                   ? content.CreatedOn - content.PublishedOn.Value
                                   : (TimeSpan?)null,
                           PublishedByUser = content.Status == ContentStatus.Published || content.Status == ContentStatus.Archived ? content.PublishedByUser : null,
                           PublishedOn = content.Status == ContentStatus.Published || content.Status == ContentStatus.Archived ? content.PublishedOn : null,
                           CreatedOn = content.CreatedOn,
                           CanCurrentUserRestoreIt = canRestore
                       };
        }

        private bool HasAccessLevel(IList<Root.Models.Content> contents)
        {
            if (!cmsConfiguration.Security.AccessControlEnabled)
            {
                return true;
            }

            if (contents.Count > 0)
            {
                var content = contents[0].Original ?? contents[0];
                if (content != null)
                {
                    var pageContent = Repository.AsQueryable<Root.Models.PageContent>()
                            .Where(x => x.Content == content && !x.IsDeleted)
                            .Fetch(x => x.Page)
                            .ThenFetchMany(x => x.AccessRules)
                            .ToList()
                            .FirstOrDefault();

                    if (pageContent != null)
                    {
                        return AccessControlService.GetAccessLevel(pageContent.Page, Context.Principal) >= AccessLevel.ReadWrite;
                    }
                }
            }

            return true;
        }
    }
}