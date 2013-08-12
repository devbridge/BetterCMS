using System.Linq;
using System.Collections.Generic;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultLayoutService : ILayoutService
    {
        private IRepository repository;

        public DefaultLayoutService(IRepository repository)
        {
            this.repository = repository;
        }

        public IList<TemplateViewModel> GetTemplates()
        {
            var templates = repository
                .AsQueryable<Layout>()
                .OrderBy(t => t.Name)
                .Select(t => new TemplateViewModel
                    {
                        Title = t.Name,
                        TemplateId = t.Id,
                        PreviewUrl = t.PreviewUrl
                    })
                .ToList();

            return templates;
        }
    }
}