namespace BetterCms.Module.Search
{
    public static class SearchModuleConstants
    {
        public static class WidgetOptionNames
        {
            public const string FormMethod = "Form.Method";
            public const string QueryParameterName = "Query.Parameter.Name";
            public const string LabelTitle = "Label.Title";
            public const string SubmitTitle = "Submit.Title";
            public const string InputPlaceholder = "Input.Placeholder";
            public const string SearchResultsUrl = "SearchResults.Url";
            public const string ShowTotalResults = "SearchResults.Show.Total.Count";
            public const string TotalCountMessage = "SearchResults.Total.Count.Message";
            public const string ResultsCount = "SearchResults.Count";
        }

        public static class WidgetOptionDefaultValues
        {
            public const string FormMethod = "GET";
            public const string QueryParameterName = "Query";
        }

        public const int DefaultSearchResultsCount = 10;

        public const int SearchQueryMaximumLength = 1000;
    }
}