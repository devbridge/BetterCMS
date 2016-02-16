// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultContentProjectionService.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Projections;

using NHibernate.Proxy.DynamicProxy;

namespace BetterCms.Module.Root.Services
{
    public class DefaultContentProjectionService : IContentProjectionService
    {
        private readonly IOptionService optionService;

        private readonly PageContentProjectionFactory pageContentProjectionFactory;

        private readonly IUnitOfWork unitOfWork;

        public DefaultContentProjectionService(PageContentProjectionFactory pageContentProjectionFactory,
            IOptionService optionService, IUnitOfWork unitOfWork)
        {
            this.optionService = optionService;
            this.pageContentProjectionFactory = pageContentProjectionFactory;
            this.unitOfWork = unitOfWork;
        }

        public PageContentProjection CreatePageContentProjection(
            bool canManageContent,
            PageContent pageContent,
            List<PageContent> allPageContents, 
            IChildContent childContent = null,
            Guid? previewPageContentId = null, 
            Guid? languageId = null,
            bool retrieveCorrectVersion = true)
        {
            // Run logic of projection creation, because it's not created yet
            Models.Content contentToProject = null;
            var content = childContent == null ? pageContent.Content : (Models.Content)childContent.ChildContent;

            if (!retrieveCorrectVersion)
            {
                contentToProject = content;
            }
            else
            {
                if (childContent == null && previewPageContentId != null && previewPageContentId.Value == pageContent.Id)
                {
                    // Looks for the preview content version first.
                    if (content.Status == ContentStatus.Preview)
                    {
                        contentToProject = content;
                    }
                    else
                    {
                        contentToProject = content.History.FirstOrDefault(f => f.Status == ContentStatus.Preview);
                    }
                }

                if (contentToProject == null && (canManageContent || previewPageContentId != null))
                {
                    // Look for the draft content version if we are in the edit or preview mode.
                    if (content.Status == ContentStatus.Draft)
                    {
                        contentToProject = content;
                    }
                    else
                    {
                        contentToProject = content.History.FirstOrDefault(f => f.Status == ContentStatus.Draft);
                    }
                }

                if (contentToProject == null && content.Status == ContentStatus.Published)
                {
                    var htmlContent = content as IHtmlContent;
                    if (!canManageContent && htmlContent != null
                        && (DateTime.Now < htmlContent.ActivationDate || (htmlContent.ExpirationDate.HasValue && htmlContent.ExpirationDate.Value < DateTime.Now)))
                    {
                        // Invisible for user because of activation dates.
                        return null;
                    }

                    // Otherwise take published version.
                    contentToProject = content;
                }
            }

            if (contentToProject == null)
            {
                throw new CmsException(string.Format("A content version was not found to project on the page. PageContent={0}; CanManageContent={1}, PreviewPageContentId={2};", pageContent, canManageContent, previewPageContentId));
            }

            // Create a collection of child regions (dynamic regions) contents projections
            var childRegionContentProjections = CreateListOfChildRegionContentProjectionsRecursively(canManageContent, previewPageContentId, pageContent, allPageContents, contentToProject, languageId);

            // Create a collection of child contents (child widgets) projections
            var childContentsProjections = CreateListOfChildProjectionsRecursively(canManageContent, previewPageContentId, pageContent, allPageContents, contentToProject.ChildContents, languageId);

            Func<IPageContent, IContent, IContentAccessor, IEnumerable<ChildContentProjection>, IEnumerable<PageContentProjection>, PageContentProjection> createProjectionDelegate;
            if (childContent != null)
            {
                createProjectionDelegate = (pc, c, a, ccpl, pcpl) =>
                {
                    if (childContent.ChildContent is IProxy)
                    {
                        childContent.ChildContent = (IContent)unitOfWork.Session.GetSessionImplementation().PersistenceContext.Unproxy(childContent.ChildContent);
                    }
                    return new ChildContentProjection(pc, childContent, a, ccpl, pcpl);
                };
            }
            else
            {
                createProjectionDelegate = (pc, c, a, ccpl, pcpl) => new PageContentProjection(pc, c, a, ccpl, pcpl);
            }

            var optionValues = childContent != null ? childContent.Options : pageContent.Options;
            var options = optionService.GetMergedOptionValues(contentToProject.ContentOptions, optionValues, languageId);
            var contentProjection = pageContentProjectionFactory.Create(pageContent, contentToProject, options, childContentsProjections, childRegionContentProjections, createProjectionDelegate);

            return contentProjection;
        }

        private IEnumerable<ChildContentProjection> CreateListOfChildProjectionsRecursively(
            bool canManageContent, 
            Guid? previewPageContentId, 
            PageContent pageContent,
            List<PageContent> allPageContents,
            IEnumerable<ChildContent> children,
            Guid? languageId)
        {
            List<ChildContentProjection> childProjections;
            if (children != null && children.Any(c => !c.Child.IsDeleted))
            {
                childProjections = new List<ChildContentProjection>();
                foreach (var child in children.Where(c => !c.Child.IsDeleted).Distinct())
                {
                    var childProjection = (ChildContentProjection)CreatePageContentProjection(canManageContent, pageContent, allPageContents, child, previewPageContentId, languageId);
                    if (childProjection != null)
                    {
                        childProjections.Add(childProjection);
                    }
                }
            }
            else
            {
                childProjections = null;
            }

            return childProjections;
        }

        private IList<PageContentProjection> CreateListOfChildRegionContentProjectionsRecursively(
            bool canManageContent,
            Guid? previewPageContentId, 
            PageContent pageContent,
            List<PageContent> allPageContents,
            Models.Content contentToProject,
            Guid? languageId)
        {
            var childRegionContentProjections = new List<PageContentProjection>();
            var childRegionPageContents = allPageContents.Where(apc => apc.Parent != null 
                && apc.Parent.Id == pageContent.Id
                && contentToProject.ContentRegions != null
                && contentToProject.ContentRegions.Any(cr => cr.Region == apc.Region));
            foreach (var childPageContent in childRegionPageContents)
            {
                var childRegionContentProjection = CreatePageContentProjection(canManageContent, childPageContent, allPageContents, null, previewPageContentId, languageId);
                childRegionContentProjections.Add(childRegionContentProjection);
            }

            return childRegionContentProjections;
        }
    }
}