using System;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Models;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Root.Services
{
    public class DefaultContentService : IContentService
    {
        /// <summary>
        /// The security service.
        /// </summary>
        private readonly ISecurityService securityService;

        public DefaultContentService(ISecurityService securityService)
        {
            this.securityService = securityService;
        }

        public virtual Models.Content Execute(Models.Content content)
        {
            if (request.DesirableStatus == ContentStatus.Archived)
            {
                throw new CmsException(string.Format("Can't save a content using an Archived status directly."));
            }

            if (request.Id.HasDefaultValue())
            {
                content = AddNewContent(request);
            }
            else
            {
                content = EditContent(request);
            }

            return content.Id;
        }

          private IPageContent AddNewContent(PageContentViewModel request)
        {
            var max = Repository.AsQueryable<PageContent>().Where(f => f.Page.Id == request.PageId && !f.IsDeleted).Select(f => (int?)f.Order).Max();

            if (max == null)
            {
                max = -1;
            }

            var pageContent = new PageContent();
            pageContent.Page = Repository.AsProxy<Root.Models.Page>(request.PageId);
            pageContent.Region = Repository.AsProxy<Region>(request.RegionId);
            pageContent.Order = max.Value + 1;
        
            pageContent.Content = new HtmlContent
            {
                Name = request.ContentName,
                ActivationDate = request.LiveFrom,
                ExpirationDate = request.LiveTo,
                Html = request.PageContent ?? string.Empty,
                Status = request.DesirableStatus
            };
            
            if (request.DesirableStatus == ContentStatus.Published)
            {
                pageContent.Content.PublishedOn = DateTime.Now;
                pageContent.Content.PublishedByUser = securityService.CurrentPrincipalName;
            }

            UnitOfWork.BeginTransaction();
            Repository.Save(pageContent);
            UnitOfWork.Commit();

            return pageContent;
        }

        private IPageContent EditContent(PageContentViewModel request)
        {
            var pageContent = Repository.AsQueryable<PageContent>()
                    .Fetch(f => f.Content)
                    .First(f => !f.IsDeleted && f.Id == request.Id);

            if (pageContent.Content.Status == ContentStatus.Archived)
            {
                throw new CmsException(string.Format("Can't directly edit a content in the Archived state."));
            }

            switch (request.DesirableStatus)
            {
                case ContentStatus.Preview:
                    EditContentSaveAsPreview(pageContent, request);
                    break;

                case ContentStatus.Draft:
                    EditContentSaveAsDraft(pageContent, request);
                    break;

                case ContentStatus.Published:
                    EditContentSaveAndPublish(pageContent, request);
                    break;

                default:
                    throw new CmsException(string.Format("A content status {0} is unknown as desirable status for a content.", request.DesirableStatus));
            }

            UnitOfWork.BeginTransaction();
            Repository.Save(pageContent);
            UnitOfWork.Commit();

            return pageContent;
        }

        private IPageContent EditContentSaveAsDraft(PageContent pageContent, PageContentViewModel request)
        {
            IContent content 
            if (pageContent.Content.Status != ContentStatus.Published && pageContent.Content.Status != ContentStatus.Draft)
            {
                pageContent.Content.Name = request.ContentName,
                ActivationDate = request.LiveFrom,
                ExpirationDate = request.LiveTo,
                Html = request.PageContent ?? string.Empty,
                Status = request.DesirableStatus
            }
            }
        }

        private IPageContent EditContentSaveAndPublish(PageContent pageContent, PageContentViewModel request)
        {

        }

        private IPageContent EditContentSaveAsPreview(PageContent pageContent, PageContentViewModel request)
        {

        }
    }
}