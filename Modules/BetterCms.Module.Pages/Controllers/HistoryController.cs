using System.Web.Mvc;

using BetterCms.Module.Pages.Command.History.GetContentHistory;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Controllers
{
    public class HistoryController : CmsControllerBase
    {
        [HttpGet]
        public ActionResult PageContentHistory(string pageContentId, string pageContentVersion, string contentId, string contentVersion)
        {
            var model = GetCommand<GetContentHistoryCommand>().ExecuteCommand(new GetContentHistoryRequest
                                                                                      {
                                                                                          PageContentId = pageContentId.ToGuidOrDefault(),
                                                                                          PageContentVersion = pageContentVersion.ToIntOrDefault(),
                                                                                          ContentId = contentId.ToGuidOrDefault(),
                                                                                          ContentVersion = contentVersion.ToIntOrDefault()
                                                                                      });
            return View(model);
        }

        [HttpPost]
        public ActionResult PageContentHistory(GetContentHistoryRequest request)
        {
            var model = GetCommand<GetContentHistoryCommand>().ExecuteCommand(request);

            return View(model);
        }
    }
}
