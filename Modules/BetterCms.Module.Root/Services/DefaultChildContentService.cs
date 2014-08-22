using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;

using FluentNHibernate.Utils;

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
            destination.ChildContents
                .Where(s => source.ChildContents.All(d => s.AssignmentIdentifier != d.AssignmentIdentifier))
                .Distinct().ToList().ForEach(d => repository.Delete(d));
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
        private List<Guid> PopulateReferencesList(List<Guid> references, IEnumerable<Tuple<Guid, Guid, string>> children)
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
                    destinationChildren.Select(s => new Tuple<Guid, Guid, string>(destination.Id, s.Child.Id, s.Child.Name)));

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
                .Select(cc => new Tuple<Guid, Guid, string>(cc.ParentId, cc.ChildId, cc.Name));

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

        public void RetrieveChildrenContentsRecursively(bool canManageContent, IEnumerable<Models.Content> contents)
        {
            // Check if all child contents are OK
            var allContentsWithChildren = contents.Where(c => c.ChildContents != null).ToList();
            allContentsWithChildren.ForEach(
                    content =>
                    {
                        content.ChildContents = RetrieveCorrectVersionsOfChildContents(content.ChildContents, canManageContent);
                    });

            var childContents = allContentsWithChildren.SelectMany(c => c.ChildContents).Distinct().ToList();
            if (childContents.Any())
            {
                var childIds = childContents.Where(c => !c.Child.IsDeleted).Select(c => c.Child.Id).Distinct().ToArray();
                if (!childIds.Any())
                {
                    return;
                }
                
                var entities = repository
                    .AsQueryable<ChildContent>(c => childIds.Contains(c.Parent.Id))
                    .Fetch(c => c.Child)
                    .ThenFetchMany(c => c.ChildContents)
                    .ThenFetch(cc => cc.Child)
                    .FetchMany(c => c.Options)
                    .ToList();
                
                entities = RetrieveCorrectVersionsOfChildContents(entities, canManageContent);
                childContents.ForEach(
                    c =>
                    {
                        if (c.Child.ChildContents == null)
                        {
                            c.Child.ChildContents = new List<ChildContent>();
                        }
                        c.Child.ChildContents.Clear();

                        if (entities.Any())
                        {
                            entities.Where(e => e.Parent.Id == c.Child.Id).ToList().ForEach(c.Child.ChildContents.Add);
                        }
                    });

                if (entities.Any())
                {
                    RetrieveChildrenContentsRecursively(canManageContent, entities.Select(c => c.Child).Distinct().ToList());
                }
            }
        }

        private List<ChildContent> RetrieveCorrectVersionsOfChildContents(IList<ChildContent> childContents, bool canManageContent)
        {
            if (childContents == null)
            {
                return new List<ChildContent>();
            }

            if (canManageContent)
            {
                return childContents.ToList();
            }

            var visibleChildContents = new List<ChildContent>(childContents.Count);
            foreach (var childContent in childContents)
            {
                if (childContent.Child.Status == ContentStatus.Draft)
                {
                    if (childContent.Child.Original != null && childContent.Child.Original.Status == ContentStatus.Published)
                    {
                        childContent.Child = childContent.Child.Original;
                    }
                    else
                    {
                        continue;
                    }
                }

                visibleChildContents.Add(childContent);
            }

            return visibleChildContents;
        }
    }
}