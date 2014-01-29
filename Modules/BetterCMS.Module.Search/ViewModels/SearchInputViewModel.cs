using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Module.Root.ViewModels.Cms;
using BetterCms.Module.Search.Content.Resources;

namespace BetterCms.Module.Search.ViewModels
{
    public class SearchInputViewModel : RenderWidgetViewModel
    {
        [StringLength(SearchModuleConstants.SearchQueryMaximumLength, ErrorMessageResourceType = typeof(SearchGlobalization), ErrorMessageResourceName = "SearchForm_Query_MaximumLengthMessage")]
        [AllowHtml]
        public string Query { get; set; }
    }
}