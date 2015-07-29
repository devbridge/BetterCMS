using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

using FluentNHibernate.Utils;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultMasterPageService : IMasterPageService
    {
        private readonly IRepository repository;

        private readonly IOptionService optionService;

        public DefaultMasterPageService(IRepository repository, IOptionService optionService)
        {
            this.repository = repository;
            this.optionService = optionService;
        }

        public IList<Guid> GetPageMasterPageIds(Guid masterPageId)
        {
            // Get list of master page ids
            var ids = repository
                .AsQueryable<MasterPage>()
                .Where(mp => mp.Page.Id == masterPageId)
                .Select(mp => mp.Master.Id).ToList();
            ids.Add(masterPageId);

            return ids.ToList();
        }

        public void SetPageMasterPages(Page page, Guid masterPageId)
        {
            var masterPageIds = GetPageMasterPageIds(masterPageId);

            SetPageMasterPages(page, masterPageIds);
        }

        public void SetPageMasterPages(Page page, IList<Guid> masterPageIds)
        {
            if (page.MasterPages == null)
            {
                page.MasterPages = new List<MasterPage>();
            }

            // Delete master pages not in path
            page.MasterPages.Where(mp => !masterPageIds.Contains(mp.Master.Id)).ToList().ForEach(mp => repository.Delete(mp));

            // Add new master pages to list
            masterPageIds
                .Where(id => page.MasterPages.All(mp => mp.Master.Id != id))
                .Distinct()
                .ToList()
                .ForEach(id => page.MasterPages.Add(new MasterPage
                {
                    Master = repository.AsProxy<Page>(id),
                    Page = page
                }));
        }

        public IList<OptionValueEditViewModel> GetMasterPageOptionValues(Guid id)
        {
            var ids = repository
                    .AsQueryable<Page>()
                    .Where(p => p.Id == id)
                    .Select(p => new
                        {
                            MasterPageId = p.MasterPage != null ? p.MasterPage.Id : (Guid?)null,
                            LayoutId = p.Layout != null ? p.Layout.Id : (Guid?)null
                        })
                    .FirstOne();

            // Load master page options and set all them as parent options
            var options = optionService.GetMergedMasterPagesOptionValues(id, ids.MasterPageId, ids.LayoutId);
            options.ForEach(o =>
                {
                    o.UseDefaultValue = true;
                    o.OptionDefaultValue = o.OptionValue;
                    o.CanDeleteOption = false;
                    o.CanEditOption = false;
                });

            return options;
        }

        public void PrepareForUpdateChildrenMasterPages(PageProperties page, Guid? masterPageId, out IList<Guid> newMasterIds, out IList<Guid> oldMasterIds, out IList<Guid> childrenPageIds, out IList<MasterPage>  existingChildrenMasterPages)
        {
            if ((page.MasterPage != null && page.MasterPage.Id != masterPageId) || (page.MasterPage == null && masterPageId.HasValue))
            {
                newMasterIds = masterPageId.HasValue ? GetPageMasterPageIds(masterPageId.Value) : new List<Guid>(0);

                oldMasterIds = page.MasterPage != null && page.MasterPages != null ? page.MasterPages.Select(mp => mp.Master.Id).Distinct().ToList() : new List<Guid>(0);

                var intersectingIds = newMasterIds.Intersect(oldMasterIds).ToArray();
                foreach (var id in intersectingIds)
                {
                    oldMasterIds.Remove(id);
                    newMasterIds.Remove(id);
                }

                var updatingIds = newMasterIds.Union(oldMasterIds).Distinct().ToList();
                existingChildrenMasterPages = GetChildrenMasterPagesToUpdate(page, updatingIds, out childrenPageIds);
            }
            else
            {
                newMasterIds = null;
                oldMasterIds = null;
                childrenPageIds = null;
                existingChildrenMasterPages = null;
            }
        }

        public void SetMasterOrLayout(PageProperties page, Guid? masterPageId, Guid? layoutId)
        {
            if (masterPageId.HasValue)
            {
                if (page.MasterPage == null || page.MasterPage.Id != masterPageId.Value)
                {
                    page.MasterPage = repository.AsProxy<Page>(masterPageId.Value);
                }

                page.Layout = null;
            }
            else if (layoutId.HasValue)
            {
                if (page.Layout == null || page.Layout.Id != layoutId.Value)
                {
                    page.Layout = repository.AsProxy<Layout>(layoutId.Value);
                }

                page.MasterPage = null;
            }
        }

        /// <summary>
        /// Updates the master page children: instead of old master page inserts the new one.
        /// </summary>
        /// <param name="existingChildrenMasterPages">Already saved children master page assignments.</param>
        /// <param name="oldMasterIds">The old master ids.</param>
        /// <param name="newMasterIds">The new master ids.</param>
        /// <param name="childrenPageIds">The children page ids.</param>
        public void UpdateChildrenMasterPages(IList<MasterPage> existingChildrenMasterPages, IList<Guid> oldMasterIds, IList<Guid> newMasterIds, IEnumerable<Guid> childrenPageIds)
        {
            if (childrenPageIds == null)
            {
                return;
            }

            // Loop in all the distinct master pages
            foreach (var pageId in childrenPageIds)
            {
                // Delete master pages from path
                existingChildrenMasterPages.Where(mp => mp.Page.Id == pageId && oldMasterIds.Contains(mp.Master.Id)).ToList().ForEach(mp => repository.Delete(mp));

                // Add new ones
                newMasterIds.Where(masterPageId => !existingChildrenMasterPages.Any(mp => mp.Page.Id == pageId && mp.Master.Id == masterPageId))
                            .ToList()
                            .ForEach(
                                masterPageId =>
                                {
                                    var mp = new MasterPage
                                    {
                                        Master = repository.AsProxy<Page>(masterPageId),
                                        Page = repository.AsProxy<Page>(pageId)
                                    };
                                    repository.Save(mp);
                                });
            }
        }

        /// <summary>
        /// Retrieves all the master page children, when master page is changed.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="updatingIds">The updating ids.</param>
        /// <param name="childrenPageIds">The children page ids.</param>
        /// <returns>
        /// List of all the childer master pages, which must be changed.
        /// </returns>
        private List<MasterPage> GetChildrenMasterPagesToUpdate(PageProperties page, IList<Guid> updatingIds, out IList<Guid> childrenPageIds)
        {
            // Retrieve all master pages, refering old master and master, which include updating page also as master page
            var query = repository.AsQueryable<MasterPage>().Where(mp => mp.Page.MasterPages.Any(mp1 => mp1.Master.Id == page.Id) || mp.Page.Id == page.Id);

            childrenPageIds = query.Select(mp => mp.Page.Id).Distinct().ToList();
            if (!childrenPageIds.Contains(page.Id))
            {
                childrenPageIds.Add(page.Id);
            }

            return query.Where(mp => updatingIds.Contains(mp.Master.Id)).ToList();
        }

        public int GetLastDynamicRegionNumber()
        {
            ContentRegion crAlias = null;
            Region regionAlias = null;
            Root.Models.Content contentAlias = null;
            Root.Models.Content hAlias = null;
            PageContent pcAlias = null;

            // Load all original contents
            var originalFuture = repository
                .AsQueryOver(() => crAlias)
                .Inner.JoinAlias(() => crAlias.Region, () => regionAlias)
                .Inner.JoinAlias(() => crAlias.Content, () => contentAlias)
                .Inner.JoinAlias(() => contentAlias.PageContents, () => pcAlias)

                .Where(() => contentAlias.GetType() == typeof(HtmlContent))
                .And(() => contentAlias.Status == ContentStatus.Draft
                    || contentAlias.Status == ContentStatus.Published)
                .And(() => !crAlias.IsDeleted
                    && !contentAlias.IsDeleted
                    && !pcAlias.IsDeleted)
                .SelectList(list => list.SelectGroup(() => regionAlias.RegionIdentifier))
                .Future<string>();

            // Load all draft contents
            var draftFuture = repository
                .AsQueryOver(() => crAlias)
                .Inner.JoinAlias(() => crAlias.Region, () => regionAlias)
                .Inner.JoinAlias(() => crAlias.Content, () => hAlias)
                .Inner.JoinAlias(() => hAlias.Original, () => contentAlias)
                .Inner.JoinAlias(() => contentAlias.PageContents, () => pcAlias)

                .Where(() => contentAlias.GetType() == typeof(HtmlContent))
                .And(() => hAlias.Status == ContentStatus.Draft
                    && contentAlias.Status == ContentStatus.Published)
                .And(() => !crAlias.IsDeleted
                    && !contentAlias.IsDeleted
                    && !pcAlias.IsDeleted
                    && !hAlias.IsDeleted)
                .SelectList(list => list.SelectGroup(() => regionAlias.RegionIdentifier))
                .Future<string>();

            var regionIdentifiers = draftFuture.ToList().Concat(originalFuture.ToList()).Distinct();

            return RegionHelper.GetLastDynamicRegionNumber(regionIdentifiers);
        }
    }
}