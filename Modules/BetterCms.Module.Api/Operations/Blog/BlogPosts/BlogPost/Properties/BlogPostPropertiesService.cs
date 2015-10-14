using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Services;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Pages;
using BetterCms.Module.Api.Operations.Root;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    public class BlogPostPropertiesService : Service, IBlogPostPropertiesService
    {
        private readonly IRepository repository;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        private readonly IBlogSaveService blogSaveService;

        private readonly ISecurityService securityService;

        private readonly IPageService pageService;

        private readonly BetterCms.Module.Root.Services.ICategoryService categoriesService;

        private readonly Module.Root.Services.IOptionService optionService;

        public BlogPostPropertiesService(IRepository repository, IMediaFileUrlResolver fileUrlResolver,
            IBlogSaveService blogSaveService, ISecurityService securityService, IPageService pageService,
            Module.Root.Services.IOptionService optionService, BetterCms.Module.Root.Services.ICategoryService categoriesService)
        {
            this.repository = repository;
            this.fileUrlResolver = fileUrlResolver;
            this.blogSaveService = blogSaveService;
            this.securityService = securityService;
            this.pageService = pageService;
            this.optionService = optionService;
            this.categoriesService = categoriesService;
        }

        public GetBlogPostPropertiesResponse Get(GetBlogPostPropertiesRequest request)
        {
            var response = repository
                .AsQueryable<Module.Blog.Models.BlogPost>(blogPost => blogPost.Id == request.BlogPostId)
                .Select(blogPost =>
                    new GetBlogPostPropertiesResponse
                    {
                        Data = new BlogPostPropertiesModel
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
                                ActivationDate = blogPost.ActivationDate,
                                ExpirationDate = blogPost.ExpirationDate,
                                MainImageId = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.Id : (Guid?)null,
                                FeaturedImageId = blogPost.FeaturedImage != null && !blogPost.FeaturedImage.IsDeleted ? blogPost.FeaturedImage.Id : (Guid?)null,
                                SecondaryImageId = blogPost.SecondaryImage != null && !blogPost.SecondaryImage.IsDeleted ? blogPost.SecondaryImage.Id : (Guid?)null,
                                UseCanonicalUrl = blogPost.UseCanonicalUrl,
                                UseNoFollow = blogPost.UseNoFollow,
                                UseNoIndex = blogPost.UseNoIndex,
                                IsArchived = blogPost.IsArchived
                            },
                        MetaData = request.Data.IncludeMetaData
                                ? new MetadataModel
                                {
                                    MetaTitle = blogPost.MetaTitle,
                                    MetaDescription = blogPost.MetaDescription,
                                    MetaKeywords = blogPost.MetaKeywords
                                }
                                : null,
                        Author = blogPost.Author != null && !blogPost.Author.IsDeleted && request.Data.IncludeAuthor
                                ? new AuthorModel
                                {
                                    Id = blogPost.Author.Id,
                                    Version = blogPost.Author.Version,
                                    CreatedBy = blogPost.Author.CreatedByUser,
                                    CreatedOn = blogPost.Author.CreatedOn,
                                    LastModifiedBy = blogPost.Author.ModifiedByUser,
                                    LastModifiedOn = blogPost.Author.ModifiedOn,

                                    Name = blogPost.Author.Name,
                                    ImageId = blogPost.Author.Image != null && !blogPost.Author.Image.IsDeleted ? blogPost.Author.Image.Id : (Guid?)null
                                }
                                : null,
                        Language = blogPost.Language != null && !blogPost.Language.IsDeleted && request.Data.IncludeLanguage
                                ? new LanguageModel
                                {
                                    Id = blogPost.Language.Id,
                                    Version = blogPost.Language.Version,
                                    CreatedBy = blogPost.Language.CreatedByUser,
                                    CreatedOn = blogPost.Language.CreatedOn,
                                    LastModifiedBy = blogPost.Language.ModifiedByUser,
                                    LastModifiedOn = blogPost.Language.ModifiedOn,

                                    Name = blogPost.Language.Name,
                                    Code = blogPost.Language.Code,
                                    LanguageGroupIdentifier = blogPost.LanguageGroupIdentifier
                                }
                                : null,
                        Layout = request.Data.IncludeLayout && !blogPost.Layout.IsDeleted
                                ? new LayoutModel
                                {
                                    Id = blogPost.Layout.Id,
                                    Version = blogPost.Layout.Version,
                                    CreatedBy = blogPost.Layout.CreatedByUser,
                                    CreatedOn = blogPost.Layout.CreatedOn,
                                    LastModifiedBy = blogPost.Layout.ModifiedByUser,
                                    LastModifiedOn = blogPost.Layout.ModifiedOn,

                                    Name = blogPost.Layout.Name,
                                    LayoutPath = blogPost.Layout.LayoutPath,
                                    PreviewUrl = blogPost.Layout.PreviewUrl
                                }
                                : null,
                        MainImage = blogPost.Image != null && !blogPost.Image.IsDeleted && request.Data.IncludeImages
                                ? new ImageModel
                                {
                                    Id = blogPost.Image.Id,
                                    Version = blogPost.Image.Version,
                                    CreatedBy = blogPost.Image.CreatedByUser,
                                    CreatedOn = blogPost.Image.CreatedOn,
                                    LastModifiedBy = blogPost.Image.ModifiedByUser,
                                    LastModifiedOn = blogPost.Image.ModifiedOn,

                                    Title = blogPost.Image.Title,
                                    Caption = blogPost.Image.Caption,
                                    Url = fileUrlResolver.EnsureFullPathUrl(blogPost.Image.PublicUrl),
                                    ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(blogPost.Image.PublicThumbnailUrl)
                                }
                                : null,
                        FeaturedImage = blogPost.FeaturedImage != null && !blogPost.FeaturedImage.IsDeleted && request.Data.IncludeImages
                                ? new ImageModel
                                {
                                    Id = blogPost.FeaturedImage.Id,
                                    Version = blogPost.FeaturedImage.Version,
                                    CreatedBy = blogPost.FeaturedImage.CreatedByUser,
                                    CreatedOn = blogPost.FeaturedImage.CreatedOn,
                                    LastModifiedBy = blogPost.FeaturedImage.ModifiedByUser,
                                    LastModifiedOn = blogPost.FeaturedImage.ModifiedOn,

                                    Title = blogPost.FeaturedImage.Title,
                                    Caption = blogPost.FeaturedImage.Caption,
                                    Url = fileUrlResolver.EnsureFullPathUrl(blogPost.FeaturedImage.PublicUrl),
                                    ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(blogPost.FeaturedImage.PublicThumbnailUrl)
                                }
                                : null,
                        SecondaryImage = blogPost.SecondaryImage != null && !blogPost.SecondaryImage.IsDeleted && request.Data.IncludeImages
                                ? new ImageModel
                                {
                                    Id = blogPost.SecondaryImage.Id,
                                    Version = blogPost.SecondaryImage.Version,
                                    CreatedBy = blogPost.SecondaryImage.CreatedByUser,
                                    CreatedOn = blogPost.SecondaryImage.CreatedOn,
                                    LastModifiedBy = blogPost.SecondaryImage.ModifiedByUser,
                                    LastModifiedOn = blogPost.SecondaryImage.ModifiedOn,

                                    Title = blogPost.SecondaryImage.Title,
                                    Caption = blogPost.SecondaryImage.Caption,
                                    Url = fileUrlResolver.EnsureFullPathUrl(blogPost.SecondaryImage.PublicUrl),
                                    ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(blogPost.SecondaryImage.PublicThumbnailUrl)
                                }
                                : null
                    })
                .FirstOne();

            if (request.Data.IncludeHtmlContent || request.Data.IncludeTechnicalInfo || request.Data.IncludeChildContentsOptions)
            {
                LoadContents(request.Data.IncludeHtmlContent,
                    request.Data.IncludeTechnicalInfo,
                    request.Data.IncludeChildContentsOptions,
                    request.BlogPostId,
                    response);
            }

            if (request.Data.IncludeTags)
            {
                response.Tags = LoadTags(request.BlogPostId);
            }

            if (request.Data.IncludeAccessRules)
            {
                // Get layout options, page options and merge them
                response.AccessRules = LoadAccessRules(response.Data.Id);
            }

            response.Data.Categories = categoriesService.GetSelectedCategoriesIds<Module.Blog.Models.BlogPost, PageCategory>(request.BlogPostId).ToList();
            
            if (request.Data.IncludeCategories)
            {
                response.Categories = LoadCategories(request.BlogPostId);
            }
            return response;
        }

        private IList<CategoryModel> LoadCategories(Guid blogPostId)
        {
            return (from page in repository.AsQueryable<Module.Blog.Models.BlogPost>()
                    from category in page.Categories
                    where page.Id == blogPostId
                    select new CategoryModel
                     {
                         Id = category.Category.Id,
                         Version = category.Version,
                         CreatedBy = category.CreatedByUser,
                         CreatedOn = category.CreatedOn,
                         LastModifiedBy = category.ModifiedByUser,
                         LastModifiedOn = category.ModifiedOn,
                         Name = category.Category.Name
                     })
                     .ToList();
        }

        private void LoadContents(bool includeHtml, bool includeTechnicalInfo, bool includeChildContentsOptions,
            Guid blogPostId, GetBlogPostPropertiesResponse response)
        {
            var content =
                repository.AsQueryable<Module.Root.Models.PageContent>(pc => pc.Page.Id == blogPostId && !pc.Content.IsDeleted && pc.Content is BlogPostContent)
                    .Select(pc => new
                                  {
                                      Html = ((BlogPostContent)pc.Content).Html,
                                      OriginalText = ((BlogPostContent)pc.Content).OriginalText,
                                      ContentTextMode = ((BlogPostContent)pc.Content).ContentTextMode,
                                      ContentId = pc.Content.Id,
                                      PageContentId = pc.Id,
                                      RegionId = pc.Region.Id
                                  })
                    .FirstOrDefault();

            if (content != null)
            {
                if (includeHtml)
                {
                    response.HtmlContent = content.Html;
                    response.OriginalText = content.OriginalText;
                    response.ContentTextMode = (ContentTextMode)content.ContentTextMode;
                }

                if (includeTechnicalInfo)
                {
                    response.TechnicalInfo = new TechnicalInfoModel
                                             {
                                                 BlogPostContentId = content.ContentId,
                                                 PageContentId = content.PageContentId,
                                                 RegionId = content.RegionId
                                             };
                }

                if (includeChildContentsOptions)
                {
                    response.ChildContentsOptionValues = optionService
                        .GetChildContentsOptionValues(content.ContentId)
                        .ToServiceModel();
                }
            }
        }

        private List<TagModel> LoadTags(Guid blogPostId)
        {
            return repository
                .AsQueryable<PageTag>(pageTag => pageTag.Page.Id == blogPostId && !pageTag.Tag.IsDeleted)
                .OrderBy(tag => tag.Tag.Name)
                .Select(media =>
                    new TagModel
                    {
                        Id = media.Tag.Id,
                        Version = media.Tag.Version,
                        CreatedBy = media.Tag.CreatedByUser,
                        CreatedOn = media.Tag.CreatedOn,
                        LastModifiedBy = media.Tag.ModifiedByUser,
                        LastModifiedOn = media.Tag.ModifiedOn,
                        Name = media.Tag.Name
                    })
                .ToList();
        }

        private List<AccessRuleModel> LoadAccessRules(Guid blogPostId)
        {
            return (from page in repository.AsQueryable<Module.Blog.Models.BlogPost>()
                    from accessRule in page.AccessRules
                    where page.Id == blogPostId
                    orderby accessRule.IsForRole, accessRule.Identity
                    select new AccessRuleModel
                    {
                        AccessLevel = (AccessLevel)(int)accessRule.AccessLevel,
                        Identity = accessRule.Identity,
                        IsForRole = accessRule.IsForRole
                    })
                    .ToList();
        }

        public PostBlogPostPropertiesResponse Post(PostBlogPostPropertiesRequest request)
        {
            var result = Put(
                    new PutBlogPostPropertiesRequest
                    {
                        Data = request.Data,
                        User = request.User
                    });

            return new PostBlogPostPropertiesResponse { Data = result.Data };
        }

        public PutBlogPostPropertiesResponse Put(PutBlogPostPropertiesRequest request)
        {
            var serviceModel = request.Data.ToServiceModel();
            if (request.Id.HasValue)
            {
                serviceModel.Id = request.Id.Value;
            }

            string[] error;
            var childContentOptionValues = request.Data.ChildContentsOptionValues != null ? request.Data.ChildContentsOptionValues.ToViewModel() : null;
            var response = blogSaveService.SaveBlogPost(serviceModel, childContentOptionValues, securityService.GetCurrentPrincipal(), out error);
            if (response == null)
            {
                throw new CmsApiValidationException(error != null && error.Length > 0 ? string.Join(",", error) : "Page properties saving was canceled.");
            }

            return new PutBlogPostPropertiesResponse { Data = response.Id };
        }

        public DeleteBlogPostPropertiesResponse Delete(DeleteBlogPostPropertiesRequest request)
        {
            var model = new DeletePageViewModel
                    {
                        PageId = request.Id,
                        Version = request.Data.Version
                    };
            var result = pageService.DeletePage(model, securityService.GetCurrentPrincipal());

            return new DeleteBlogPostPropertiesResponse { Data = result };
        }
    }
}