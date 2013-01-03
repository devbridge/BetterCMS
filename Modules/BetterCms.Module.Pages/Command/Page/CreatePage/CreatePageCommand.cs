using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Command.Page.SavePageProperties;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.CreatePage
{
    public class CreatePageCommand : CommandBase, ICommand<AddNewPageViewModel, SavePageResponse>
    {
        /// <summary>
        /// The page service
        /// </summary>
        private readonly IPageService pageService;

        /// <summary>
        /// The redirect service
        /// </summary>
        private readonly IRedirectService redirectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePageCommand" /> class.
        /// </summary>
        /// <param name="pageService">The page service.</param>
        /// <param name="redirectService">The redirect service.</param>
        public CreatePageCommand(IPageService pageService, IRedirectService redirectService)
        {
            this.pageService = pageService;
            this.redirectService = redirectService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The page view model.</param>
        /// <returns>Created page</returns>
        public virtual SavePageResponse Execute(AddNewPageViewModel request)
        {
            request.PagePermalink = redirectService.FixUrl(request.PagePermalink);

            // Validate Url
            pageService.ValidatePageUrl(request.PagePermalink);

            var page = new PageProperties
                {
                    PageUrl = request.PagePermalink,
                    Title = request.PageTitle,
                    Layout = Repository.AsProxy<Root.Models.Layout>(request.TemplateId)
                };
                
            Repository.Save(page);
            UnitOfWork.Commit();

            return new SavePageResponse(page);
        }
    }
}