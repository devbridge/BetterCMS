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
        /// Initializes a new instance of the <see cref="SavePagePropertiesCommand" /> class.
        /// </summary>
        /// <param name="pageService">The page service.</param>
        /// <param name="redirectService">The redirect service.</param>
        public SavePagePropertiesCommand(IPageService pageService, IRedirectService redirectService)
        {
            this.pageService = pageService;
            this.redirectService = redirectService;
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

            UpdatePageTags(page, request.Tags);
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

        private void UpdatePageTags(PageProperties page, IList<string> tags)
        {
            Root.Models.Tag tagAlias = null;

            // Tags merge:
            IList<PageTag> pageTags = UnitOfWork.Session.QueryOver<PageTag>()
                                                    .Where(tag => !tag.IsDeleted && tag.Page.Id == page.Id)
                                                    .Fetch(tag => tag.Tag).Eager
                                                    .List<PageTag>();

            // Remove deleted tags:
            for (int i = pageTags.Count - 1; i >= 0; i--)
            {
                string tag = null;
                if (tags != null)
                {
                    tag = tags.FirstOrDefault(s => s.ToLower() == pageTags[i].Tag.Name.ToLower());
                }
                if (tag == null)
                {
                    UnitOfWork.Session.Delete(pageTags[i]);
                }
            }

            // Add new tags:
            if (tags != null)
            {
                List<string> tagsInsert = new List<string>();
                foreach (string tag in tags)
                {
                    PageTag existPageTag = pageTags.FirstOrDefault(pageTag => pageTag.Tag.Name.ToLower() == tag.ToLower());
                    if (existPageTag == null)
                    {
                        tagsInsert.Add(tag);
                    }
                }

                if (tagsInsert.Count > 0)
                {
                    // Get existing tags:
                    IList<Root.Models.Tag> existingTags = UnitOfWork.Session.QueryOver(() => tagAlias)
                                                                .Where(Restrictions.In(NHibernate.Criterion.Projections.Property(() => tagAlias.Name), tagsInsert))
                                                                .List<Root.Models.Tag>();

                    foreach (string tag in tagsInsert)
                    {
                        PageTag pageTag = new PageTag();
                        pageTag.Page = page;

                        Root.Models.Tag existTag = existingTags.FirstOrDefault(t => t.Name.ToLower() == tag.ToLower());
                        if (existTag != null)
                        {
                            pageTag.Tag = existTag;
                        }
                        else
                        {
                            Root.Models.Tag newTag = new Root.Models.Tag();
                            newTag.Name = tag;
                            UnitOfWork.Session.SaveOrUpdate(newTag);

                            pageTag.Tag = newTag;
                        }

                        UnitOfWork.Session.SaveOrUpdate(pageTag);
                    }
                }
            }
        }
    }
}