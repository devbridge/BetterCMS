// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SavePagePublishStatusCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Web.Mvc.Commands;


namespace BetterCms.Module.Pages.Command.Page.SavePagePublishStatus
{
    /// <summary>
    /// Command to updated page status.
    /// </summary>
    public class SavePagePublishStatusCommand : CommandBase, ICommand<SavePagePublishStatusRequest, bool>
    {
        /// <summary>
        /// The content service.
        /// </summary>
        private readonly IContentService contentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePagePublishStatusCommand"/> class.
        /// </summary>
        /// <param name="contentService">The content service.</param>
        public SavePagePublishStatusCommand(IContentService contentService)
        {
            this.contentService = contentService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> if succeeded, otherwise <c>false</c></returns>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">If page status is not correct.</exception>
        public bool Execute(SavePagePublishStatusRequest request)
        {
            AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.PublishContent);     // This would rise security exception if user has no access.

            var page = UnitOfWork.Session
                .QueryOver<PageProperties>().Where(p => p.Id == request.PageId && !p.IsDeleted)
                .SingleOrDefault<PageProperties>();

            if (page != null)
            {
                if (page.IsMasterPage)
                {
                    return false;
                }

                var initialStatus = page.Status;

                if (page.Status == PageStatus.Draft || page.Status == PageStatus.Preview)
                {
                    var message = string.Format(PagesGlobalization.SavePageStatus_PageIsInInappropriateStatus_Message);
                    var logMessage = string.Format("Draft/Preview page id={0} can not be published.", page.Id);
                    throw new ValidationException(() => message, logMessage);
                }

                if (request.IsPublished)
                {
                    page.Status = PageStatus.Published;
                    page.PublishedOn = DateTime.Now;
                }
                else
                {
                    page.Status = PageStatus.Unpublished;
                }

                // NOTE: When transaction is enabled exception is raised from DefaultEntityTrackingService.DemandReadWriteRule() saying that DB timeouted...
                // UnitOfWork.BeginTransaction();

                Repository.Save(page);

                if (request.IsPublished)
                {
                    contentService.PublishDraftContent(page.Id);
                }

                UnitOfWork.Commit();

                if (page.Status != initialStatus)
                {
                    Events.PageEvents.Instance.OnPagePublishStatusChanged(page);
                }
            }

            return true;
        }
    }
}