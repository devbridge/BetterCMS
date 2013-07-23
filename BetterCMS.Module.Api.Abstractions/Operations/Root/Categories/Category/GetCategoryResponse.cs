using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    [DataContract]
    public class GetCategoryResponse : ResponseBase<CategoryModel>
    {
    }
}