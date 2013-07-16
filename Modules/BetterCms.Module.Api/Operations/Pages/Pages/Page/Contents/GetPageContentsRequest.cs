using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents
{
    [Route("/pages/{PageId}/contents")]
    public class GetPageContentsRequest : ListRequestBase, IReturn<GetPageContentsResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetPageContentsRequest" /> class.
        /// </summary>
        public GetPageContentsRequest()
        {
            FieldExceptions.Add("ContentType");
        }

        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public System.Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        /// <value>
        /// The region id.
        /// </value>
        public System.Guid? RegionId { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        public string RegionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include unpublished contents.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include unpublished contents; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeUnpublished { get; set; }
    }
}