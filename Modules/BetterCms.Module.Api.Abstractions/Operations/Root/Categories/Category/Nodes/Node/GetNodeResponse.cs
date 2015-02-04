using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node
{
    [Serializable]
    [DataContract]
    public class GetNodeResponse : ResponseBase<NodeModel>
    {
    }
}