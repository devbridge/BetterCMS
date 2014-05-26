using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Search
{
    [DataContract]
    [Serializable]
    public class SearchResultModel
    {
        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>
        /// The page title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the page link.
        /// </summary>
        /// <value>
        /// The page link.
        /// </value>
        [DataMember]
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the formatted page URL.
        /// </summary>
        /// <value>
        /// The formatted page URL.
        /// </value>
        [DataMember]
        public string FormattedUrl { get; set; }

        /// <summary>
        /// Gets or sets the page snippet.
        /// </summary>
        /// <value>
        /// The page snippet.
        /// </value>
        [DataMember]
        public string Snippet { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether page is denied to current principal.
        /// </summary>
        /// <value>
        ///   <c>true</c> if page is denied to current principal; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsDenied { get; set; }
    }
}