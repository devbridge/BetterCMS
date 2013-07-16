using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    [Route("/contents/html/{ContentId}", Verbs = "GET")]
    public class GetHtmlContentRequest : RequestBase, IReturn<GetHtmlContentResponse>
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