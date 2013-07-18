using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

using BetterCms.Core.Exceptions.Service;
using BetterCms.Module.Viddler.Services.Models;

namespace BetterCms.Module.Viddler.Services
{
    public class DefaultViddlerService : IViddlerService
    {
        private const string apiUrl = "http://api.viddler.com/api/v2/";

        private const string playerUrl = "http://viddler.com/embed/{0}";

        private const string videoUrl = "http://www.viddler.com/v/{0}";

        private const string ResponseFormat = "xml";

        private readonly string apiKey = string.Empty;

        private readonly string userName = string.Empty;

        private readonly string password = string.Empty;

        public DefaultViddlerService(ICmsConfiguration cmsConfiguration)
        {
            try
            {
#if DEBUG
                apiKey = Environment.GetEnvironmentVariable("BETTERCMS_VIDDLER_API_KEY", EnvironmentVariableTarget.Machine);
                userName = Environment.GetEnvironmentVariable("BETTERCMS_VIDDLER_USERNAME", EnvironmentVariableTarget.Machine);
                password = Environment.GetEnvironmentVariable("BETTERCMS_VIDDLER_PASSWORD", EnvironmentVariableTarget.Machine);
#else
                var serviceSection = cmsConfiguration.Video;
                apiKey = serviceSection.GetValue("ViddlerApiKey");
                userName = serviceSection.GetValue("ViddlerUserName");
                password = serviceSection.GetValue("ViddlerPassword");
#endif
                if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                {
                    throw new ConfigurationErrorsException("Configuration for Viddler not found.");
                }

            }
            catch (Exception e)
            {
                throw new VideoProviderException(string.Format("Failed to initialize service {0}.", GetType()), e);
            }
        }

        /// <summary>
        /// Gets the player URL.
        /// </summary>
        /// <param name="videoId">The video id.</param>
        /// <returns>
        /// URL for using in iFrame.
        /// </returns>
        public string GetPlayerUrl(string videoId)
        {
            return string.Format(playerUrl, videoId);
        }

        /// <summary>
        /// Gets the video URL.
        /// </summary>
        /// <param name="videoId">The video id.</param>
        /// <returns>
        /// URL to access video on the website.
        /// </returns>
        public string GetVideoUrl(string videoId)
        {
            return string.Format(videoUrl, videoId);
        }

        /// <summary>
        /// Gets the session id.
        /// </summary>
        /// <returns>Session id.</returns>
        public string GetSessionId()
        {
            var queryString = GetQueryString(
                "viddler.users.auth", new Dictionary<string, string> { { "key", apiKey }, { "user", userName }, { "password", password } });
            var response = RunApiGetCall(apiUrl, queryString, new List<string> { "sessionid" });
            return response["sessionid"].ToString();
        }

        /// <summary>
        /// Gets the upload data.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns>Data for video upload.</returns>
        /// <exception cref="VideoProviderException">Failed to get video upload data.</exception>
        public Upload GetUploadData(string sessionId)
        {
            try
            {
                var queryString = GetQueryString("viddler.videos.prepareUpload", new Dictionary<string, string> { { "key", apiKey }, { "sessionid", sessionId } });
                var values = RunApiGetCall(apiUrl, queryString, new List<string> { "endpoint", "token" });
                return new Upload
                {
                    SessionId = sessionId,
                    Token = values["token"].ToString(),
                    Endpoint = values["endpoint"].ToString(),
                };
            }
            catch (Exception ex)
            {
                throw new VideoProviderException("Failed to get video upload data.", ex);
            }
        }

        /// <summary>
        /// Gets the video details.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="videoId">The video id.</param>
        /// <returns>Video details.</returns>
        /// <exception cref="VideoProviderException">Failed to get video details.</exception>
        public Video GetVideoDetails(string sessionId, string videoId)
        {
            try
            {
                var queryString = GetQueryString(
                    "viddler.videos.getDetails", new Dictionary<string, string> { { "key", apiKey }, { "sessionid", sessionId }, { "video_id", videoId } });
                var values = RunApiGetCall(apiUrl, queryString, new List<string> { "title", "description", "url", "thumbnail_url", "length", "view_count", "status" });
                return new Video
                {
                    Id = videoId,
                    Title = values["title"].ToString(),
                    Description = values["description"].ToString(),
                    Url = values["url"].ToString(),
                    ThumbnailUrl = values["thumbnail_url"].ToString(),
                    Length = long.Parse(values["length"].ToString()),
                    ViewCount = long.Parse(values["view_count"].ToString()),
                    IsReady = values["status"].ToString().ToLowerInvariant() == "ready",
                };
            }
            catch (Exception ex)
            {
                throw new VideoProviderException("Failed to get video details.", ex);
            }
        }

