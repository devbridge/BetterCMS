using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Security;

using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Sitemap.GetSitemapsForNewPage
{
    /// <summary>
    /// Command to get sitemap data.
    /// </summary>
    public class GetSitemapsForNewPageCommand : CommandBase, ICommandOut<List<SitemapViewModel>>
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the language service.
        /// </summary>
        /// <value>
        /// The language service.
        /// </value>
        public ILanguageService LanguageService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <returns>
        /// Sitemap list.
        /// </returns>
        public List<SitemapViewModel> Execute()
        {
            var sitemaps = new List<SitemapViewModel>();
            var languagesFuture = CmsConfiguration.EnableMultilanguage ? LanguageService.GetLanguagesLookupValues() : null;
            var pagesToFuture = SitemapHelper.GetPagesToFuture(CmsConfiguration.EnableMultilanguage, Repository);
            IQueryable<Models.Sitemap> sitemapQuery =
                Repository.AsQueryable<Models.Sitemap>().FetchMany(map => map.AccessRules).FetchMany(map => map.Nodes).ThenFetch(node => node.Page);

            if (CmsConfiguration.EnableMultilanguage)
            {
                sitemapQuery = sitemapQuery
                    .FetchMany(map => map.Nodes)
                    .ThenFetchMany(node => node.Translations);
            }

            var allSitmaps = sitemapQuery.Distinct().ToFuture().ToList();
            var languages = CmsConfiguration.EnableMultilanguage ? languagesFuture.ToList() : new List<LookupKeyValue>();
            foreach (var sitemap in allSitmaps)
            {
                var model = new SitemapViewModel
                    {
                        Id = sitemap.Id,
                        Version = sitemap.Version,
                        Title = sitemap.Title,
                        RootNodes =
                            SitemapHelper.GetSitemapNodesInHierarchy(
                                CmsConfiguration.EnableMultilanguage,
                                sitemap.Nodes.Distinct().Where(f => f.ParentNode == null).ToList(),
                                sitemap.Nodes.Distinct().ToList(),
                                languages.Select(l => l.Key.ToGuidOrDefault()).ToList(),
                                (pagesToFuture ?? new List<SitemapHelper.PageData>()).ToList()),
                        AccessControlEnabled = CmsConfiguration.Security.AccessControlEnabled,
                        ShowLanguages = CmsConfiguration.EnableMultilanguage && languages.Any(),
                        Languages = languages,
                        ShowMacros = false
                    };

                if (CmsConfiguration.Security.AccessControlEnabled)
                {
                    model.UserAccessList = sitemap.AccessRules.Distinct().Select(x => new UserAccessViewModel(x)).ToList();
                    var rules = model.UserAccessList.Cast<IAccessRule>().ToList();
                    SetIsReadOnly(model, rules);
                }

                if (!model.IsReadOnly)
                {
                    sitemaps.Add(model);
                }
            }

            return sitemaps.Count > 0 ? sitemaps.OrderBy(map => map.Title).ToList() : null;
        }
    }
}