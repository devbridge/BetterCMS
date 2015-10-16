using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using BetterModules.Core.DataAccess;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Services;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using BetterModules.Core.Exceptions.DataTier;

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

        /// <summary>
        /// The option service
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// The child content service
        /// </summary>
        private readonly IChildContentService childContentService;

        public DefaultContentService(ISecurityService securityService, IRepository repository, IOptionService optionService,
            IChildContentService childContentService)
        {
            this.securityService = securityService;
            this.repository = repository;
            this.optionService = optionService;
            this.childContentService = childContentService;
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

            // Fill content with dynamic contents info
            UpdateDynamicContainer(updatedContent);

            if (updatedContent.Id == default(Guid))
            {
                /* Just create a new content with requested status.*/
                if (requestedStatus == ContentStatus.Published)
                {
                    if (!updatedContent.PublishedOn.HasValue)
                    {
                        updatedContent.PublishedOn = DateTime.Now;
                    }
                    if (string.IsNullOrWhiteSpace(updatedContent.PublishedByUser))
                    {
                        updatedContent.PublishedByUser = securityService.CurrentPrincipalName;
                    }
                }

                updatedContent.Status = requestedStatus;
                repository.Save(updatedContent);

                return updatedContent;
            }
            var originalContent = GetOriginalContent(updatedContent.Id);

            if (originalContent == null)
            {
                throw new EntityNotFoundException(typeof(Models.Content), updatedContent.Id);
            }
            
            if (originalContent.Original != null)
            {
                originalContent = originalContent.Original;
            }

            originalContent = repository.UnProxy(originalContent);

            if (originalContent.History == null)
            {
                originalContent.History = new List<Models.Content>();
            }

            childContentService.ValidateChildContentsCircularReferences(originalContent, updatedContent);

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

        private Models.Content GetOriginalContent(Guid id)
        {
            var content = repository.AsQueryable<Models.Content>().Where(c => c.Id == id).Fetch(c => c.Original).FirstOrDefault();
            if (content == null)
            {
                throw new EntityNotFoundException(typeof(Models.Content), id);
            }

            var historyFuture = repository.AsQueryable<Models.Content>().Where(c => c.Original.Id == content.Id).ToFuture();
            var contentOptionsFuture = repository.AsQueryable<ContentOption>().Where(co => co.Content.Id == content.Id).FetchMany(co => co.Translations).ToFuture();
            var childContentsFuture = repository.AsQueryable<ChildContent>().Where(cc => cc.Parent.Id == content.Id).ToFuture();
            var contentRegionsFuture = repository.AsQueryable<ContentRegion>().Where(cr => cr.Content.Id == content.Id).Fetch(cr => cr.Region).ToFuture();
            if (content.Original != null)
            {
                var originalHistoryFuture = repository.AsQueryable<Models.Content>().Where(c => c.Original.Id == content.Original.Id).ToFuture();
                var originalContentOptionsFuture = repository.AsQueryable<ContentOption>().Where(co => co.Content.Id == content.Original.Id).FetchMany(co => co.Translations).ToFuture();

                content.Original.History = originalHistoryFuture as IList<Models.Content> ?? originalHistoryFuture.ToList();
                content.Original.ContentOptions = originalContentOptionsFuture as IList<ContentOption> ?? originalContentOptionsFuture.ToList();
            }
            content.History = historyFuture as IList<Models.Content> ?? historyFuture.ToList();
            content.ContentOptions = contentOptionsFuture as IList<ContentOption> ?? contentOptionsFuture.ToList();
            content.ChildContents = childContentsFuture as IList<ChildContent> ?? childContentsFuture.ToList();
            content.ContentRegions = contentRegionsFuture as IList<ContentRegion> ?? contentRegionsFuture.ToList();

            return content;
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
                SetCategories(contentVersionOfRequestedStatus, updatedContent);
                SetContentOptions(contentVersionOfRequestedStatus, updatedContent);
                SetContentRegions(contentVersionOfRequestedStatus, updatedContent);
                childContentService.CopyChildContents(contentVersionOfRequestedStatus, updatedContent);

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

                // Load draft content's child contents options, if saving from draft to public
                var draftVersion = originalContent.History.FirstOrDefault(f => f.Status == ContentStatus.Draft && !f.IsDeleted);
                if (draftVersion != null)
                {
                    updatedContent
                        .ChildContents
                        .ForEach(cc => cc.Options = draftVersion
                            .ChildContents
                            .Where(cc1 => cc1.AssignmentIdentifier == cc.AssignmentIdentifier)
                            .SelectMany(cc1 => cc1.Options)
                            .ToList());
                }

                updatedContent.CopyDataTo(originalContent, false);
                SetCategories(originalContent, updatedContent);
                SetContentOptions(originalContent, updatedContent);
                SetContentRegions(originalContent, updatedContent);
                childContentService.CopyChildContents(originalContent, updatedContent);

                originalContent.Status = requestedStatus;
                if (!originalContent.PublishedOn.HasValue)
                {
                    originalContent.PublishedOn = DateTime.Now;
                }
                if (string.IsNullOrWhiteSpace(originalContent.PublishedByUser))
                {
                    originalContent.PublishedByUser = securityService.CurrentPrincipalName;
                }
                repository.Save(originalContent);

                IList<Models.Content> contentsToRemove = originalContent.History.Where(f => f.Status == ContentStatus.Preview || f.Status == ContentStatus.Draft).ToList();
                foreach (var redundantContent in contentsToRemove)
                {
                    repository.Delete(redundantContent);
                    originalContent.History.Remove(redundantContent);
                }
            }
        }

        private void SetCategories(Models.Content destinationContent, Models.Content sourceContent)
        {
            var destination = destinationContent as ICategorized;
            var source = sourceContent as ICategorized;
            if (destination == null || source == null)
            {
                return;
            }

            if (destination.Categories != null)
            {
                var categoriesToRemove = destination.Categories.ToList();
                categoriesToRemove.ForEach(repository.Delete);
            }

            if (source.Categories == null)
            {
                return;
            }

            source.Categories.ForEach(destination.AddCategory);
            if (destination.Categories != null)
            {
                destination.Categories.ForEach(e => e.SetEntity(destinationContent));
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
                SetCategories(previewOrDraftContentVersion, updatedContent);
                SetContentOptions(previewOrDraftContentVersion, updatedContent);
                SetContentRegions(previewOrDraftContentVersion, updatedContent);
                childContentService.CopyChildContents(previewOrDraftContentVersion, updatedContent);

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
                SetCategories(originalContent, updatedContent);
                SetContentOptions(originalContent, updatedContent);
                SetContentRegions(originalContent, updatedContent);
                childContentService.CopyChildContents(originalContent, updatedContent);

                originalContent.Status = requestedStatus;
                if (!originalContent.PublishedOn.HasValue)
                {
                    originalContent.PublishedOn = DateTime.Now;
                }
                if (string.IsNullOrWhiteSpace(originalContent.PublishedByUser))
                {
                    originalContent.PublishedByUser = securityService.CurrentPrincipalName;
                }

                repository.Save(originalContent);
            }
        }

        public Models.Content RestoreContentFromArchive(Models.Content restoreFrom)
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
            originalContent.ChildContentsLoaded = true;

            // Save entities
            return SaveContentWithStatusUpdate(originalContent, ContentStatus.Published);
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
                                  .FetchMany(p => p.ContentRegions)
                                  .ThenFetch(cr => cr.Region)
                                  .ToList()
                                  .FirstOrDefault();

            if (content != null)
            {
                return content.FindEditableContentVersion();
            }

            return null;
        }

        public int GetPageContentNextOrderNumber(Guid pageId, Guid? parentPageContentId)
        {
            var page = repository.AsProxy<Page>(pageId);
            PageContent parent = parentPageContentId.HasValue && !parentPageContentId.Value.HasDefaultValue() 
                ? repository.AsProxy<PageContent>(parentPageContentId.Value) : null;

            var max = repository
                .AsQueryable<PageContent>()
                .Where(f => f.Page == page && !f.IsDeleted && f.Parent == parent)
                .Select(f => (int?)f.Order)
                .Max();

            if (max == null)
            {
                return 0;
            }
           
            return max.Value + 1;
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

        private void CollectDynamicRegions(string html, Models.Content content, IList<ContentRegion> contentRegions)
        {
            var regionIdentifiers = GetRegionIds(html);

            if (regionIdentifiers.Length > 0)
            {
                var regionIdentifiersLower = regionIdentifiers.Select(s => s.ToLowerInvariant()).ToArray();
                var existingRegions = repository
                    .AsQueryable<Region>()
                    .Where(r => regionIdentifiersLower.Contains(r.RegionIdentifier.ToLowerInvariant()))
                    .ToArray();

                foreach (var regionId in regionIdentifiers.Where(s => contentRegions.All(region => region.Region.RegionIdentifier != s)))
                {
                    var region = existingRegions.FirstOrDefault(r => r.RegionIdentifier.ToLowerInvariant() == regionId.ToLowerInvariant());

                    if (region == null)
                    {
                        region = contentRegions
                            .Where(cr => cr.Region.RegionIdentifier.ToLowerInvariant() == regionId.ToLowerInvariant())
                            .Select(cr => cr.Region).FirstOrDefault();

                        if (region == null)
                        {
                            region = new Region { RegionIdentifier = regionId };
                        }
                    }

                    var contentRegion = new ContentRegion { Region = region, Content = content };
                    contentRegions.Add(contentRegion);
                }
            }
        }

        private string[] GetRegionIds(string searchIn)
        {
            if (string.IsNullOrWhiteSpace(searchIn))
            {
                return new string[0];
            }

            var ids = new List<string>();

            var matches = Regex.Matches(searchIn, RootModuleConstants.DynamicRegionRegexPattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    ids.Add(match.Groups[1].Value);
                }
            }

            return ids.Distinct().ToArray();
        }

        private void SetContentOptions(IOptionContainer<Models.Content> destination, IOptionContainer<Models.Content> source)
        {
            optionService.SetOptions<ContentOption, Models.Content>(destination, source.Options, () => new ContentOptionTranslation());
        }

        private void SetContentRegions(Models.Content destination, Models.Content source)
        {
            if (destination.ContentRegions == null)
            {
                destination.ContentRegions = new List<ContentRegion>();
            }
            if (source.ContentRegions == null)
            {
                source.ContentRegions = new List<ContentRegion>();
            }

            // Add regions, which not exists in destination.
            source.ContentRegions
                .Where(s => destination.ContentRegions.All(d => s.Region.RegionIdentifier.ToLowerInvariant() != d.Region.RegionIdentifier.ToLowerInvariant()))
                .Distinct().ToList()
                .ForEach(s => 
                    {
                        destination.ContentRegions.Add(new ContentRegion { Region = s.Region, Content = destination });
                    });

            // Remove regions, which not exist in source.
            var regionsToDelete = destination.ContentRegions
                .Where(s => source.ContentRegions.All(d => s.Region.RegionIdentifier.ToLowerInvariant() != d.Region.RegionIdentifier.ToLowerInvariant()))
                .Distinct().ToList();
            regionsToDelete.ForEach(d =>
                {
                    destination.ContentRegions.Remove(d);
                    repository.Delete(d);
                });
        }

        /// <summary>
        /// Validates if content has no children.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="contentId">The content id.</param>
        /// <param name="html">The HTML.</param>
        /// <returns>
        /// Boolean value, indicating, if content has any children contents, which are based on deleting regions
        /// </returns>
        public bool CheckIfContentHasDeletingChildren(Guid? pageId, Guid contentId, string html = null)
        {
            bool hasAnyContents = false;
            var regionIdentifiers = GetRegionIds(html).Select(s => s.ToLowerInvariant()).ToArray();

            // Get regions going to be deleted
            var regionIds = repository.AsQueryable<ContentRegion>()
                .Where(cr => cr.Content.Id == contentId
                    && !regionIdentifiers.Contains(cr.Region.RegionIdentifier.ToLowerInvariant()))
                .Select(cr => cr.Region.Id)
                .ToArray();

            if (regionIds.Length > 0)
            {
                var validationQuery = repository
                    .AsQueryable<PageContent>()
                    .Where(pc => regionIds.Contains(pc.Region.Id));
                if (pageId.HasValue)
                {
                    validationQuery = validationQuery.Where(pc => pc.Page.MasterPage.Id == pageId);
                }
                
                hasAnyContents = validationQuery.Any();
            }

            return hasAnyContents;
        }

        public void CheckIfContentHasDeletingChildrenWithException(Guid? pageId, Guid contentId, string html = null)
        {
            var hasAnyChildren = CheckIfContentHasDeletingChildren(pageId, contentId, html);
            if (hasAnyChildren)
            {
                var message = RootGlobalization.SaveContent_ContentHasChildrenContents_RegionDeleteConfirmationMessage;
                var logMessage = string.Format("User is trying to delete content regions which has children contents. Confirmation is required. ContentId: {0}, PageId: {1}", contentId, pageId);
                throw new ConfirmationRequestException(() => message, logMessage);
            }
        }

        public void UpdateDynamicContainer(Models.Content content)
        {
            var dynamicContainer = content as IDynamicContentContainer;
            if (dynamicContainer != null)
            {
                if (content.ContentRegions == null)
                {
                    content.ContentRegions = new List<ContentRegion>();
                }
                if (content.ChildContents == null)
                {
                    content.ChildContents = new List<ChildContent>();
                }
                CollectDynamicRegions(dynamicContainer.Html, content, content.ContentRegions);
                childContentService.CollectChildContents(dynamicContainer.Html, content);
            }
        }

        public TEntity GetDraftOrPublishedContent<TEntity>(TEntity content) where TEntity : Models.Content
        {
            if (content.History != null)
            {
                var draft = content.History.FirstOrDefault(c => c.Status == ContentStatus.Draft);
                if (draft != null)
                {
                    return (TEntity)draft;
                }
            }

            return content;
        }
    }
}