using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

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
        /// The page accessor.
        /// </summary>
        private readonly IPageAccessor pageAccessor;

        /// <summary>
        /// The configuration loader
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The cache service
        /// </summary>
        private readonly ICacheService cacheService;        

        private readonly ISecurityService securityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsController" /> class.
        /// </summary>
        /// <param name="pageAccessor">The page accessor.</param>
        /// <param name="cmsConfiguration">The configuration loader.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="securityService">The security service.</param>
        public CmsController(IPageAccessor pageAccessor, ICmsConfiguration cmsConfiguration, ICacheService cacheService, ISecurityService securityService)
        {
            this.securityService = securityService;            
            this.pageAccessor = pageAccessor;
            this.cmsConfiguration = cmsConfiguration;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// Default entry point for all CMS pages.
        /// </summary>
        /// <returns>Returns page or redirect or page not found result.</returns>
        public ActionResult Index()
        {
            var virtualPath = HttpUtility.UrlDecode(Http.GetAbsolutePath());
            
            CmsRequestViewModel model = GetPageModel(virtualPath);
           
            if (model == null && !string.IsNullOrWhiteSpace(cmsConfiguration.PageNotFoundUrl))            
            {
                model = GetPageModel(HttpUtility.UrlDecode(cmsConfiguration.PageNotFoundUrl));
            }

            if (model != null)
            {                
                if (model.Redirect != null && !string.IsNullOrEmpty(model.Redirect.RedirectUrl))
                {
                    return new RedirectResult(model.Redirect.RedirectUrl, true);
                }
                
                if (model.RenderPage != null)
                {                    
                    ViewBag.pageId = model.RenderPage.Id;
                    return View(model.RenderPage);
                }
            }

            return HttpNotFound();
        }

        private CmsRequestViewModel GetPageModel(string virtualPath)
        {
            CmsRequestViewModel model = null;
            virtualPath = VirtualPathUtility.AppendTrailingSlash(virtualPath);
            string cacheKey = "CMS_" + virtualPath + "_050cc001f75942648e57e58359140d1a";
            IPrincipal principal = securityService.GetCurrentPrincipal();
            bool canManageContent = securityService.CanManageContent(principal);

            var useCaching = cmsConfiguration.Cache.Enabled && !canManageContent;

            if (useCaching)
            {

                model = cacheService.Get(cacheKey, cmsConfiguration.Cache.Timeout, () => GetCommand<GetPageToRenderCommand>().ExecuteCommand(virtualPath));
            }
            else
            {
                model = GetCommand<GetPageToRenderCommand>().ExecuteCommand(virtualPath);
            }

            if (model != null && model.RenderPage != null)
            {
                model.RenderPage.CanManageContent = canManageContent;
            }

            return model;
        }   
    }
}