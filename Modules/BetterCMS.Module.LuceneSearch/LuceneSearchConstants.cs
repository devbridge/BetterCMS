namespace BetterCMS.Module.LuceneSearch
{
    public static class LuceneSearchConstants
    {
        public static class ConfigurationKeys
        {
            public const string LuceneWebSiteUrl = "LuceneWebSiteUrl";

            public const string LucenePagesWatcherFrequency = "LucenePagesWatcherFrequency";

            public const string LuceneIndexerFrequency = "LuceneIndexerFrequency";

            public const string LuceneFileSystemDirectory = "LuceneFileSystemDirectory";

            public const string LuceneMaxPagesPerQuery = "LuceneMaxPagesPerQuery";

            public const string LucenePageExpireTimeout = "LucenePageExpireTimeout";

            public const string LuceneDisableStopWords = "LuceneDisableStopWords";

            public const string LuceneIndexPrivatePages = "LuceneIndexPrivatePages";

            public const string LuceneAuthorizationUrl = "LuceneAuthorizationUrl";

            public const string LuceneAuthorizationFormFieldPrefix = "LuceneAuthorizationForm.";

            public const string LuceneSearchForPartOfWordsPrefix = "LuceneSearchForPartOfWords";
        }

        public const string LuceneSearchModuleLoggerNamespace = "LuceneSearchModule";
    }
}