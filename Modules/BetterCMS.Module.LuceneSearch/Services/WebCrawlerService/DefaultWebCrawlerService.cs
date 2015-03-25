using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

using BetterCMS.Module.LuceneSearch.Helpers;

using BetterCms;
using BetterCms.Configuration;

using Common.Logging;

using HtmlAgilityPack;

namespace BetterCMS.Module.LuceneSearch.Services.WebCrawlerService
{
    public class DefaultWebCrawlerService : IWebCrawlerService
    {
        private enum AuthMode
        {
            None,
            Forms,
            Windows
        }

        private static readonly ILog Log = LogManager.GetLogger(LuceneSearchConstants.LuceneSearchModuleLoggerNamespace);

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly string webServer;

        private readonly bool indexPrivatePages;

        private readonly AuthMode authMode = AuthMode.None;

        private readonly TimeSpan fetchTimeout = TimeSpan.FromMinutes(1);

        private CookieCollection authorizationCookies;

        public DefaultWebCrawlerService(ICmsConfiguration cmsConfiguration)
        {
            this.cmsConfiguration = cmsConfiguration;

            webServer = cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneWebSiteUrl) ?? string.Empty;

            bool.TryParse(cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneIndexPrivatePages), out indexPrivatePages);

            if (indexPrivatePages)
            {
                var authModeString = cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneAuthorizationMode);
                if (!string.IsNullOrWhiteSpace(authModeString))
                {
                    switch (authModeString.ToLower().Trim())
                    {
                        case "windows":
                            authMode = AuthMode.Windows;
                            break;
                        default:
                            authMode = AuthMode.Forms;
                            break;
                    }
                }
            }

            HtmlAgilityPackHelper.FixMissingTagClosings();

            TimeSpan timeout;
            if (TimeSpan.TryParse(cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneIndexerPageFetchTimeout), out timeout)
                && timeout > TimeSpan.FromSeconds(0))
            {
                fetchTimeout = timeout;
            }
        }

        public PageData FetchPage(string url)
        {
            if (indexPrivatePages && authorizationCookies == null && authMode != AuthMode.Windows)
            {
                TryAuthenticate(); // Forms authentication.
            }

            var fullUrl = string.Concat(webServer.TrimEnd('/'), "/", url.TrimStart('/'));
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(fullUrl);

            if (indexPrivatePages && authMode == AuthMode.Windows)
            {
                var userName = cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneAuthorizationWindows_UserName);
                var password = cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneAuthorizationWindows_Password);
                httpWebRequest.Credentials = new NetworkCredential(userName, password);
            }

            httpWebRequest.AllowAutoRedirect = true;
            httpWebRequest.Timeout = (int)fetchTimeout.TotalMilliseconds;
            httpWebRequest.CookieContainer = new CookieContainer();

            if (authorizationCookies != null)
            {
                foreach (Cookie authCookie in authorizationCookies)
                {
                    Cookie cookie = new Cookie(authCookie.Name, authCookie.Value, authCookie.Path, authCookie.Domain);
                    httpWebRequest.CookieContainer.Add(cookie);
                }
            }

            HttpWebResponse httpWebResponse = null;
            var response = new PageData();
            response.AbsoluteUri = httpWebRequest.RequestUri.AbsoluteUri;
            response.AbsolutePath = httpWebRequest.RequestUri.AbsolutePath;

            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                response.StatusCode = httpWebResponse.StatusCode;
                response.AbsolutePath = httpWebResponse.ResponseUri.AbsolutePath;
                response.AbsoluteUri = httpWebResponse.ResponseUri.AbsoluteUri;

                using (Stream responseStream = httpWebResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var streamReader = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            response.Content = new HtmlDocument();
                            response.Content.LoadHtml(streamReader.ReadToEnd());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Lucene web crawler: Failed to fetch page by url {0}.", ex, url);

                if (ex.GetType() == typeof(WebException))
                {
                    var webException = (WebException)ex;
                    response.StatusCode = ((HttpWebResponse)webException.Response).StatusCode;
                }
            }
            finally
            {
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
            }

            return response;
        }

        public bool IsConfigured(out string message)
        {
            if (string.IsNullOrWhiteSpace(webServer))
            {
                message = "LuceneWebSiteUrl is not set in the cms.config's search section.";
                return false;
            }

            message = null;
            return true;
        }

        private bool TryAuthenticate()
        {
            var url = cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneAuthorizationUrl);
            if (string.IsNullOrWhiteSpace(url))
            {
                authorizationCookies = new CookieCollection();
                Log.ErrorFormat("Lucene web crawler: failed to authenticate user: url is not set in lucene configuration.");

                return false;
            }

            authorizationCookies = null;
            string parameters = "";
            var prefixLength = LuceneSearchConstants.ConfigurationKeys.LuceneAuthorizationFormFieldPrefix.Length;
            foreach (KeyValueElement element in (ConfigurationElementCollection)cmsConfiguration.Search)
            {
                if (!element.Key.StartsWith(LuceneSearchConstants.ConfigurationKeys.LuceneAuthorizationFormFieldPrefix))
                {
                    continue;
                }
                if (!string.IsNullOrWhiteSpace(parameters))
                {
                    parameters = string.Concat(parameters, "&");
                }
                parameters = string.Format("{0}{1}={2}", parameters, element.Key.Substring(prefixLength, element.Key.Length - prefixLength), HttpUtility.UrlEncode(element.Value));
            }

            HttpWebRequest requestLogin = (HttpWebRequest)WebRequest.Create(url);
            requestLogin.Method = "POST";
            requestLogin.CookieContainer = new CookieContainer();
            requestLogin.ContentType = "application/x-www-form-urlencoded";
            requestLogin.AllowAutoRedirect = false;
            requestLogin.ContentLength = parameters.Length;

            HttpWebResponse httpWebResponse = null;
            try
            {
                using (StreamWriter stOut = new StreamWriter(requestLogin.GetRequestStream(), Encoding.ASCII))
                {
                    stOut.Write(parameters);
                    stOut.Close();
                }

                httpWebResponse = (HttpWebResponse)requestLogin.GetResponse();
                var cookies = httpWebResponse.Cookies;
                authorizationCookies = cookies;

                foreach (Cookie cookie in authorizationCookies)
                {
                    cookie.Expires = DateTime.Now.AddYears(1);
                }
            }
            catch (Exception exc)
            {
                Log.ErrorFormat("Lucene web crawler: Failed to authenticate user.", exc);

                return false;
            }
            finally
            {
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
            }

            return true;
        }
    }
}
