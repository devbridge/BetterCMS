using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
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
        /// The CMS configuration.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

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
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public DeletePageCommand(IRedirectService redirectService, ISitemapService sitemapService, IUrlService urlService, ICmsConfiguration cmsConfiguration)
        {
            this.redirectService = redirectService;
            this.sitemapService = sitemapService;
            this.urlService = urlService;
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="ConcurrentDataException"></exception>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">
        /// </exception>
        public virtual bool Execute(DeletePageViewModel request)
        {
            var page = Repository.First<PageProperties>(request.PageId);
            if (page.Version != request.Version)
            {
                throw new ConcurrentDataException(page);
            }

            if (page.IsMasterPage && Repository.AsQueryable<MasterPage>(mp => mp.Master == page).Any())
            {
                var message = PagesGlobalization.DeletePageCommand_MasterPageHasChildren_Message;
                var logMessage = string.Format("Failed to delete page. Page is selected as master page. Id: {0} Url: {1}", page.Id, page.PageUrl);
                throw new ValidationException(() => message, logMessage);
            }

            request.RedirectUrl = urlService.FixUrl(request.RedirectUrl);

            if (request.UpdateSitemap)
            {
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.EditContent);
            }

            var sitemaps = new Dictionary<Models.Sitemap, bool>();
            var sitemapNodes = sitemapService.GetNodesByPage(page);
            if (request.UpdateSitemap)
            {
                sitemapNodes.Select(node => node.Sitemap)
                            .Distinct()
                            .ToList()
                            .ForEach(
                                sitemap =>
                                sitemaps.Add(
                                    sitemap,
                                    !cmsConfiguration.Security.AccessControlEnabled || AccessControlService.GetAccessLevel(sitemap, Context.Principal) == AccessLevel.ReadWrite));

                foreach (var node in sitemapNodes)
                {
                    if (sitemaps[node.Sitemap] && node.ChildNodes.Count > 0)
                    {
                        var logMessage = string.Format("In {0} sitemap node {1} has {2} child nodes.", node.Sitemap.Id, node.Id, node.ChildNodes.Count);
                        throw new ValidationException(() => PagesGlobalization.DeletePageCommand_SitemapNodeHasChildNodes_Message, logMessage);
                    }
                }
            }

            UnitOfWork.BeginTransaction();

            IList<SitemapNode> updatedNodes = new List<SitemapNode>();
            IList<SitemapNode> deletedNodes = new List<SitemapNode>();
            if (sitemapNodes != null)
            {
                // Archive sitemaps before update.
                sitemaps.Select(pair => pair.Key).ToList().ForEach(sitemap => sitemapService.ArchiveSitemap(sitemap.Id));
                foreach (var node in sitemapNodes)
                {
                    if (!node.IsDeleted)
                    {
                        if (request.UpdateSitemap && sitemaps[node.Sitemap])
                        {
                            // Delete sitemap node.
                            sitemapService.DeleteNode(node, ref deletedNodes);
                        }
                        else
                        {
                            // Unlink sitemap node.
                            if (node.Page != null && node.Page.Id == page.Id)
                            {
                                node.Page = null;
                                node.Url = page.PageUrl;
                                node.UrlHash = page.PageUrlHash;
                                Repository.Save(node);
                                updatedNodes.Add(node);
                            }
                        }
                    }
                }
            }

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

            // Delete child entities.            
            if (page.PageTags != null)
            {
                foreach (var pageTag in page.PageTags)
                {
                    Repository.Delete(pageTag);
                }
            }

            if (page.PageContents != null)
            {
                foreach (var pageContent in page.PageContents)
                {
                    Repository.Delete(pageContent);
                }
            }

            if (page.Options != null)
            {
                foreach (var option in page.Options)
                {
                    Repository.Delete(option);
                }
            }

            if (page.AccessRules != null)
            {
                var rules = page.AccessRules.ToList();
                rules.ForEach(page.RemoveRule);
            }

            if (page.MasterPages != null)
            {
                foreach (var master in page.MasterPages)
                {
                    Repository.Delete(master);
                }
            }

            // Delete page
            Repository.Delete<Root.Models.Page>(request.PageId, request.Version);

            // Commit
            UnitOfWork.Commit();

            var updatedSitemaps = new List<Models.Sitemap>();
            foreach (var node in updatedNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeUpdated(node);
                if (!updatedSitemaps.Contains(node.Sitemap))
                {
                    updatedSitemaps.Add(node.Sitemap);
                }
            }

            foreach (var node in deletedNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeDeleted(node);
                if (!updatedSitemaps.Contains(node.Sitemap))
                {
                    updatedSitemaps.Add(node.Sitemap);
                }
            }

            foreach (var updatedSitemap in updatedSitemaps)
            {
                Events.SitemapEvents.Instance.OnSitemapUpdated(updatedSitemap);
            }

            // Notifying, that page is deleted.
            Events.PageEvents.Instance.OnPageDeleted(page);

            if (sitemaps.Any(tuple => !tuple.Value))
            {
                // Some sitemaps where skipped, because user has no permission to edit.
                Context.Messages.AddSuccess(PagesGlobalization.DeletePage_SitemapSkipped_Message);
            }

            return true;
        }
    }
}