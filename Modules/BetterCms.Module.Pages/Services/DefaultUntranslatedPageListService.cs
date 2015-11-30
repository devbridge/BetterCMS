// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultUntranslatedPageListService.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.Security;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Category;

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
        /// <param name="categoriesLookupList">The categories.</param>
        /// <returns>
        /// Model
        /// </returns>
        protected override PagesGridViewModel<SiteSettingPageViewModel> CreateModel(System.Collections.Generic.IEnumerable<PageProperties> pages,
            PagesFilter request, NHibernate.IFutureValue<int> count,
            System.Collections.Generic.IList<LookupKeyValue> layouts, System.Collections.Generic.IList<CategoryLookupModel> categoriesLookupList )
        {
            var pagesList = new List<SiteSettingPageViewModel>();
            foreach (var page in pages)
            {
                var model = new SiteSettingPageViewModel();
                model.Id = page.Id;
                model.Version = page.Version;
                model.Title = page.Title;
                model.PageStatus = page.Status;
                model.CreatedOn = page.CreatedOn.ToFormattedDateString();
                model.ModifiedOn = page.ModifiedOn.ToFormattedDateString();
                model.PageUrl = page.PageUrl;
                model.IsMasterPage = page.IsMasterPage;
                model.LanguageId = page.Language != null ? page.Language.Id : Guid.Empty;
                pagesList.Add(model);
            }
            return new UntranslatedPagesGridViewModel<SiteSettingPageViewModel>(
                pagesList,
                request as UntranslatedPagesFilter,
                count.Value) { Layouts = layouts, CategoriesLookupList = categoriesLookupList};
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