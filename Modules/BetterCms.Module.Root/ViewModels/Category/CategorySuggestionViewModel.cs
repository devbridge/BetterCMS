using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Module.Root.ViewModels.Autocomplete;

namespace BetterCms.Module.Root.ViewModels.Category
{
    public class CategorySuggestionViewModel : SuggestionViewModel
    {
        public string CategoryTreeForKey { get; set; }
    }
}