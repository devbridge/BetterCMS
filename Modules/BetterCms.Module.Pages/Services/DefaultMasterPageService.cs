using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

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
    }
}