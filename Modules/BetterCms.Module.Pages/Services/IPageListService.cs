using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.SiteSettings;

namespace BetterCms.Module.Pages.Services
{
    public interface IPageListService
    {
        /// <summary>
        /// Gets the filtered pages list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The filtered and sorted list pf pages</returns>
        PagesGridViewModel<SiteSettingPageViewModel> GetFilteredPagesList(PagesFilter request);
    }
}