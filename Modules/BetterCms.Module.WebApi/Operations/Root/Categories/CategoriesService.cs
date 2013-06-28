using System.Collections.Generic;

using BetterCms.Core.Api.DataContracts;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    public class CategoriesService : Service, ICategoriesService
    {
        public GetCategoriesResponse Get(GetCategoriesRequest request)
        {
            return new GetCategoriesResponse
                       {
                           Data =
                               new DataListResponse<CategoryModel>
                                   {
                                       Items =
                                           new List<CategoryModel>
                                               {
                                                   new CategoryModel(),
                                                   new CategoryModel(),
                                                   new CategoryModel()
                                               },
                                       TotalCount = 10
                                   }
                       };
        }
    }
}