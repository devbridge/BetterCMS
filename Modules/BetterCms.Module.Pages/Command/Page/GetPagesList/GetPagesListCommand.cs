using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.SiteSettings;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Page.GetPagesList
{
    /// <summary>
    /// Command for loading  sorted/filtered list of page view models
    /// </summary>
    public class GetPagesListCommand : CommandBase, ICommand<PagesFilter, PagesGridViewModel<SiteSettingPageViewModel>>
    {
        private readonly IPageListService pageListService;

        public GetPagesListCommand(IPageListService pageListService)
        {
            this.pageListService = pageListService;
        }

        public virtual PagesGridViewModel<SiteSettingPageViewModel> Execute(PagesFilter request)
        {
            return pageListService.GetFilteredPagesList(request);
        }
    }
}