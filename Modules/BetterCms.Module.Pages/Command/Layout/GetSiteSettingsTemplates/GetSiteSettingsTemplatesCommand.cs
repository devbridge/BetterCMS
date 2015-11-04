using System.Linq;

using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Module.Pages.ViewModels.SiteSettings;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Layout.GetSiteSettingsTemplates
{
    public class GetSiteSettingsTemplatesCommand : CommandBase, ICommand<SearchableGridOptions, SiteSettingTemplateListViewModel>
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="gridOptions">The request.</param>
        /// <returns>A list of paged\sorted widgets.</returns>
        public SiteSettingTemplateListViewModel Execute(SearchableGridOptions gridOptions)
        {
            var masterPages =
                Repository.AsQueryable<Root.Models.Page>()
                    .Where(f => f.IsDeleted == false && f.IsMasterPage)
                    .Select(
                        f =>
                        new SiteSettingTemplateItemViewModel
                        {
                            Id = f.Id,
                            Version = f.Version,
                            TemplateName = f.Title,
                            IsMasterPage = true,
                            Url = f.PageUrl
                        })
                    .ToList();

            var layouts =
               Repository.AsQueryable<Root.Models.Layout>()
                         .Where(f => f.IsDeleted == false)
                         .Select(
                             f =>
                             new SiteSettingTemplateItemViewModel
                             {
                                 Id = f.Id,
                                 Version = f.Version,
                                 TemplateName = f.Name,
                                 IsMasterPage = false
                             });

            var query = masterPages.Union(layouts).AsQueryable();

            if (gridOptions != null)
            {
                gridOptions.SetDefaultSortingOptions("TemplateName");
                if (!string.IsNullOrWhiteSpace(gridOptions.SearchQuery))
                {
                    var searchQuery = gridOptions.SearchQuery.ToLowerInvariant();
                    query = query.Where(f => f.TemplateName.ToLower().Contains(searchQuery));
                }
            }

            var count = query.Count();
            var templates = query.AddSortingAndPaging(gridOptions).ToList();

            return new SiteSettingTemplateListViewModel(templates, gridOptions, count);
        }
    }
}