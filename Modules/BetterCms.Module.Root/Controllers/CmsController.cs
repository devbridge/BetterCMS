using System;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;

using BetterCms.Api;
using BetterCms.Core.Mvc.Attributes;
using BetterCms.Core.Services;
using BetterCms.Core.Services.Caching;
using BetterCms.Module.Root.Commands.GetPageToRender;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Root.Controllers
{
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
        /// Initializes a new instance of the <see cref="CmsController" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The configuration loader.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="securityService">The security service.</param>
        public CmsController(ICmsConfiguration cmsConfiguration, ICacheService cacheService)
        {
            this.cmsConfiguration = cmsConfiguration;
            this.cacheService = cacheService;
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
            
            CmsRequestViewModel model = GetRequestModel(virtualPath);
           
            if (model == null && !string.IsNullOrWhiteSpace(cmsConfiguration.PageNotFoundUrl))            
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
                    
                    // Notify.
                    RootApiContext.Events.OnPageRendering(model.RenderPage);

                    return View(model.RenderPage);
                }
            }

            return HttpNotFound();
        }

        private CmsRequestViewModel GetRequestModel(string virtualPath)
        {
            CmsRequestViewModel model;
            virtualPath = VirtualPathUtility.AppendTrailingSlash(virtualPath);            
            var principal = SecurityService.GetCurrentPrincipal();
            bool canManageContent = SecurityService.IsAuthorized(principal, RootModuleConstants.UserRoles.ManageContent);
            var useCaching = cmsConfiguration.Cache.Enabled && !canManageContent;
            var request = new GetPageToRenderRequest {
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
                model = GetCommand<GetPageToRenderCommand>().ExecuteCommand(request);
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