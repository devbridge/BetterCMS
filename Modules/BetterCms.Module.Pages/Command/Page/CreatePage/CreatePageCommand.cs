using System;

using BetterCms.Api;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Command.Page.SavePageProperties;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

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
            // Create / fix page url
            var pageUrl = request.PageUrl;
            var createPageUrl = (pageUrl == null);
            if (createPageUrl && !string.IsNullOrWhiteSpace(request.PageTitle))
            {
                pageUrl = request.PageTitle.Transliterate();
            }
            pageUrl = redirectService.FixUrl(pageUrl);

            // Add parent page url, if is set
            if (createPageUrl)
            {
                var parentPageUrl = request.ParentPageUrl.Trim('/');
                if (!string.IsNullOrWhiteSpace(parentPageUrl))
                {
                    pageUrl = string.Concat(parentPageUrl, pageUrl);
                    pageUrl = redirectService.FixUrl(pageUrl);
                }
            }

            // Validate Url
            pageService.ValidatePageUrl(pageUrl);

            var page = new PageProperties
                {
                    PageUrl = pageUrl,
                    Title = request.PageTitle,
                    Layout = Repository.First<Root.Models.Layout>(request.TemplateId),
                    IsPublic = true
                };
                
            Repository.Save(page);
            UnitOfWork.Commit();

            // Notifying, that page is created
            PagesApiContext.Events.OnPageCreated(page);

            return new SavePageResponse(page);
        }
    }
}