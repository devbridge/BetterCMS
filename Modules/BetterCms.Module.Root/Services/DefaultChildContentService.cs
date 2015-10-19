using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;

using NHibernate.Criterion;
using NHibernate.Linq;

namespace BetterCms.Module.Root.Services
{
    public class DefaultChildContentService : IChildContentService
    {
        private readonly IRepository repository;

        public DefaultChildContentService(IRepository repository)
        {
            this.repository = repository;
        }

        public void CollectChildContents(string html, Models.Content content)
        {
            var widgetModels = PageContentRenderHelper.ParseWidgetsFromHtml(html, true);
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
        }

        public void CopyChildContents(Models.Content destination, Models.Content source)
        {
            if (destination.ChildContents == null)
            {
                destination.ChildContents = new List<ChildContent>();
            }
            if (source.ChildContents == null)
            {
                source.ChildContents = new List<ChildContent>();
            }

            CopyChildContents(destination.ChildContents, source.ChildContents);
            foreach (var sourceChildContent in source.ChildContents)
            {
                var destinationChildContent = destination.ChildContents.First(d => sourceChildContent.AssignmentIdentifier == d.AssignmentIdentifier);
                destinationChildContent.Parent = destination;

                // Remove unneeded options
                if (source.ChildContentsLoaded)
                {
                    destinationChildContent.Options
                        .Where(s => sourceChildContent.Options.All(d => s.Key != d.Key))
                        .Distinct().ToList().ForEach(d => repository.Delete(d));
                }
            }

            // Remove childs, which not exists in source.
            var childsToDelete = destination.ChildContents
                .Where(s => source.ChildContents.All(d => s.AssignmentIdentifier != d.AssignmentIdentifier))
                .Distinct().ToList();
            childsToDelete.ForEach(d =>
                {
                    destination.ChildContents.Remove(d);
                    repository.Delete(d);
                });
        }

        private void CopyChildContents(IList<ChildContent> destinationChildren, IList<ChildContent> sourceChildren)
        {
            // Update all child and their options
            foreach (var sourceChildContent in sourceChildren)
            {
                var destinationChildContent = destinationChildren.FirstOrDefault(d => sourceChildContent.AssignmentIdentifier == d.AssignmentIdentifier);
                if (destinationChildContent == null)
                {
                    destinationChildContent = new ChildContent();
                    destinationChildren.Add(destinationChildContent);
                }

                destinationChildContent.AssignmentIdentifier = sourceChildContent.AssignmentIdentifier;
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
            }
        }

        /// <summary>
        /// Populates the references dictionary.
        /// </summary>
        /// <param name="references">The references.</param>
        /// <param name="children">The children: list of Tuple, where Item1: Parent, Item2: Childs.</param>
        /// <returns></returns>
        private List<Guid> PopulateReferencesList(List<Guid> references, IEnumerable<System.Tuple<Guid, Guid, string>> children)
        {
            var childrenIds = new List<Guid>();

            foreach (var childContent in children)
            {
                if (!references.Contains(childContent.Item1) && !childContent.Item1.HasDefaultValue())
                {
                    references.Add(childContent.Item1);
                }
                if (!references.Contains(childContent.Item2) && !childrenIds.Contains(childContent.Item2))
                {
                    childrenIds.Add(childContent.Item2);
                }
            }

            return childrenIds;
        }

        public void ValidateChildContentsCircularReferences(Models.Content destination, Models.Content source)
        {
            var destinationChildren = new List<ChildContent>();
            if (destination.ChildContents != null)
            {
                destinationChildren.AddRange(destination.ChildContents);
            }

            var sourceChildren = new List<ChildContent>();
            if (source.ChildContents != null)
            {
                sourceChildren.AddRange(source.ChildContents);
            }
            CopyChildContents(destinationChildren, sourceChildren);

            // Remove childs, which not exists in source.
            destinationChildren
                .Where(s => sourceChildren.All(d => s.AssignmentIdentifier != d.AssignmentIdentifier))
                .ToList()
                .ForEach(d => destinationChildren.Remove(d));

            if (destinationChildren.Any())
            {
                // Cannot add itself as child
                if (destinationChildren.Any(dc => dc.Child.Id == destination.Id))
                {
                    var message = string.Format(RootGlobalization.ChildContent_CirculatReferenceDetected, destination.Name);
                    throw new ValidationException(() => message, message);
                }

                var references = new List<Guid>();
                var childrenIds = PopulateReferencesList(
                    references,
                    destinationChildren.Select(s => new System.Tuple<Guid, Guid, string>(destination.Id, s.Child.Id, s.Child.Name)));

                ValidateChildContentsCircularReferences(childrenIds, references);
            }
        }

