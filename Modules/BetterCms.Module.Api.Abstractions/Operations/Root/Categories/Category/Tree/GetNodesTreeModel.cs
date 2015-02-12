using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Tree
{
    [Serializable]
    [DataContract]
    public class GetNodesTreeModel
    {
        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        /// <value>
        /// The node id.
        /// </value>
        [DataMember]
        public Guid? NodeId { get; set; }
    }
}