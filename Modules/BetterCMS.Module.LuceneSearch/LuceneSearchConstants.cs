// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LuceneSearchConstants.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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