using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents
{
    [DataContract]
    public class GetPageContentsModel : DataOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetPageContentsModel" /> class.
        /// </summary>
        public GetPageContentsModel()
        {
            FieldExceptions.Add("ContentType");
        }

        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        [DataMember]
        public System.Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        /// <value>
        /// The region id.
        /// </value>
        [DataMember]
        public System.Guid? RegionId { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        [DataMember]
        public string RegionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include unpublished contents.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include unpublished contents; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeUnpublished { get; set; }
    }
}