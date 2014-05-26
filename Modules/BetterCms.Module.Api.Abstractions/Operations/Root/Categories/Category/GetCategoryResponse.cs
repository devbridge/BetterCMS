using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    [DataContract]
    [Serializable]
    public class GetCategoryResponse : ResponseBase<CategoryModel>
    {
    }
}