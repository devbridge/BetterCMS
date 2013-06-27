using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetPageContents
{
    [DataContract]
    public class GetPageContentsRequest : ListRequestBase
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        [DataMember(Order = 10, Name = "pageId")]
        public System.Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        /// <value>
        /// The region id.
        /// </value>
        [DataMember(Order = 20, Name = "regionId")]
        public System.Guid? RegionId { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        [DataMember(Order = 30, Name = "regionIdentifier")]
        public string RegionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include unpublished contents.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include unpublished contents; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 40, Name = "includeUnpublished")]
        public bool IncludeUnpublished { get; set; }
    }
}