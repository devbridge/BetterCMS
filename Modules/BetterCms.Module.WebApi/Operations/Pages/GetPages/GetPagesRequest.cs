using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetPages
{
    [DataContract]
    public class GetPagesRequest : ListRequestBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include archived pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived pages; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 10, Name = "includeArchived")]
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include unpublished pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include unpublished pages; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "includeUnpublished")]
        public bool IncludeUnpublished { get; set; }
    }
}