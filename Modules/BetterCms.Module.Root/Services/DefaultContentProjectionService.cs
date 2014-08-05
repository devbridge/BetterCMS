using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Projections;

namespace BetterCms.Module.Root.Services
{
    public class DefaultContentProjectionService : IContentProjectionService
    {
        private readonly IOptionService optionService;

        private readonly PageContentProjectionFactory pageContentProjectionFactory;

        public DefaultContentProjectionService(PageContentProjectionFactory pageContentProjectionFactory,
            IOptionService optionService)
        {
            this.optionService = optionService;
            this.pageContentProjectionFactory = pageContentProjectionFactory;
        }

        public PageContentProjection CreatePageContentProjection(
            bool canManageContent,
            PageContent pageContent,
            IChildContent childContent = null,
            Guid? previewPageContentId = null)
        {
            Models.Content contentToProject = null;
            var content = childContent == null ? pageContent.Content : (Models.Content)childContent.ChildContent;

            if (childContent == null
                && previewPageContentId != null
                && previewPageContentId.Value == pageContent.Id)
            {
                // Looks for the preview content version first.
                if (pageContent.Content.Status == ContentStatus.Preview)
                {
                    contentToProject = pageContent.Content;
                }
                else
                {
                    contentToProject = pageContent.Content.History.FirstOrDefault(f => f.Status == ContentStatus.Preview);
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
                IHtmlContent htmlContent = content as IHtmlContent;
                if (!canManageContent && htmlContent != null && (DateTime.Now < htmlContent.ActivationDate || (htmlContent.ExpirationDate.HasValue && htmlContent.ExpirationDate.Value < DateTime.Now)))
                {
                    // Invisible for user because of activation dates.
                    return null;
                }

                // Otherwise take published version.
                contentToProject = content;
            }

            if (contentToProject == null)
            {
                throw new CmsException(string.Format("A content version was not found to project on the page. PageContent={0}; CanManageContent={1}, PreviewPageContentId={2};", pageContent, canManageContent, previewPageContentId));
            }

            // Create a collection of child contents (child widgets) projections
            var childContentsProjections = CreateListOfChildProjectionsRecursively(canManageContent, previewPageContentId, pageContent, contentToProject.ChildContents);

            Func<IPageContent, IContent, IContentAccessor, IEnumerable<ChildContentProjection>, PageContentProjection> createProjectionDelegate;
            if (childContent != null)
            {
                createProjectionDelegate = (pc, c, a, ccpl) => new ChildContentProjection(pc, childContent, a, ccpl);
            }
            else
            {
                createProjectionDelegate = (pc, c, a, ccpl) => new PageContentProjection(pc, c, a, ccpl);
            }

            var optionValues = childContent != null ? childContent.Options : pageContent.Options;
            var options = optionService.GetMergedOptionValues(contentToProject.ContentOptions, optionValues);
            return pageContentProjectionFactory.Create(pageContent, contentToProject, options, childContentsProjections, createProjectionDelegate);
        }

        private IEnumerable<ChildContentProjection> CreateListOfChildProjectionsRecursively(
            bool canManageContent, 
            Guid? previewPageContentId, 
            PageContent pageContent,
            IEnumerable<ChildContent> children)
        {
            List<ChildContentProjection> childProjections;
            if (children != null && children.Any(c => !c.Child.IsDeleted))
            {
                childProjections = new List<ChildContentProjection>();
                foreach (var child in children.Where(c => !c.Child.IsDeleted).Distinct())
                {
                    var childProjection = (ChildContentProjection)CreatePageContentProjection(canManageContent, pageContent, child, previewPageContentId);
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
    }
}