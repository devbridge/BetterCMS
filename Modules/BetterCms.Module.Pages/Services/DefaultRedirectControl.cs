using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Services;
using BetterCms.Module.Pages.Models;

using BetterModules.Core.Web.Services.Caching;
using BetterModules.Events;

namespace BetterCms.Module.Pages.Services
{
    /// <summary>
    /// Class responsible for controlling redirect strategy
    /// </summary>
    public class DefaultRedirectControl : IRedirectControl
    {
        private readonly IRedirectService redirectService;

        private readonly ICacheService cacheService;

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly IUrlService urlService;

        private string cacheKey = "CMS_redirect_control_all";

        public DefaultRedirectControl(IRedirectService redirectService, ICacheService cacheService, ICmsConfiguration cmsConfiguration, IUrlService urlService)
        {
            this.redirectService = redirectService;
            this.cacheService = cacheService;
            this.cmsConfiguration = cmsConfiguration;
            this.urlService = urlService;

            Events.PageEvents.Instance.RedirectCreated += InvalidateCache;
            Events.PageEvents.Instance.RedirectUpdated += InvalidateCache;
            Events.PageEvents.Instance.RedirectDeleted += InvalidateCache;
        }

        /// <summary>
        /// Finds the redirect.
        /// </summary>
        /// <param name="source">The source url.</param>
        /// <returns>
        /// Destination url
        /// </returns>
        public string FindRedirect(string source)
        {
            string redirectDestinationUrl = null;
            var useCache = cmsConfiguration.Cache.Enabled;
            if (urlService.ValidateInternalUrl(source))
            {
                source = urlService.FixUrl(source);
            }
            if (useCache)
            {
                var redirects = cacheService.Get(cacheKey, cmsConfiguration.Cache.Timeout, () => redirectService.GetAllRedirects());
                redirectDestinationUrl = redirects.Where(x => x.PageUrl.Equals(source, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.RedirectUrl).FirstOrDefault();
            }
            else
            {
                return redirectService.GetRedirect(source);
            }

            return redirectDestinationUrl;
        }

        private void InvalidateCache(SingleItemEventArgs<Redirect> args)
        {
            cacheService.Remove(cacheKey);
        }
    }
}