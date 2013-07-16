using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.Helpers;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    public class PagesService : Service, IPagesService
    {
        private readonly IRepository repository;

        public PagesService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetPagesResponse Get(GetPagesRequest request)
        {
            request.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<Module.Pages.Models.PageProperties>();

            if (!request.IncludeArchived)
            {
                query = query.Where(b => !b.IsArchived);
            }

            if (!request.IncludeUnpublished)
            {
                query = query.Where(b => b.Status == PageStatus.Published);
            }

            query = query.ApplyTagsFilter(
                request,
                tagName => { return page => page.PageTags.Any(tag => tag.Tag.Name == tagName); });

            var listResponse = query
                .Select(page => new PageModel
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
                        CategoryName = page.Category.Name,
                        MainImageId = page.Image.Id,
                        MainImageUrl = page.Image.PublicUrl,
                        MainImageThumbnauilUrl = page.Image.PublicThumbnailUrl,
                        MainImageCaption = page.Image.Caption,
                        IsArchived = page.IsArchived
                    }).ToDataListResponse(request);

            return new GetPagesResponse
            {
                Data = listResponse
            };
        }
    }
}