        /// <summary>
        /// Makes the video public.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="videoId">The video id.</param>
        /// <param name="makePublic">if set to <c>true</c> to make public or <c>false</c> to make private.</param>
        /// <returns><c>true</c> if video is public, <c>false</c> otherwise.</returns>
        /// <exception cref="VideoProviderException">Failed to change video privacy.</exception>
        public bool MakeVideoPublic(string sessionId, string videoId, bool makePublic)
        {
            try
            {
                var queryString = GetQueryString(
                    "viddler.videos.setDetails",
                    new Dictionary<string, string>
                        {
                            { "key", apiKey },
                            { "sessionid", sessionId },
                            { "video_id", videoId },
                            { "view_perm", makePublic ? "embed" : "private" }
                        });

                var response = RunApiPostCall(apiUrl, queryString, new List<string> { "permissions" });

                var retVal = false;
                var t = response["permissions"] as string[];
                if (t != null)
                {
                    retVal = t[0] == (makePublic ? "embed" : "private");
                }

                return retVal;
            }
            catch (Exception ex)
            {
                throw new VideoProviderException("Failed to change video privacy.", ex);
            }
        }

        /// <summary>
        /// Removes the video.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="videoId">The video id.</param>
        /// <returns><c>true</c> if successfully removed, <c>false</c> otherwise.</returns>
        /// <exception cref="VideoProviderException">Failed to remove video.</exception>
        public bool RemoveVideo(string sessionId, string videoId)
        {
            try
            {
                var queryString = GetQueryString(
                    "viddler.videos.delete", new Dictionary<string, string> { { "api_key", apiKey }, { "sessionid", sessionId }, { "video_id", videoId } });

                var response = RunApiPostCall(apiUrl, queryString, new List<string> { "success" });

                return (bool)response["success"];
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse resp = ex.Response as HttpWebResponse;
                    if (resp != null && resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        // 404 error - video does not exist.
                        return true;
                    }
                }

                throw;
            }
            catch (Exception ex)
            {
                throw new VideoProviderException("Failed to remove video.", ex);
            }
        }

        /// <summary>
        /// Gets the uploading status.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="token">The token.</param>
        /// <returns>Percentage video upload status.</returns>
        /// <exception cref="VideoProviderException">Failed to get video uploading status.</exception>
        public int GetUploadingStatus(string sessionId, string token)
        {
            try
            {
                var queryString = GetQueryString(
                    "viddler.videos.uploadProgress", new Dictionary<string, string> { { "key", apiKey }, { "sessionid", sessionId }, { "token", token } });

                var response = RunApiGetCall(apiUrl, queryString, new List<string> { "percent" });

                return Convert.ToInt32(Math.Round(Convert.ToDouble(response["percent"])));
            }
            catch (Exception ex)
            {
                throw new VideoProviderException("Failed to get video uploading status.", ex);
            }
        }

        #region Private

        private Dictionary<string, object> RunApiPostCall(string uri, string queryString, List<string> responseFields)
        {
            var request = WebRequest.Create(uri + queryString) as HttpWebRequest;
            request.Method = "POST";
            var response = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
            return GetResponseValueDictionary(response, responseFields);
        }

        private Dictionary<string, object> RunApiGetCall(string uri, string queryString, List<string> responseFields)
        {
            var request = WebRequest.Create(uri + queryString) as HttpWebRequest;
            request.Method = "GET";
            var response = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
            return GetResponseValueDictionary(response, responseFields);
        }

        private string GetQueryString(string method, Dictionary<string, string> vals)
        {
            var result = string.Format("{0}.{1}", method, ResponseFormat);
            var first = true;
            foreach (var i in vals)
            {
                string seperator;
                if (first)
                {
                    seperator = "?";
                    first = false;
                }
                else
                {
                    seperator = "&";
                }

                result += string.Format("{0}{1}={2}", seperator, i.Key, i.Value);
            }

            return result;
        }

        private Dictionary<string, object> GetResponseValueDictionary(string response, List<string> responseFields)
        {
            var xml = XDocument.Parse(response);
            var result = new Dictionary<string, object>();
            foreach (var key in responseFields)
            {
                var firstOrDefault = xml.Descendants(key).FirstOrDefault();
                if (firstOrDefault != null)
                {
                    switch (key)
                    {
                        case "tags":
                            result.Add(key, firstOrDefault.Elements("tag").Elements("text").Select(c => c.Value).ToArray());
                            break;
                        case "permissions":
                            result.Add(key, firstOrDefault.Elements("view").Elements("level").Select(c => c.Value).ToArray());
                            break;
                        case "success":
                            result.Add(key, string.IsNullOrEmpty(firstOrDefault.Value));
                            break;
                        default:
                            result.Add(key, firstOrDefault.Value);
                            break;
                    }
                }
            }

            return result;
        }

        #endregion
    }
}