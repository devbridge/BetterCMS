using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Pages.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents
{
    public class PageContentsService : Service, IPageContentsService
    {
        private readonly IRepository repository;

        public PageContentsService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetPageContentsResponse Get(GetPageContentsRequest request)
        {
            // TODO add validation for only one of RegionId / RegionIdentifier specified

            request.SetDefaultOrder("Order");

            var query = repository
                 .AsQueryable<Module.Root.Models.PageContent>(pageContent => pageContent.Page.Id == request.PageId);

            if (request.RegionId.HasValue)
            {
                query = query.Where(pageContent => pageContent.Region.Id == request.RegionId);
            }
            else if (!string.IsNullOrWhiteSpace(request.RegionIdentifier))
            {
                query = query.Where(pageContent => pageContent.Region.RegionIdentifier == request.RegionIdentifier);
            }

            if (!request.IncludeUnpublished)
            {
                query = query.Where(pageContent => pageContent.Content.Status == ContentStatus.Published);
            }

            var dataListResult = query.Select(pageContent => new PageContentModel
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
                     }).ToDataListResponse(request);

            return new GetPageContentsResponse { Data = dataListResult };
        }
    }
}