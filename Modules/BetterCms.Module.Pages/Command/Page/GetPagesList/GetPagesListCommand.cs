using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.SiteSettings;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Services;

using FluentNHibernate.Utils;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Page.GetPagesList
{
    /// <summary>
    /// Command for loading  sorted/filtered list of page view models
    /// </summary>
    public class GetPagesListCommand : CommandBase, ICommand<PagesFilter, PagesGridViewModel<SiteSettingPageViewModel>>
    {
        private readonly ICategoryService categoryService;

        private readonly ILanguageService languageService;
        
        private readonly ILayoutService layoutService;

        private readonly ICmsConfiguration configuration;
        
        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagesListCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="languageService">The language service.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="layoutService">The layout service.</param>
        public GetPagesListCommand(ICategoryService categoryService, ICmsConfiguration configuration,
            ILanguageService languageService, IAccessControlService accessControlService, ILayoutService layoutService)
        {
            this.configuration = configuration;
            this.categoryService = categoryService;
            this.languageService = languageService;
            this.accessControlService = accessControlService;
            this.layoutService = layoutService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Result model.</returns>
        public virtual PagesGridViewModel<SiteSettingPageViewModel> Execute(PagesFilter request)
        {
            request.SetDefaultSortingOptions("Title");

            PageProperties alias = null;
            SiteSettingPageViewModel modelAlias = null;

            var query = UnitOfWork.Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted && alias.Status != PageStatus.Preview);

            // NOTE: below does not work - need to find out how to rewrite it.
            // var nodesSubQuery = QueryOver.Of<SitemapNode>()
            //     .Where(x => x.Page.Id == alias.Id || x.UrlHash == alias.PageUrlHash)
            //     .Select(s => 1)
            //     .Take(1);
            var hasSeoDisjunction =
                Restrictions.Disjunction()
                    .Add(RestrictionsExtensions.IsNullOrWhiteSpace(Projections.Property(() => alias.MetaTitle)))
                    .Add(RestrictionsExtensions.IsNullOrWhiteSpace(Projections.Property(() => alias.MetaKeywords)))
                    .Add(RestrictionsExtensions.IsNullOrWhiteSpace(Projections.Property(() => alias.MetaDescription)));

            var hasSeoProjection = Projections.Conditional(hasSeoDisjunction,
                //.Add(Restrictions.IsNull(Projections.SubQuery(nodesSubQuery))),
                Projections.Constant(false, NHibernateUtil.Boolean),
                Projections.Constant(true, NHibernateUtil.Boolean));

            query = FilterQuery(query, request, hasSeoDisjunction);

            var sitemapNodesFuture = Repository
                .AsQueryable<SitemapNode>()
                .Where(n => !n.IsDeleted && !n.Sitemap.IsDeleted).ToFuture();

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
                    .Select(() => alias.Language.Id).WithAlias(() => modelAlias.LanguageId))
                .TransformUsing(Transformers.AliasToBean<SiteSettingPageViewModel>());

            if (configuration.Security.AccessControlEnabled)
            {
                IEnumerable<Guid> deniedPages = accessControlService.GetDeniedObjects<PageProperties>();
                foreach (var deniedPageId in deniedPages)
                {
                    var id = deniedPageId;
                    query = query.Where(f => f.Id != id);
                }
            }

            var count = query.ToRowCountFutureValue();

            var categoriesFuture = categoryService.GetCategories();
            IEnumerable<LookupKeyValue> languagesFuture = configuration.EnableMultilanguage ? languageService.GetLanguages() : null;

            var pages = query.AddSortingAndPaging(request).Future<SiteSettingPageViewModel>();
            
            var layouts = layoutService
                        .GetAvailableLayouts()
                        .Select(l => new LookupKeyValue(
                            string.Format("{0}-{1}", l.IsMasterPage ? "m" : "l", l.TemplateId), 
                            l.Title))
                        .ToList();

            var model = CreateModel(pages, request, count, categoriesFuture, layouts);

            if (languagesFuture != null)
            {
                model.Languages = languagesFuture.ToList();
                model.Languages.Insert(0, languageService.GetInvariantLanguageModel());
            }

            // NOTE: Query over with subquery in CASE statement and paging des not work.
            if (sitemapNodesFuture != null)
            {
                var nodes = sitemapNodesFuture.ToList();
                foreach (var pageViewModel in model.Items)
                {
                    var hash = pageViewModel.Url.UrlHash();
                    pageViewModel.HasSEO = pageViewModel.HasSEO && nodes.Any(n => n.UrlHash == hash || (n.Page != null && n.Page.Id == pageViewModel.Id));
                }
            }

            return model;
        }

        protected virtual PagesGridViewModel<SiteSettingPageViewModel> CreateModel(IEnumerable<SiteSettingPageViewModel> pages, 
            PagesFilter request, IFutureValue<int> count, IEnumerable<LookupKeyValue> categoriesFuture, IList<LookupKeyValue> layouts)
        {
            return new PagesGridViewModel<SiteSettingPageViewModel>(
                pages.ToList(),
                request,
                count.Value,
                categoriesFuture.ToList())
                   {
                       Layouts = layouts
                   };
        }

        protected virtual IQueryOver<PageProperties, PageProperties> FilterQuery(IQueryOver<PageProperties, PageProperties> query, 
            PagesFilter request, Junction hasSeoDisjunction)
        {
            PageProperties alias = null;

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

            if (request.LanguageId.HasValue)
            {
                if (request.LanguageId.Value.HasDefaultValue())
                {
                    query = query.Where(Restrictions.IsNull(Projections.Property(() => alias.Language.Id)));
                }
                else
                {
                    query = query.Where(Restrictions.Eq(Projections.Property(() => alias.Language.Id), request.LanguageId.Value));
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

            if (request.Status.HasValue)
            {
                if (request.Status.Value == PageStatusFilterType.OnlyPublished)
                {
                    query = query.Where(() => alias.Status == PageStatus.Published);
                }
                else if (request.Status.Value == PageStatusFilterType.OnlyUnpublished)
                {
                    query = query.Where(() => alias.Status != PageStatus.Published);
                }
                else if (request.Status.Value == PageStatusFilterType.ContainingUnpublishedContents)
                {
                    const ContentStatus draft = ContentStatus.Draft;
                    Root.Models.Content contentAlias = null;
                    var subQuery = QueryOver.Of<PageContent>()
                        .JoinAlias(p => p.Content, () => contentAlias)
                        .Where(pageContent => pageContent.Page.Id == alias.Id)
                        .And(() => contentAlias.Status == draft)
                        .And(() => !contentAlias.IsDeleted)
                        .Select(pageContent => 1);

                    query = query.WithSubquery.WhereExists(subQuery);
                }
            }

            if (request.SeoStatus.HasValue)
            {
                var subQuery = QueryOver.Of<SitemapNode>()
                    .Where(x => x.Page.Id == alias.Id || x.UrlHash == alias.PageUrlHash)
                    .And(x => !x.IsDeleted)
                    .JoinQueryOver(s => s.Sitemap)
                    .And(x => !x.IsDeleted)
                    .Select(s => 1);

                if (request.SeoStatus.Value == SeoStatusFilterType.HasSeo)
                {
                    // NOT(seo disjunction) AND EXISTS(subquery)
                    query = query
                        .Where(Restrictions.Not(hasSeoDisjunction))
                        .WithSubquery.WhereExists(subQuery);
                }
                else
                {
                    // seo disjunction OR NOT EXISTS(subquery)
                    var disjunction = hasSeoDisjunction
                        .DeepClone()
                        .Add(Subqueries.WhereNotExists(subQuery));
                    query = query.Where(disjunction);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Layout))
            {
                Guid id;
                var length = request.Layout.Length - 2;
                if (request.Layout.StartsWith("m-") && Guid.TryParse(request.Layout.Substring(2, length), out id))
                {
                    query = query.Where(() => alias.MasterPage.Id == id);
                }

                if (request.Layout.StartsWith("l-") && Guid.TryParse(request.Layout.Substring(2, length), out id))
                {
                    query = query.Where(() => alias.Layout.Id == id);
                }
            }

            if (request.ContentId.HasValue)
            {
                Root.Models.Content contentAlias = null;
                var subQuery = QueryOver.Of<PageContent>()
                    .JoinAlias(p => p.Content, () => contentAlias)
                    .Where(pageContent => pageContent.Page.Id == alias.Id
                        && pageContent.Content.Id == request.ContentId.Value
                        && !pageContent.IsDeleted)
                    .And(() => !contentAlias.IsDeleted)
                    .Select(pageContent => 1);

                query = query.WithSubquery.WhereExists(subQuery);
            }

            return query;
        }
    }
}