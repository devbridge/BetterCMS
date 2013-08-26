using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Configuration;
using BetterCms.Core.Mvc.Attributes;
using BetterCms.Core.Security;
using BetterCms.Core.Services.Caching;
using BetterCms.Module.Root.Commands.GetPageToRender;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Cms;

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

                if (!string.IsNullOrWhiteSpace(cmsConfiguration.PageNotFoundUrl) && model == null)
                {
                    model = GetRequestModel(HttpUtility.UrlDecode(cmsConfiguration.PageNotFoundUrl));
                    pageNotFound = true;
                }

                if (model != null)
                {
                    if (model.Redirect != null && !string.IsNullOrEmpty(model.Redirect.RedirectUrl))
                    {
                        return new RedirectResult(model.Redirect.RedirectUrl, true);
                    }

                    if (model.RenderPage != null)
                    {
                        if (pageNotFound)
                        {
                            Response.StatusCode = 404;
                        }

                        ViewBag.pageId = model.RenderPage.Id;

                        if (!HasAccess(model.RenderPage.Id))
                        {
                            Response.StatusCode = 403;
                            // throw new HttpException(403, "Access to the page forbidden.");
                            return Content("403 Access Forbidden", "text/plain");
                        }

                        // Notify.
                        Events.RootEvents.Instance.OnPageRendering(model.RenderPage);

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

            throw new HttpException(404, "Page Not Found");
        }

        private bool HasAccess(Guid pageId)
        {
            if (!cmsConfiguration.AccessControlEnabled)
            {
                return true;
            }

            if (accessControlService == null)
            {
                return true;
            }

            var principal = SecurityService.GetCurrentPrincipal();
            var accessLevel = accessControlService.GetAccessLevel<PageAccess>(pageId, principal);

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
            var request = new GetPageToRenderRequest
            {
                PageUrl = virtualPath,
                CanManageContent = canManageContent,
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
    }
}