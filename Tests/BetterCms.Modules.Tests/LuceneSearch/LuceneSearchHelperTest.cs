using BetterCMS.Module.LuceneSearch.Helpers;

using Lucene.Net.Search;

using NUnit.Framework;

namespace BetterCms.Test.Module.LuceneSearch
{
    [TestFixture]
    public class LuceneSearchHelperTest : IntegrationTestBase
    {
        private bool luceneSearchDelegateFired;

        [Test]
        public void Should_Fire_Lucene_Helper_Delegate_Event()
        {
            luceneSearchDelegateFired = false;

            if (LuceneSearchHelper.Search != null)
            {
                LuceneSearchHelper.Search(new BooleanQuery(), null, null);
            }
            System.Threading.Thread.Sleep(10);
            Assert.IsFalse(luceneSearchDelegateFired);

            LuceneSearchHelper.Search += delegate
            {
                luceneSearchDelegateFired = true;
                return null;
            };

            if (LuceneSearchHelper.Search != null)
            {
                LuceneSearchHelper.Search(new BooleanQuery(), null, null);
            }
            System.Threading.Thread.Sleep(10);
            Assert.IsTrue(luceneSearchDelegateFired);
        }
    }
}
