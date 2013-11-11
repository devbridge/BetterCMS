using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultMasterPageService : IMasterPageService
    {
        private readonly IRepository repository;

        public DefaultMasterPageService(IRepository repository)
        {
            this.repository = repository;
        }

        public IList<System.Guid> GetPageMasterPageIds(System.Guid masterPageId)
        {
            // Get list of master page ids
            var ids = repository
                .AsQueryable<MasterPage>()
                .Where(mp => mp.Page.Id == masterPageId)
                .Select(mp => mp.Master.Id).ToList();
            ids.Add(masterPageId);

            return ids.ToList();
        }

        public void SetPageMasterPages(Page page, System.Guid masterPageId)
        {
            var masterPageIds = GetPageMasterPageIds(masterPageId);

            SetPageMasterPages(page, masterPageIds);
        }

        public void SetPageMasterPages(Page page, IList<System.Guid> masterPageIds)
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
                .ToList()
                .ForEach(id => page.MasterPages.Add(new MasterPage
                {
                    Master = repository.AsProxy<Page>(id),
                    Page = page
                }));
        }
    }
}