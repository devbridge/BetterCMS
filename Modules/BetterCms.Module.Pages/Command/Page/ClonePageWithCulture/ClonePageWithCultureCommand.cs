using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;

using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.ClonePageWithCulture
{
    /// <summary>
    /// A command to clone given page.
    /// </summary>
    public class ClonePageWithCultureCommand : CommandBase, ICommand<ClonePageWithCultureViewModel, ClonePageWithCultureViewModel>
    {
        private readonly IPageCloneService cloneService;
        private readonly IPageService pageService;

        public ClonePageWithCultureCommand(IPageCloneService cloneService, IPageService pageService)
        {
            this.cloneService = cloneService;
            this.pageService = pageService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The page view model.</param>
        /// <returns>true if page cloned successfully; false otherwise.</returns>
        public virtual ClonePageWithCultureViewModel Execute(ClonePageWithCultureViewModel request)
        {
            var mainPageCultureId = pageService.GetMainCulturePageId(request.PageId);
            var newPage = cloneService.ClonePageWithCulture(request.PageId, request.PageTitle, 
                request.PageUrl, request.UserAccessList, request.CultureId, mainPageCultureId);

            return new ClonePageWithCultureViewModel
                {
                    PageId = newPage.Id,
                    PageTitle = newPage.Title,
                    PageUrl = newPage.PageUrl,
                    IsMasterPage = newPage.IsMasterPage,
                    CultureId = request.CultureId
                };
        }
    }
}