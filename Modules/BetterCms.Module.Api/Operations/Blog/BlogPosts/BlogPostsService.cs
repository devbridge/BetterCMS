using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Security;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;
using BetterCms.Module.Api.Operations.Root;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.MediaManager.Services;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Services;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

using AccessLevel = BetterCms.Module.Api.Operations.Root.AccessLevel;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    public class BlogPostsService : Service, IBlogPostsService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The file URL resolver.
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// The access control service.
        /// </summary>
        private readonly IAccessControlService accessControlService;


        private readonly ICategoryService categoryService;
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPostsService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        /// <param name="accessControlService">The access control service.</param>
        public BlogPostsService(
            IRepository repository,
            IMediaFileUrlResolver fileUrlResolver,
            IAccessControlService accessControlService,
            ICategoryService categoryService)
        {
            this.repository = repository;
            this.fileUrlResolver = fileUrlResolver;
            this.accessControlService = accessControlService;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetBlogPostsResponse</c> with a list of blog posts.</returns>
        public GetBlogPostsResponse Get(GetBlogPostsRequest request)
        {
            request.Data.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<Module.Blog.Models.BlogPost>();

            if (!request.Data.IncludeArchived)
            {
                query = query.Where(b => !b.IsArchived);
            }

            if (!request.Data.IncludeUnpublished)
            {
                query = query.Where(b => b.Status == PageStatus.Published);
            }

            if (request.User != null && !string.IsNullOrWhiteSpace(request.User.Name))
            {
                var principal = new ApiPrincipal(request.User);
                IEnumerable<Guid> deniedPages = accessControlService.GetPrincipalDeniedObjects<PageProperties>(principal, false);
                foreach (var deniedPageId in deniedPages)
                {
                    var id = deniedPageId;
                    query = query.Where(f => f.Id != id);
                }
            }

            query = query.ApplyPageTagsFilter(request.Data)
                .ApplyCategoriesFilter(categoryService, request.Data);

            var listResponse = query
                .Select(blogPost => new BlogPostModel
                    {
                        Id = blogPost.Id,
                        Version = blogPost.Version,
                        CreatedBy = blogPost.CreatedByUser,
                        CreatedOn = blogPost.CreatedOn,
                        LastModifiedBy = blogPost.ModifiedByUser,
                        LastModifiedOn = blogPost.ModifiedOn,

                        BlogPostUrl = blogPost.PageUrl,
                        Title = blogPost.Title,
                        IntroText = blogPost.Description,
                        IsPublished = blogPost.Status == PageStatus.Published,
                        PublishedOn = blogPost.PublishedOn,
                        LayoutId = blogPost.Layout != null && !blogPost.Layout.IsDeleted ? blogPost.Layout.Id : (Guid?)null,
                        MasterPageId = blogPost.MasterPage != null && !blogPost.MasterPage.IsDeleted ? blogPost.MasterPage.Id : (Guid?)null,
                        AuthorId = blogPost.Author != null && !blogPost.Author.IsDeleted ? blogPost.Author.Id : (Guid?)null,
                        AuthorName = blogPost.Author != null && !blogPost.Author.IsDeleted ? blogPost.Author.Name : null,
                        MainImageId = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.Id : (Guid?)null,
                        MainImageUrl = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.PublicUrl : null,
                        MainImageThumbnauilUrl = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.PublicThumbnailUrl : null,
                        MainImageThumbnailUrl = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.PublicThumbnailUrl : null,
                        MainImageCaption = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.Caption : null,
                        SecondaryImageId = blogPost.SecondaryImage != null && !blogPost.SecondaryImage.IsDeleted ? blogPost.SecondaryImage.Id : (Guid?)null,
                        SecondaryImageUrl = blogPost.SecondaryImage != null && !blogPost.SecondaryImage.IsDeleted ? blogPost.SecondaryImage.PublicUrl : null,
                        SecondaryImageThumbnailUrl = blogPost.SecondaryImage != null && !blogPost.SecondaryImage.IsDeleted ? blogPost.SecondaryImage.PublicThumbnailUrl : null,
                        SecondaryImageCaption = blogPost.SecondaryImage != null && !blogPost.SecondaryImage.IsDeleted ? blogPost.SecondaryImage.Caption : null,
                        FeaturedImageId = blogPost.FeaturedImage != null && !blogPost.FeaturedImage.IsDeleted ? blogPost.FeaturedImage.Id : (Guid?)null,
                        FeaturedImageUrl = blogPost.FeaturedImage != null && !blogPost.FeaturedImage.IsDeleted ? blogPost.FeaturedImage.PublicUrl : null,
                        FeaturedImageThumbnailUrl = blogPost.FeaturedImage != null && !blogPost.FeaturedImage.IsDeleted ? blogPost.FeaturedImage.PublicThumbnailUrl : null,
                        FeaturedImageCaption = blogPost.FeaturedImage != null && !blogPost.FeaturedImage.IsDeleted ? blogPost.FeaturedImage.Caption : null,
                        ActivationDate = blogPost.ActivationDate,
                        ExpirationDate = blogPost.ExpirationDate,
                        IsArchived = blogPost.IsArchived,
                        LanguageId = blogPost.Language != null ? blogPost.Language.Id : (Guid?)null,
                        LanguageCode = blogPost.Language != null ? blogPost.Language.Code : null,
                        LanguageGroupIdentifier = blogPost.LanguageGroupIdentifier,
                        ContentId = blogPost.PageContents.Where(pc => !pc.Content.IsDeleted && pc.Content is BlogPostContent)
                            .Select(pc => pc.Content.Id)
                            .FirstOrDefault()
                    })
                    .ToDataListResponse(request);

            foreach (var model in listResponse.Items)
            {
                model.MainImageUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageUrl);
                model.MainImageThumbnauilUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageThumbnauilUrl);
                model.MainImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageThumbnailUrl);

                model.SecondaryImageUrl = fileUrlResolver.EnsureFullPathUrl(model.SecondaryImageUrl);
                model.SecondaryImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.SecondaryImageThumbnailUrl);

                model.FeaturedImageUrl = fileUrlResolver.EnsureFullPathUrl(model.FeaturedImageUrl);
                model.FeaturedImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.FeaturedImageThumbnailUrl);
            }

            if (listResponse.Items.Count > 0 && (request.Data.IncludeTags || request.Data.IncludeAccessRules))
            {
                LoadTags(listResponse, request.Data.IncludeTags, request.Data.IncludeAccessRules);
            }


            if (request.Data.IncludeCategories)
            {
                listResponse.Items.ForEach(page =>
                {
                    page.Categories = (from pagePr in repository.AsQueryable<Module.Blog.Models.BlogPost>()
                                       from category in pagePr.Categories
                                       where pagePr.Id == page.Id && !category.IsDeleted
                                       select new CategoryModel
                                       {
                                           Id = category.Category.Id,
                                           Version = category.Version,
                                           CreatedBy = category.CreatedByUser,
                                           CreatedOn = category.CreatedOn,
                                           LastModifiedBy = category.ModifiedByUser,
                                           LastModifiedOn = category.ModifiedOn,
                                           Name = category.Category.Name
                                       }).ToList();
                });
            }

            return new GetBlogPostsResponse
                       {
                           Data = listResponse
                       };
        }

        private void LoadTags(DataListResponse<BlogPostModel> response, bool includeTags, bool includeAccessRules)
        {
            var pageIds = response.Items.Select(i => i.Id).Distinct().ToArray();

            IEnumerable<TagModel> tagsFuture;
            if (includeTags)
            {
                tagsFuture =
                    repository.AsQueryable<PageTag>(pt => pageIds.Contains(pt.Page.Id))
                        .Select(pt => new TagModel { PageId = pt.Page.Id, TagName = pt.Tag.Name })
                        .OrderBy(o => o.TagName)
                        .ToFuture();
            }
            else
            {
                tagsFuture = null;
            }

            IEnumerable<AccessRuleModelEx> rulesFuture;
            if (includeAccessRules)
            {
                rulesFuture = (from page in repository.AsQueryable<Module.Root.Models.Page>()
                               from accessRule in page.AccessRules
                               where pageIds.Contains(page.Id)
                               orderby accessRule.IsForRole, accessRule.Identity
                               select new AccessRuleModelEx
                               {
                                   AccessRule = new AccessRuleModel
                                   {
                                       AccessLevel = (AccessLevel)(int)accessRule.AccessLevel,
                                       Identity = accessRule.Identity,
                                       IsForRole = accessRule.IsForRole
                                   },
                                   PageId = page.Id
                               })
                    .ToFuture();
            }
            else
            {
                rulesFuture = null;
            }


            if (tagsFuture != null)
            {
                var tags = tagsFuture.ToList();
                response.Items.ToList().ForEach(
                    page =>
                    {
                        page.Tags = tags
                            .Where(tag => tag.PageId == page.Id)
                            .Select(tag => tag.TagName)
                            .ToList();
                    });
            }

            if (rulesFuture != null)
            {
                var rules = rulesFuture.ToList();
                response.Items.ToList().ForEach(page =>
                {
                    page.AccessRules = rules
                        .Where(rule => rule.PageId == page.Id)
                        .Select(rule => rule.AccessRule)
                        .ToList();
                });
            }
        }

        private class TagModel
        {
            public Guid PageId { get; set; }
            public string TagName { get; set; }
        }

        private class AccessRuleModelEx
        {
            public AccessRuleModel AccessRule { get; set; }
            public Guid PageId { get; set; }
        }
    }
}