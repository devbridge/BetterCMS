using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node
{
    /// <summary>
    /// Request to save category node.
    /// </summary>
    [Serializable]
    [DataContract]
    public class PutNodeRequest : PutRequestBase<SaveNodeModel>
    {
        [DataMember]
        public Guid CategoryTreeId { get; set; }
    }
}