using System.Linq;

using BetterCms.Module.Api.Operations.Blog.Authors.Author;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;

namespace BetterCms.Module.Api.Extensions
{
    public static class BlogPostExtensions
    {
        public static PutBlogPostPropertiesRequest ToPutRequest(this GetBlogPostPropertiesResponse response)
        {
            var model = MapBlogPostModel(response);

            return new PutBlogPostPropertiesRequest { Data = model, BlogPostId = response.Data.Id };
        }
        
        public static PostBlogPostPropertiesRequest ToPostRequest(this GetBlogPostPropertiesResponse response)
        {
            var model = MapBlogPostModel(response);

            return new PostBlogPostPropertiesRequest { Data = model };
        }

        public static PutAuthorRequest ToPutRequest(this GetAuthorResponse response)
        {
            var model = MapAuthorModel(response);

            return new PutAuthorRequest { Data = model, AuthorId = response.Data.Id };
        }

        public static PostAuthorRequest ToPostRequest(this GetAuthorResponse response)
        {
            var model = MapAuthorModel(response);

            return new PostAuthorRequest { Data = model };
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
                            CategoryId = response.Data.CategoryId,
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
                            MetaData = response.MetaData,
                            TechnicalInfo = response.TechnicalInfo,
                            AccessRules = response.AccessRules,
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
                        };

            return model;
        }
    }
}
