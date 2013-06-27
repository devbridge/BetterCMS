using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.GetVideos
{
    [DataContract]
    public class GetVideosResponse : ListResponseBase<MediaModel>
    {
    }
}