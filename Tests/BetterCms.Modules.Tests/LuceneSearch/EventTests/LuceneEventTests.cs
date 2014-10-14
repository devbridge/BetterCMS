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
