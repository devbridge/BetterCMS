using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;
using BetterCms.Module.Root.Projections;

namespace BetterCms.Module.Root.Services
{
    public class DefaultChildContentService : IChildContentService
    {
        private readonly IRepository repository;
        
        private readonly IOptionService optionService;

        private readonly PageContentProjectionFactory pageContentProjectionFactory;

        public DefaultChildContentService(IRepository repository, PageContentProjectionFactory pageContentProjectionFactory,
            IOptionService optionService)
        {
            this.repository = repository;
            this.optionService = optionService;
            this.pageContentProjectionFactory = pageContentProjectionFactory;
        }

        public string CollectChildContents(string html, Models.Content content)
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

        public void RetrieveChildrenContentsRecursively(IEnumerable<Models.Content> contents)
        {
            var childContents = contents.Where(c => c.ChildContents != null).SelectMany(c => c.ChildContents).ToList();
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

                childContents.ForEach(c =>
                {
                    if (c.Child.ChildContents == null)
                    {
                        c.Child.ChildContents = new List<ChildContent>();
                    }
                    c.Child.ChildContents.Clear();

                    entities.Where(e => e.Parent.Id == c.Child.Id).ToList().ForEach(c.Child.ChildContents.Add);
                });

                RetrieveChildrenContentsRecursively(entities.Select(c => c.Child).Distinct().ToList());
            }
        }

        public IEnumerable<ChildContentProjection> CreateListOfChildProjectionsRecursively(PageContent pageContent, IEnumerable<ChildContent> children)
        {
            List<ChildContentProjection> childProjections;
            if (children != null && children.Any(c => !c.Child.IsDeleted))
            {
                childProjections = new List<ChildContentProjection>();
                foreach (var child in children.Where(c => !c.Child.IsDeleted).Distinct())
                {
                    var childChildProjections = CreateListOfChildProjectionsRecursively(pageContent, child.Child.ChildContents);
                    var options = optionService.GetMergedOptionValues(child.Child.ContentOptions, child.Options);
                    var childProjection = pageContentProjectionFactory.Create(pageContent, child.Child, options, childChildProjections,
                        (pc, c, a, ch) => new ChildContentProjection(pc, child, a, ch));

                    childProjections.Add(childProjection);
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