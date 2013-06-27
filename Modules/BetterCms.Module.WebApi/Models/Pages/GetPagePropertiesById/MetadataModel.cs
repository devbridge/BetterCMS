using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetPagePropertiesById
{
    [DataContract]
    public class MetadataModel
    {
        /// <summary>
        /// Gets or sets the page meta title.
        /// </summary>
        /// <value>
        /// The page meta title.
        /// </value>
        [DataMember(Order = 10, Name = "metaTitle")]
        public string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets the page meta keywords.
        /// </summary>
        /// <value>
        /// The page meta keywords.
        /// </value>
        [DataMember(Order = 20, Name = "metaKeywords")]
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the page meta description.
        /// </summary>
        /// <value>
        /// The page meta description.
        /// </value>
        [DataMember(Order = 30, Name = "metaDescription")]
        public string MetaDescription { get; set; }
    }
}