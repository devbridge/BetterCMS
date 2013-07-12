using BetterCms.Module.Vimeo.Services.Models;

namespace BetterCms.Module.Vimeo.Services
{
    public interface IVimeoService
    {
        string GetCurrentUserId();
        VideoList GetUserVideos(string userId, int pageNumber, int itemsPerPage);
        VideoList SearchVideo(string search, string userId, int pageNumber, int itemsPerPage);
        Video GetVideo(string videoId);

        string GetPlayerUrl(string videoId);
        string GetVideoUrl(string videoId);
    }
}