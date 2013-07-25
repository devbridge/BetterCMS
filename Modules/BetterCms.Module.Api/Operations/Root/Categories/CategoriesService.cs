using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    public class CategoriesService : Service, ICategoriesService
    {
        private readonly IRepository repository;

        public CategoriesService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetCategoriesResponse Get(GetCategoriesRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var listResponse = repository
                .AsQueryable<Module.Root.Models.Category>()
                .Select(category => new CategoryModel
                    {
                        Id = category.Id,
                        Version = category.Version,
                        CreatedBy = category.CreatedByUser,
                        CreatedOn = category.CreatedOn,
                        LastModifiedBy = category.ModifiedByUser,
                        LastModifiedOn = category.ModifiedOn,

                        Name = category.Name
                    })
                .ToDataListResponse(request);

            return new GetCategoriesResponse
                       {
                           Data = listResponse
                       };
        }
    }
}