using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Criterion;

namespace BetterCms.Module.Pages.Command.Page.SavePageProperties
{
    public class SavePagePropertiesCommand : CommandBase, ICommand<EditPagePropertiesViewModel, SavePageResponse>
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
        /// The tag service
        /// </summary>
        private readonly ITagService tagService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePagePropertiesCommand" /> class.
        /// </summary>
        /// <param name="pageService">The page service.</param>
        /// <param name="redirectService">The redirect service.</param>
        /// <param name="tagService">The tag service.</param>
        public SavePagePropertiesCommand(IPageService pageService, IRedirectService redirectService, ITagService tagService)
        {
            this.pageService = pageService;
            this.redirectService = redirectService;
            this.tagService = tagService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="CmsException">Failed to save page properties.</exception>
        public SavePageResponse Execute(EditPagePropertiesViewModel request)
        {
            UnitOfWork.BeginTransaction();

            var page = Repository.First<PageProperties>(request.Id);

            request.PagePermalink = redirectService.FixUrl(request.PagePermalink);

            pageService.ValidatePageUrl(request.PagePermalink, request.Id);

            if (request.RedirectFromOldUrl && !string.Equals(page.PageUrl, request.PagePermalink, StringComparison.OrdinalIgnoreCase))
            {
                var redirect = redirectService.CreateRedirectEntity(page.PageUrl, request.PagePermalink);
                if (redirect != null)
                {
                    Repository.Save(redirect);
                }
            }

            tagService.SavePageTags(page, request.Tags);
            UpdateCategories(page, request.Categories);

            page.Layout = Repository.AsProxy<Root.Models.Layout>(request.TemplateId);
            page.Author = request.AuthorId.HasValue ? Repository.AsProxy<Author>(request.AuthorId.Value) : null;
            page.Title = request.PageName;
            page.CustomCss = request.PageCSS;
            page.PageUrl = request.PagePermalink;
            page.IsPublic = request.IsVisibleToEveryone;
            page.UseNoFollow = request.UseNoFollow;
            page.UseNoIndex = request.UseNoIndex;
            page.Version = request.Version;

            Repository.Save(page);
            UnitOfWork.Commit();

            return new SavePageResponse(page);
        }

        private void UpdateCategories(PageProperties page, IList<string> categories)
        {
            Category categoryAlias = null;

            IList<PageCategory> pageCategories = UnitOfWork.Session.QueryOver<PageCategory>()
                                                                   .Fetch(category => category.Category).Eager
                                                                   .Where(category => !category.IsDeleted && category.Page.Id == page.Id)
                                                                   .List<PageCategory>();

            // Remove categories
            for (int i = pageCategories.Count-1; i >= 0; i--)
            {
                string category = null;
                if (categories != null)
                {
                    category = categories.FirstOrDefault(s => s.ToLower() == pageCategories[i].Category.Name.ToLower());
                }
                if (category == null)
                {
                    UnitOfWork.Session.Delete(pageCategories[i]);
                }
            }

            // Add new categories:
            if (categories != null)
            {
                List<string> categoriesInsert = new List<string>();
                foreach (string category in categories)
                {
                    PageCategory existPageCategory = pageCategories.FirstOrDefault(pageCategory => pageCategory.Category.Name.ToLower() == category.ToLower());
                    if (existPageCategory == null)
                    {
                        categoriesInsert.Add(category);
                    }
                }

                if (categoriesInsert.Count > 0)
                {
                    // Get existing categories:
                    IList<Category> existingCategories = UnitOfWork.Session.QueryOver(() => categoryAlias)
                                                                               .Where(Restrictions.In(NHibernate.Criterion.Projections.Property(() => categoryAlias.Name), categoriesInsert))
                                                                               .List<Category>();

                    foreach (string category in categoriesInsert)
                    {
                        PageCategory pageCategory = new PageCategory();
                        pageCategory.Page = page;

                        Category existCategory = existingCategories.FirstOrDefault(x => x.Name.ToLower() == category.ToLower());
                        if (existCategory != null)
                        {
                            pageCategory.Category = existCategory;
                        }
                        else
                        {
                            Category newCategory = new Category();
                            newCategory.Name = category;

                            UnitOfWork.Session.SaveOrUpdate(newCategory);

                            pageCategory.Category = newCategory;
                        }

                        UnitOfWork.Session.SaveOrUpdate(pageCategory);
                    }
                }
            }
        }
    }
}