using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    [DataContract]
    [Serializable]
    public class GetCategoriesResponse : ListResponseBase<CategoryModel>
    {
    }
}