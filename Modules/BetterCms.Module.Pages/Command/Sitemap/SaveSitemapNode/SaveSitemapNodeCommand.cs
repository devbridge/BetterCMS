using System.Linq;

using BetterCms.Core.Security;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Sitemap.SaveSitemapNode
{
    /// <summary>
    /// Saves sitemap node data.
    /// </summary>
    public class SaveSitemapNodeCommand : CommandBase, ICommand<SitemapNodeViewModel, SitemapNodeViewModel>
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
        public SitemapNodeViewModel Execute(SitemapNodeViewModel request)
        {
            var sitemap =
                Repository.AsQueryable<Models.Sitemap>()
                          .Where(map => map.Id == request.SitemapId)
                          .FetchMany(f => f.AccessRules)
                          .FetchMany(map => map.Nodes)
                          .ThenFetch(mapNode => mapNode.Page)
                          .FetchMany(map => map.Nodes)
                          .ThenFetch(mapNode => mapNode.Translations)
                          .Distinct()
                          .ToList()
                          .First();

            if (CmsConfiguration.Security.AccessControlEnabled)
            {
                AccessControlService.DemandAccess(sitemap, Context.Principal, AccessLevel.ReadWrite);
            }

            UnitOfWork.BeginTransaction();

            if (!request.SitemapId.HasDefaultValue())
            {
                SitemapService.ArchiveSitemap(sitemap);
            }

            bool updatedInDB;
            var node = SitemapService.SaveNode(out updatedInDB, sitemap, request.Id, request.Version, request.Url, request.Title, request.Macro, request.PageId, request.UsePageTitleAsNodeTitle, request.DisplayOrder, request.ParentId);

            UnitOfWork.Commit();

            if (updatedInDB)
            {
                if (request.Id.HasDefaultValue())
                {
                    Events.SitemapEvents.Instance.OnSitemapNodeCreated(node);
                }
                else
                {
                    Events.SitemapEvents.Instance.OnSitemapNodeUpdated(node);
                }

                Events.SitemapEvents.Instance.OnSitemapUpdated(node.Sitemap);
            }

            return new SitemapNodeViewModel
                {
                    Id = node.Id,
                    Version = node.Version
                };
        }
    }
}