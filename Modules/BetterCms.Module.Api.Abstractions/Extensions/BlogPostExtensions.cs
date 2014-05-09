using System.Linq;

using BetterCms.Module.Api.Operations.Blog.BlogPosts;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;

namespace BetterCms.Module.Api.Extensions
{
    public static class BlogPostExtensions
    {
        public static PutBlogPostRequest ToPutRequest(this GetBlogPostPropertiesResponse response)
        {
            var model = MapBlogPostModel(response);

            return new PutBlogPostRequest { Data = model, BlogPostId = response.Data.Id };
        }
        
        public static PostBlogPostRequest ToPostRequest(this GetBlogPostPropertiesResponse response)
        {
            var model = MapBlogPostModel(response);

            return new PostBlogPostRequest { Data = model };
        }

        private static SaveBlogPostModel MapBlogPostModel(GetBlogPostPropertiesResponse response)
        {
            var model = new SaveBlogPostModel
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
    }
}
