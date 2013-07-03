using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Pages.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    public class PagePropertiesService : Service, IPagePropertiesService
    {
        private readonly IRepository repository;

        public PagePropertiesService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetPagePropertiesResponse Get(GetPagePropertiesRequest request)
        {
            // TODO: validate request - one and only one of these can be specified: PageUrl / PageId

            var query = repository.AsQueryable<Module.Pages.Models.PageProperties>();

            if (request.PageId.HasValue)
            {
                query = query.Where(page => page.Id == request.PageId.Value);
            }
            else
            {
                query = query.Where(page => page.PageUrl == request.PageUrl);
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
                                CanonicalUrl = page.CanonicalUrl,
                                UseCanonicalUrl = page.UseCanonicalUrl,
                                UseNoFollow = page.UseNoFollow,
                                UseNoIndex = page.UseNoIndex
                            },
                        MetaData = request.IncludeMetaData ? new MetadataModel
                            {
                                MetaTitle = page.MetaTitle,
                                MetaDescription = page.MetaDescription,
                                MetaKeywords = page.MetaKeywords
                            } : null,
                        Category = page.Category != null && request.IncludeCategory ? new CategoryModel
                            {
                                Id = page.Category.Id,
                                Version = page.Category.Version,
                                CreatedBy = page.Category.CreatedByUser,
                                CreatedOn = page.Category.CreatedOn,
                                LastModifiedBy = page.Category.ModifiedByUser,
                                LastModifiedOn = page.Category.ModifiedOn,

                                Name = page.Category.Name
                            } : null,
                        Layout = request.IncludeLayout ? new LayoutModel
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
                        MainImage = page.Image != null && request.IncludeImages ? new ImageModel
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
                        FeaturedImage = page.FeaturedImage != null && request.IncludeImages ? new ImageModel
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
                        SecondaryImage = page.SecondaryImage != null && request.IncludeImages ? new ImageModel
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

            if (request.IncludeTags)
            {
                response.Tags = LoadTags(response.Data.Id);
            }
            
            if (request.IncludePageContents)
            {
                response.PageContents = LoadPageContents(response.Data.Id);
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
            return repository
                 .AsQueryable<Module.Root.Models.PageContent>(pageContent => pageContent.Page.Id == blogPostId)
                 .OrderBy(pageContent => pageContent.Order)
                 .Select(pageContent => new PageContentModel
                     {
                         Id = pageContent.Id,
                         Version = pageContent.Version,
                         CreatedBy = pageContent.CreatedByUser,
                         CreatedOn = pageContent.CreatedOn,
                         LastModifiedBy = pageContent.ModifiedByUser,
                         LastModifiedOn = pageContent.ModifiedOn,

                         ContentId = pageContent.Content.Id,
                         // TODO: ContentType = ???? - implement content type - projection ??????
                         Name = pageContent.Content.Name,
                         RegionId = pageContent.Region.Id,
                         RegionIdentifier = pageContent.Region.RegionIdentifier,
                         Order = pageContent.Order,
                         IsPublished = pageContent.Content.Status == ContentStatus.Published
                     }).ToList();
        }
    }
}