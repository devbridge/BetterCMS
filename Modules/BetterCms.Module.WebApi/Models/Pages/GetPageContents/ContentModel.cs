using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetPageContents
{
    [DataContract]
    public class ContentModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the content name.
        /// </summary>
        /// <value>
        /// The content name.
        /// </value>
        [DataMember(Order = 10, Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        /// <value>
        /// The region id.
        /// </value>
        [DataMember(Order = 10, Name = "regionId")]
        public System.Guid RegionId { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        [DataMember(Order = 10, Name = "regionIdentifier")]
        public string RegionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the content order number in the region.
        /// </summary>
        /// <value>
        /// The content order number in the region.
        /// </value>
        [DataMember(Order = 10, Name = "order")]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the content HTML.
        /// </summary>
        /// <value>
        /// The content HTML.
        /// </value>
        [DataMember(Order = 10, Name = "html")]
        public string Html { get; set; }

        /// <summary>
        /// Gets or sets the content custom CSS.
        /// </summary>
        /// <value>
        /// The content custom CSS.
        /// </value>
        [DataMember(Order = 10, Name = "customCss")]
        public string CustomCss { get; set; }

        /// <summary>
        /// Gets or sets the content custom java script.
        /// </summary>
        /// <value>
        /// The content custom java script.
        /// </value>
        [DataMember(Order = 10, Name = "customJavaScript")]
        public string CustomJavaScript { get; set; }
    }
}