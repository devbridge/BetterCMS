using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Tree
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    public class SitemapTreeService : Service, ISitemapTreeService
    {
        private readonly IRepository repository;

        private readonly ICmsConfiguration cmsConfiguration;

        public SitemapTreeService(IRepository repository, ICmsConfiguration cmsConfiguration)
        {
            this.repository = repository;
            this.cmsConfiguration = cmsConfiguration;
        }

        public GetSitemapTreeResponse Get(GetSitemapTreeRequest request)
        {
            var pagesToFuture = SitemapHelper.GetPagesToFuture(cmsConfiguration.EnableMultilanguage, repository);
            var translationsToFuture = cmsConfiguration.EnableMultilanguage && request.Data.LanguageId.HasValue
                                           ? repository.AsQueryable<Module.Pages.Models.SitemapNodeTranslation>()
                                                       .Where(t => t.Node.Sitemap.Id == request.SitemapId && t.Language.Id == request.Data.LanguageId.Value && !t.IsDeleted && !t.Node.IsDeleted)
                                                       .Select(t => new SitemapTreeNodeTranslationModel
                                                           {
                                                               Id = t.Id,
                                                               Version = t.Version,
                                                               CreatedBy = t.CreatedByUser,
                                                               CreatedOn = t.CreatedOn,
                                                               LastModifiedBy = t.ModifiedByUser,
                                                               LastModifiedOn = t.ModifiedOn,

                                                               NodeId = t.Node.Id,
                                                               LanguageId = t.Language.Id,
                                                               Title = t.Title,
                                                               Url = t.Url,
                                                               UsePageTitleAsNodeTitle = t.UsePageTitleAsNodeTitle,
                                                               Macro = t.Macro
                                                           })
                                                       .ToFuture()
                                           : null;
            
            var allNodes = repository
                .AsQueryable<Module.Pages.Models.SitemapNode>()
                .Where(node => node.Sitemap.Id == request.SitemapId && !node.IsDeleted)
                .OrderBy(node => node.DisplayOrder)
                .Select(node => new SitemapTreeNodeModel
                    {
                        Id = node.Id,
                        Version = node.Version,
                        CreatedBy = node.CreatedByUser,
                        CreatedOn = node.CreatedOn,
                        LastModifiedBy = node.ModifiedByUser,
                        LastModifiedOn = node.ModifiedOn,

                        ParentId = node.ParentNode != null && !node.ParentNode.IsDeleted ? node.ParentNode.Id : (Guid?)null,
                        Title = node.Page != null && node.UsePageTitleAsNodeTitle ? node.Page.Title : node.Title,
                        Url = node.Page != null ? node.Page.PageUrl : node.Url,
                        PageId = node.Page != null ? node.Page.Id : (Guid?)null,
                        PageIsPublished = node.Page != null && !node.Page.IsDeleted && node.Page.Status == PageStatus.Published,
                        UsePageTitleAsNodeTitle = node.UsePageTitleAsNodeTitle,
                        DisplayOrder = node.DisplayOrder,
                        Macro = node.Macro
                    })
                .ToFuture()
                .ToList();

            var nodes = GetChildren(allNodes, request.Data.NodeId);

            if (pagesToFuture != null && translationsToFuture != null)
            {
                Translate(nodes, request.Data.LanguageId, translationsToFuture.ToList(), pagesToFuture.ToList());
            }

            return new GetSitemapTreeResponse { Data = nodes };
        }

        private static void Translate(IList<SitemapTreeNodeModel> nodes, Guid? languageId, IList<SitemapTreeNodeTranslationModel> translations, IList<SitemapHelper.PageData> pages)
        {
            foreach (var node in nodes)
            {
                if (!languageId.HasValue)
                {
                    // Get all translations.
                    node.Translations = translations
                        .Where(t => t.NodeId == node.Id)
                        .ToList();
                    if (node.PageId.HasValue)
                    {
                        // Translate page Ids.
                        var linkedPage = pages.FirstOrDefault(p => p.Id == node.PageId.Value);
                        if (linkedPage == null)
                        {
                            continue;
                        }

                        var pageTranslations = linkedPage.LanguageGroupIdentifier.HasValue && !linkedPage.LanguageGroupIdentifier.Value.HasDefaultValue()
                                                   ? pages.Where(p => p.LanguageGroupIdentifier.HasValue && p.LanguageGroupIdentifier.Value == linkedPage.LanguageGroupIdentifier.Value).ToList()
                                                   : new List<SitemapHelper.PageData> { linkedPage };

                        var defaultPage = pageTranslations.FirstOrDefault(p => !p.LanguageId.HasValue || p.LanguageId.Value.HasDefaultValue()) ?? linkedPage;
                        node.PageId = defaultPage.Id;
                        node.PageIsPublished = defaultPage.IsPublished;
                        node.PageLanguageId = defaultPage.LanguageId.ToNullOrValue();
                        node.Url = defaultPage.Url;
                        node.Title = node.UsePageTitleAsNodeTitle ? defaultPage.Title : node.Title;

                        foreach (var translationModel in node.Translations)
                        {
                            var languagePage =
                                pageTranslations.FirstOrDefault(
                                    p => p.LanguageId.HasValue && !p.LanguageId.Value.HasDefaultValue() && p.LanguageId.Value == translationModel.LanguageId);

                            if (languagePage != null)
                            {
                                translationModel.Url = languagePage.Url;
                                translationModel.Title = translationModel.UsePageTitleAsNodeTitle ? languagePage.Title : translationModel.Title;
                            }
                        }
                    }
                }
                else
                {
                    if (languageId.Value.HasDefaultValue())
                    {
                        // Get translated to the default language.
                        if (node.PageId.HasValue)
                        {
                            var linkedPage = pages.FirstOrDefault(p => p.Id == node.PageId.Value);
                            if (linkedPage == null)
                            {
                                continue;
                            }

                            var pageTranslations = linkedPage.LanguageGroupIdentifier.HasValue && !linkedPage.LanguageGroupIdentifier.Value.HasDefaultValue()
                                                       ? pages.Where(p => p.LanguageGroupIdentifier.HasValue && p.LanguageGroupIdentifier.Value == linkedPage.LanguageGroupIdentifier.Value).ToList()
                                                       : new List<SitemapHelper.PageData> { linkedPage };

                            var defaultPage = pageTranslations.FirstOrDefault(p => !p.LanguageId.HasValue || p.LanguageId.Value.HasDefaultValue()) ?? linkedPage;
                            node.PageId = defaultPage.Id;
                            node.PageIsPublished = defaultPage.IsPublished;
                            node.PageLanguageId = defaultPage.LanguageId.ToNullOrValue();
                            node.Url = defaultPage.Url;
                            node.Title = node.UsePageTitleAsNodeTitle ? defaultPage.Title : node.Title;
                        }
                    }
                    else
                    {
                        // Get translated to specified language.
                        var translation = translations.FirstOrDefault(t => t.NodeId == node.Id && t.LanguageId == languageId.Value);
                        if (translation != null)
                        {
                            node.Title = translation.Title;
                            node.Url = translation.Url;
                            node.UsePageTitleAsNodeTitle = translation.UsePageTitleAsNodeTitle;
                            node.Macro = translation.Macro;
                        }

                        if (node.PageId.HasValue)
                        {
                            var linkedPage = pages.FirstOrDefault(p => p.Id == node.PageId.Value);
                            if (linkedPage == null)
                            {
                                continue;
                            }

                            var pageTranslations = linkedPage.LanguageGroupIdentifier.HasValue && !linkedPage.LanguageGroupIdentifier.Value.HasDefaultValue()
                                                       ? pages.Where(p => p.LanguageGroupIdentifier.HasValue && p.LanguageGroupIdentifier.Value == linkedPage.LanguageGroupIdentifier.Value).ToList()
                                                       : new List<SitemapHelper.PageData> { linkedPage };

                            var languagePage =
                                pageTranslations.FirstOrDefault(
                                    p => p.LanguageId.HasValue && !p.LanguageId.Value.HasDefaultValue() && p.LanguageId.Value == languageId.Value);

                            if (translation == null)
                            {
                                node.UsePageTitleAsNodeTitle = true;
                            }

                            if (languagePage != null)
                            {
                                node.PageId = languagePage.Id;
                                node.PageIsPublished = languagePage.IsPublished;
                                node.PageLanguageId = languagePage.LanguageId.ToNullOrValue();
                                node.Url = languagePage.Url;
                                node.Title = node.UsePageTitleAsNodeTitle ? languagePage.Title : node.Title;
                            }
                            else
                            {
                                var defaultPage = pageTranslations.FirstOrDefault(p => !p.LanguageId.HasValue || p.LanguageId.Value.HasDefaultValue()) ?? linkedPage;
                                node.PageId = defaultPage.Id;
                                node.PageIsPublished = defaultPage.IsPublished;
                                node.PageLanguageId = defaultPage.LanguageId.ToNullOrValue();
                                node.Url = defaultPage.Url;
                                node.Title = node.UsePageTitleAsNodeTitle ? defaultPage.Title : node.Title;
                            }
                        }
                    }
                }

                Translate(node.ChildrenNodes, languageId, translations, pages);
            }
        }

        private static List<SitemapTreeNodeModel> GetChildren(List<SitemapTreeNodeModel> allItems, Guid? parentId)
        {
            var childItems = allItems.Where(item => item.ParentId == parentId && item.Id != parentId).OrderBy(node => node.DisplayOrder).ToList();

            foreach (var item in childItems)
            {
                item.ChildrenNodes = GetChildren(allItems, item.Id);
            }

            return childItems;
        }
    }
}