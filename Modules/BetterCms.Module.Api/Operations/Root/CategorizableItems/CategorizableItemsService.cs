using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Root.Models;

using BetterModules.Core.DataAccess;

namespace BetterCms.Module.Api.Operations.Root.CategorizableItems
{
    [RoutePrefix("bcms-api")]
    public class CategorizableItemsController : ApiController, ICategorizableItemsService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        public CategorizableItemsController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("categorizable-items")]
        public GetCategorizableItemsResponse Get([ModelBinder(typeof(JsonModelBinder))]GetCategorizableItemsRequest request)
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