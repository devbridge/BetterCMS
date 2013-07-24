using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Videos
{
    [DataContract]
    public class GetVideosResponse : ListResponseBase<MediaModel>
    {
    }
}