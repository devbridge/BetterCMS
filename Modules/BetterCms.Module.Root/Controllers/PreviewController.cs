using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Core.Mvc.Attributes;
using BetterCms.Core.Security;

using BetterCms.Module.Root.Commands.GetPageToRender;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Preview controller.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
    [ActionLinkArea(RootModuleDescriptor.RootAreaName)]
    public class PreviewController : CmsControllerBase
    {
        /// <summary>
        /// The CMS configuration.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewController"/> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public PreviewController(ICmsConfiguration cmsConfiguration)
        {
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Previews the specified page id.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="pageContentId">The page content id.</param>
        /// <returns>
        /// Returns an action result to render a page preview. 
        /// </returns>
        [IgnoreAutoRoute]
        public ActionResult Index(string pageId, string pageContentId)
        {
            var principal = SecurityService.GetCurrentPrincipal();

            var allRoles = new List<string>(RootModuleConstants.UserRoles.AllRoles);
            if (!string.IsNullOrEmpty(cmsConfiguration.Security.FullAccessRoles))
            {
                allRoles.Add(cmsConfiguration.Security.FullAccessRoles);
            }

            GetPageToRenderRequest request = new GetPageToRenderRequest
                {
                    PageId = pageId.ToGuidOrDefault(),
                    PreviewPageContentId = pageContentId.ToGuidOrDefault(),
                    IsPreview = true,
                    HasContentAccess = SecurityService.IsAuthorized(principal, RootModuleConstants.UserRoles.MultipleRoles(allRoles.ToArray()))
                };

            var model = GetCommand<GetPageToRenderCommand>().ExecuteCommand(request);

            if (model != null && model.RenderPage != null)
            {
                // Render page with hierarchical master pages
                var html = this.RenderPageToString(model.RenderPage);
                html = PageHtmlRenderer.ReplaceRegionRepresentationHtml(html, string.Empty);

                return Content(html);
            }

            return HttpNotFound();
        }
    }
}