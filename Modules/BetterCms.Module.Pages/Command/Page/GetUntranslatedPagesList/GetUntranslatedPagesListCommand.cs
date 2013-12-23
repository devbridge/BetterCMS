using System.Linq;

using BetterCms.Module.Pages.Command.Page.GetPagesList;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Filter;

using BetterCms.Module.Root.Services;

using NHibernate.Criterion;

namespace BetterCms.Module.Pages.Command.Page.GetUntranslatedPagesList
{
    public class GetUntranslatedPagesListCommand : GetPagesListCommand
    {
        public GetUntranslatedPagesListCommand(ICategoryService categoryService, ICmsConfiguration configuration, ICultureService cultureService, IPageService pageService)
            : base(categoryService, configuration, cultureService, pageService)
        {
        }

        public override PagesGridViewModel<ViewModels.SiteSettings.SiteSettingPageViewModel> Execute(PagesFilter request)
        {
            var model = base.Execute(request);
            if (model != null)
            {
                model.HideMasterPagesFiltering = true;

                var filter = request as UntranslatedPagesFilter;
                if (model.Cultures != null && filter != null && filter.ExcludedCultureId.HasValue)
                {
                    var cultureToExclude = model.Cultures.FirstOrDefault(c => c.Key == filter.ExcludedCultureId.Value.ToString().ToLowerInvariant());
                    if (cultureToExclude != null)
                    {
                        model.Cultures.Remove(cultureToExclude);
                    }
                }

                model.Action = controller => controller.SearchUntranslatedPages(null);
            }

            return model;
        }

        protected override NHibernate.IQueryOver<PageProperties, PageProperties> FilterQuery(NHibernate.IQueryOver<PageProperties, PageProperties> query, PagesFilter request)
        {
            query = base.FilterQuery(query, request);

            var filter = request as UntranslatedPagesFilter;
            if (filter != null)
            {
                PageProperties alias = null;

                // Exclude from results
                if (filter.ExistingItemsArray.Any())
                {
                    query = query.Where(Restrictions.Not(Restrictions.In(Projections.Property(() => alias.Id), filter.ExistingItemsArray)));
                }

                // Excluded culture id
                if (filter.ExcludedCultureId.HasValue)
                {
                    var cultureProxy = Repository.AsProxy<Root.Models.Culture>(filter.ExcludedCultureId.Value);
                    query = query.Where(() => (alias.Culture != cultureProxy || alias.Culture == null));
                }
                
                if (filter.ExcplicitlyIncludedPagesArray.Any())
                {
                    // Include to results explicitly included or untranslated
                    query = query.Where(Restrictions.Disjunction()
                        .Add(Restrictions.In(Projections.Property(() => alias.Id), filter.ExcplicitlyIncludedPagesArray))
                        .Add(Restrictions.IsNull(Projections.Property(() => alias.CultureGroupIdentifier))));
                }
                else
                {
                    // Only untranslated
                    query = query.Where(Restrictions.IsNull(Projections.Property(() => alias.CultureGroupIdentifier)));
                }
            }

            return query;
        }
    }
}