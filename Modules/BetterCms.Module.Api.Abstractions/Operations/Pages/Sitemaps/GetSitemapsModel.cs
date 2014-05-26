using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps
{
    /// <summary>
    /// Data model for getting sitemaps list.
    /// </summary>
    [DataContract]
    [Serializable]
    public class GetSitemapsModel : DataOptions, IFilterByTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSitemapsModel" /> class.
        /// </summary>
        public GetSitemapsModel()
        {
            FilterByTagsConnector = FilterConnector.And;
        }

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

        /// <summary>
        /// Gets or sets a value indicating whether to include tags collections to results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if  to include tags collections to results; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeTags { get; set; }
    }
}