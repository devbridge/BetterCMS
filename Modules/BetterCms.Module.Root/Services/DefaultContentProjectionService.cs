using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Root.Commands.GetPageToRender;
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
            GetPageToRenderRequest renderPageRequest,
            PageContent pageContent,
            List<PageContent> allPageContents, 
            IChildContent childContent = null)
        {
            Models.Content contentToProject = null;
            var content = childContent == null ? pageContent.Content : (Models.Content)childContent.ChildContent;

            if (childContent == null 
                && renderPageRequest.PreviewPageContentId != null 
                && renderPageRequest.PreviewPageContentId.Value == pageContent.Id)
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

            if (contentToProject == null && (renderPageRequest.CanManageContent || renderPageRequest.PreviewPageContentId != null))
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
                if (!renderPageRequest.CanManageContent && htmlContent != null && (DateTime.Now < htmlContent.ActivationDate || (htmlContent.ExpirationDate.HasValue && htmlContent.ExpirationDate.Value < DateTime.Now)))
                {
                    // Invisible for user because of activation dates.
                    return null;
                }

                // Otherwise take published version.
                contentToProject = content;
            }

            if (contentToProject == null)
            {
                throw new CmsException(string.Format("A content version was not found to project on the page. PageContent={0}; Request={1};", pageContent, renderPageRequest));
            }

            // Create a collection of child regions (dynamic regions) contents projections
            var childRegionContentProjections = CreateListOfChildRegionContentProjectionsRecursively(renderPageRequest, pageContent, allPageContents);

            // Create a collection of child contents (child widgets) projections
            var childContentsProjections = CreateListOfChildProjectionsRecursively(renderPageRequest, pageContent, allPageContents, contentToProject.ChildContents);

            Func<IPageContent, IContent, IContentAccessor, IEnumerable<ChildContentProjection>, IEnumerable<PageContentProjection>, PageContentProjection> createProjectionDelegate;
            if (childContent != null)
            {
                createProjectionDelegate = (pc, c, a, ccpl, pcpl) => new ChildContentProjection(pc, childContent, a, ccpl, pcpl);
            }
            else
            {
                createProjectionDelegate = (pc, c, a, ccpl, pcpl) => new PageContentProjection(pc, c, a, ccpl, pcpl);
            }

            var options = optionService.GetMergedOptionValues(contentToProject.ContentOptions, pageContent.Options);
            return pageContentProjectionFactory.Create(pageContent, contentToProject, options, childContentsProjections, childRegionContentProjections, createProjectionDelegate);
        }

        private IEnumerable<ChildContentProjection> CreateListOfChildProjectionsRecursively(
            GetPageToRenderRequest renderPageRequest,
            PageContent pageContent,
            List<PageContent> allPageContents,
            IEnumerable<ChildContent> children)
        {
            List<ChildContentProjection> childProjections;
            if (children != null && children.Any(c => !c.Child.IsDeleted))
            {
                childProjections = new List<ChildContentProjection>();
                foreach (var child in children.Where(c => !c.Child.IsDeleted).Distinct())
                {
                    var childProjection = (ChildContentProjection)CreatePageContentProjection(renderPageRequest, pageContent, allPageContents, child);

                    childProjections.Add(childProjection);
                }
            }
            else
            {
                childProjections = null;
            }

            return childProjections;
        }

        private IList<PageContentProjection> CreateListOfChildRegionContentProjectionsRecursively(
            GetPageToRenderRequest renderPageRequest,
            PageContent pageContent,
            List<PageContent> allPageContents)
        {
            var childRegionContentProjections = new List<PageContentProjection>();
            var childRegionPageContents = allPageContents.Where(apc => apc.Parent != null && apc.Parent.Id == pageContent.Id);
            foreach (var childPageContent in childRegionPageContents)
            {
                var childRegionContentProjection = CreatePageContentProjection(renderPageRequest, childPageContent, allPageContents);
                childRegionContentProjections.Add(childRegionContentProjection);
            }

            return childRegionContentProjections;
        }
    }
}