        /// <summary>
        /// Validates the list of child contents for circular references.
        /// </summary>
        /// <param name="childrenIds">The children ids.</param>
        /// <param name="references">The references list.</param>
        private void ValidateChildContentsCircularReferences(List<Guid> childrenIds, List<Guid> references)
        {
            var children = repository
                .AsQueryable<ChildContent>()
                .Where(cc => childrenIds.Contains(cc.Parent.Id))
                .Select(cc => new { ParentId = cc.Parent.Id, ChildId = cc.Child.Id, Name = cc.Child.Name })
                .ToList()
                .Distinct()
                .Select(cc => new System.Tuple<Guid, Guid, string>(cc.ParentId, cc.ChildId, cc.Name));

            var circularReference = children.FirstOrDefault(c => !references.Contains(c.Item1) && references.Contains(c.Item2));
            if (circularReference != null)
            {
                var message = string.Format(RootGlobalization.ChildContent_CirculatReferenceDetected, circularReference.Item3);
                throw new ValidationException(() => message, message);
            }

            childrenIds = PopulateReferencesList(references, children);
            if (childrenIds.Any())
            {
                ValidateChildContentsCircularReferences(childrenIds, references);
            }
        }

        public IList<ChildContent> RetrieveChildrenContentsRecursively(bool canManageContent, IEnumerable<Guid> contentIds)
        {
            var dictionary = new Dictionary<Guid, IList<ChildContent>>();
            dictionary = RetrieveChildrenContentsRecursively(canManageContent, contentIds, dictionary);

            return dictionary.Where(d => d.Value != null).SelectMany(d => d.Value).Distinct().ToList();
        }

        private Dictionary<Guid, IList<ChildContent>> RetrieveChildrenContentsRecursively(bool canManageContent, IEnumerable<Guid> contentIds, Dictionary<Guid, IList<ChildContent>> dictionary)
        {
            var contentIdsToRetrieve = contentIds.Where(contentId => !dictionary.ContainsKey(contentId)).Distinct().ToArray();
            if (contentIdsToRetrieve.Any())
            {
                // Load child contents
                var childContents = GetChildContents(contentIdsToRetrieve, canManageContent);

                // Add parents without children to the dictionary
                contentIdsToRetrieve
                    .Where(parentId => childContents.All(cc => cc.Parent.Id != parentId))
                    .ToList()
                    .ForEach(parentId =>
                        {
                            dictionary[parentId] = null;
                        });

                if (childContents.Any())
                {
                    // Collect children
                    childContents.Select(cc => cc.Parent.Id)
                        .Distinct()
                        .ToList()
                        .ForEach(parentId =>
                            {
                                dictionary[parentId] = childContents
                                    .Where(cc => cc.Parent.Id == parentId)
                                    .Select(cc => RetrieveCorrectVersionOfChildContents(cc, canManageContent)).ToList();
                            });

                    var childContentIds = childContents
                        .Select(cc => cc.Child.Id)
                        .Distinct();
                    dictionary = RetrieveChildrenContentsRecursively(canManageContent, childContentIds, dictionary);
                }
            }

            return dictionary;
        }

