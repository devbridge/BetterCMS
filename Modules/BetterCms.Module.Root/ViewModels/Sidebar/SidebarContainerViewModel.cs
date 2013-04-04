using BetterCms.Module.Root.ViewModels;

namespace BetterCms.Module.Root.Models.Sidebar
{
    public class SidebarContainerViewModel
    {
        public PageProjectionsViewModel HeaderProjections { get; set; }

        public PageProjectionsViewModel SideProjections { get; set; }

        public PageProjectionsViewModel BodyProjections { get; set; }

        public string Version { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Version: {0}", Version);
        }
    }
}