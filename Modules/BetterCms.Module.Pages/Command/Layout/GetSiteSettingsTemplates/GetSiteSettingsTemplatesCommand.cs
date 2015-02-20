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
            var query =
               Repository.AsQueryable<Root.Models.Layout>()
                         .Where(f => f.IsDeleted == false)
                         .Select(
                             f =>
                             new SiteSettingTemplateItemViewModel
                             {
                                 Id = f.Id,
                                 Version = f.Version,
                                 TemplateName = f.Name,
                             });

            if (gridOptions != null)
            {
                gridOptions.SetDefaultSortingOptions("TemplateName");
                if (!string.IsNullOrWhiteSpace(gridOptions.SearchQuery))
                {
                    var searchQuery = gridOptions.SearchQuery.ToLowerInvariant();
                    query = query.Where(f => f.TemplateName.ToLower().Contains(searchQuery));
                }
            }

            var count = query.ToRowCountFutureValue();
            var templates = query.AddSortingAndPaging(gridOptions).ToFuture().ToList();

            return new SiteSettingTemplateListViewModel(templates, gridOptions, count.Value);
        }
    }
}