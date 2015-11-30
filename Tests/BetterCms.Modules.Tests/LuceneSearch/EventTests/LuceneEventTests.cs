// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LuceneEventTests.cs" company="Devbridge Group LLC">
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
using System;
using System.Collections.Generic;

using BetterCms.Events;

using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using BetterCms.Module.Search.Models;

using Lucene.Net.Documents;
using Lucene.Net.Search;

using NUnit.Framework;

namespace BetterCms.Test.Module.LuceneSearch.EventTests
{
    [TestFixture]
    public class LuceneEventTests : IntegrationTestBase
    {
        private bool documentSaving;

        private bool searchQueryExecuting;

        private bool searchResultRetrieving;

        [Test]
        public void Should_Fire_Document_Saving_Event()
        {
            documentSaving = false;

            Events.LuceneEvents.Instance.OnDocumentSaving(new Document(), new PageData());
            System.Threading.Thread.Sleep(10);
            Assert.IsFalse(documentSaving);

            Events.LuceneEvents.Instance.DocumentSaving += InstanceOnDocumentSaving;

            Events.LuceneEvents.Instance.OnDocumentSaving(new Document(), new PageData());
            System.Threading.Thread.Sleep(10);
            Assert.IsTrue(documentSaving);

            Events.LuceneEvents.Instance.DocumentSaving -= InstanceOnDocumentSaving;
        }

        private void InstanceOnDocumentSaving(DocumentSavingEventArgs args)
        {
            documentSaving = true;
        }

        [Test]
        public void Should_Fire_Search_Query_Executing_Event()
        {
            searchQueryExecuting = false;

            Events.LuceneEvents.Instance.OnSearchQueryExecuting(new BooleanQuery(), string.Empty);
            System.Threading.Thread.Sleep(10);
            Assert.IsFalse(searchQueryExecuting);

            Events.LuceneEvents.Instance.SearchQueryExecuting += InstanceOnSearchQueryExecuting;

            Events.LuceneEvents.Instance.OnSearchQueryExecuting(new BooleanQuery(), string.Empty);
            System.Threading.Thread.Sleep(10);
            Assert.IsTrue(searchQueryExecuting);

            Events.LuceneEvents.Instance.SearchQueryExecuting -= InstanceOnSearchQueryExecuting;
        }

        private void InstanceOnSearchQueryExecuting(SearchQueryExecutingEventArgs args)
        {
            searchQueryExecuting = true;
        }

        [Test]
        public void Should_Fire_Search_Result_Retrieving_Event()
        {
            searchResultRetrieving = false;

            Events.LuceneEvents.Instance.OnSearchResultRetrieving(new List<Document>(), new List<SearchResultItem>());
            System.Threading.Thread.Sleep(10);
            Assert.IsFalse(searchResultRetrieving);

            Events.LuceneEvents.Instance.SearchResultRetrieving += InstanceOnSearchResultRetrieving;

            Events.LuceneEvents.Instance.OnSearchResultRetrieving(new List<Document>(), new List<SearchResultItem>());
            System.Threading.Thread.Sleep(10);
            Assert.IsTrue(searchResultRetrieving);

            Events.LuceneEvents.Instance.SearchResultRetrieving -= InstanceOnSearchResultRetrieving;
        }

        private void InstanceOnSearchResultRetrieving(SearchResultRetrievingEventArgs args)
        {
            searchResultRetrieving = true;
        }
    }
}
