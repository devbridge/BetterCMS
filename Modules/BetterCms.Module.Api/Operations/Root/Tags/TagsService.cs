using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    public class TagsService : Service, ITagsService
    {
        private readonly IRepository repository;

        public TagsService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetTagsResponse Get(GetTagsRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var listResponse = repository
                .AsQueryable<Module.Root.Models.Tag>()
                .Select(tag => new TagModel
                    {
                        Id = tag.Id,
                        Version = tag.Version,
                        CreatedBy = tag.CreatedByUser,
                        CreatedOn = tag.CreatedOn,
                        LastModifiedBy = tag.ModifiedByUser,
                        LastModifiedOn = tag.ModifiedOn,

                        Name = tag.Name
                    })
                .ToDataListResponse(request);

            return new GetTagsResponse
                       {
                           Data = listResponse
                       };
        }
    }
}