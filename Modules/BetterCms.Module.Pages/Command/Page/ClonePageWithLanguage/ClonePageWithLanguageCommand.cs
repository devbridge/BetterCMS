using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.ClonePageWithLanguage
{
    /// <summary>
    /// A command to clone given page.
    /// </summary>
    public class ClonePageWithLanguageCommand : CommandBase, ICommand<ClonePageWithLanguageViewModel, ClonePageWithLanguageViewModel>
    {
        private readonly IPageCloneService cloneService;
        private readonly IPageService pageService;

        public ClonePageWithLanguageCommand(IPageCloneService cloneService, IPageService pageService)
        {
            this.cloneService = cloneService;
            this.pageService = pageService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The page view model.</param>
        /// <returns>true if page cloned successfully; false otherwise.</returns>
        public virtual ClonePageWithLanguageViewModel Execute(ClonePageWithLanguageViewModel request)
        {
            var languageGroupIdentifier = Repository
                .AsQueryable<Root.Models.Page>(p => p.Id == request.PageId)
                .Select(p => new { LanguageGroupIdentifier = p.LanguageGroupIdentifier })
                .FirstOne()
                .LanguageGroupIdentifier;
            
            if (!languageGroupIdentifier.HasValue)
            {
                languageGroupIdentifier = System.Guid.NewGuid();
            }

            var languageId = !request.LanguageId.HasValue ? System.Guid.Empty : request.LanguageId.Value;

            var newPage = cloneService.ClonePageWithLanguage(request.PageId, request.PageTitle, 
                request.PageUrl, request.UserAccessList, languageId, languageGroupIdentifier.Value);

            return new ClonePageWithLanguageViewModel
                {
                    PageId = newPage.Id,
                    PageTitle = newPage.Title,
                    PageUrl = newPage.PageUrl,
                    IsMasterPage = newPage.IsMasterPage,
                    LanguageId = request.LanguageId
                };
        }
    }
}