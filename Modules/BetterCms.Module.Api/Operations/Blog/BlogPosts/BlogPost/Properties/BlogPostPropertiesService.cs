using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.MediaManager.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    public class BlogPostPropertiesService : Service, IBlogPostPropertiesService
    {
        private readonly IRepository repository;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        public BlogPostPropertiesService(IRepository repository, IMediaFileUrlResolver fileUrlResolver)
        {
            this.repository = repository;
            this.fileUrlResolver = fileUrlResolver;
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
                                LayoutId = blogPost.Layout != null && !blogPost.Layout.IsDeleted ? blogPost.Layout.Id : Guid.Empty,
                                CategoryId = blogPost.Category != null && !blogPost.Category.IsDeleted ? blogPost.Category.Id : (Guid?)null,
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
                        Category = blogPost.Category != null && !blogPost.Category.IsDeleted && request.Data.IncludeCategory 
                                ? new CategoryModel
                                {
                                    Id = blogPost.Category.Id,
                                    Version = blogPost.Category.Version,
                                    CreatedBy = blogPost.Category.CreatedByUser,
                                    CreatedOn = blogPost.Category.CreatedOn,
                                    LastModifiedBy = blogPost.Category.ModifiedByUser,
                                    LastModifiedOn = blogPost.Category.ModifiedOn,    
                                
                                    Name = blogPost.Category.Name    
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

            if (request.Data.IncludeHtmlContent)
            {
                response.HtmlContent = LoadHtml(request.BlogPostId);
            }

            if (request.Data.IncludeTags)
            {
                response.Tags = LoadTags(request.BlogPostId);
            }

            return response;
        }

        private string LoadHtml(Guid blogPostId)
        {
            return repository
                .AsQueryable<Module.Root.Models.PageContent>(pc => pc.Page.Id == blogPostId && !pc.Content.IsDeleted && pc.Content is BlogPostContent)
                .Select(pc => ((BlogPostContent)pc.Content).Html)
                .FirstOrDefault();
        }

        private List<TagModel> LoadTags(Guid blogPostId)
        {
            return repository
                .AsQueryable<Module.Pages.Models.PageTag>(pageTag => pageTag.Page.Id == blogPostId && !pageTag.Tag.IsDeleted)                
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
    }
}