using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

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
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Execution result.</returns>
        public bool Execute(SitemapViewModel request)
        {
            var sitemap = Repository.First<Models.Sitemap>(request.Id);

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

            // TODO: add new event.
            Events.SitemapEvents.Instance.OnSitemapUpdated(sitemap);

            return true;
        }
    }
}