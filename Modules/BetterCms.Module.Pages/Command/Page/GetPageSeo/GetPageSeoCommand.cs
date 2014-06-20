using System;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Seo;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
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

            var page = Repository
                .AsQueryable<PagesView>()
                .Fetch(p => p.Page)
                .Where(f => f.Id == pageId)
                .Select(
                    f => new
                        {
                            PageId = f.Page.Id,
                            PageTitle = f.Page.Title,
                            PageUrl = f.Page.PageUrl,
                            PageUrlHash = f.Page.PageUrlHash,
                            MetaTitle = f.Page.MetaTitle,
                            MetaKeywords = f.Page.MetaKeywords,
                            MetaDescription = f.Page.MetaDescription,
                            UseCanonicalUrl = ((PageProperties)f.Page).UseCanonicalUrl,
                            Version = f.Page.Version,
                            LanguageGroupIdentifier = f.Page.LanguageGroupIdentifier,
                            IsInSitemap = f.IsInSitemap
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
                model.UpdateSitemap = true;
                model.IsInSitemap = page.IsInSitemap;

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