using System;
using System.Text;
using System.Web.Script.Serialization;

using BetterCms.Core.Services.Caching;
using BetterCms.Module.Vimeo.Services.Models;
using BetterCms.Module.Vimeo.Services.Models.CheckAccessToken;
using BetterCms.Module.Vimeo.Services.Models.GetUserVideos;
using BetterCms.Module.Vimeo.Services.Models.GetVideo;
using BetterCms.Module.Vimeo.Services.Models.SearchVideo;
using BetterCms.Module.Vimeo.Services.OAuth;

using Common.Logging;

namespace BetterCms.Module.Vimeo.Services
{
    internal class DefaultVimeoService : IVimeoService
    {
        private const string CacheKey = "CMS_VIMEO_USER_ID";

        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly ICacheService cacheService;

        public DefaultVimeoService(ICmsConfiguration cmsConfiguration, ICacheService cacheService)
        {
            this.cmsConfiguration = cmsConfiguration;
            this.cacheService = cacheService;
        }

        public string GetCurrentUserId()
        {
            return cmsConfiguration.Cache.Enabled
                ? cacheService.Get(CacheKey, cmsConfiguration.Cache.Timeout, () => GetCurrentAuthentication().User.Id)
                : GetCurrentAuthentication().User.Id;
        }

        private Models.OAuth GetCurrentAuthentication()
        {
            var request = new CheckAccessTokenRequest();
            var response = LoadResults<CheckAccessTokenResponse>(request);
            if (response != null && response.IsOK())
            {
                return response.OAuth;
            }

            throw new Exception("Failed to check access token.");
        }

        public VideoList GetUserVideos(string userId, int pageNumber, int itemsPerPage)
        {
            var request = new GetUserVideosRequest(userId, VimeoSorting.Default, pageNumber, itemsPerPage);
            var response = LoadResults<GetUserVideosResponse>(request);
            if (response != null && response.IsOK())
            {
                return response.Videos;
            }
            throw new Exception(string.Format("Failed to get videos for user {0}.", userId));
        }

        public VideoList SearchVideo(string search, string userId, int pageNumber, int itemsPerPage)
        {
            var request = new SearchVideoRequest(search, userId, VimeoSorting.Default, pageNumber, itemsPerPage);
            var response = LoadResults<SearchVideoResponse>(request);
            if (response != null && response.IsOK())
            {
                return response.Videos;
            }
            throw new Exception(string.Format("Failed to get videos by search '{0}' for user {1}.", search, userId));
        }

        public Video GetVideo(string videoId)
        {
            var request = new GetVideoRequest(videoId);
            var response = LoadResults<GetVideoResponse>(request);
            if (response != null && response.IsOK() && response.Video != null && response.Video.Length > 0)
            {
                return response.Video[0];
            }
            throw new Exception(string.Format("Failed to get video by id '{0}'.", videoId));
        }

        public string GetPlayerUrl(string videoId)
        {
            return string.Format("http://player.vimeo.com/video/{0}", videoId);
        }

        public string GetVideoUrl(string videoId)
        {
            return string.Format("https://vimeo.com/{0}", videoId);
        }

        private static T LoadResults<T>(VimeoRequestBase request) where T : VimeoResponseBase
        {
            var url = request.GetUrl();
            var oauth = new VimeoOAuth();
            var client = new System.Net.WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.Headers.Add("Authorization", oauth.GetAuthorization(url));

            var result = string.Empty;
            try
            {
                result = client.DownloadString(url);
                var response = new JavaScriptSerializer().Deserialize<T>(result);

                if (response.IsOK())
                {
                    return response;
                }

                throw new Exception();
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to get data.", ex);
            }

            var message = new StringBuilder();
            message.AppendFormat("Failed to load results from URL {0}.", url);
            message.AppendLine();
            message.AppendLine("Result:");
            message.AppendLine(result);
            Logger.Error(message.ToString());

            return default(T);
        }
    }
}