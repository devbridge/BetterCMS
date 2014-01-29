using System.Linq;
using System.Web.Mvc;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Search.Command.SearchQuery;
using BetterCms.Module.Search.Content.Resources;
using BetterCms.Module.Search.ViewModels;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Search.Controllers
{
    [ActionLinkArea(SearchModuleDescriptor.SearchAreaName)]
    public class SearchController : CmsControllerBase
    {
        public ActionResult Results(SearchRequestViewModel model)
        {
            SearchResultsViewModel results = null;
            if (ModelState.IsValid)
            {
                results = GetCommand<SearchQueryCommand>().ExecuteCommand(model);
            }
            else
            {
                var errorMessage = SearchGlobalization.SearchResults_FailedToGetResults;
                var modelError = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(modelError))
                {
                    errorMessage = string.Concat(errorMessage, " ", modelError);
                }

                Messages.AddError(errorMessage);
            }

            if (results == null && Messages.Error != null && Messages.Error.Count > 0)
            {
                results = new SearchResultsViewModel { ErrorMessage = Messages.Error[0] };
            }

            return PartialView("SearchResultsWidget", results);
        }
    }
}
