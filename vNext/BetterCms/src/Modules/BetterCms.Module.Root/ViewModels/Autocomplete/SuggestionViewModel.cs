namespace BetterCms.Module.Root.ViewModels.Autocomplete
{
    public class SuggestionViewModel
    {
        // TODO request validation is not supported in MVC6 https://github.com/aspnet/Mvc/issues/324
        //[AllowHtml]
        public string Query { get; set; }

        //[AllowHtml]
        public string ExistingItems { get; set; }

        public string[] ExistingItemsArray
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ExistingItems))
                {
                    return new string[0];
                }

                return ExistingItems.Split('|');
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Query: {Query}, ExistingItems: {ExistingItems}";
        }
    }
}