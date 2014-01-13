using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.History;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.History.RestoreSitemapVersion
{
    /// <summary>
    /// Command for restoring sitemap version.
    /// </summary>
    public class RestoreSitemapVersionCommand : CommandBase, ICommand<SitemapRestoreViewModel, SiteSettingSitemapViewModel>
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the content service.
        /// </summary>
        /// <value>
        /// The content service.
        /// </value>
        public ISitemapService SitemapService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c>, if successfully restored.
        /// </returns>
        public SiteSettingSitemapViewModel Execute(SitemapRestoreViewModel request)
        {
            var archive =
                Repository.AsQueryable<SitemapArchive>()
                          .Where(sitemapArchive => sitemapArchive.Id == request.SitemapVersionId)
                          .Fetch(sitemapArchive => sitemapArchive.Sitemap)
                          .ThenFetchMany(f => f.AccessRules)
                          .Fetch(sitemapArchive => sitemapArchive.Sitemap)
                          .ThenFetchMany(map => map.Nodes)
                          .ThenFetch(node => node.Page)
                          .Fetch(sitemapArchive => sitemapArchive.Sitemap)
                          .ThenFetchMany(map => map.Nodes)
                          .ThenFetchMany(node => node.Translations)
                          .Distinct()
                          .ToList()
                          .First();

            if (CmsConfiguration.Security.AccessControlEnabled)
            {
                AccessControlService.DemandAccess(archive.Sitemap, Context.Principal, AccessLevel.ReadWrite);
            }

// TODO: do we need a such confirmation?
//            // Check if user has confirmed restoration if exist nodes linked to deleted pages.
//            if (!request.IsUserConfirmed)
//            {
//                var hasDeletedPages = SitemapService.CheckIfArchiveHasDeletedPages(archive);
//                if (hasDeletedPages)
//                {
//                    var message = NavigationGlobalization.RestoreSitemap_ArchiveHasDeletedPages_RestoreConfirmationMessage;
//                    var logMessage = string.Format("User is trying to restore sitemap which has deleted pages. Confirmation is required. archiveId: {0}, sitemapId: {1}", archive.Id, archive.Sitemap.Id);
//                    throw new ConfirmationRequestException(() => message, logMessage);
//                }
//            }

            UnitOfWork.BeginTransaction();

            SitemapService.ArchiveSitemap(archive.Sitemap);
            var restoredSitemap = SitemapService.RestoreSitemapFromArchive(archive);

            UnitOfWork.Commit();

            return new SiteSettingSitemapViewModel
                {
                    Id = restoredSitemap.Id,
                    Title = restoredSitemap.Title,
                    Version = restoredSitemap.Version
                };
        }
    }
}