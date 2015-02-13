using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Response for categories list.
    /// </summary>
    [DataContract]
    [Serializable]
    public class GetCategoryTreesResponse : ListResponseBase<CategoryTreeModel>
    {
    }
}