using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Seo;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.GetPageSeo
{
    /// <summary>
    /// A command to get page SEO information.
    /// </summary>
    public class GetPageSeoCommand : CommandBase, ICommand<Guid, EditSeoViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>EditSeoView model filled with page SEO information.</returns>
        public virtual EditSeoViewModel Execute(Guid pageId)
        {
            var page = Repository
                .AsQueryable<PageProperties>()
                .Select(
                    f => new
                        {
                            PageId = f.Id,
                            PageTitle = f.Title,
                            PageUrl = f.PageUrl,
                            MetaTitle = f.MetaTitle,
                            MetaKeywords = f.MetaKeywords,
                            MetaDescription = f.MetaDescription,
                            Version = f.Version
                        })
                .FirstOrDefault(f => f.PageId == pageId);

            EditSeoViewModel model = new EditSeoViewModel();
            if (page != null)
            {
                model.PageId = page.PageId;
                model.Version = page.Version;
                model.CreatePermanentRedirect = false;
                model.PageTitle = page.PageTitle;
                model.PageUrlPath = page.PageUrl;
                model.ChangedUrlPath = page.PageUrl;
                model.MetaTitle = page.MetaTitle;
                model.MetaKeywords = page.MetaKeywords;
                model.MetaDescription = page.MetaDescription;
                model.CreatePermanentRedirect = true;
            }

            return model;
        }
    }
}