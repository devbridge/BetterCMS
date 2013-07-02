using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.History
{
    [Route("/contents/{ContentId}/history", Verbs = "GET")]
    public class GetContentHistoryRequest : ListRequestBase, IReturn<GetContentHistoryResponse>
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The content id.
        /// </value>
        public System.Guid ContentId { get; set; }
    }
}