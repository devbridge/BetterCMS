using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes
{
    [Serializable]
    [DataContract]
    public class GetCategoryNodesResponse : ListResponseBase<CategoryNodeModel>
    {
    }
}