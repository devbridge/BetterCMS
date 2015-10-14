using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node
{
    /// <summary>
    /// Request to delete category node.
    /// </summary>
    [Serializable]
    [DataContract]
    public class DeleteNodeRequest : DeleteRequestBase
    {
        [DataMember]
        public Guid CategoryTreeId { get; set; }
    }
}