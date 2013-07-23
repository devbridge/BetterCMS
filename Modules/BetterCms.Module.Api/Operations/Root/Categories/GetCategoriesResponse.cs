using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    [DataContract]
    public class GetCategoriesResponse : ListResponseBase<CategoryModel>
    {
    }
}