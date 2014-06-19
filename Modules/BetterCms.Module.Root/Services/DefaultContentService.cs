using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Services;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;

using FluentNHibernate.Conventions.AcceptanceCriteria;

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
        /// The option service
        /// </summary>
        private readonly IOptionService optionService;

        public DefaultContentService(ISecurityService securityService, IRepository repository, IOptionService optionService)
        {
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

            // Fill content with dynamic contents info
            var dynamicContainer = updatedContent as IDynamicContentContainer;
            if (dynamicContainer != null)
            {
                if (updatedContent.ContentRegions == null)
                {
                    updatedContent.ContentRegions = new List<ContentRegion>();
                }
                if (updatedContent.ChildContents == null)
                {
                    updatedContent.ChildContents = new List<ChildContent>();
                }
                CollectDynamicRegions(dynamicContainer.Html, updatedContent, updatedContent.ContentRegions);
                dynamicContainer.Html = CollectChildWidgets(dynamicContainer.Html, updatedContent);
            }

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
            
            var originalContent =
                repository.AsQueryable<Models.Content>()
                          .Where(f => f.Id == updatedContent.Id && !f.IsDeleted)
                          .Fetch(f => f.Original).ThenFetchMany(f => f.History)
                          .Fetch(f => f.Original).ThenFetchMany(f => f.ContentOptions)
                          .FetchMany(f => f.History)
                          .FetchMany(f => f.ContentOptions)
                          .FetchMany(f => f.ChildContents)
                          .FetchMany(f => f.ContentRegions)            
                          .ThenFetch(f => f.Region)
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
            else
            {
                originalContent.History = new List<Models.Content>();
            }

            if (originalContent.ContentOptions != null)
            {
                originalContent.ContentOptions = originalContent.ContentOptions.Distinct().ToList();
            }

            if (originalContent.ContentRegions != null)
            {
                originalContent.ContentRegions = originalContent.ContentRegions.Distinct().ToList();
            }
            
            if (originalContent.ChildContents != null)
            {
                originalContent.ChildContents = originalContent.ChildContents.Distinct().ToList();
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
                SetContentRegions(contentVersionOfRequestedStatus, updatedContent);

                contentVersionOfRequestedStatus.Original = originalContent;
                contentVersionOfRequestedStatus.Status = requestedStatus;
                originalContent.History.Add(contentVersionOfRequestedStatus);
                SetChildContents(contentVersionOfRequestedStatus, updatedContent);
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
                SetContentRegions(originalContent, updatedContent);

                originalContent.Status = requestedStatus;
                if (!originalContent.PublishedOn.HasValue)
                {
                    originalContent.PublishedOn = DateTime.Now;
                }
                if (string.IsNullOrWhiteSpace(originalContent.PublishedByUser))
                {
                    originalContent.PublishedByUser = securityService.CurrentPrincipalName;
                }
                SetChildContents(originalContent, updatedContent);
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
                SetContentRegions(previewOrDraftContentVersion, updatedContent);

                previewOrDraftContentVersion.Status = requestedStatus;
                SetChildContents(previewOrDraftContentVersion, updatedContent);
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
                SetContentRegions(originalContent, updatedContent);

                originalContent.Status = requestedStatus;
                if (!originalContent.PublishedOn.HasValue)
                {
                    originalContent.PublishedOn = DateTime.Now;
                }
                if (string.IsNullOrWhiteSpace(originalContent.PublishedByUser))
                {
                    originalContent.PublishedByUser = securityService.CurrentPrincipalName;
                }

                SetChildContents(originalContent, updatedContent);
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

        private string CollectChildWidgets(string html, Models.Content content)
        {
            var widgetModels = ChildContentRenderHelper.ParseWidgetsFromHtml(html, true);
            if (widgetModels != null && widgetModels.Count > 0)
            {
                // Validate widget ids
                var widgetIds = widgetModels.Select(w => w.WidgetId).Distinct().ToArray();
                var widgets = repository.AsQueryable<Models.Content>(c => widgetIds.Contains(c.Id)).Select(c => c.Id).ToList();
                widgetIds.Where(id => widgets.All(dbId => dbId != id)).ToList().ForEach(
                    id =>
                    {
                        var message = RootGlobalization.ChildContent_WidgetNotFound_ById;
                        var logMessage = string.Format("{0} Id: {1}", message, id);
                        throw new ValidationException(() => message, logMessage);
                    });

                // Validate child content
                var group = widgetModels.GroupBy(w => w.AssignmentIdentifier).FirstOrDefault(g => g.Count() > 1);
                if (group != null)
                {
                    var message = string.Format(RootGlobalization.ChildContent_AssignmentAlreadyAdded, group.First().AssignmentIdentifier);
                    throw new ValidationException(() => message, message);
                }

                foreach (var model in widgetModels)
                {
                    // Remove data-is-new attribute
                    if (model.IsNew)
                    {
                        html = ChildContentRenderHelper.RemoveIsNewAttribute(html, model);
                    }

                    // Add child content only if it's not added yet (for example, it may be added when restoring / cloning a content)
                    if (content.ChildContents.All(cc => cc.AssignmentIdentifier != model.AssignmentIdentifier))
                    {
                        // Create child content
                        var childContent = new ChildContent
                        {
                            Id = Guid.NewGuid(),
                            Child = repository.AsProxy<Models.Content>(model.WidgetId),
                            Parent = content,
                            AssignmentIdentifier = model.AssignmentIdentifier
                        };
                        content.ChildContents.Add(childContent);
                    }
                }
            }

            return html;
        }

        private void CollectDynamicRegions(string html, Models.Content content, IList<ContentRegion> contentRegions)
        {
            var regionIdentifiers = GetRegionIds(html);

            if (regionIdentifiers.Length > 0)
            {
                var existingRegions = repository
                    .AsQueryable<Region>()
                    .Where(r => regionIdentifiers.Contains(r.RegionIdentifier))
                    .ToArray();

                foreach (var regionId in regionIdentifiers.Where(s => contentRegions.All(region => region.Region.RegionIdentifier != s)))
                {
                    var region = existingRegions.FirstOrDefault(r => r.RegionIdentifier == regionId);

                    if (region == null)
                    {
                        region = contentRegions
                            .Where(cr => cr.Region.RegionIdentifier == regionId)
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
            optionService.SetOptions<ContentOption, Models.Content>(destination, source.Options);
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
                .Where(s => destination.ContentRegions.All(d => s.Region.RegionIdentifier != d.Region.RegionIdentifier))
                .Distinct().ToList()
                .ForEach(s =>
                             {
                                 destination.ContentRegions.Add(new ContentRegion { Region = s.Region, Content = destination });
                                 source.ContentRegions.Remove(s);
                             });

            // Remove regions, which not exists in source.
            destination.ContentRegions
                .Where(s => source.ContentRegions.All(d => s.Region.RegionIdentifier != d.Region.RegionIdentifier))
                .Distinct().ToList()
                .ForEach(d => repository.Delete(d));
        }

        private void SetChildContents(Models.Content destination, Models.Content source)
        {
            if (destination.ChildContents == null)
            {
                destination.ChildContents = new List<ChildContent>();
            }
            if (source.ChildContents == null)
            {
                source.ChildContents = new List<ChildContent>();
            }

            // Update all child and their options
            foreach (var sourceChildContent in source.ChildContents)
            {
                var destinationChildContent = destination.ChildContents.FirstOrDefault(d => sourceChildContent.AssignmentIdentifier == d.AssignmentIdentifier);
                if (destinationChildContent == null)
                {
                    destinationChildContent = new ChildContent();
                    destination.ChildContents.Add(destinationChildContent);
                }

                destinationChildContent.AssignmentIdentifier = sourceChildContent.AssignmentIdentifier;
                destinationChildContent.Parent = destination;
                destinationChildContent.Child = sourceChildContent.Child;

                // Update all the options
                if (sourceChildContent.Options == null)
                {
                    sourceChildContent.Options = new List<ChildContentOption>();
                }
                if (destinationChildContent.Options == null)
                {
                    destinationChildContent.Options = new List<ChildContentOption>();
                }

                // Add new options
                foreach (var sourceChildOption in sourceChildContent.Options)
                {
                    var destinationChildOption = destinationChildContent.Options.FirstOrDefault(o => o.Key == sourceChildOption.Key);
                    if (destinationChildOption == null)
                    {
                        destinationChildOption = new ChildContentOption();
                        destinationChildContent.Options.Add(destinationChildOption);
                    }

                    sourceChildOption.CopyDataTo(destinationChildOption);
                    destinationChildOption.ChildContent = destinationChildContent;
                }

                // Remove unneeded options
                destinationChildContent.Options
                    .Where(s => sourceChildContent.Options.All(d => s.Key != d.Key))
                    .Distinct().ForEach(d => repository.Delete(d));
            }

            // Remove childs, which not exists in source.
            destination.ChildContents
                .Where(s => source.ChildContents.All(d => s.AssignmentIdentifier != d.AssignmentIdentifier))
                .Distinct().ForEach(d => repository.Delete(d));

            if (destination.ChildContents.Any())
            {
                var dictionary = new Dictionary<Guid, List<Guid>>();
                var childrenIds = PopulateReferencesDictionary(
                    dictionary,
                    destination.ChildContents.Select(s => new System.Tuple<Guid, Guid, string>(destination.Id, s.Child.Id, s.Child.Name)));

                ValidateChildContentsCircularReferences(childrenIds, dictionary);
            }
        }

        /// <summary>
        /// Populates the references dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="children">The children: list of Tuple, where Item1: Parent, Item2: Childs.</param>
        /// <returns></returns>
        private List<Guid> PopulateReferencesDictionary(Dictionary<Guid, List<Guid>> dictionary, 
            IEnumerable<System.Tuple<Guid, Guid, string>> children)
        {
            var childrenIds = new List<Guid>();

            foreach (var childContent in children)
            {
                if (!dictionary.ContainsKey(childContent.Item1))
                {
                    dictionary[childContent.Item1] = new List<Guid>();
                }
                if (!dictionary[childContent.Item1].Contains(childContent.Item2))
                {
                    dictionary[childContent.Item1].Add(childContent.Item2);
                }
                if (!dictionary.ContainsKey(childContent.Item2) && !childrenIds.Contains(childContent.Item2))
                {
                    childrenIds.Add(childContent.Item2);
                }
            }

            return childrenIds;
        }

        /// <summary>
        /// Validates the list of child contents for circular references.
        /// </summary>
        /// <param name="childrenIds">The children ids.</param>
        /// <param name="dictionary">The dictionary.</param>
        private void ValidateChildContentsCircularReferences(List<Guid> childrenIds, Dictionary<Guid, List<Guid>> dictionary)
        {
            var children = repository
                .AsQueryable<ChildContent>()
                .Where(cc => childrenIds.Contains(cc.Parent.Id))
                .Select(cc => new { ParentId = cc.Parent.Id, ChildId = cc.Child.Id, Name = cc.Child.Name })
                .ToList()
                .Distinct()
                .Select(cc => new System.Tuple<Guid, Guid, string>(cc.ParentId, cc.ChildId, cc.Name));

            var circularReference = children.FirstOrDefault(c => !dictionary.ContainsKey(c.Item1) && dictionary.ContainsKey(c.Item2));
            if (circularReference != null)
            {
                // TODO: add to translations
                var message = string.Format("Cannot add widget as child widget! One of child widgets references widget \"{0}\", which causes circular reference.", circularReference.Item3);
                throw new ValidationException(() => message, message);
            }

            childrenIds = PopulateReferencesDictionary(dictionary, children);
            if (childrenIds.Any())
            {
                ValidateChildContentsCircularReferences(childrenIds, dictionary);
            }
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
        public bool CheckIfContentHasDeletingChildren(Guid pageId, Guid contentId, string html = null)
        {
            bool hasAnyContents = false;
            var regionIdentifiers = GetRegionIds(html);

            // Get regions going to be deleted
            var regionIds = repository.AsQueryable<ContentRegion>()
                .Where(cr => cr.Content.Id == contentId
                    && !regionIdentifiers.Contains(cr.Region.RegionIdentifier))
                .Select(cr => cr.Region.Id)
                .ToArray();

            if (regionIds.Length > 0)
            {
                hasAnyContents = repository
                    .AsQueryable<PageContent>()
                    .Any(pc => pc.Page.MasterPage.Id == pageId && regionIds.Contains(pc.Region.Id));
            }

            return hasAnyContents;
        }
    }
}