// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveHtmlContentWidgetCommand.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Widgets;

namespace BetterCms.Module.Pages.Command.Widget.SaveWidget
{
    public class SaveHtmlContentWidgetCommand : SaveWidgetCommandBase<EditHtmlContentWidgetViewModel>
    {
        public virtual IWidgetService WidgetService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public override SaveWidgetResponse Execute(SaveWidgetCommandRequest<EditHtmlContentWidgetViewModel> request)
        {
            HtmlContentWidget widget;
            HtmlContentWidget originalWiget;
            WidgetService.SaveHtmlContentWidget(request.Content, request.ChildContentOptionValues, out widget, out originalWiget);

            var response = new SaveWidgetResponse
                    {
                        Id = widget.Id,
                        OriginalId = originalWiget.Id,
                        WidgetName = widget.Name,
                        Version = widget.Version,
                        OriginalVersion = originalWiget.Version,
                        WidgetType = WidgetType.HtmlContent.ToString(),
                        IsPublished = originalWiget.Status == ContentStatus.Published,
                        HasDraft = originalWiget.Status == ContentStatus.Draft || originalWiget.History != null && originalWiget.History.Any(f => f.Status == ContentStatus.Draft),
                        DesirableStatus = request.Content.DesirableStatus,
                        PreviewOnPageContentId = request.Content.PreviewOnPageContentId
                    };

            if (request.Content.IncludeChildRegions)
            {
                response.Regions = WidgetService.GetWidgetChildRegionViewModels(widget);
            }

            return response;
        }
    }
}