using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Root.Helpers;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Category;
using BetterCms.Module.Root.ViewModels.Security;

using NHibernate.Linq;

namespace BetterCms.Module.Root.Commands.Category.GetCategoryTree
{
    public class GetCategoryTreeCommand : CommandBase, ICommand<Guid, CategoryTreeViewModel>
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        public CategoryTreeViewModel Execute(Guid sitemapId)
        {
            if (sitemapId.HasDefaultValue())
            {
// TODO:                var langs = CmsConfiguration.EnableMultilanguage ? LanguageService.GetLanguagesLookupValues().ToList() : new List<LookupKeyValue>();
                return new CategoryTreeViewModel()
                {
// TODO:                    AccessControlEnabled = CmsConfiguration.Security.AccessControlEnabled,
// TODO:                    UserAccessList = AccessControlService.GetDefaultAccessList(SecurityService.GetCurrentPrincipal()).Select(f => new UserAccessViewModel(f)).ToList(),
// TODO:                    ShowLanguages = CmsConfiguration.EnableMultilanguage && langs.Any(),
// TODO:                   Languages = langs,
                    ShowMacros = CmsConfiguration.EnableMacros
                };
            }

// TODO:
//            IEnumerable<AccessRule> userAccessFuture;
//            if (CmsConfiguration.Security.AccessControlEnabled)
//            {
//                userAccessFuture = Repository
//                    .AsQueryable<Models.Sitemap>()
//                    .Where(x => x.Id == sitemapId && !x.IsDeleted)
//                    .SelectMany(x => x.AccessRules)
//                    .OrderBy(x => x.Identity)
//                    .ToFuture();
//            }
//            else
//            {
//                userAccessFuture = null;
//            }

//            var tagsFuture = TagService.GetSitemapTagNames(sitemapId);
//            var languagesFuture = CmsConfiguration.EnableMultilanguage ? LanguageService.GetLanguagesLookupValues() : null;
//            var pagesToFuture = SitemapHelper.GetPagesToFuture(CmsConfiguration.EnableMultilanguage, Repository);

            IQueryable<CategoryTree> sitemapQuery = Repository.AsQueryable<CategoryTree>()
                .Where(map => map.Id == sitemapId)
                .FetchMany(map => map.Categories);

//            if (CmsConfiguration.EnableMultilanguage)
//            {
//                sitemapQuery = sitemapQuery
//                    .ThenFetchMany(node => node.Translations);
//            }

            var sitemap = sitemapQuery.Distinct().ToFuture().ToList().First();
//            var languages = CmsConfiguration.EnableMultilanguage ? languagesFuture.ToList() : new List<LookupKeyValue>();
            var model = new CategoryTreeViewModel
            {
                Id = sitemap.Id,
                Version = sitemap.Version,
                Title = sitemap.Title,
                RootNodes =
                    CategoriesHelper.GetCategoryTreeNodesInHierarchy(
                        CmsConfiguration.EnableMultilanguage,
                        sitemap.Categories.Distinct().Where(f => f.ParentCategory == null).ToList(),
                        sitemap.Categories.Distinct().ToList(),
                        null),  // TODO: languages.Select(l => l.Key.ToGuidOrDefault()).ToList(),
//                Tags = tagsFuture.ToList(),
//                AccessControlEnabled = CmsConfiguration.Security.AccessControlEnabled,
//                ShowLanguages = CmsConfiguration.EnableMultilanguage && languages.Any(),
//                Languages = languages,
                ShowMacros = CmsConfiguration.EnableMacros
            };

//            if (userAccessFuture != null)
//            {
//                model.UserAccessList = userAccessFuture.Select(x => new UserAccessViewModel(x)).ToList();
//
//                var rules = model.UserAccessList.Cast<IAccessRule>().ToList();
//
//                SetIsReadOnly(model, rules);
//            }

            return model;
        }

    }
}