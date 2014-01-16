using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Tree
{
    public class SitemapTreeService : Service, ISitemapTreeService
    {
        private class TranslationData
        {
            public Guid NodeId { get; set; }

            public Guid LanguageId { get; set; }

            public string Title { get; set; }

            public string Url { get; set; }

            public bool UsePageTitleAsNodeTitle { get; set; }
        }

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
                                                       .Where(t => t.Node.Sitemap.Id == request.SitemapId && t.Language.Id == request.Data.LanguageId.Value)
                                                       .Select(p => new TranslationData
                                                           {
                                                               NodeId = p.Node.Id,
                                                               LanguageId = p.Language.Id,
                                                               Title = p.Title,
                                                               Url = p.Url,
                                                               UsePageTitleAsNodeTitle = p.UsePageTitleAsNodeTitle
                                                           })
                                                       .ToFuture()
                                           : null;
            
            var allNodes = repository
                .AsQueryable<Module.Pages.Models.SitemapNode>()
                .Where(node => node.Sitemap.Id == request.SitemapId)
                .OrderBy(node => node.DisplayOrder)
                .Select(node => new SitemapTreeNodeModel
                    {
                        Id = node.Id,
                        Version = node.Version,
                        CreatedBy = node.CreatedByUser,
                        CreatedOn = node.CreatedOn,
                        LastModifiedBy = node.ModifiedByUser,
                        LastModifiedOn = node.ModifiedOn,

                        ParentId = node.ParentNode != null && !node.ParentNode.IsDeleted ? node.ParentNode.Id : (System.Guid?)null,
                        Title = node.Page != null && node.UsePageTitleAsNodeTitle ? node.Page.Title : node.Title,
                        Url = node.Page != null ? node.Page.PageUrl : node.Url,
                        PageId = node.Page != null ? node.Page.Id : (Guid?)null,
                        UsePageTitleAsNodeTitle = node.UsePageTitleAsNodeTitle,
                        DisplayOrder = node.DisplayOrder
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

        private void Translate(IList<SitemapTreeNodeModel> nodes, Guid? languageId, IList<TranslationData> translations, IList<SitemapHelper.PageData> pages)
        {
            foreach (var node in nodes)
            {
                if (!node.PageId.HasValue || node.PageId.Value.HasDefaultValue())
                {
                    TranslateNodeWithoutPage(node, languageId, translations);
                }
                else
                {
                    TranslateNodeWithPage(node, languageId, translations, pages);
                }

                Translate(node.ChildrenNodes, languageId, translations, pages);
            }
        }

        private void TranslateNodeWithoutPage(SitemapTreeNodeModel node, Guid? languageId, IList<TranslationData> translations)
        {
            if (!languageId.HasValue || languageId.Value.HasDefaultValue())
            {
                // Do nothing - node is already in default translation.
            }
            else
            {
                var translation = translations.FirstOrDefault(t => t.NodeId == node.Id && t.LanguageId == languageId.Value);
                if (translation != null)
                {
                    node.Title = translation.Title;
                    node.Url = translation.Url;
                }
            }
        }

        private void TranslateNodeWithPage(SitemapTreeNodeModel node, Guid? languageId, IList<TranslationData> translations, IList<SitemapHelper.PageData> pages)
        {
            var nodePage = pages.First(p => p.Id == node.PageId.Value);
            if (!languageId.HasValue || languageId.Value.HasDefaultValue())
            {
                // Node is already in default translation.
                if (!nodePage.LanguageId.HasValue)
                {
                    // TODO: implement.
                }
            }
            else
            {
                var translation = translations.FirstOrDefault(t => t.NodeId == node.Id && t.LanguageId == languageId.Value);
                if (translation != null)
                {
                    node.Title = translation.Title;
                    node.Url = translation.Url;
                }
            }
        }

        private List<SitemapTreeNodeModel> GetChildren(List<SitemapTreeNodeModel> allItems, System.Guid? parentId)
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