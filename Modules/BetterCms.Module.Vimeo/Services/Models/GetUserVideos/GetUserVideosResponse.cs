namespace BetterCms.Module.Vimeo.Services.Models.GetUserVideos
{
    internal class GetUserVideosResponse : VimeoResponseBase
    {
        public VideoList Videos { get; set; }
    }
}