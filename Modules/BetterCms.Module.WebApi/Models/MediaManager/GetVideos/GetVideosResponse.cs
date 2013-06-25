using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager.GetVideos
{
    [DataContract]
    public class GetVideosResponse : ListResponseBase<MediaModel>
    {
    }
}