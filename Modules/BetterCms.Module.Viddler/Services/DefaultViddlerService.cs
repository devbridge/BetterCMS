using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

using BetterCms.Core.Exceptions.Service;

namespace BetterCms.Module.Viddler.Services
{
    public class DefaultViddlerService : IViddlerService
    {
        private const string apiUrl = "http://api.viddler.com/api/v2/";

        private const string playerUrl = "http://viddler.com/embed/{0}";

        private const string videoUrl = "http://www.viddler.com/v/{0}";

        private readonly string apiKey = string.Empty;

        private readonly string userName = string.Empty;

        private readonly string password = string.Empty;

        private const string ResponseFormat = "xml";

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
            var queryString = GetQueryString(
                "viddler.users.auth", new Dictionary<string, string> { { "key", apiKey }, { "user", userName }, { "password", password } });
            var response = RunApiGetCall(apiUrl, queryString, new List<string> { "sessionid" });
            return response["sessionid"].ToString();
        }

        public Dictionary<string, object> GetDataForVideoUpload(string sessionId)
        {
            try
            {
                var queryString = GetQueryString("viddler.videos.prepareUpload", new Dictionary<string, string> { { "key", apiKey }, { "sessionid", sessionId } });

                return RunApiGetCall(apiUrl, queryString, new List<string> { "endpoint", "token" });
            }
            catch (Exception ex)
            {
                throw new VideoProviderException("Failed to get data for video upload.", ex);
            }
        }

        public Dictionary<string, object> GetVideoDetails(string videoId)
        {
            try
            {
                var sessionId = GetSessionId();
                var queryString = GetQueryString(
                    "viddler.videos.getDetails", new Dictionary<string, string> { { "key", apiKey }, { "sessionid", sessionId }, { "video_id", videoId } });

                return RunApiGetCall(apiUrl, queryString, new List<string> { "title", "description", "url", "html5_video_source", "tags", "thumbnail_url", "status" });
            }
            catch (Exception ex)
            {
                throw new VideoProviderException("Failed to get video details.", ex);
            }
        }

        public bool MakeVideoPublic(string videoId, bool makePublic)
        {
            try
            {
                var sessionId = GetSessionId();
                var queryString = GetQueryString(
                    "viddler.videos.setDetails",
                    new Dictionary<string, string>
                        {
                            { "key", apiKey },
                            { "sessionid", sessionId },
                            { "video_id", videoId },
                            { "view_perm", makePublic ? "public" : "private" }
                        });

                var response = RunApiPostCall(apiUrl, queryString, new List<string> { "permissions" });

                var retVal = false;
                var t = response["permissions"] as string[];
                if (t != null)
                {
                    retVal = t[0] == (makePublic ? "public" : "private");
                }
                return retVal;
            }
            catch (Exception ex)
            {
                throw new VideoProviderException("Failed to make video public.", ex);
            }
        }

        public bool RemoveVideo(string videoId)
        {
            try
            {
                var sessionId = GetSessionId();
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
            var seperator = string.Empty;
            foreach (var i in vals)
            {
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