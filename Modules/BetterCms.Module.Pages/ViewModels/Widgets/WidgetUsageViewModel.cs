using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    public class WidgetUsageViewModel : IEditableGridItem
    {
        public System.Guid Id { get; set; }

        public int Version { get; set; }

        public WidgetUsageType Type { get; set; }

        public string Title { get; set; }
        
        public string Url { get; set; }
    }
}