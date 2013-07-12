using System;
using System.Configuration;
using System.Text;
using System.Web;

using BetterCms.Core.Exceptions.Service;
using BetterCms.Module.Vimeo.Services.OAuth;

namespace BetterCms.Module.Vimeo.Services
{
    internal class VimeoOAuthService : OAuthBase, IOAuthService
    {
        private string consumerKey = string.Empty;
        private string consumerSecret = string.Empty;
        private string accessToken = string.Empty;
        private string accessTokenSecret = string.Empty;

        public VimeoOAuthService(ICmsConfiguration cmsConfiguration)
        {
            try
            {
#if DEBUG
                consumerKey = Environment.GetEnvironmentVariable("BETTERCMS_VIMEO_CONSUMER_KEY", EnvironmentVariableTarget.Machine);
                consumerSecret = Environment.GetEnvironmentVariable("BETTERCMS_VIMEO_CONSUMER_SECRET", EnvironmentVariableTarget.Machine);
                accessToken = Environment.GetEnvironmentVariable("BETTERCMS_VIMEO_ACCESS_TOKEN", EnvironmentVariableTarget.Machine);
                accessTokenSecret = Environment.GetEnvironmentVariable("BETTERCMS_VIMEO_ACCESS_TOKEN_SECRET", EnvironmentVariableTarget.Machine);
#else
                var serviceSection = cmsConfiguration.Video;
                consumerKey = serviceSection.GetValue("VimeoConsumerKey");
                consumerSecret = serviceSection.GetValue("VimeoConsumerSecret");
                accessToken = serviceSection.GetValue("VimeoAccessToken");
                accessTokenSecret = serviceSection.GetValue("VimeoAccessTokenSecret");
#endif
                if (string.IsNullOrEmpty(consumerKey) || string.IsNullOrEmpty(consumerSecret) || string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(accessTokenSecret))
                {
                    throw new ConfigurationErrorsException("Configuration for Vimeo OAuth not found.");
                }

            }
            catch (Exception e)
            {
                throw new VideoProviderException(string.Format("Failed to initialize service {0}.", GetType()), e);
            }
        }

        public string GetAuthorizationProperty(string url)
        {
            var nonce = GenerateNonce();
            var timeStamp = GenerateTimeStamp();

            string normalizedUrl;
            string normalizedRequestParameters;

            var uri = new Uri(url);
            var signature = GenerateSignature(
                uri, consumerKey, consumerSecret, accessToken, accessTokenSecret, "GET", timeStamp, nonce, out normalizedUrl, out normalizedRequestParameters);

            signature = HttpUtility.UrlEncode(signature);

            var sb = new StringBuilder();

            sb.AppendFormat("OAuth realm=\"\",");
            sb.AppendFormat("oauth_consumer_key=\"{0}\",", consumerKey);
            sb.AppendFormat("oauth_version=\"{0}\",", "1.0");
            sb.AppendFormat("oauth_signature_method=\"{0}\",", "HMAC-SHA1");
            sb.AppendFormat("oauth_timestamp=\"{0}\",", timeStamp);
            sb.AppendFormat("oauth_nonce=\"{0}\",", nonce);
            sb.AppendFormat("oauth_token=\"{0}\",", accessToken);
            sb.AppendFormat("oauth_signature=\"{0}\",", signature);

            return sb.ToString();
        }
    }
}