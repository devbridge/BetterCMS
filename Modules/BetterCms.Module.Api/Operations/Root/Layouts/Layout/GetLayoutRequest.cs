using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    public class GetLayoutModel
    {
        /// <summary>
        /// Gets or sets the layout id.
        /// </summary>
        /// <value>
        /// The layout id.
        /// </value>
        public System.Guid LayoutId { get; set; }
    }

    [Route("/layouts/{LayoutId}", Verbs = "GET")]
    public class GetLayoutRequest : RequestBase<GetLayoutModel>, IReturn<GetLayoutResponse>
    {
    }
}