using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Videos.Video
{
    public class VideoService : Service, IVideoService
    {
        public GetVideoResponse Get(GetVideoRequest request)
        {
            // TODO: implement
            return new GetVideoResponse
            {
                Data = new VideoModel()
            };
        }
    }
}