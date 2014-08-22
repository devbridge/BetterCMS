using BetterCms.Module.Api.Operations.Root.Categories.Category;

namespace BetterCms.Module.Api.Extensions
{
    public static class CategoryExtensions
    {
        public static PostCategoryRequest ToPostRequest(this GetCategoryResponse response)
        {
            var model = MapModel(response);

            return new PostCategoryRequest { Data = model };
        }

        public static PutCategoryRequest ToPutRequest(this GetCategoryResponse response)
        {
            var model = MapModel(response);

            return new PutCategoryRequest { Data = model, Id = response.Data.Id };
        }

        private static SaveCategoryModel MapModel(GetCategoryResponse response)
        {
            var model = new SaveCategoryModel
                        {
                            Version = response.Data.Version,
                            Name = response.Data.Name,
                        };

            return model;
        }
    }
}
