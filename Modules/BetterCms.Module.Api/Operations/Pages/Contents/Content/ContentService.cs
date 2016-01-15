// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentService.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Operations.Pages.Contents.Content.Draft;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.History;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content
{
    public class ContentService : Service, IContentService
    {
        private readonly IHtmlContentService htmlService;

        private readonly IContentHistoryService historyService;
        
        private readonly IContentDraftService draftService;

        public ContentService(IHtmlContentService htmlService, IContentHistoryService historyService, IContentDraftService draftService)
        {
            this.htmlService = htmlService;
            this.historyService = historyService;
            this.draftService = draftService;
        }

        IHtmlContentService IContentService.Html
        {
            get
            {
                return htmlService;
            }
        }

        IContentHistoryService IContentService.History
        {
            get
            {
                return historyService;
            }
        }
        
        IContentDraftService IContentService.Draft
        {
            get
            {
                return draftService;
            }
        }
    }
}