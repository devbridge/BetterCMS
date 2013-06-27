using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetPagePropertiesById
{
    [DataContract]
    public class PageContentModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The content id.
        /// </value>
        [DataMember(Order = 10, Name = "contentId")]
        public System.Guid ContentId { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        [DataMember(Order = 20, Name = "contentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the content name.
        /// </summary>
        /// <value>
        /// The content name.
        /// </value>
        [DataMember(Order = 30, Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        /// <value>
        /// The region id.
        /// </value>
        [DataMember(Order = 40, Name = "regionId")]
        public System.Guid RegionId { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        [DataMember(Order = 50, Name = "regionIdentifier")]
        public string RegionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the content order number in the region.
        /// </summary>
        /// <value>
        /// The content order number in the region.
        /// </value>
        [DataMember(Order = 60, Name = "order")]
        public int Order { get; set; }
    }
}