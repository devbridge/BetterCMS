using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

using BetterCms.Configuration;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Attributes;
using BetterCms.Core.Security;
using BetterCms.Module.Root.Commands.GetPageToRender;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.ViewModels.Cms;

using Common.Logging;

using BetterModules.Core.Web.Services.Caching;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    [ActionLinkArea(RootModuleDescriptor.RootAreaName)]
    public class CmsController : CmsControllerBase
    {
        /// <summary>
        /// The configuration loader
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The cache service
        /// </summary>
        private readonly ICacheService cacheService;

        /// <summary>
        /// The cache service
        /// </summary>
        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsController" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The configuration loader.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="accessControlService">The access control service.</param>
        public CmsController(ICmsConfiguration cmsConfiguration, ICacheService cacheService, IAccessControlService accessControlService)
        {
            this.cmsConfiguration = cmsConfiguration;
            this.cacheService = cacheService;
            this.accessControlService = accessControlService;
        }

        /// <summary>
        /// Default entry point for all CMS pages.
        /// </summary>
        /// <returns>
        /// Returns page or redirect or page not found result.
        /// </returns>
        [IgnoreAutoRoute]
        public ActionResult Index()
        {
            var virtualPath = HttpUtility.UrlDecode(Http.GetAbsolutePath());
            bool pageNotFound = false;
            CmsRequestViewModel model;

            try
            {
                model = GetRequestModel(virtualPath);

                // When URL rewrite occurs, checking child absolute path
                if (model == null && Request.Url != null && Request.RawUrl != Request.Url.PathAndQuery)
                {
                    virtualPath = HttpUtility.UrlDecode(Http.GetAbsolutePath(Request.Url.AbsolutePath));
                    model = GetRequestModel(virtualPath);
                }

                if (!string.IsNullOrWhiteSpace(cmsConfiguration.PageNotFoundUrl) && model == null)
                {
                    model = GetRequestModel(HttpUtility.UrlDecode(cmsConfiguration.PageNotFoundUrl));
                    pageNotFound = true;
                }

                if (model != null)
                {
                    if (model.Redirect != null && !string.IsNullOrEmpty(model.Redirect.RedirectUrl))
                    {
                        return RedirectPermanent(model.Redirect.RedirectUrl);
                    }

                    if (model.RenderPage != null)
                    {
                        if (pageNotFound)
                        {
                            Response.StatusCode = 404;
                            LogPageNotFound(virtualPath);
                        }

                        ViewBag.pageId = model.RenderPage.Id;

                        // Force protocol.
                        switch (model.RenderPage.ForceAccessProtocol)
                        {
                            case ForceProtocolType.ForceHttp:
                                if (Request.Url.Scheme.Equals("https"))
                                {
                                    Response.Redirect(Request.Url.AbsoluteUri.Replace("https://", "http://"));
                                    return null;
                                }
                                break;
                            case ForceProtocolType.ForceHttps:
                                if (!Request.Url.Scheme.Contains("https"))
                                {
                                    Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"));
                                    return null;
                                }
                                break;
                        }

                        if (!HasCurrentPrincipalAccess(model.RenderPage))
                        {
                            try
                            {
                                // Pre-renders the given request model.
                                this.RenderPageToString(model.RenderPage);
                            }
                            catch (Exception ex)
                            {
                               log.FatalFormat("Failed to pre-render the request model {0}.", ex, model.RenderPage);
                            }
                            
                            Response.StatusCode = 403;
                            LogAccessForbidden(model.RenderPage);

                            throw new HttpException(403, "Access to the page forbidden.");
                        }

                        // Notify.
                        Events.RootEvents.Instance.OnPageRendering(model.RenderPage);

                        if (model.RenderPage != null && model.RenderPage.MasterPage != null)
                        {
                            // Render page with hierarchical master pages
                            return Content(this.RenderPageToString(model.RenderPage));
                        }

                        // Render regular MVC Razor view
                        return View(model.RenderPage);
                    }
                }
            }
            catch (HttpException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new HttpException(500, "Failed to load a CMS page.", ex);
            }

            LogPageNotFound(virtualPath);
            throw new HttpException(404, "Page Not Found");
        }

        private bool HasCurrentPrincipalAccess(IAccessSecuredObject page)
        {
            if (!cmsConfiguration.Security.AccessControlEnabled)
            {
                return true;
            }

            if (accessControlService == null)
            {
                return true;
            }

            var principal = SecurityService.GetCurrentPrincipal();
            var accessLevel = accessControlService.GetAccessLevel(page, principal);

            return accessLevel != AccessLevel.Deny;
        }

        private CmsRequestViewModel GetRequestModel(string virtualPath)
        {
            CmsRequestViewModel model;
            if (virtualPath.Trim() != "/")
            {
                switch (cmsConfiguration.UrlMode)
                {
                    case TrailingSlashBehaviorType.TrailingSlash:
                        virtualPath = VirtualPathUtility.AppendTrailingSlash(virtualPath);
                        break;
                    case TrailingSlashBehaviorType.NoTrailingSlash:
                        virtualPath = VirtualPathUtility.RemoveTrailingSlash(virtualPath);
                        break;
                }
            }

            var principal = SecurityService.GetCurrentPrincipal();

            var allRoles = new List<string>(RootModuleConstants.UserRoles.AllRoles);
            if (!string.IsNullOrEmpty(cmsConfiguration.Security.FullAccessRoles))
            {
                allRoles.Add(cmsConfiguration.Security.FullAccessRoles);
            }
            var canManageContent = SecurityService.IsAuthorized(principal, RootModuleConstants.UserRoles.MultipleRoles(allRoles.ToArray()));

            var useCaching = cmsConfiguration.Cache.Enabled && !canManageContent;
            var request = new GetPageToRenderRequest {
                                                         PageUrl = virtualPath,
                                                         CanManageContent = canManageContent,
                                                         HasContentAccess = canManageContent,
                                                         IsAuthenticated = principal != null && principal.Identity.IsAuthenticated
                                                     };

            if (useCaching)
            {
                string cacheKey = "CMS_" + CalculateHash(virtualPath) + "_050cc001f75942648e57e58359140d1a";
                model = cacheService.Get(cacheKey, cmsConfiguration.Cache.Timeout, () => GetCommand<GetPageToRenderCommand>().ExecuteCommand(request));
            }
            else
            {
                var command = GetCommand<GetPageToRenderCommand>();
                model = command.Execute(request);
            }

            return model;
        }

        private static string CalculateHash(string input)
        {
            // step 1, calculate SHA1 hash from input
            var sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = sha1.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private void LogPageNotFound(string virtualPath)
        {
            log.InfoFormat("Failed to load page by URL: {0}. Page not found.", virtualPath);

            // Notifying, that page was not found.
            Events.RootEvents.Instance.OnPageNotFound(virtualPath);
        }
        
        private void LogAccessForbidden(RenderPageViewModel model)
        {
            log.WarnFormat("Failed to load page by URL: {0}. Access is forbidden.", model.PageUrl);

            // Notifying, that page access was forbidden.
            Events.RootEvents.Instance.OnPageAccessForbidden(model);
        }
    }
}