using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Security;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Sitemap.GetSitemap
{
    /// <summary>
    /// Command to get sitemap data.
    /// </summary>
    public class GetSitemapCommand : CommandBase, ICommand<Guid, SitemapViewModel>
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the tag service.
        /// </summary>
        /// <value>
        /// The tag service.
        /// </value>
        public ITagService TagService { get; set; }

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
        /// <param name="sitemapId">The sitemap identifier.</param>
        /// <returns>
        /// Sitemap view model.
        /// </returns>
        public SitemapViewModel Execute(Guid sitemapId)
        {
            if (sitemapId.HasDefaultValue())
            {
                return new SitemapViewModel()
                    {
                        AccessControlEnabled = CmsConfiguration.Security.AccessControlEnabled,
                        UserAccessList = AccessControlService.GetDefaultAccessList(SecurityService.GetCurrentPrincipal()).Select(f => new UserAccessViewModel(f)).ToList()
                    };
            }

            IEnumerable<AccessRule> userAccessFuture;
            if (CmsConfiguration.Security.AccessControlEnabled)
            {
                userAccessFuture = Repository
                    .AsQueryable<Models.Sitemap>()
                    .Where(x => x.Id == sitemapId && !x.IsDeleted)
                    .SelectMany(x => x.AccessRules)
                    .OrderBy(x => x.Identity)
                    .ToFuture();
            }
            else
            {
                userAccessFuture = null;
            }

            var tagsFuture = TagService.GetSitemapTagNames(sitemapId);

            var languagesFuture = CmsConfiguration.EnableMultilanguage ? LanguageService.GetLanguages() : null;

            IQueryable<Models.Sitemap> sitemapQuery = Repository.AsQueryable<Models.Sitemap>()
                .Where(map => map.Id == sitemapId)
                .FetchMany(map => map.Nodes)
                .ThenFetch(node => node.Page);

            if (CmsConfiguration.EnableMultilanguage)
            {
                sitemapQuery = sitemapQuery
                    .FetchMany(map => map.Nodes)
                    .ThenFetchMany(node => node.Translations);
            }

            var sitemap = sitemapQuery.Distinct().ToList().First();

            var model = new SitemapViewModel
                {
                    Id = sitemap.Id,
                    Version = sitemap.Version,
                    Title = sitemap.Title,
                    RootNodes = GetSitemapNodesInHierarchy(sitemap.Nodes.Distinct().Where(f => f.ParentNode == null).ToList(), sitemap.Nodes.Distinct().ToList()),
                    Tags = tagsFuture.ToList(),
                    AccessControlEnabled = CmsConfiguration.Security.AccessControlEnabled,
                    ShowLanguages = CmsConfiguration.EnableMultilanguage,
                    Languages = CmsConfiguration.EnableMultilanguage ? languagesFuture.ToList() : null
                };

            if (userAccessFuture != null)
            {
                model.UserAccessList = userAccessFuture.Select(x => new UserAccessViewModel(x)).ToList();

                var rules = model.UserAccessList.Cast<IAccessRule>().ToList();

                SetIsReadOnly(model, rules);
            }

            return model;
        }

        /// <summary>
        /// Gets the sitemap nodes in hierarchy.
        /// </summary>
        /// <param name="sitemapNodes">The sitemap nodes.</param>
        /// <param name="allNodes">All nodes.</param>
        /// <returns>The list with all root nodes.</returns>
        private List<SitemapNodeViewModel> GetSitemapNodesInHierarchy(IList<SitemapNode> sitemapNodes, IList<SitemapNode> allNodes)
        {
            var nodeList = new List<SitemapNodeViewModel>();

            foreach (var node in sitemapNodes)
            {
                var nodeViewModel = new SitemapNodeViewModel
                    {
                        Id = node.Id,
                        Version = node.Version,
                        Title = node.Title,
                        Url = node.Page != null ? node.Page.PageUrl : node.Url,
                        PageId = node.Page != null ? node.Page.Id : Guid.Empty,
                        DisplayOrder = node.DisplayOrder,
                        ChildNodes = GetSitemapNodesInHierarchy(allNodes.Where(f => f.ParentNode == node).ToList(), allNodes)
                    };

                if (CmsConfiguration.EnableMultilanguage)
                {
                    nodeViewModel.Translations = node.Translations
                        .Distinct()
                        .Select(t => new SitemapNodeTranslationViewModel
                            {
                                Id = t.Id,
                                LanguageId = t.Language.Id,
                                Title = t.Title,
                                Url = GetTranslatedUrl(t, node),
                                Version = t.Version
                            })
                        .ToList();
                }

                nodeList.Add(nodeViewModel);
            }

            return nodeList.OrderBy(n => n.DisplayOrder).ToList();
        }

        private string GetTranslatedUrl(SitemapNodeTranslation sitemapNodeTranslation, SitemapNode node)
        {
            if (node.Page != null)
            {
                if (node.Page.Language != null && node.Page.Language.Id == sitemapNodeTranslation.Language.Id)
                {
                    return node.Page.PageUrl;
                }

                var pageInLanguage =
                    Repository.AsQueryable<Root.Models.Page>().FirstOrDefault(page => page.Language != null
                                                                                      && page.Language.Id == sitemapNodeTranslation.Language.Id
                                                                                      && page.LanguageGroupIdentifier == node.Page.LanguageGroupIdentifier);

                if (pageInLanguage != null)
                {
                    return pageInLanguage.PageUrl;
                }

                return node.Page.PageUrl;
            }

            return sitemapNodeTranslation.Url;
        }
    }
}