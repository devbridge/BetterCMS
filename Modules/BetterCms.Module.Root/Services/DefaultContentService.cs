using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Services;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Helpers;

using NHibernate.Linq;

namespace BetterCms.Module.Root.Services
{
    internal class DefaultContentService : IContentService
    {
        /// <summary>
        /// A security service.
        /// </summary>
        private readonly ISecurityService securityService;

        /// <summary>
        /// A repository contract.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The option service
        /// </summary>
        private readonly IOptionService optionService;

        public DefaultContentService(ISecurityService securityService, IRepository repository, IUnitOfWork unitOfWork, IOptionService optionService)
        {
            this.unitOfWork = unitOfWork;
            this.securityService = securityService;
            this.repository = repository;
            this.optionService = optionService;
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
                          .Where(f => f.Id == updatedContent.Id && !f.IsDeleted)
                          .Fetch(f => f.Original).ThenFetchMany(f => f.History)
                          .Fetch(f => f.Original).ThenFetchMany(f => f.ContentOptions)
                          .FetchMany(f => f.History)
                          .FetchMany(f => f.ContentOptions)            
                          .ToList()
                          .FirstOrDefault();

            if (originalContent == null)
            {
                throw new EntityNotFoundException(typeof(Models.Content), updatedContent.Id);
            }
            
            if (originalContent.Original != null)
            {
                originalContent = originalContent.Original;
            }

            originalContent = repository.UnProxy(originalContent);
           
            if (originalContent.History != null)
            {
                originalContent.History = originalContent.History.Distinct().ToList();
            }

            if (originalContent.ContentOptions != null)
            {
                originalContent.ContentOptions = originalContent.ContentOptions.Distinct().ToList();
            }

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

                updatedContent.CopyDataTo(contentVersionOfRequestedStatus, false);
                SetContentOptions(contentVersionOfRequestedStatus, updatedContent);

                contentVersionOfRequestedStatus.Original = originalContent;
                contentVersionOfRequestedStatus.Status = requestedStatus;
                originalContent.History.Add(contentVersionOfRequestedStatus);
                repository.Save(contentVersionOfRequestedStatus);                
            }
            
            if (requestedStatus == ContentStatus.Published)
            {
                // Original is copied with options and saved.
                // Removes options from original.
                // Locks new stuff from view model.

                var originalToArchive = originalContent.Clone();
                originalToArchive.Status = ContentStatus.Archived;
                originalToArchive.Original = originalContent;
                originalContent.History.Add(originalToArchive);
                repository.Save(originalToArchive);

                updatedContent.CopyDataTo(originalContent, false);
                SetContentOptions(originalContent, updatedContent);

                originalContent.Status = requestedStatus;
                originalContent.PublishedOn = DateTime.Now;
                originalContent.PublishedByUser = securityService.CurrentPrincipalName;
                repository.Save(originalContent);

                IList<Models.Content> contentsToRemove = originalContent.History.Where(f => f.Status == ContentStatus.Preview || f.Status == ContentStatus.Draft).ToList();
                foreach (var redundantContent in contentsToRemove)
                {
                    repository.Delete(redundantContent);
                    originalContent.History.Remove(redundantContent);
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
                    if (originalContent.Status == requestedStatus 
                        || (originalContent.Status == ContentStatus.Preview && requestedStatus == ContentStatus.Draft))
                    {
                        previewOrDraftContentVersion = originalContent;
                    }
                    else
                    {
                        previewOrDraftContentVersion = originalContent.Clone();
                        previewOrDraftContentVersion.Original = originalContent;
                        originalContent.History.Add(previewOrDraftContentVersion);
                    }
                }

                updatedContent.CopyDataTo(previewOrDraftContentVersion, false);
                SetContentOptions(previewOrDraftContentVersion, updatedContent);
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
                    originalContent.History.Add(originalToArchive);
                    repository.Save(originalToArchive);
                }

