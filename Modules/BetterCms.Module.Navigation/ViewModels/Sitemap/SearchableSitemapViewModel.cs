using System.Collections.Generic;

namespace BetterCms.Module.Navigation.ViewModels.Sitemap
{
    /// <summary>
    /// View model for image media data.
    /// </summary>
    public class SearchableSitemapViewModel
    {
        public string SearchQuery { get; set; }

        public IList<SitemapNodeViewModel> RootNodes { get; set; }
    }
}