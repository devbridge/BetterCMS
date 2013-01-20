using System.Web.Mvc;

using BetterCms.Core.Mvc.Attributes;
using BetterCms.Module.Root.Commands.GetPageToPreview;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Cms;

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
            GetPageToPreviewRequest request = new GetPageToPreviewRequest {
                                                                              PageId = pageId.ToGuidOrDefault(),
                                                                              PageContentId = pageContentId.ToGuidOrDefault()
                                                                          };

            RenderPageViewModel model = GetCommand<GetPageToPreviewCommand>().ExecuteCommand(request);

            return View(model);
        }
    }
}