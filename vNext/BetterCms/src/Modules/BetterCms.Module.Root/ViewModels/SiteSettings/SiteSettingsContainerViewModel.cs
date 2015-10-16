using BetterCms.Module.Root.ViewModels;

namespace BetterCms.Module.Root.Models.SiteSettingsMenu
{
    /// <summary>
    /// View model for site settings container.
    /// </summary>
    public class SiteSettingsContainerViewModel
    {
        /// <summary>
        /// Gets or sets the menu items.
        /// </summary>
        /// <value>The menu items.</value>
        public PageProjectionsViewModel MenuItems { get; set; }

        public override string ToString()
        {
            // TODO: cannot add any key from this object:
            return string.Empty;
        }
    }
}