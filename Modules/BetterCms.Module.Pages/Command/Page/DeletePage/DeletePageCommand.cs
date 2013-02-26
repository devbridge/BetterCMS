using System;

using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Events;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.DeletePage
{
    /// <summary>
    /// Command deletes a CMS page
    /// </summary>
    public class DeletePageCommand : CommandBase, ICommand<DeletePageViewModel, bool>
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
        /// Initializes a new instance of the <see cref="DeletePageCommand" /> class.
        /// </summary>
        /// <param name="pageService">The page service.</param>
        /// <param name="redirectService">The redirect service.</param>
        public DeletePageCommand(IPageService pageService, IRedirectService redirectService)
        {
            this.pageService = pageService;
            this.redirectService = redirectService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public virtual bool Execute(DeletePageViewModel request)
        {
            var page = Repository.First<PageProperties>(request.PageId);
            if (page.Version != request.Version)
            {
                throw new ConcurrentDataException(page);
            }

            request.RedirectUrl = redirectService.FixUrl(request.RedirectUrl);

            if (!string.IsNullOrWhiteSpace(request.RedirectUrl))
            {
                if (string.Equals(page.PageUrl, request.RedirectUrl, StringComparison.OrdinalIgnoreCase))
                {
                    var logMessage = string.Format("Circular redirect loop from url {0} to url {0}.", request.RedirectUrl);
                    throw new ValidationException(() => PagesGlobalization.ValidatePageUrlCommand_SameUrlPath_Message, logMessage);
                }

                // Validate url
                if (!redirectService.ValidateUrl(request.RedirectUrl))
                {
                    var logMessage = string.Format("Invalid page url {0}.", request.RedirectUrl);
                    throw new ValidationException(() => PagesGlobalization.ValidatePageUrlCommand_InvalidUrlPath_Message, logMessage);
                }

                var redirect = redirectService.GetPageRedirect(page.PageUrl);
                if (redirect != null)
                {
                    redirect.RedirectUrl = request.RedirectUrl;
                }
                else
                {
                    redirect = redirectService.CreateRedirectEntity(page.PageUrl, request.RedirectUrl);    
                }
                
                if (redirect != null)
                {
                    Repository.Save(redirect);
                }
            }

            // Delete childs
            foreach (var pageTag in page.PageTags)
            {
                Repository.Delete(pageTag);
            }
            foreach (var pageContent in page.PageContents)
            {
                Repository.Delete(pageContent);
            }

            // Delete page
            Repository.Delete<Root.Models.Page>(request.PageId, request.Version);
            
            // Commit
            UnitOfWork.Commit();

            // Notifying, that page is deleted
            PagesApiContext.Events.OnPageDeleted(this, new PageDeletedEventArgs(page));

            return true;
        }
    }
}