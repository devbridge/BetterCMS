using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Tree
{
    [DataContract]
    public class GetSitemapTreeModel
    {
        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        /// <value>
        /// The node id.
        /// </value>
        [DataMember]
        public System.Guid? NodeId { get; set; }
    }
}