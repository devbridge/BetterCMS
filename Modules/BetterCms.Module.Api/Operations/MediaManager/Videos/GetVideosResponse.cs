using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.Videos
{
    [DataContract]
    public class GetVideosResponse : ListResponseBase<MediaModel>
    {
    }
}