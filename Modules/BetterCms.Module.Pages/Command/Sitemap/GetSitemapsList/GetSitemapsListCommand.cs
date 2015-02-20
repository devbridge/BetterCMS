using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Security;
using BetterCms.Core.Services;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.SiteSettings;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;

using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Sitemap.GetSitemapsList
{
    /// <summary>
    /// Command for loading  sorted/filtered list of sitemap view models.
    /// </summary>
    public class GetSitemapsListCommand : CommandBase, ICommand<SitemapsFilter, SitemapsGridViewModel<SiteSettingSitemapViewModel>>
    {
        /// <summary>
        /// The access control service.
        /// </summary>
        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// The security service.
        /// </summary>
        private readonly ISecurityService securityService;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSitemapsListCommand" /> class.
        /// </summary>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="configuration">The configuration.</param>
        public GetSitemapsListCommand(IAccessControlService accessControlService, ISecurityService securityService, ICmsConfiguration configuration)
        {
            this.configuration = configuration;
            this.securityService = securityService;
            this.accessControlService = accessControlService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Result model.</returns>
        public SitemapsGridViewModel<SiteSettingSitemapViewModel> Execute(SitemapsFilter request)
        {
            request.SetDefaultSortingOptions("Title");

            Models.Sitemap alias = null;
            SiteSettingSitemapViewModel modelAlias = null;

            var query = UnitOfWork.Session.QueryOver(() => alias).Where(() => !alias.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query = query.Where(Restrictions.Disjunction().Add(Restrictions.InsensitiveLike(Projections.Property(() => alias.Title), searchQuery)));
            }

            if (request.Tags != null)
            {
                foreach (var tagKeyValue in request.Tags)
                {
                    var id = tagKeyValue.Key.ToGuidOrDefault();
                    query = query.WithSubquery.WhereExists(QueryOver.Of<PageTag>().Where(tag => tag.Tag.Id == id && tag.Page.Id == alias.Id).Select(tag => 1));
                }
            }

            query =
                query.SelectList(
                    select =>
                    select.Select(() => alias.Id)
                          .WithAlias(() => modelAlias.Id)
                          .Select(() => alias.Version)
                          .WithAlias(() => modelAlias.Version)
                          .Select(() => alias.Title)
                          .WithAlias(() => modelAlias.Title)).TransformUsing(Transformers.AliasToBean<SiteSettingSitemapViewModel>());

            if (configuration.Security.AccessControlEnabled)
            {
                IEnumerable<Guid> deniedPages = GetDeniedSitemaps();
                foreach (var deniedPageId in deniedPages)
                {
                    var id = deniedPageId;
                    query = query.Where(f => f.Id != id);
                }
            }

            var count = query.ToRowCountFutureValue();

            var sitemaps = query.AddSortingAndPaging(request).Future<SiteSettingSitemapViewModel>();

            return new SitemapsGridViewModel<SiteSettingSitemapViewModel>(sitemaps.ToList(), request, count.Value);
        }

        /// <summary>
        /// Gets the denied sitemaps.
        /// </summary>
        /// <returns>Denied Sitemaps it list.</returns>
        private IEnumerable<Guid> GetDeniedSitemaps()
        {
            var query = Repository.AsQueryable<Models.Sitemap>().Where(f => f.AccessRules.Any(b => b.AccessLevel == AccessLevel.Deny)).FetchMany(f => f.AccessRules);

            var list = query.ToList().Distinct();
            var principle = securityService.GetCurrentPrincipal();

            foreach (var sitemap in list)
            {
                var accessLevel = accessControlService.GetAccessLevel(sitemap, principle);
                if (accessLevel == AccessLevel.Deny)
                {
                    yield return sitemap.Id;
                }
            }
        }
    }
}