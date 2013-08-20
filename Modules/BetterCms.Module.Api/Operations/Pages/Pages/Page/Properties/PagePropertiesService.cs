using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    public class PagePropertiesService : Service, IPagePropertiesService
    {
        private readonly IRepository repository;

        private readonly IUrlService urlService;
        
        private readonly IOptionService optionService;

        public PagePropertiesService(IRepository repository, IUrlService urlService, IOptionService optionService)
        {
            this.repository = repository;
            this.urlService = urlService;
            this.optionService = optionService;
        }

        public GetPagePropertiesResponse Get(GetPagePropertiesRequest request)
        {
            var query = repository.AsQueryable<Module.Pages.Models.PageProperties>();

            if (request.PageId.HasValue)
            {
                query = query.Where(page => page.Id == request.PageId.Value);
            }
            else
            {
                var url = urlService.FixUrl(request.PageUrl);
                query = query.Where(page => page.PageUrl == url);
            }

            var response = query
                .Select(page => new GetPagePropertiesResponse
                    {
                        Data = new PagePropertiesModel
                            {
                                Id = page.Id,
                                Version = page.Version,
                                CreatedBy = page.CreatedByUser,
                                CreatedOn = page.CreatedOn,
                                LastModifiedBy = page.ModifiedByUser,
                                LastModifiedOn = page.ModifiedOn,

                                PageUrl = page.PageUrl,
                                Title = page.Title,
                                Description = page.Description,
                                IsPublished = page.Status == PageStatus.Published,
                                PublishedOn = page.PublishedOn,
                                LayoutId = page.Layout.Id,
                                CategoryId = page.Category.Id,
                                IsArchived = page.IsArchived,
                                MainImageId = page.Image.Id,
                                FeaturedImageId = page.FeaturedImage.Id,
                                SecondaryImageId = page.SecondaryImage.Id,
                                CustomCss = page.CustomCss,
                                CustomJavaScript = page.CustomJS,
                                UseCanonicalUrl = page.UseCanonicalUrl,
                                UseNoFollow = page.UseNoFollow,
                                UseNoIndex = page.UseNoIndex
                            },
                        MetaData = request.Data.IncludeMetaData ? new MetadataModel
                            {
                                MetaTitle = page.MetaTitle,
                                MetaDescription = page.MetaDescription,
                                MetaKeywords = page.MetaKeywords
                            } : null,
                        Category = page.Category != null && request.Data.IncludeCategory ? new CategoryModel
                            {
                                Id = page.Category.Id,
                                Version = page.Category.Version,
                                CreatedBy = page.Category.CreatedByUser,
                                CreatedOn = page.Category.CreatedOn,
                                LastModifiedBy = page.Category.ModifiedByUser,
                                LastModifiedOn = page.Category.ModifiedOn,

                                Name = page.Category.Name
                            } : null,
                        Layout = request.Data.IncludeLayout ? new LayoutModel
                            {
                                Id = page.Layout.Id,
                                Version = page.Layout.Version,
                                CreatedBy = page.Layout.CreatedByUser,
                                CreatedOn = page.Layout.CreatedOn,
                                LastModifiedBy = page.Layout.ModifiedByUser,
                                LastModifiedOn = page.Layout.ModifiedOn,

                                Name = page.Layout.Name,
                                LayoutPath = page.Layout.LayoutPath,
                                PreviewUrl = page.Layout.PreviewUrl
                            } : null,
                        MainImage = page.Image != null && request.Data.IncludeImages ? new ImageModel
                            {
                                Id = page.Image.Id,
                                Version = page.Image.Version,
                                CreatedBy = page.Image.CreatedByUser,
                                CreatedOn = page.Image.CreatedOn,
                                LastModifiedBy = page.Image.ModifiedByUser,
                                LastModifiedOn = page.Image.ModifiedOn,

                                Title = page.Image.Title,
                                Caption = page.Image.Caption,
                                Url = page.Image.PublicUrl,
                                ThumbnailUrl = page.Image.PublicThumbnailUrl
                            } : null,
                        FeaturedImage = page.FeaturedImage != null && request.Data.IncludeImages ? new ImageModel
                            {
                                Id = page.FeaturedImage.Id,
                                Version = page.FeaturedImage.Version,
                                CreatedBy = page.FeaturedImage.CreatedByUser,
                                CreatedOn = page.FeaturedImage.CreatedOn,
                                LastModifiedBy = page.FeaturedImage.ModifiedByUser,
                                LastModifiedOn = page.FeaturedImage.ModifiedOn,

                                Title = page.FeaturedImage.Title,
                                Caption = page.FeaturedImage.Caption,
                                Url = page.FeaturedImage.PublicUrl,
                                ThumbnailUrl = page.FeaturedImage.PublicThumbnailUrl
                            } : null,
                        SecondaryImage = page.SecondaryImage != null && request.Data.IncludeImages ? new ImageModel
                            {
                                Id = page.SecondaryImage.Id,
                                Version = page.SecondaryImage.Version,
                                CreatedBy = page.SecondaryImage.CreatedByUser,
                                CreatedOn = page.SecondaryImage.CreatedOn,
                                LastModifiedBy = page.SecondaryImage.ModifiedByUser,
                                LastModifiedOn = page.SecondaryImage.ModifiedOn,

                                Title = page.SecondaryImage.Title,
                                Caption = page.SecondaryImage.Caption,
                                Url = page.SecondaryImage.PublicUrl,
                                ThumbnailUrl = page.SecondaryImage.PublicThumbnailUrl
                            } : null
                    })
                .FirstOne();

            if (request.Data.IncludeTags)
            {
                response.Tags = LoadTags(response.Data.Id);
            }

            if (request.Data.IncludePageContents)
            {
                response.PageContents = LoadPageContents(response.Data.Id);
            }

            if (request.Data.IncludePageOptions)
            {
                // Get layout options, page options and merge them
                var layoutOptions = repository
                    .AsQueryable<LayoutOption>(lo => lo.Layout.Id == response.Data.LayoutId).ToList();
                var pageOptions = repository
                    .AsQueryable<PageOption>(p => p.Page.Id == response.Data.Id)
                    .ToList();

                response.PageOptions = optionService
                    .GetMergedOptionValuesForEdit(layoutOptions, pageOptions)
                    .Select(o => new OptionModel
                            {
                                Key = o.OptionKey,
                                Value = o.OptionValue,
                                DefaultValue = o.OptionDefaultValue,
                                Type = ((Root.OptionType)(int)o.Type)
                            })
                    .ToList();
            }

            return response;
        }

        private System.Collections.Generic.List<TagModel> LoadTags(System.Guid blogPostId)
        {
            return repository
                .AsQueryable<Module.Pages.Models.PageTag>(pageTag => pageTag.Page.Id == blogPostId)
                .Select(media => new TagModel
                    {
                        Id = media.Tag.Id,
                        Version = media.Tag.Version,
                        CreatedBy = media.Tag.CreatedByUser,
                        CreatedOn = media.Tag.CreatedOn,
                        LastModifiedBy = media.Tag.ModifiedByUser,
                        LastModifiedOn = media.Tag.ModifiedOn,

                        Name = media.Tag.Name
                    }).ToList();
        }
        
        private System.Collections.Generic.List<PageContentModel> LoadPageContents(System.Guid blogPostId)
        {
            var results = repository
                 .AsQueryable<PageContent>(pageContent => pageContent.Page.Id == blogPostId)
                 .OrderBy(pageContent => pageContent.Order)
                 .Select(pageContent => new
                    {
                        Type = pageContent.Content.GetType(),
                        Model = new PageContentModel
                            {
                                Id = pageContent.Id,
                                Version = pageContent.Version,
                                CreatedBy = pageContent.CreatedByUser,
                                CreatedOn = pageContent.CreatedOn,
                                LastModifiedBy = pageContent.ModifiedByUser,
                                LastModifiedOn = pageContent.ModifiedOn,

                                ContentId = pageContent.Content.Id,
                                Name = pageContent.Content.Name,
                                RegionId = pageContent.Region.Id,
                                RegionIdentifier = pageContent.Region.RegionIdentifier,
                                Order = pageContent.Order,
                                IsPublished = pageContent.Content.Status == ContentStatus.Published
                            }
                    }).ToList();

            // Set content types
            results.ToList().ForEach(item => item.Model.ContentType = item.Type.ToContentTypeString());

            return results.Select(item => item.Model).ToList();
        }
    }
}