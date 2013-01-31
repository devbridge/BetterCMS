using System;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Seo;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.SavePageSeo
{
    /// <summary>
    /// A command to save SEO information.
    /// </summary>
    public class SavePageSeoCommand : CommandBase, ICommand<EditSeoViewModel, EditSeoViewModel>
    {
        /// <summary>
        /// The page service
        /// </summary>
        private readonly IPageService pageService;
        
        /// <summary>
        /// The redirect service
        /// </summary>
        private readonly IRedirectService redirectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePageSeoCommand" /> class.
        /// </summary>
        /// <param name="redirectService">The redirect service.</param>
        /// <param name="pageService">The page service.</param>
        public SavePageSeoCommand(IRedirectService redirectService, IPageService pageService)
        {
            this.pageService = pageService;
            this.redirectService = redirectService;
        }

        /// <summary>
        /// Saves SEO information.
        /// </summary>
        /// <param name="model">The SEO information model.</param>
        /// <returns>
        /// true if SEO information saved successfully; false otherwise.
        /// </returns>
        public virtual EditSeoViewModel Execute(EditSeoViewModel model)
        {
            var page = Repository.AsProxy<PageProperties>(model.PageId);
            page.Version = model.Version;
            page.Title = model.PageTitle;

            model.ChangedUrlPath = redirectService.FixUrl(model.ChangedUrlPath);

            if (!string.Equals(model.PageUrlPath, model.ChangedUrlPath, StringComparison.OrdinalIgnoreCase))
            {
                pageService.ValidatePageUrl(model.ChangedUrlPath, model.PageId);
                if (model.CreatePermanentRedirect)
                {
                    var redirect = redirectService.CreateRedirectEntity(model.PageUrlPath, model.ChangedUrlPath);
                    if (redirect != null)
                    {
                        Repository.Save(redirect);
                    }
                }

                page.PageUrl = model.ChangedUrlPath;
            }

            page.MetaTitle = model.MetaTitle;
            page.MetaKeywords = model.MetaKeywords;
            page.MetaDescription = model.MetaDescription;

            Repository.Save(page);
            UnitOfWork.Commit();

            return new EditSeoViewModel { PageUrlPath = page.PageUrl };
        }
    }
}