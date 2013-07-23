using System.Runtime.Serialization;

using BetterCms.Module.Api.Operations.Enums;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    [DataContract]
    public class GetPagesModel : DataOptions, IFilterByTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagesModel" /> class.
        /// </summary>
        public GetPagesModel()
        {
            FilterByTagsConnector = FilterConnector.And;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived pages; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include unpublished pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include unpublished pages; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeUnpublished { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember]
        public System.Collections.Generic.List<string> FilterByTags { get; set; }

        /// <summary>
        /// Gets or sets the tags filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        [DataMember]
        public FilterConnector FilterByTagsConnector { get; set; }        
    }
}