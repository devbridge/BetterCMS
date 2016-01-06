using System.Linq;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Root.Models;

using BetterModules.Core.DataAccess;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.CategorizableItems
{
    public class CategorizableItemsService : Service, ICategorizableItemsService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        public CategorizableItemsService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetCategorizableItemsResponse Get(GetCategorizableItemsRequest request)
        {
            request.Data.SetDefaultOrder("Name");
            var response = repository
                .AsQueryable<CategorizableItem>()
                .Select(ci => new CategorizableItemModel
                {
                    Id = ci.Id,
                    Version = ci.Version,
                    CreatedBy = ci.CreatedByUser,
                    CreatedOn = ci.CreatedOn,
                    LastModifiedBy = ci.ModifiedByUser,
                    LastModifiedOn = ci.ModifiedOn,

                    Name = ci.Name
                })
                .ToDataListResponse(request);

            return new GetCategorizableItemsResponse
            {
                Data = response
            };
        }
    }
}