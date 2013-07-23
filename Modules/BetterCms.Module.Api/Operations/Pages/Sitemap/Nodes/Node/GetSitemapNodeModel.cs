using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node
{
    [DataContract]
    public class GetSitemapNodeModel
    {
        /// <summary>
        /// Gets or sets the sitemap node id.
        /// </summary>
        /// <value>
        /// The sitemap node id.
        /// </value>
        [DataMember]
        public System.Guid NodeId { get; set; }
    }
}