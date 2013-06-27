using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetSitemapNodeById
{
    [DataContract]
    public class GetSitemapNodeByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the sitemap node id.
        /// </summary>
        /// <value>
        /// The sitemap node id.
        /// </value>
        [DataMember(Order = 10, Name = "nodeId")]
        public System.Guid NodeId { get; set; }
    }
}