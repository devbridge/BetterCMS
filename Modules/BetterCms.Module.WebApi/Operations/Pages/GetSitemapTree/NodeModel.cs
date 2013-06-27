using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetSitemapTree
{
    [DataContract]
    public class NodeModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the parent node id.
        /// </summary>
        /// <value>
        /// The parent node id.
        /// </value>
        [DataMember(Order = 10, Name = "parentId")]
        public System.Guid? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the node title.
        /// </summary>
        /// <value>
        /// The node title.
        /// </value>
        [DataMember(Order = 20, Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the node URL.
        /// </summary>
        /// <value>
        /// The ndoe URL.
        /// </value>
        [DataMember(Order = 30, Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the node display order.
        /// </summary>
        /// <value>
        /// The node display order.
        /// </value>
        [DataMember(Order = 40, Name = "displayOrder")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the children nodes.
        /// </summary>
        /// <value>
        /// The children nodes.
        /// </value>
        [DataMember(Order = 50, Name = "childrenNodes")]
        public IList<NodeModel> ChildrenNodes { get; set; }
    }
}