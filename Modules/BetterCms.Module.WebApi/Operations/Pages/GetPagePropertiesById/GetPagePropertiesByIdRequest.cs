using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetPagePropertiesById
{
    [DataContract]
    public class GetPagePropertiesByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        [DataMember(Order = 10, Name = "pageId")]
        public string PageId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include tags.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include tags; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "includeTags")]
        public bool IncludeTags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include category.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include category; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 30, Name = "includeCategory")]
        public bool IncludeCategory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include layout.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include layout; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 40, Name = "includeLayout")]
        public bool IncludeLayout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include image.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include image; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 50, Name = "includeImages")]
        public bool IncludeImages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include meta data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include meta data; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 60, Name = "includeMetaData")]
        public bool IncludeMetaData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include page contents.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include page contents; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 70, Name = "includePageContents")]
        public bool IncludePageContents { get; set; }
    }
}