        private IList<ChildContent> GetChildContents(IEnumerable<Guid> ids, bool canManageContent)
        {
            var childContents = repository.AsQueryable<ChildContent>()
                .Where(i => ids.Contains(i.Parent.Id) && !i.IsDeleted)
                .Fetch(i => i.Child).ToList();

            if (!canManageContent)
            {
                childContents = childContents.Where(c => c.Child.Status == ContentStatus.Published).ToList();
            }

            var childIds = childContents.Select(i => i.Child.Id).Distinct().ToList();
            var childContentIds = childContents.Select(i => i.Id).Distinct().ToList();

            foreach (var childContent in childContents)
            {
                childContent.Child.ContentOptions = new List<ContentOption>();
                childContent.Child.ContentRegions = new List<ContentRegion>();
                childContent.Child.ChildContents = new List<ChildContent>();
                childContent.Options = new List<ChildContentOption>();
            }

            var contentOptions = repository.AsQueryable<ContentOption>()
                .Where(i => childIds.Contains(i.Content.Id)).ToFuture();

            var contentRegions = repository.AsQueryable<ContentRegion>()
                .Where(i => childIds.Contains(i.Content.Id))
                .Fetch(i => i.Region).ToFuture();

            IEnumerable<Models.Content> histories = new List<Models.Content>();
            if (canManageContent)
            {
                histories = repository.AsQueryable<Models.Content>()
                    .Where(i => childIds.Contains(i.Original.Id) && i.Status != ContentStatus.Archived)
                    .FetchMany(i => i.ChildContents)
                    .ThenFetch(i => i.Child).ToFuture();
            }

            var childContentOptions = repository.AsQueryable<ChildContentOption>()
                .Where(i => childContentIds.Contains(i.ChildContent.Id)).FetchMany(x => x.Translations).ToFuture();

            var childChildContents = repository.AsQueryable<ChildContent>()
                .Where(i => childIds.Contains(i.Parent.Id)).ToFuture().ToList();

            SetChildContentData(childContents, contentOptions.ToList(), contentRegions.ToList(),
                childChildContents, childContentOptions.ToList(), histories.ToList());

            return childContents;
        }

        private void SetChildContentData(IList<ChildContent> childContents, IList<ContentOption> contentOptions,
            IList<ContentRegion> contentRegions, IList<ChildContent> childChildContents, IList<ChildContentOption> childContentOptions,
            IList<Models.Content> histories)
        {
            foreach (var childContent in childContents)
            {
                childContent.Child.ContentOptions = contentOptions.Where(i => i.Content.Id == childContent.Child.Id).ToList();
                childContent.Child.ContentRegions = contentRegions.Where(i => i.Content.Id == childContent.Child.Id).ToList();
                childContent.Child.ChildContents = childChildContents.Where(i => i.Parent.Id == childContent.Child.Id).ToList();
                childContent.Options = childContentOptions.Where(i => i.ChildContent.Id == childContent.Id).ToList();
                if (histories.Count > 0)
                {
                    childContent.Child.History = histories.Where(i => i.Original.Id == childContent.Child.Id).ToList();
                }
            }
        }

        public void RetrieveChildrenContentsRecursively(bool canManageContent, IEnumerable<Models.Content> contents)
        {
            var dictionary = new Dictionary<Guid, IList<ChildContent>>();
            var idsToRetrieve = new List<Guid>();
            var contentsList = contents.ToList();

            contentsList.ForEach(
                c =>
                {
                    if (!idsToRetrieve.Contains(c.Id))
                    {
                        idsToRetrieve.Add(c.Id);
                    }
                    if (c.ChildContents != null)
                    {
                        c.ChildContents.ToList().ForEach(
                            cc =>
                            {
                                if (!idsToRetrieve.Contains(cc.Child.Id))
                                {
                                    idsToRetrieve.Add(cc.Child.Id);
                                }
                            });
                    }
                });

            dictionary = RetrieveChildrenContentsRecursively(canManageContent, idsToRetrieve, dictionary);

            contentsList.ForEach(content => SetChildContentsRecursively(content, dictionary));
        }

        private ChildContent RetrieveCorrectVersionOfChildContents(ChildContent childContent, bool canManageContent)
        {
            if (canManageContent)
            {
                return childContent;
            }

            if (childContent.Child.Status == ContentStatus.Draft)
            {
                if (childContent.Child.Original != null && childContent.Child.Original.Status == ContentStatus.Published)
                {
                    var childContentWithOriginalContent = childContent.Clone();
                    childContentWithOriginalContent.Child = childContent.Child.Original;

                    return childContentWithOriginalContent;
                }

                return null;
            }

            return childContent;
        }

        private void SetChildContentsRecursively(Models.Content content, Dictionary<Guid, IList<ChildContent>> dictionary)
        {
            content.ChildContents = dictionary[content.Id];
            if (content.ChildContents != null)
            {
                foreach (var childContent in content.ChildContents)
                {
                    // Retrieve child contents
                    SetChildContentsRecursively(childContent.Child, dictionary);
                }
            }
        }
    }
}