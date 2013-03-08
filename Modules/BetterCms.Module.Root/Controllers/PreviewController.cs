using System.Web.Mvc;

using BetterCms.Core.Mvc.Attributes;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Commands.GetPageToRender;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Preview controller.
    /// </summary>
    public class PreviewController : CmsControllerBase
    {
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
            GetPageToRenderRequest request = new GetPageToRenderRequest {
                                                                            PageId = pageId.ToGuidOrDefault(),
                                                                            PreviewPageContentId = pageContentId.ToGuidOrDefault(),
                                                                            IsPreview = true
                                                                        };
        

            var model = GetCommand<GetPageToRenderCommand>().ExecuteCommand(request);

            if (model.RenderPage != null)
            {
                return View(model.RenderPage);
            }

            return HttpNotFound();
        }
    }
}