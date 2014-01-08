using System.Web.Mvc;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Search.Command.SearchQuery;
using BetterCms.Module.Search.ViewModels;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Search.Controllers
{
    [ActionLinkArea(SearchModuleDescriptor.SearchAreaName)]
    public class SearchController : CmsControllerBase
    {
        public ActionResult Results(SearchRequestViewModel model)
        {
            var results = GetCommand<SearchQueryCommand>().ExecuteCommand(model);

            return PartialView("SearchResultsWidget", results);
        }
    }
}
