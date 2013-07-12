using System.Collections.Generic;

namespace BetterCms.Module.Viddler.Services
{
    internal interface IViddlerService
    {
        string GetPlayerUrl(string videoId);

        string GetVideoUrl(string videoId);

        string GetSessionId();

        Dictionary<string, object> GetDataForVideoUpload(string sessionId);

        Dictionary<string, object> GetVideoDetails(string videoId);

        bool MakeVideoPublic(string videoId, bool makePublic);

        bool RemoveVideo(string videoId);

        int GetUploadingStatus(string sessionId, string token);
    }
}