using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Sitemap.DeleteSitemap
{
    /// <summary>
    /// Delete sitemap.
    /// </summary>
    public class DeleteSitemapCommand : CommandBase, ICommand<SitemapViewModel, bool>
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the sitemap service.
        /// </summary>
        /// <value>
        /// The sitemap service.
        /// </value>
        public ISitemapService SitemapService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Execution result.</returns>
        public bool Execute(SitemapViewModel request)
        {
            var sitemap = Repository
                .AsQueryable<Models.Sitemap>()
                .Where(map => map.Id == request.Id)
                .FetchMany(map => map.AccessRules)
                .Distinct()
                .ToList()
                .First();

            var roles = new[] { RootModuleConstants.UserRoles.EditContent };
            if (CmsConfiguration.Security.AccessControlEnabled)
            {
                AccessControlService.DemandAccess(sitemap, Context.Principal, AccessLevel.ReadWrite, roles);
            }

            UnitOfWork.BeginTransaction();

            if (sitemap.AccessRules != null)
            {
                var rules = sitemap.AccessRules.ToList();
                rules.ForEach(sitemap.RemoveRule);
            }

            sitemap = Repository.Delete<Models.Sitemap>(request.Id, request.Version);

            UnitOfWork.Commit();

            Events.SitemapEvents.Instance.OnSitemapDeleted(sitemap);

            return true;
        }
    }
}