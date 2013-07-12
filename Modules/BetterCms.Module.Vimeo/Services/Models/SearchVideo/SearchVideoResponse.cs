namespace BetterCms.Module.Vimeo.Services.Models.GetUserVideos
{
    internal class SearchVideoResponse : VimeoResponseBase
    {
        public VideoList Videos { get; set; }
    }
}