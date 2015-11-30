// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InsertContentToPageCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Content;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.Services;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Content.InsertContent
{
    public class InsertContentToPageCommand : CommandBase, ICommand<InsertContentToPageRequest, ChangedContentResultViewModel>
    {
        private readonly IContentService contentService;
        private readonly PageContentProjectionFactory projectionFactory;
        private readonly IWidgetService widgetService;

        public InsertContentToPageCommand(IContentService contentService, PageContentProjectionFactory projectionFactory,
            IWidgetService widgetService)
        {
            this.contentService = contentService;
            this.projectionFactory = projectionFactory;
            this.widgetService = widgetService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public ChangedContentResultViewModel Execute(InsertContentToPageRequest request)
        {
            UnitOfWork.BeginTransaction();
            
            var page = Repository.AsProxy<Root.Models.Page>(request.PageId);
            var region = Repository.AsProxy<Region>(request.RegionId);
            var content = Repository
                .AsQueryable<Root.Models.Content>()
                .Where(c => c.Id == request.ContentId)
                .FetchMany(c => c.ContentRegions)
                .FetchMany(c => c.History)
                .ToList()
                .FirstOne();
            
            PageContent parentPageContent = null;
            if (request.ParentPageContentId.HasValue && !request.ParentPageContentId.Value.HasDefaultValue())
            {
                parentPageContent = Repository.AsProxy<PageContent>(request.ParentPageContentId.Value);
            }

            var pageContent = new PageContent
                                {
                                    Page = page,
                                    Content = content,
                                    Region = region,
                                    Parent = parentPageContent
                                };
            pageContent.Order = contentService.GetPageContentNextOrderNumber(request.PageId, request.ParentPageContentId);


            Repository.Save(pageContent);
            UnitOfWork.Commit();

            // Notify.
            Events.PageEvents.Instance.OnPageContentInserted(pageContent);

            var accessor = projectionFactory.GetAccessorForType(content);

            var contentData = (pageContent.Content.History != null
                    ? pageContent.Content.History.FirstOrDefault(c => c.Status == ContentStatus.Draft) ?? pageContent.Content
                    : pageContent.Content);

            var model = new ChangedContentResultViewModel
                {
                    PageContentId = pageContent.Id,
                    ContentId = content.Id,
                    RegionId = request.RegionId,
                    PageId = request.PageId,
                    DesirableStatus = contentData.Status,
                    Title = contentData.Name,
                    ContentVersion = contentData.Version,
                    PageContentVersion = pageContent.Version,
                    ContentType = accessor != null ? accessor.GetContentWrapperType() : null
                };

            if (request.IncludeChildRegions)
            {
                model.Regions = widgetService.GetWidgetChildRegionViewModels(contentData);
            }

            return model;
        }
    }
}