                updatedContent.CopyDataTo(originalContent, false);
                SetContentOptions(originalContent, updatedContent);
                originalContent.Status = requestedStatus;
                originalContent.PublishedOn = DateTime.Now;
                originalContent.PublishedByUser = securityService.CurrentPrincipalName;
                repository.Save(originalContent);
            }
        }

        public void RestoreContentFromArchive(Models.Content restoreFrom)
        {
            if (restoreFrom == null)
            {
                throw new CmsException("Nothing to restore from.", new ArgumentNullException("restoreFrom"));
            }

            if (restoreFrom.Status != ContentStatus.Archived)
            {
                throw new CmsException("A page content can be restored only from the archived version.");
            }

            // Replace original version with restored entity data
            var originalContent = restoreFrom.Clone();
            originalContent.Id = restoreFrom.Original.Id;
            originalContent.Version = restoreFrom.Original.Version;
            originalContent.Status = ContentStatus.Published;
            originalContent.Original = null;

            // Save entities
            SaveContentWithStatusUpdate(originalContent, ContentStatus.Published);
        }

        public System.Tuple<PageContent, Models.Content> GetPageContentForEdit(Guid pageContentId)
        {
            PageContent pageContent = repository.AsQueryable<PageContent>()
                                  .Where(p => p.Id == pageContentId && !p.IsDeleted)
                                  .Fetch(p => p.Content).ThenFetchMany(p => p.History)
                                  .Fetch(p => p.Page)
                                  .Fetch(f => f.Region)
                                  .ToList()
                                  .FirstOrDefault();

            if (pageContent != null)
            {
                Models.Content content = pageContent.Content.FindEditableContentVersion();

                if (content == null)
                {
                    return null;
                }

                return new System.Tuple<PageContent, Models.Content>(pageContent, content);
            }

            return null;
        }

        public Models.Content GetContentForEdit(Guid contentId)
        {
            Models.Content content = repository.AsQueryable<Models.Content>()
                                  .Where(p => p.Id == contentId && !p.IsDeleted)
                                  .FetchMany(p => p.History)
                                  .ToList()
                                  .FirstOrDefault();

            if (content != null)
            {
                return content.FindEditableContentVersion();
            }

            return null;
        }

        public int GetPageContentNextOrderNumber(Guid pageId)
        {
            var page = repository.AsProxy<Page>(pageId);
            var max = repository
                .AsQueryable<PageContent>()
                .Where(f => f.Page == page && !f.IsDeleted)
                .Select(f => (int?)f.Order)
                .Max();
            int order;

            if (max == null)
            {
                order = 0;
            }
            else
            {
                order = max.Value + 1;
            }

            return order;
        }

        public void PublishDraftContent(Guid pageId)
        {
            var pageContents = repository.AsQueryable<PageContent>()
                .Where(content => content.Page.Id == pageId)
                .Fetch(f => f.Content)
                .ThenFetchMany(f => f.History)
                .ToList();

            var draftContents = pageContents
                .Where(
                    content =>
                    (content.Content.Status == ContentStatus.Draft
                     && (content.Content.History == null || content.Content.History.All(content1 => content1.Status != ContentStatus.Published)))
                    || (content.Content.Status != ContentStatus.Published
                        && content.Content.History.All(content1 => content1.Status != ContentStatus.Published)
                        && content.Content.History.Any(content1 => content1.Status == ContentStatus.Draft)))
                .ToList();

            foreach (var pageContent in draftContents.Where(pageContent => !(pageContent.Content is Widget)))
            {
                var draftContent = pageContent.Content.FindEditableContentVersion();
                if (draftContent != null)
                {
                    pageContent.Content = SaveContentWithStatusUpdate(draftContent, ContentStatus.Published);
                    repository.Save(pageContent);
                }
            }
        }

        private void SetContentOptions(IOptionContainer<Models.Content> destination, IOptionContainer<Models.Content> source)
        {
            optionService.SetOptions<ContentOption, Models.Content>(destination, source.Options);
        }
    }
}