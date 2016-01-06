namespace BetterCMS.Module.LuceneSearch
{
    public static class LuceneSearchConstants
    {
        public static class ConfigurationKeys
        {
            public const string LuceneWebSiteUrl = "LuceneWebSiteUrl";

            public const string LucenePagesWatcherFrequency = "LucenePagesWatcherFrequency";

            public const string LuceneIndexerFrequency = "LuceneIndexerFrequency";

            public const string LuceneIndexerPageFetchTimeout = "LuceneIndexerPageFetchTimeout";

            public const string LuceneFileSystemDirectory = "LuceneFileSystemDirectory";

            public const string LuceneMaxPagesPerQuery = "LuceneMaxPagesPerQuery";

            public const string LucenePageExpireTimeout = "LucenePageExpireTimeout";

            public const string LuceneDisableStopWords = "LuceneDisableStopWords";

            public const string LuceneIndexPrivatePages = "LuceneIndexPrivatePages";

            public const string LuceneAuthorizationUrl = "LuceneAuthorizationUrl";

            public const string LuceneAuthorizationMode = "LuceneAuthorizationMode";

            public const string LuceneAuthorizationFormFieldPrefix = "LuceneAuthorizationForm.";

            public const string LuceneAuthorizationWindows_UserName = "LuceneAuthorizationWindows.UserName";

            public const string LuceneAuthorizationWindows_Password = "LuceneAuthorizationWindows.Password";

            public const string LuceneSearchForPartOfWordsPrefix = "LuceneSearchForPartOfWords";

            public const string LuceneIndexerDeleteLockFileOnStart = "LuceneIndexerDeleteLockFileOnStart";

            public const string LuceneIndexerRunsOnlyOnHost = "LuceneIndexerRunsOnlyOnHost";

            public const string LuceneExcludedNodes = "LuceneExcludedNodes";

            public const string LuceneExcludedIds = "LuceneExcludedIds";

            public const string LuceneExcludedClasses = "LuceneExcludedClasses";

            public const string LuceneExcludedPages = "LuceneExcludedPages";
        }

        public const string LuceneSearchModuleLoggerNamespace = "LuceneSearchModule";
    }
}