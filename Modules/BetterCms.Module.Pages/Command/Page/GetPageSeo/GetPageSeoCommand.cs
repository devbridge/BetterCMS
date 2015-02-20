using System;
using System.Linq;

using BetterCms.Core.Security;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Seo;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

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

            var model = Repository
                .AsQueryable<PagesView>()
                .Where(f => f.Page.Id == pageId)
                .Select(
                    f => new EditSeoViewModel
                        {
                            PageId = f.Page.Id,
                            PageTitle = f.Page.Title,
                            PageUrlPath = f.Page.PageUrl,
                            ChangedUrlPath = f.Page.PageUrl,
                            MetaTitle = f.Page.MetaTitle,
                            MetaKeywords = f.Page.MetaKeywords,
                            MetaDescription = f.Page.MetaDescription,
                            UseCanonicalUrl = ((PageProperties)f.Page).UseCanonicalUrl,
                            Version = f.Page.Version,
                            IsInSitemap = f.IsInSitemap
                        })
                .FirstOne();

            model.CreatePermanentRedirect = true;
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

            return model;
        }
    }
}