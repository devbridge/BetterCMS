using System.Linq;

using BetterCms.Module.Api.Operations.Blog.Authors.Author;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.Settings;
using BetterCms.Module.Api.Operations.Pages;

namespace BetterCms.Module.Api.Extensions
{
    public static class BlogPostExtensions
    {
        public static PutBlogPostPropertiesRequest ToPutRequest(this GetBlogPostPropertiesResponse response)
        {
            var model = MapBlogPostModel(response);

            return new PutBlogPostPropertiesRequest { Data = model, Id = response.Data.Id };
        }
        
        public static PostBlogPostPropertiesRequest ToPostRequest(this GetBlogPostPropertiesResponse response)
        {
            var model = MapBlogPostModel(response);

            return new PostBlogPostPropertiesRequest { Data = model };
        }

        public static PutAuthorRequest ToPutRequest(this GetAuthorResponse response)
        {
            var model = MapAuthorModel(response);

            return new PutAuthorRequest { Data = model, Id = response.Data.Id };
        }

        public static PostAuthorRequest ToPostRequest(this GetAuthorResponse response)
        {
            var model = MapAuthorModel(response);

            return new PostAuthorRequest { Data = model };
        }

        public static PutBlogPostsSettingsRequest ToPutRequest(this GetBlogPostsSettingsResponse response)
        {
            var model = MapSettingsModel(response);

            return new PutBlogPostsSettingsRequest { Data = model };
        }

        private static SaveBlogPostsSettingsModel MapSettingsModel(GetBlogPostsSettingsResponse response)
        {
            return new SaveBlogPostsSettingsModel
                       {
                           Version = response.Data != null ? response.Data.Version : 0,
                           DefaultMasterPageId = response.Data != null ? response.Data.DefaultMasterPageId : null,
                           DefaultLayoutId = response.Data != null ? response.Data.DefaultLayoutId : null
                       };
        }

        private static SaveBlogPostPropertiesModel MapBlogPostModel(GetBlogPostPropertiesResponse response)
        {
            var model = new SaveBlogPostPropertiesModel
                        {
                            Version = response.Data.Version,
                            BlogPostUrl = response.Data.BlogPostUrl,
                            Title = response.Data.Title,
                            IntroText = response.Data.IntroText,
                            IsPublished = response.Data.IsPublished,
                            PublishedOn = response.Data.PublishedOn,
                            LayoutId = response.Data.LayoutId,
                            MasterPageId = response.Data.MasterPageId,
                            Categories = response.Data.Categories,
                            AuthorId = response.Data.AuthorId,
                            MainImageId = response.Data.MainImageId,
                            FeaturedImageId = response.Data.FeaturedImageId,
                            SecondaryImageId = response.Data.SecondaryImageId,
                            ActivationDate = response.Data.ActivationDate,
                            ExpirationDate = response.Data.ExpirationDate,
                            IsArchived = response.Data.IsArchived,
                            UseCanonicalUrl = response.Data.UseCanonicalUrl,
                            UseNoFollow = response.Data.UseNoFollow,
                            UseNoIndex = response.Data.UseNoIndex,
                            HtmlContent = response.HtmlContent,
                            OriginalText = response.OriginalText,
                            ContentTextMode = response.ContentTextMode ?? ContentTextMode.Html,
                            MetaData = response.MetaData,
                            Language = response.Language,
                            TechnicalInfo = response.TechnicalInfo,
                            AccessRules = response.AccessRules,
                            ChildContentsOptionValues = response.ChildContentsOptionValues
                        };

            if (response.Tags != null)
            {
                model.Tags = response.Tags.Select(t => t.Name).ToList();
            }

            return model;
        }

        private static SaveAuthorModel MapAuthorModel(GetAuthorResponse response)
        {
            var model = new SaveAuthorModel
                        {
                            Version = response.Data.Version,
                            Name = response.Data.Name,
                            Description = response.Data.Description,
                            ImageId = response.Data.ImageId
                        };

            return model;
        }
    }
}
