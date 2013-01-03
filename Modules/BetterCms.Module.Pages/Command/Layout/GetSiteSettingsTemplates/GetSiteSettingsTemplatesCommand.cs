using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Pages.ViewModels.Tags;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;

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
                                 PreviewUrl = f.PreviewUrl,
                                 //WidgetEntityType = f.GetType()
                             });

            if (gridOptions != null)
            {
                gridOptions.SetDefaultSortingOptions("TemplateName");
            }

            var count = query.ToRowCountFutureValue();

            var templates = query.AddSortingAndPaging(gridOptions).ToFuture().ToList();

            return new SiteSettingTemplateListViewModel(templates, gridOptions, count.Value);
        }
    }
}