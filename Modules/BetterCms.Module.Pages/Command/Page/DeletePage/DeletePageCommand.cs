using System;
using System.Collections.Generic;

using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.DeletePage
{
    /// <summary>
    /// Command deletes a CMS page
    /// </summary>
    public class DeletePageCommand : CommandBase, ICommand<DeletePageViewModel, bool>
    {
        /// <summary>
        /// The redirect service.
        /// </summary>
        private readonly IRedirectService redirectService;

        /// <summary>
        /// The url service
        /// </summary>
        private readonly IUrlService urlService;

        /// <summary>
        /// The sitemap service.
        /// </summary>
        private readonly ISitemapService sitemapService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeletePageCommand" /> class.
        /// </summary>
        /// <param name="redirectService">The redirect service.</param>
        /// <param name="sitemapService">The sitemap service.</param>
        /// <param name="urlService">The URL service.</param>
        public DeletePageCommand(IRedirectService redirectService,
            ISitemapService sitemapService, IUrlService urlService)
        {
            this.redirectService = redirectService;
            this.sitemapService = sitemapService;
            this.urlService = urlService;
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

            request.RedirectUrl = urlService.FixUrl(request.RedirectUrl);

            IList<SitemapNode> sitemapNodes = null;

            if (request.UpdateSitemap)
            {
                DemandAccess(RootModuleConstants.UserRoles.EditContent);
            }

            if (request.UpdateSitemap && page.NodeCountInSitemap > 0)
            {
                sitemapNodes = sitemapService.GetNodesByUrl(page.PageUrl);
                foreach (var node in sitemapNodes)
                {
                    if (node.ChildNodes.Count > 0)
                    {
                        var logMessage = string.Format("Sitemap node {0} has {1} child nodes.", node.Id, node.ChildNodes.Count);
                        throw new ValidationException(() => PagesGlobalization.DeletePageCommand_SitemapNodeHasChildNodes_Message, logMessage);
                    }
                }
            }

            UnitOfWork.BeginTransaction();

            if (!string.IsNullOrWhiteSpace(request.RedirectUrl))
            {
                if (string.Equals(page.PageUrl, request.RedirectUrl, StringComparison.OrdinalIgnoreCase))
                {
                    var logMessage = string.Format("Circular redirect loop from url {0} to url {0}.", request.RedirectUrl);
                    throw new ValidationException(() => PagesGlobalization.ValidatePageUrlCommand_SameUrlPath_Message, logMessage);
                }

                // Validate url
                if (!urlService.ValidateUrl(request.RedirectUrl))
                {
                    var logMessage = string.Format("Invalid redirect url {0}.", request.RedirectUrl);
                    throw new ValidationException(() => PagesGlobalization.ValidatePageUrlCommand_InvalidUrlPath_Message, logMessage);
                }

                string patternsValidationMessage;
                if (!urlService.ValidateUrlPatterns(request.RedirectUrl, out patternsValidationMessage, PagesGlobalization.DeletePage_RedirectUrl_Name))
                {
                    var logMessage = string.Format("{0}. URL: {1}.", patternsValidationMessage, request.RedirectUrl);
                    throw new ValidationException(() => patternsValidationMessage, logMessage);
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

            // Delete sitemapNodes.
            if (sitemapNodes != null)
            {
                foreach (var node in sitemapNodes)
                {
                    sitemapService.DeleteNodeWithoutPageUpdate(node);
                }

                page.NodeCountInSitemap -= sitemapNodes.Count;
                Repository.Save(page);
            }

            // Delete page
            Repository.Delete<Root.Models.Page>(request.PageId, request.Version);

            // Commit
            UnitOfWork.Commit();

            // Notifying, that page is deleted.
            Events.PageEvents.Instance.OnPageDeleted(page);

            return true;
        }
    }
}