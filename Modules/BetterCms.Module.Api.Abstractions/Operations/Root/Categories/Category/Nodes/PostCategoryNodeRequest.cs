using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes
{
    /// <summary>
    /// Request for page creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostCategoryNodeRequest : RequestBase<SaveNodeModel>
    {
        [DataMember]
        public Guid CategoryTreeId { get; set; }
    }
}
