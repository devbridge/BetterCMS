using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Pages.ViewModels.Widgets;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using BetterModules.Core.Web.Mvc.Commands;

using MvcContrib.Sorting;

namespace BetterCms.Module.Pages.Command.Widget.GetWidgetUsages
{
    /// <summary>
    /// Gets a list of widget usages view models (pages and other widgets).
    /// </summary>
    public class GetWidgetUsagesCommand : CommandBase, ICommand<GetWidgetUsagesCommandRequest, SearchableGridViewModel<WidgetUsageViewModel>>
    {
        private readonly IPageListService pageListService;
        
        private readonly IWidgetService widgetService;

        public GetWidgetUsagesCommand(IPageListService pageListService, IWidgetService widgetService)
        {
            this.pageListService = pageListService;
            this.widgetService = widgetService;
        }

        public SearchableGridViewModel<WidgetUsageViewModel> Execute(GetWidgetUsagesCommandRequest request)
        {
            int pagesTotalCount;
            var pages = GetPages(request, out pagesTotalCount);
            var pageUsages = pages.Select(p => new WidgetUsageViewModel
                {
                    Title = p.Title,
                    Url = p.Url,
                    Id = p.Id,
                    Version = p.Version,
                    Type = p.IsMasterPage ? WidgetUsageType.MasterPage : WidgetUsageType.Page
                });

            int widgetsTotalCount;
            var widgets = GetWidgets(request, out widgetsTotalCount);
            var widgetUsages = widgets.Select(p => new WidgetUsageViewModel
                {
                    Title = p.WidgetName,
                    Id = p.Id,
                    Version = p.Version,
                    Type = WidgetUsageType.HtmlWidget
                });

            var items = pageUsages.Concat(widgetUsages).AsQueryable();
            items = (request.Options.Direction == SortDirection.Descending) ? items.OrderByDescending(i => i.Title) : items.OrderBy(i => i.Title);
            items = items.AddPaging(request.Options);

            return new SearchableGridViewModel<WidgetUsageViewModel>(items, request.Options, pagesTotalCount + widgetsTotalCount);
        }

        private List<SiteSettingWidgetItemViewModel> GetWidgets(GetWidgetUsagesCommandRequest request, out int totalCount)
        {
            var filter = new WidgetsFilter();
            filter.CopyFrom(request.Options);
            filter.PageSize = filter.PageNumber * filter.PageSize;
            filter.PageNumber = 1;
            filter.Column = null;
            filter.ChildContentId = request.WidgetId;

            var widgets = widgetService.GetFilteredWidgetsList(filter);
            totalCount = widgets.TotalCount;

            return widgets.Items.ToList();
        }

        private List<SiteSettingPageViewModel> GetPages(GetWidgetUsagesCommandRequest request, out int totalCount)
        {
            var filter = new PagesFilter();
            filter.CopyFrom(request.Options);
            filter.PageSize = filter.PageNumber * filter.PageSize;
            filter.PageNumber = 1;
            filter.ContentId = request.WidgetId;
            filter.Column = null;
            filter.IncludeArchived = true;
            filter.IncludeMasterPages = true;

            var pages = pageListService.GetFilteredPagesList(filter);
            totalCount = pages.TotalCount;

            return pages.Items.ToList();
        }
    }
}