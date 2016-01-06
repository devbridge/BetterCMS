using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;

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
            request.Data.SetDefaultOrder("Order");

            var query = repository
                 .AsQueryable<Module.Root.Models.PageContent>(pageContent => pageContent.Page.Id == request.PageId && !pageContent.Page.IsDeleted && !pageContent.Content.IsDeleted);

            if (request.Data.RegionId.HasValue)
            {
                query = query.Where(pageContent => pageContent.Region != null && !pageContent.Region.IsDeleted && pageContent.Region.Id == request.Data.RegionId);
            }
            else if (!string.IsNullOrWhiteSpace(request.Data.RegionIdentifier))
            {
                query = query.Where(pageContent => pageContent.Region != null && !pageContent.Region.IsDeleted && pageContent.Region.RegionIdentifier == request.Data.RegionIdentifier);
            }

            if (!request.Data.IncludeUnpublished)
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
                         ParentPageContentId = pageContent.Parent != null ? pageContent.Parent.Id : (System.Guid?)null,
                         OriginalContentType = pageContent.Content.GetType(),
                         Name = pageContent.Content.Name,
                         RegionId = pageContent.Region.Id,
                         RegionIdentifier = pageContent.Region.RegionIdentifier,
                         Order = pageContent.Order,
                         IsPublished = pageContent.Content.Status == ContentStatus.Published
                     }).ToDataListResponse(request);

            // Set content types
            dataListResult.Items.ToList().ForEach(item => item.ContentType = item.OriginalContentType.ToContentTypeString());

            return new GetPageContentsResponse { Data = dataListResult };
        }
    }
}