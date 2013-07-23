using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Videos.Video
{
    [Route("/videos/{VideoId}", Verbs = "GET")]
    [DataContract]
    public class GetVideoRequest : RequestBase<GetVideoModel>, IReturn<GetVideoResponse>
    {
    }
}