using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetSitemapTree
{
    [DataContract]
    public class GetSitemapTreeRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        /// <value>
        /// The node id.
        /// </value>
        [DataMember(Order = 10, Name = "nodeId")]
        public System.Guid? NodeId { get; set; }
    }
}