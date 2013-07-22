using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.MediaTree
{
    [Route("/media-tree", Verbs = "GET")]
    public class GetMediaTreeRequest : RequestBase<GetMediaTreeModel>, IReturn<GetMediaTreeResponse>
    {
    }
}