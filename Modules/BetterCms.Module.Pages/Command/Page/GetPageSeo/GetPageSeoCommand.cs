using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Seo;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.ViewModels.Security;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Page.GetPageSeo
{
    /// <summary>
    /// A command to get page SEO information.
    /// </summary>
    public class GetPageSeoCommand : CommandBase, ICommand<Guid, EditSeoViewModel>
    {
        private readonly ICmsConfiguration cmsConfiguration;

        public GetPageSeoCommand(ICmsConfiguration cmsConfiguration)
        {
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>EditSeoView model filled with page SEO information.</returns>
        public virtual EditSeoViewModel Execute(Guid pageId)
        {
            if (pageId.HasDefaultValue())
            {
                return new EditSeoViewModel();
            }

            var inSitemapFuture = Repository.AsQueryable<SitemapNode>().Where(node => node.Page.Id == pageId && !node.IsDeleted && !node.Sitemap.IsDeleted).Select(node => node.Id).ToFuture();
            var page = Repository
                .AsQueryable<PageProperties>()
                .Where(f => f.Id == pageId)
                .Select(
                    f => new
                        {
                            PageId = f.Id,
                            PageTitle = f.Title,
                            PageUrl = f.PageUrl,
                            MetaTitle = f.MetaTitle,
                            MetaKeywords = f.MetaKeywords,
                            MetaDescription = f.MetaDescription,
                            UseCanonicalUrl = f.UseCanonicalUrl,
                            Version = f.Version
                        })
                .FirstOne();

            EditSeoViewModel model = new EditSeoViewModel();
            if (page != null)
            {
                model.PageId = page.PageId;
                model.Version = page.Version;
                model.CreatePermanentRedirect = true;
                model.PageTitle = page.PageTitle;
                model.PageUrlPath = page.PageUrl;
                model.ChangedUrlPath = page.PageUrl;
                model.MetaTitle = page.MetaTitle;
                model.MetaKeywords = page.MetaKeywords;
                model.MetaDescription = page.MetaDescription;
                model.UseCanonicalUrl = page.UseCanonicalUrl;
                var urlHash = page.PageUrl.UrlHash();
                model.IsInSitemap = inSitemapFuture.Any() || Repository.AsQueryable<SitemapNode>().Any(node => node.UrlHash == urlHash && !node.IsDeleted && !node.Sitemap.IsDeleted);
                model.UpdateSitemap = true;

                if (cmsConfiguration.Security.AccessControlEnabled)
                {
                    var accessRules = Repository.AsQueryable<Root.Models.Page>()
                                                .Where(x => x.Id == pageId && !x.IsDeleted)
                                                .SelectMany(x => x.AccessRules)
                                                .OrderBy(x => x.Identity).ToList()
                                                .Select(x => new UserAccessViewModel(x)).Cast<IAccessRule>().ToList();
                 
                    SetIsReadOnly(model, accessRules);                      
                }
            }

            return model;
        }
    }
}