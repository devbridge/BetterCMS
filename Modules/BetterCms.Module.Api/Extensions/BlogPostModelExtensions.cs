using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Security;

using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.Api.Extensions
{
    public static class BlogPostModelExtensions
    {
        public static BlogPostViewModelExtender ToServiceModel(this SaveBlogPostPropertiesModel model)
        {
            var serviceModel = new BlogPostViewModelExtender();
            
            serviceModel.Version = model.Version;
            serviceModel.Title = model.Title;
            serviceModel.IntroText = model.IntroText;
            serviceModel.Content = model.HtmlContent;
            serviceModel.OriginalText = model.OriginalText;
            serviceModel.ContentTextMode = (ContentTextMode)model.ContentTextMode;
            serviceModel.LiveFromDate = model.ActivationDate;
            serviceModel.LiveToDate = model.ExpirationDate;
            serviceModel.BlogUrl = model.BlogPostUrl;
            serviceModel.AuthorId = model.AuthorId;
            serviceModel.Categories = model.Categories != null ? model.Categories.Select(c => new LookupKeyValue()
            {
                Key = c.ToString(),
            }).ToList() : new List<LookupKeyValue>();
            serviceModel.DesirableStatus = model.IsPublished ? ContentStatus.Published : ContentStatus.Draft;
            serviceModel.Tags = model.Tags;
            serviceModel.Image = new ImageSelectorViewModel { ImageId = model.MainImageId };
            serviceModel.FeaturedImageId = model.FeaturedImageId;
            serviceModel.SecondaryImageId = model.SecondaryImageId;
            serviceModel.IsArchived = model.IsArchived;
            serviceModel.PublishedOn = model.PublishedOn;
            serviceModel.LayoutId = model.LayoutId;
            serviceModel.MasterPageId = model.MasterPageId;

            serviceModel.ContentId = model.TechnicalInfo != null && model.TechnicalInfo.BlogPostContentId.HasValue ? model.TechnicalInfo.BlogPostContentId.Value : Guid.Empty;
            serviceModel.PageContentId = model.TechnicalInfo != null && model.TechnicalInfo.PageContentId.HasValue ? model.TechnicalInfo.PageContentId.Value : (Guid?)null;
            serviceModel.RegionId = model.TechnicalInfo != null && model.TechnicalInfo.RegionId.HasValue ? model.TechnicalInfo.RegionId.Value : (Guid?)null;

            serviceModel.UseCanonicalUrl = model.UseCanonicalUrl;
            serviceModel.UseNoFollow = model.UseNoFollow;
            serviceModel.UseNoIndex = model.UseNoIndex;
            serviceModel.MetaKeywords = model.MetaData != null ? model.MetaData.MetaKeywords : null;
            serviceModel.MetaDescription = model.MetaData != null ? model.MetaData.MetaDescription : null;
            serviceModel.MetaTitle = model.MetaData != null ? model.MetaData.MetaTitle : null;

            if (model.Language != null)
            {
                serviceModel.UpdateLanguage = true;
                serviceModel.LanguageGroupIdentifier = model.Language.LanguageGroupIdentifier;
                serviceModel.LanguageId = model.Language.Id;
            }

            if (model.AccessRules != null)
            {
                serviceModel.AccessRules = model.AccessRules
                    .Select(r => (IAccessRule)new UserAccessViewModel
                            {
                                AccessLevel = (AccessLevel)(int)r.AccessLevel, 
                                Identity = r.Identity, 
                                IsForRole = r.IsForRole
                            })
                    .ToList();
            }

            return serviceModel;
        }
    }
}