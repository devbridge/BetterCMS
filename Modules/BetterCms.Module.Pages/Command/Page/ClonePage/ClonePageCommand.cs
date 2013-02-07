using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Page.ClonePage
{
    /// <summary>
    /// A command to clone given page.
    /// </summary>
    public class ClonePageCommand : CommandBase, ICommand<ClonePageViewModel, ClonePageViewModel>
    {
        /// <summary>
        /// Gets or sets the page service.
        /// </summary>
        /// <value>
        /// The page service.
        /// </value>
        public IPageService PageService { get; set; }

        /// <summary>
        /// Gets or sets the redirect service.
        /// </summary>
        /// <value>
        /// The redirect service.
        /// </value>
        public IRedirectService RedirectService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The page view model.</param>
        /// <returns>true if page cloned successfully; false otherwise.</returns>
        public virtual ClonePageViewModel Execute(ClonePageViewModel request)
        {
            // Create / fix page url
            var pageUrl = request.PageUrl;
            if (pageUrl == null && !string.IsNullOrWhiteSpace(request.PageTitle))
            {
                pageUrl = request.PageTitle.Transliterate();
            }
            pageUrl = RedirectService.FixUrl(pageUrl);

            // Validate Url
            PageService.ValidatePageUrl(pageUrl);

            var page = Repository.FirstOrDefault<PageProperties>(request.PageId);

            UnitOfWork.BeginTransaction();

            var pageContents = Repository.AsQueryable<PageContent>()
                .Where(x => x.Page.Id == page.Id)
                .Fetch(x => x.Region)
                .Fetch(x => x.Content)
                .ToList();

            var pageTags = Repository.AsQueryable<PageTag>()
                .Where(x => x.Page.Id == page.Id)
                .Fetch(x => x.Tag)
                .ToList();

            var pageContentOptions = Repository.AsQueryable<PageContentOption>()
                .Where(f => f.PageContent.Page.Id == page.Id)
                .ToList();

            var newPage = ClonePageOnly(page, request.PageTitle, pageUrl);

            // Clone HTML contents and Controls:
            pageContents.ForEach(pageContent => ClonePageContent(pageContent, newPage));
            pageTags.ForEach(pageTag => ClonePageTags(pageTag, newPage));
            //pageContentOptions.ForEach(pageContentOption => ClonePageContentOptions(pageContentOption, newPage));

            UnitOfWork.Commit();

            return new ClonePageViewModel
                       {
                           PageId = newPage.Id,
                           Version = newPage.Version,
                           PageTitle = newPage.Title,
                           PageUrl = newPage.PageUrl
                       };
        }

        private void ClonePageTags(PageTag pageTag, PageProperties newPage)
        {
            var newPageHtmlControl = new PageTag
            {
                Page = newPage,
                Tag = pageTag.Tag
            };

            Repository.Save(newPageHtmlControl);
        }

        private PageProperties ClonePageOnly(PageProperties page, string newPageTitle, string newPageUrl)
        {
            var newPage = new PageProperties
            {
                // New page data:
                Title = newPageTitle,
                PageUrl = newPageUrl,
                IsPublished = false,

                // Cloned data:
                MetaTitle = page.MetaTitle,                
                MetaKeywords = page.MetaKeywords,
                MetaDescription = page.MetaDescription,
                IsPublic = page.IsPublic,
                UseCanonicalUrl = page.UseCanonicalUrl,
                CustomCss = page.CustomCss,
                CustomJS = page.CustomJS,
                Description = page.Description,
                UseNoFollow = page.UseNoFollow,
                UseNoIndex = page.UseNoIndex,
                Layout = page.Layout,
                Image = page.Image,
                Category = page.Category,
            };

            Repository.Save(newPage);

            return newPage;
        }

        private void ClonePageContent(PageContent pageContent, PageProperties newPage)
        {
            var newPageContent = new PageContent();
            newPageContent.Page = newPage;
            newPageContent.Order = pageContent.Order;
            newPageContent.Region = pageContent.Region;

            if (pageContent.Content is HtmlContentWidget || pageContent.Content is ServerControlWidget)
            {
                // Do not need to clone widgets.
                newPageContent.Content = pageContent.Content;
            }
            else
            {
                newPageContent.Content = pageContent.Content.Clone();
            }

            Repository.Save(newPageContent);           
        }
    }
}