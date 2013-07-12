using System;
using System.Configuration;
using System.Text;
using System.Web;

namespace BetterCms.Module.Vimeo.Services.OAuth
{
    internal class VimeoOAuth: OAuthBase
    {
        private string consumerKey = "";
        private string consumerSecret = "";
        private string accessToken = "";
        private string accessTokenSecret = "";

        public string GetAuthorization(string url)
        {
#if DEBUG
            consumerKey = Environment.GetEnvironmentVariable("BETTERCMS_VIMEO_CONSUMER_KEY", EnvironmentVariableTarget.Machine);
            consumerSecret = Environment.GetEnvironmentVariable("BETTERCMS_VIMEO_CONSUMER_SECRET", EnvironmentVariableTarget.Machine);
            accessToken = Environment.GetEnvironmentVariable("BETTERCMS_VIMEO_ACCESS_TOKEN", EnvironmentVariableTarget.Machine);
            accessTokenSecret = Environment.GetEnvironmentVariable("BETTERCMS_VIMEO_ACCESS_TOKEN_SECRET", EnvironmentVariableTarget.Machine);
#else
            // TODO: implement credentials loading.
#endif

            if (string.IsNullOrEmpty(consumerKey) || string.IsNullOrEmpty(consumerSecret) || string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(accessTokenSecret))
            {
                throw new ConfigurationErrorsException("Configuration for Vimeo OAuth not found.");
            }

            var oauth = new VimeoOAuth();

            var nonce = oauth.GenerateNonce();
            var timeStamp = oauth.GenerateTimeStamp();

            string normalizedUrl;
            string normalizedRequestParameters;

            var uri = new Uri(url);
            var signature = oauth.GenerateSignature(
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