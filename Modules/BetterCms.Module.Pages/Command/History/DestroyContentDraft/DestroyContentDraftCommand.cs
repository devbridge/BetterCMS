// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DestroyContentDraftCommand.cs" company="Devbridge Group LLC">
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
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Exceptions.DataTier;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.History.DestroyContentDraft
{
    public class DestroyContentDraftCommand : CommandBase, ICommand<DestroyContentDraftCommandRequest, DestroyContentDraftCommandResponse>
    {
        private IDraftService draftService;
        
        private IWidgetService widgetService;

        public DestroyContentDraftCommand(IDraftService draftService, IWidgetService widgetService)
        {
            this.draftService = draftService;
            this.widgetService = widgetService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="ConcurrentDataException"></exception>
        /// <exception cref="CmsException"></exception>
        public DestroyContentDraftCommandResponse Execute(DestroyContentDraftCommandRequest request)
        {
            var content = draftService.DestroyDraftContent(request.Id, request.Version, Context.Principal);

            var response = new DestroyContentDraftCommandResponse
                       {
                           PublishedId = content.Original.Id,
                           Id = content.Original.Id,
                           OriginalId = content.Original.Id,
                           Version = content.Original.Version,
                           OriginalVersion = content.Original.Version,
                           WidgetName = content.Original.Name,
                           IsPublished = true,
                           HasDraft = false,
                           DesirableStatus = ContentStatus.Published
                       };

            // Try to cast to widget
            // TODO Widget categories
//            var widget = content.Original as HtmlContentWidget;
//            if (widget != null && widget.Category != null && !widget.Category.IsDeleted)
//            {
//                response.CategoryName = widget.Category.Name;
//            }

            if (request.IncludeChildRegions)
            {
                response.Regions = widgetService.GetWidgetChildRegionViewModels(content.Original);
            }

            return response;
        }
    }
}