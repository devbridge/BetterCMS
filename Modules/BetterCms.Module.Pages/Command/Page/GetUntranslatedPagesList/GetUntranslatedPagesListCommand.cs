using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.SiteSettings;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Page.GetUntranslatedPagesList
{
    public class GetUntranslatedPagesListCommand : CommandBase, ICommand<PagesFilter, PagesGridViewModel<SiteSettingPageViewModel>>
    {
        private readonly IUntranslatedPageListService pageListServce;

        public GetUntranslatedPagesListCommand(IUntranslatedPageListService pageListServce)
        {
            this.pageListServce = pageListServce;
        }

        public virtual PagesGridViewModel<SiteSettingPageViewModel> Execute(PagesFilter request)
        {
            return pageListServce.GetFilteredUntranslatedPagesList(request);
        }
    }
}