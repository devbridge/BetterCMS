using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets
{
    [Route("/widgets", Verbs = "GET")]
    public class GetWidgetsRequest : ListRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetWidgetsRequest" /> class.
        /// </summary>
        public GetWidgetsRequest()
        {
            IncludeUnpublished = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include unpublished blog posts.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include unpublished blog posts; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeUnpublished { get; set; }
    }
}