using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Core.Services;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.SiteSettings;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Services;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Page.GetPagesList
{
    /// <summary>
    /// Command for loading  sorted/filtered list of page view models
    /// </summary>
    public class GetPagesListCommand : CommandBase, ICommand<PagesFilter, PagesGridViewModel<SiteSettingPageViewModel>>
    {
        private readonly ICategoryService categoryService;

        private readonly ICultureService cultureService;

        private readonly IAccessControlService accessControlService;

        private readonly ISecurityService securityService;

        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagesListCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cultureService">The culture service.</param>
        public GetPagesListCommand(ICategoryService categoryService, IAccessControlService accessControlService, ISecurityService securityService,
                                   ICmsConfiguration configuration, ICultureService cultureService)
        {
            this.configuration = configuration;
            this.securityService = securityService;
            this.accessControlService = accessControlService;
            this.categoryService = categoryService;
            this.cultureService = cultureService;
        }

        private IEnumerable<Guid> GetDeniedPages(PagesFilter request)
        {
            var query = Repository.AsQueryable<Root.Models.Page>()
                            .Where(f => f.AccessRules.Any(b => b.AccessLevel == AccessLevel.Deny))
                            .FetchMany(f => f.AccessRules);                            

            var list = query.ToList().Distinct();
            var principle = securityService.GetCurrentPrincipal();
            
            foreach (var page in list)
            {
                var accessLevel = accessControlService.GetAccessLevel(page, principle);
                if (accessLevel == AccessLevel.Deny)
                {
                    yield return page.Id;
                }
            }           
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Result model.</returns>
        public PagesGridViewModel<SiteSettingPageViewModel> Execute(PagesFilter request)
        {
            request.SetDefaultSortingOptions("Title");

            PageProperties alias = null;
            SiteSettingPageViewModel modelAlias = null;

            var query = UnitOfWork.Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted && alias.Status != PageStatus.Preview);

            if (!request.IncludeArchived)
            {
                query = query.Where(() => !alias.IsArchived);
            }

            if (request.OnlyMasterPages)
            {
                query = query.Where(() => alias.IsMasterPage);
            }
            else if (!request.IncludeMasterPages)
            {
                query = query.Where(() => !alias.IsMasterPage);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query = query.Where(Restrictions.Disjunction()
                                        .Add(Restrictions.InsensitiveLike(Projections.Property(() => alias.Title), searchQuery))
                                        .Add(Restrictions.InsensitiveLike(Projections.Property(() => alias.PageUrl), searchQuery))
                                        .Add(Restrictions.InsensitiveLike(Projections.Property(() => alias.MetaTitle), searchQuery))
                                        .Add(Restrictions.InsensitiveLike(Projections.Property(() => alias.MetaDescription), searchQuery))
                                        .Add(Restrictions.InsensitiveLike(Projections.Property(() => alias.MetaKeywords), searchQuery)));
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(Restrictions.Eq(Projections.Property(() => alias.Category.Id), request.CategoryId.Value));
            }
            
            if (request.CultureId.HasValue)
            {
                if (request.CultureId.Value.HasDefaultValue())
                {
                    query = query.Where(Restrictions.IsNull(Projections.Property(() => alias.Culture.Id)));
                }
                else
                {
                    query = query.Where(Restrictions.Eq(Projections.Property(() => alias.Culture.Id), request.CultureId.Value));
                }
            }

            if (request.Tags != null)
            {
                foreach (var tagKeyValue in request.Tags)
                {
                    var id = tagKeyValue.Key.ToGuidOrDefault();
                    query = query.WithSubquery.WhereExists(QueryOver.Of<PageTag>().Where(tag => tag.Tag.Id == id && tag.Page.Id == alias.Id).Select(tag => 1));
                }
            }

            IProjection hasSeoProjection = Projections.Conditional(
                Restrictions.Disjunction()
                    .Add(RestrictionsExtensions.IsNullOrWhiteSpace(Projections.Property(() => alias.MetaTitle)))
                    .Add(RestrictionsExtensions.IsNullOrWhiteSpace(Projections.Property(() => alias.MetaKeywords)))
                    .Add(RestrictionsExtensions.IsNullOrWhiteSpace(Projections.Property(() => alias.MetaDescription))),
                Projections.Constant(false, NHibernateUtil.Boolean),
                Projections.Constant(true, NHibernateUtil.Boolean));

            query = query
                .SelectList(select => select
                    .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                    .Select(() => alias.Version).WithAlias(() => modelAlias.Version)
                    .Select(() => alias.Title).WithAlias(() => modelAlias.Title)
                    .Select(() => alias.Status).WithAlias(() => modelAlias.PageStatus)
                    .Select(hasSeoProjection).WithAlias(() => modelAlias.HasSEO)
                    .Select(() => alias.CreatedOn).WithAlias(() => modelAlias.CreatedOn)
                    .Select(() => alias.ModifiedOn).WithAlias(() => modelAlias.ModifiedOn)
                    .Select(() => alias.PageUrl).WithAlias(() => modelAlias.Url)
                    .Select(() => alias.Culture.Id).WithAlias(() => modelAlias.CultureId))
                .TransformUsing(Transformers.AliasToBean<SiteSettingPageViewModel>());

            if (configuration.Security.AccessControlEnabled)
            {
                IEnumerable<Guid> deniedPages = GetDeniedPages(request);
                foreach (var deniedPageId in deniedPages)
                {
                    var id = deniedPageId;
                    query = query.Where(f => f.Id != id);
                }
            }

            var count = query.ToRowCountFutureValue();

            var categoriesFuture = categoryService.GetCategories();
            IEnumerable<LookupKeyValue> culturesFuture = configuration.EnableMultilanguage ? cultureService.GetCultures() : null;

            var pages = query.AddSortingAndPaging(request).Future<SiteSettingPageViewModel>();

            var model = new PagesGridViewModel<SiteSettingPageViewModel>(
                pages.ToList(), 
                request, 
                count.Value, 
                categoriesFuture.ToList());

            if (culturesFuture != null)
            {
                model.Cultures = culturesFuture.ToList();
                model.Cultures.Insert(0, new LookupKeyValue(Guid.Empty.ToString(), PagesGlobalization.InvariantCulture_Title));
            }

            return model;
        }
    }
}