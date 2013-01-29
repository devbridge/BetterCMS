using System.Web.Mvc;

using BetterCms.Module.Pages.Command.History.GetContentHistory;
using BetterCms.Module.Pages.Command.History.GetContentVersion;
using BetterCms.Module.Pages.Command.History.RestoreContentVersion;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Controllers
{
    public class HistoryController : CmsControllerBase
    {
        [HttpGet]
        public ActionResult ContentHistory(string contentId)
        {
            var model = GetCommand<GetContentHistoryCommand>().ExecuteCommand(new GetContentHistoryRequest
                                                                                      {
                                                                                          ContentId = contentId.ToGuidOrDefault(),
                                                                                      });
            return View(model);
        }

        [HttpPost]
        public ActionResult ContentHistory(GetContentHistoryRequest request)
        {
            var model = GetCommand<GetContentHistoryCommand>().ExecuteCommand(request);

            return View(model);
        }

        [HttpGet]
        public ActionResult ContentVersion(string id)
        {
            var model = GetCommand<GetContentVersionCommand>().ExecuteCommand(id.ToGuidOrDefault());

            return View(model);
        }
        
        [HttpPost]
        public ActionResult RestorePageContentVersion(string id)
        {
            var result = GetCommand<RestoreContentVersionCommand>().ExecuteCommand(id.ToGuidOrDefault());

            return WireJson(result);
        }
    }
}
