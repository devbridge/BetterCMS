using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    public class CategoryService : Service, ICategoryService
    {
        public GetCategoryResponse Get(GetCategoryRequest request)
        {
            return new GetCategoryResponse
                       {
                           Data = new CategoryModel()
                       };
        }
    }
}