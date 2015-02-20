using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.Security;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;

using NHibernate.Criterion;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultUntranslatedPageListService : DefaultPageListService, IUntranslatedPageListService
    {
        private readonly IRepository repository;

        public DefaultUntranslatedPageListService(ICategoryService categoryService, 
            ICmsConfiguration configuration, 
            ILanguageService languageService, 
            IAccessControlService accessControlService, 
            ILayoutService layoutService, 
            IUnitOfWork unitOfWork,
            IRepository repository)
            : base(categoryService, configuration, languageService, accessControlService, layoutService, unitOfWork)
        {
            this.repository = repository;
        }

        public PagesGridViewModel<SiteSettingPageViewModel> GetFilteredUntranslatedPagesList(PagesFilter request)
        {
            var model = GetFilteredPagesList(request);
            if (model != null)
            {
                model.HideMasterPagesFiltering = true;

                var filter = request as UntranslatedPagesFilter;
                if (model.Languages != null && filter != null && filter.ExcludedLanguageId.HasValue)
                {
                    var languageToExclude = model.Languages.FirstOrDefault(c => c.Key == filter.ExcludedLanguageId.Value.ToString().ToLowerInvariant());
                    if (languageToExclude != null)
                    {
                        model.Languages.Remove(languageToExclude);
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <param name="pages">The pages.</param>
        /// <param name="request">The request.</param>
        /// <param name="count">The count.</param>
        /// <param name="categoriesFuture">The categories future.</param>
        /// <param name="layouts">The layouts.</param>
        /// <returns>
        /// Model
        /// </returns>
        protected override PagesGridViewModel<SiteSettingPageViewModel> CreateModel(System.Collections.Generic.IEnumerable<SiteSettingPageViewModel> pages,
            PagesFilter request, NHibernate.IFutureValue<int> count,
            System.Collections.Generic.IList<LookupKeyValue> layouts)
        {
            return new UntranslatedPagesGridViewModel<SiteSettingPageViewModel>(
                pages.ToList(),
                request as UntranslatedPagesFilter,
                count.Value) { Layouts = layouts };
        }

        /// <summary>
        /// Filters the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <param name="hasnotSeoDisjunction">The has seo disjunction.</param>
        /// <returns>
        /// Query, filtered with specified filter parameters
        /// </returns>
        protected override NHibernate.IQueryOver<PagesView, PagesView> FilterQuery(NHibernate.IQueryOver<PagesView, PagesView> query,
            PagesFilter request, Junction hasnotSeoDisjunction)
        {
            query = base.FilterQuery(query, request, hasnotSeoDisjunction);

            var filter = request as UntranslatedPagesFilter;
            if (filter != null)
            {
                PageProperties alias = null;

                // Exclude from results
                if (filter.ExistingItemsArray.Any())
                {
                    query = query.Where(Restrictions.Not(Restrictions.In(Projections.Property(() => alias.Id), filter.ExistingItemsArray)));
                }

                // Excluded language id
                if (filter.ExcludedLanguageId.HasValue)
                {
                    var languageProxy = repository.AsProxy<Language>(filter.ExcludedLanguageId.Value);
                    query = query.Where(() => (alias.Language != languageProxy || alias.Language == null));
                }

                if (filter.ExcplicitlyIncludedPagesArray.Any())
                {
                    // Include to results explicitly included or untranslated
                    query = query.Where(Restrictions.Disjunction()
                        .Add(Restrictions.In(Projections.Property(() => alias.Id), filter.ExcplicitlyIncludedPagesArray))
                        .Add(Restrictions.IsNull(Projections.Property(() => alias.LanguageGroupIdentifier))));
                }
                else
                {
                    // Only untranslated
                    query = query.Where(Restrictions.IsNull(Projections.Property(() => alias.LanguageGroupIdentifier)));
                }
            }

            return query;
        }
    }
}