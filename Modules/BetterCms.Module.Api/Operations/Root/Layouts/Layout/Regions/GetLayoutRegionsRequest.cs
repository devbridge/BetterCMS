using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions
{
    [Route("/layouts/{LayoutId}/regions", Verbs = "GET")]
    public class GetLayoutRegionsRequest : ListRequestBase, IReturn<GetLayoutRegionsResponse>
    {
        /// <summary>
        /// Gets or sets the layout id.
        /// </summary>
        /// <value>
        /// The layout id.
        /// </value>
        public System.Guid LayoutId { get; set; }
    }
}