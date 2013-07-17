using System;

using BetterCms.Module.Viddler.Services.Models;

namespace BetterCms.Module.Viddler.Services
{
    public class MockupViddlerService : IViddlerService
    {
        private const string playerUrl = "http://viddler.com/embed/{0}";

        private const string videoUrl = "http://www.viddler.com/v/{0}";

        public string GetPlayerUrl(string videoId)
        {
            return string.Format(playerUrl, videoId);
        }

        public string GetVideoUrl(string videoId)
        {
            return string.Format(videoUrl, videoId);
        }

        public string GetSessionId()
        {
            return "f6752122_f6752122";
        }

        public Upload GetUploadData(string sessionId)
        {
            return new Upload
                {
                    SessionId = sessionId,
                    Token = "u81989c2a3eef4531b0c94db25b07f0b2b0d9fcda",
                    // Endpoint = "http://www.viddler.com/uploadnode/upload/uploadvideo"
                    Endpoint = "http://bettercms.sandbox.mvc4.local/bcms-viddler/Videos/VideoUploaded?video_id=f6752122"
                };
        }

        public Video GetVideoDetails(string sessionId, string videoId)
        {
            return new Video
                {
                    Id = videoId,
                    Description = "Some description is here.",
                    IsReady = true,
                    Length = 125,
                    ThumbnailUrl = "http://thumbs.cdn-ec.viddler.com/thumbnail_2_f6752122_v5.jpg",
                    Title = "Adaptive Bitrate",
                    Url = GetPlayerUrl(videoId),
                    ViewCount = 5312
                };
        }

        public bool MakeVideoPublic(string sessionId, string videoId, bool makePublic)
        {
            return makePublic;
        }

        public bool RemoveVideo(string sessionId, string videoId)
        {
            return new Random().Next(100) % 2 == 0;
        }

        public int GetUploadingStatus(string sessionId, string token)
        {
            return new Random().Next(100);
        }
    }
}