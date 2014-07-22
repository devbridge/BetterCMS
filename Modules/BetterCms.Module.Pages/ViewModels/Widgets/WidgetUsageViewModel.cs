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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Type:{1}, Id:{2}, Title: {3}, Url: {4}", base.ToString(), Type, Id, Title, Url);
        }
    }
}