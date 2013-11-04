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

        public void SetPageMasterPages(Page page, System.Guid masterPageId)
        {
            if (page.MasterPages == null)
            {
                page.MasterPages = new List<MasterPage>();
            }

            // Get list of master page ids
            var ids = repository
                .AsQueryable<MasterPage>()
                .Where(mp => mp.Page.Id == masterPageId)
                .Select(mp => mp.Master.Id).ToList();
            ids.Add(masterPageId);

            // Delete master pages not in path
            page.MasterPages.Where(mp => !ids.Contains(mp.Master.Id)).ToList().ForEach(mp => repository.Delete(mp));

            // Add new master pages to list
            ids
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