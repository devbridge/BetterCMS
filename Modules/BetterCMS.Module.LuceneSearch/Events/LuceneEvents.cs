// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LuceneEvents.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;

using BetterCMS.Module.LuceneSearch.Models;
using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;
using BetterCms.Module.Search.Models;

using BetterModules.Events;

using Lucene.Net.Documents;
using Lucene.Net.Search;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    public class LuceneEvents : EventsBase<LuceneEvents>
    {
        /// <summary>
        /// Occurs before document is saved.
        /// </summary>
        public event DefaultEventHandler<DocumentSavingEventArgs> DocumentSaving;

        /// <summary>
        /// Occurs before document is saved.
        /// </summary>
        public event DefaultEventHandler<SearchQueryExecutingEventArgs> SearchQueryExecuting;

        /// <summary>
        /// Occurs before document is saved.
        /// </summary>
        public event DefaultEventHandler<SearchResultRetrievingEventArgs> SearchResultRetrieving;

        /// <summary>
        /// Occurs before index sources are saved to database and allows to inject additional index sources.
        /// </summary>
        public event DefaultEventHandler<FetchingNewUrlsEventArgs> FetchingNewUrls;

        public DocumentSavingEventArgs OnDocumentSaving(Document document, PageData pageData)
        {
            var args = new DocumentSavingEventArgs(document, pageData);

            if (DocumentSaving != null)
            {
                DocumentSaving(args);
            }

            return args;
        }

        public SearchQueryExecutingEventArgs OnSearchQueryExecuting(Query query, string requestQuery)
        {
            var args = new SearchQueryExecutingEventArgs(query, requestQuery);

            if (SearchQueryExecuting != null)
            {
                SearchQueryExecuting(args);
            }

            return args;
        }

        public SearchResultRetrievingEventArgs OnSearchResultRetrieving(List<Document> documents, List<SearchResultItem> resultItems)
        {
            var args = new SearchResultRetrievingEventArgs(documents, resultItems);

            if (SearchResultRetrieving != null)
            {
                SearchResultRetrieving(args);
            }

            return args;
        }

        public FetchingNewUrlsEventArgs OnFetchingNewUrls()
        {
            var args = new FetchingNewUrlsEventArgs { IndexSources = new List<IndexSource>() };

            if (FetchingNewUrls != null)
            {
                FetchingNewUrls(args);
            }

            return args;
        }
    }
}
