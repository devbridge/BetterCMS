namespace BetterCms.Module.Root.ViewModels.Autocomplete
{
    public class SuggestionViewModel
    {
        public string Query { get; set; }
        
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
            return string.Format("{0}, Query: {1}, ExistingItems: {2}", base.ToString(), Query, ExistingItems);
        }
    }
}