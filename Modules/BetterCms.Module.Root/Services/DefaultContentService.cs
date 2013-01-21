using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Models;
using BetterCms.Core.Services;

using NHibernate.Linq;

namespace BetterCms.Module.Root.Services
{
    public class DefaultContentService : IContentService
    {
        /// <summary>
        /// A security service.
        /// </summary>
        private readonly ISecurityService securityService;

        /// <summary>
        /// A repository contract.
        /// </summary>
        private readonly IRepository repository;

        public DefaultContentService(ISecurityService securityService, IRepository repository)
        {
            this.securityService = securityService;
            this.repository = repository;
        }

        public Models.Content SaveContentWithStatusUpdate(Models.Content updatedContent, ContentStatus requestedStatus)
        {
            if (updatedContent == null)
            {
                throw new CmsException("Nothing to save.", new ArgumentNullException("updatedContent"));
            }

            if (requestedStatus == ContentStatus.Archived)
            {
                throw new CmsException(string.Format("Can't switch a content to the Archived state directly."));
            }


            if (updatedContent.Id == default(Guid))
            {
                /* Just create a new content with requested status.*/
                if (requestedStatus == ContentStatus.Published)
                {
                    updatedContent.PublishedOn = DateTime.Now;
                    updatedContent.PublishedByUser = securityService.CurrentPrincipalName;
                }

                updatedContent.Status = requestedStatus;
                repository.Save(updatedContent);

                return updatedContent;
            }
            
            var originalContent =
                repository.AsQueryable<Models.Content>()
                          .Fetch(f => f.Original)
                          .FetchMany(f => f.History)
                          .Where(f => f.Id == updatedContent.Id && !f.IsDeleted)
                          .ToList()
                          .FirstOrDefault();

            if (originalContent == null)
            {
                throw new CmsException(string.Format("An original content was not found by id={0}.", updatedContent.Id));
            }

            originalContent = repository.UnProxy(originalContent);

            /* Update existing content. */
            switch (originalContent.Status)
            {
                case ContentStatus.Published:
                    SavePublishedContentWithStatusUpdate(originalContent, updatedContent, requestedStatus);
                    break;

                case ContentStatus.Preview:
                case ContentStatus.Draft:
                    SavePreviewOrDraftContentWithStatusUpdate(originalContent, updatedContent, requestedStatus);
                    break;

                case ContentStatus.Archived:
                    throw new CmsException(string.Format("Can't edit a content in the {0} state.", originalContent.Status));

                default:
                    throw new CmsException(string.Format("Unknown content status {0}.", updatedContent.Status), new NotSupportedException());
            }

            return originalContent;
        }

        private void SavePublishedContentWithStatusUpdate(Models.Content originalContent, Models.Content updatedContent, ContentStatus requestedStatus)
        {            
            /* 
             * Edit published content:
             * -> Save as draft, preview - look for draft|preview version in history list or create a new history version with requested status (draft, preview) with reference to an original content.
             * -> Publish - current published version should be cloned to archive version with reference to original (archive state) and original updated with new data (published state).
             *              Look for preview|draft versions - if exists remote it.
             */
            if (requestedStatus == ContentStatus.Preview || requestedStatus == ContentStatus.Draft)
            {
                var contentVersionOfRequestedStatus = originalContent.History.FirstOrDefault(f => f.Status == requestedStatus && !f.IsDeleted);
                if (contentVersionOfRequestedStatus == null)
                {
                    contentVersionOfRequestedStatus = originalContent.Clone();
                }

                updatedContent.CopyDataTo(contentVersionOfRequestedStatus);
                contentVersionOfRequestedStatus.Original = originalContent;
                contentVersionOfRequestedStatus.Status = requestedStatus;
                repository.Save(contentVersionOfRequestedStatus);                
            }
            
            if (requestedStatus == ContentStatus.Published)
            {
                var originalToArchive = originalContent.Clone();
                originalToArchive.Status = ContentStatus.Archived;
                originalToArchive.Original = originalContent;
                repository.Save(originalToArchive);

                updatedContent.CopyDataTo(originalContent);
                originalContent.Status = requestedStatus;
                originalContent.PublishedOn = DateTime.Now;
                originalContent.PublishedByUser = securityService.CurrentPrincipalName;
                repository.Save(originalContent);

                foreach (var previewOrDraftVersionContent in originalContent.History.Where(f => f.Status == ContentStatus.Preview || f.Status == ContentStatus.Draft))
                {
                    repository.Delete(previewOrDraftVersionContent);
                }
            }
        }

        private void SavePreviewOrDraftContentWithStatusUpdate(Models.Content originalContent, Models.Content updatedContent, ContentStatus requestedStatus)
        {
            /* 
             * Edit preview or draft content:
             * -> Save as preview or draft - look for preview or draft version in a history list or create a new history version with requested preview status with reference to an original content.
             * -> Save draft - just update field and save.
             * -> Publish - look if the published content (look for original) exists:
             *              - published content exits:
             *                  | create a history content version of the published (clone it). update original with draft data and remove draft|preview.
             *              - published content not exists:
             *                  | save draft content as published
             */
            if (requestedStatus == ContentStatus.Preview || requestedStatus == ContentStatus.Draft)
            {
                var previewOrDraftContentVersion = originalContent.History.FirstOrDefault(f => f.Status == requestedStatus && !f.IsDeleted);
                if (previewOrDraftContentVersion == null)
                {
                    if (originalContent.Original == null)
                    {
                        previewOrDraftContentVersion = originalContent;
                    }
                    else
                    {
                        previewOrDraftContentVersion = originalContent.Clone();
                        previewOrDraftContentVersion.Original = originalContent;
                    }
                }

                updatedContent.CopyDataTo(previewOrDraftContentVersion);                
                previewOrDraftContentVersion.Status = requestedStatus;
                repository.Save(previewOrDraftContentVersion); 
            }            
            else if (requestedStatus == ContentStatus.Published)
            {
                var publishedVersion = originalContent.History.FirstOrDefault(f => f.Status == requestedStatus && !f.IsDeleted);
                if (publishedVersion != null)
                {
                    var originalToArchive = originalContent.Clone();
                    originalToArchive.Status = ContentStatus.Archived;
                    originalToArchive.Original = originalContent;
                    repository.Save(originalToArchive);

                    updatedContent.CopyDataTo(originalContent);
                    originalContent.Status = requestedStatus;
                    originalContent.PublishedOn = DateTime.Now;
                    originalContent.PublishedByUser = securityService.CurrentPrincipalName;
                    repository.Save(originalContent);
                }
                else
                {
                    updatedContent.CopyDataTo(originalContent);
                    originalContent.Status = requestedStatus;
                    originalContent.PublishedOn = DateTime.Now;
                    originalContent.PublishedByUser = securityService.CurrentPrincipalName;
                    repository.Save(originalContent);
                }
            }
        }
    }
}