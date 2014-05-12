using System.Linq;

using BetterCms.Module.Api.Operations.Pages.Pages.Page;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;

namespace BetterCms.Module.Api.Extensions
{
    public static class PageExtensions
    {
        public static PutPagePropertiesRequest ToPutRequest(this GetPagePropertiesResponse response)
        {
            var model = MapPageModel(response);

            return new PutPagePropertiesRequest { Data = model, PageId = response.Data.Id };
        }

        public static PostPagePropertiesRequest ToPostRequest(this GetPagePropertiesResponse response)
        {
            var model = MapPageModel(response);

            return new PostPagePropertiesRequest { Data = model };
        }

        private static SavePagePropertiesModel MapPageModel(GetPagePropertiesResponse response)
        {
            var model = new SavePagePropertiesModel
            {
                Version = response.Data.Version,
                PageUrl = response.Data.PageUrl,
                Title = response.Data.Title,
                Description = response.Data.Description,
                IsPublished = response.Data.IsPublished,
                PublishedOn = response.Data.PublishedOn,
                LayoutId = response.Data.LayoutId,
                MasterPageId = response.Data.MasterPageId,
                CategoryId = response.Data.CategoryId,
                IsArchived = response.Data.IsArchived,
                MainImageId = response.Data.MainImageId,
                FeaturedImageId = response.Data.FeaturedImageId,
                SecondaryImageId = response.Data.SecondaryImageId,
                CustomCss = response.Data.CustomCss,
                CustomJavaScript = response.Data.CustomJavaScript,
                UseCanonicalUrl = response.Data.UseCanonicalUrl,
                UseNoFollow = response.Data.UseNoFollow,
                UseNoIndex = response.Data.UseNoIndex,
                IsMasterPage = response.Data.IsMasterPage,
                LanguageId = response.Data.LanguageId,
                LanguageGroupIdentifier = response.Data.LanguageGroupIdentifier,
                MetaData = response.MetaData,
                AccessRules = response.AccessRules,
                PageOptions = response.PageOptions,
            };

            if (response.Tags != null)
            {
                model.Tags = response.Tags.Select(t => t.Name).ToList();
            }

            return model;
        }
    }
}
