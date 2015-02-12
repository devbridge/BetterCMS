using BetterCms.Module.Root.ViewModels.Autocomplete;

namespace BetterCms.Module.Root.ViewModels.Category
{
    public class CategorySuggestionViewModel : SuggestionViewModel
    {
        public string CategoryTreeForKey { get; set; }

        public override string ToString()
        {
            return string.Format("CategoryTreeForKey: {0}", CategoryTreeForKey);
        }
    }
}