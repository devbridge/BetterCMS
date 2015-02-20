using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Page.ClonePage
{
    /// <summary>
    /// A command to clone given page.
    /// </summary>
    public class ClonePageCommand : CommandBase, ICommand<ClonePageViewModel, ClonePageViewModel>
    {
        private readonly IPageCloneService cloneService;

        private readonly ICmsConfiguration cmsConfiguration;

        public ClonePageCommand(IPageCloneService cloneService, ICmsConfiguration cmsConfiguration)
        {
            this.cloneService = cloneService;
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The page view model.</param>
        /// <returns>true if page cloned successfully; false otherwise.</returns>
        public virtual ClonePageViewModel Execute(ClonePageViewModel request)
        {
            var newPage = cloneService.ClonePage(request.PageId, request.PageTitle, request.PageUrl, request.UserAccessList, request.CloneAsMasterPage);

            return new ClonePageViewModel
                       {
                           PageId = newPage.Id,
                           PageTitle = newPage.Title,
                           PageUrl = newPage.PageUrl,
                           IsMasterPage = newPage.IsMasterPage,
                           IsSitemapActionEnabled = ConfigurationHelper.IsSitemapActionEnabledAfterCloningPage(cmsConfiguration)
                       };
        }
    }
}