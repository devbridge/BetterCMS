using System.Web.Mvc;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Search.Command.SearchQuery;
using BetterCms.Module.Search.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Search.Controllers
{
    /// <summary>
    /// Newsletter subscribers controller.
    /// </summary>
    [ActionLinkArea(SearchModuleDescriptor.SearchAreaName)]
    public class SearchController : CmsControllerBase
    {
        public ActionResult Results(string query)
        {
            var results = GetCommand<SearchQueryCommand>().ExecuteCommand(new SearchRequest(query));

            return PartialView("SearchResults", results);
        }
    }
}
