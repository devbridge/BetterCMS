using System;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Pages.ViewModels.Widgets;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Pages.Command.Widget.GetWidgetUsages
{
    /// <summary>
    /// Gets a list of widget usages view models (pages and other widgets).
    /// </summary>
    public class GetWidgetUsagesCommand : CommandBase, ICommand<GetWidgetUsagesCommandRequest, SearchableGridViewModel<WidgetUsageViewModel>>
    {
        public SearchableGridViewModel<WidgetUsageViewModel> Execute(GetWidgetUsagesCommandRequest request)
        {
            var items = new System.Collections.Generic.List<WidgetUsageViewModel>
                        {
                            new WidgetUsageViewModel
                            {
                                Title = "Page 1",
                                Url = "/tests",
                                Id = new Guid("7329B110-4E6F-4A1E-B89D-0CB0C1299B73"),
                                Version = 1,
                                Type = WidgetUsageType.Page
                            },

                            new WidgetUsageViewModel
                            {
                                Title = "Widget 1",
                                Id = new Guid("AFA0AFEF-6D71-4962-9EF4-324BB9344F92"),
                                Version = 2,
                                Type = WidgetUsageType.HtmlWidget
                            }
                        };

            return new SearchableGridViewModel<WidgetUsageViewModel>(items, request.Options, 46);
        }
    }
}