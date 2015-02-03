using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Tree
{
    [Serializable]
    [DataContract]
    public class GetNodesTreeResponse : ResponseBase<System.Collections.Generic.List<NodesTreeNodeModel>>
    {
    }
}