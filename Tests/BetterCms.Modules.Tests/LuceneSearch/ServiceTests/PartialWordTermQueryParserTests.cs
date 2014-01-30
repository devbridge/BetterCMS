using BetterCMS.Module.LuceneSearch.Services.IndexerService;

using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

using NUnit.Framework;

namespace BetterCms.Test.Module.LuceneSearch.ServiceTests
{
    [TestFixture]
    public class PartialWordTermQueryParserTests : TestBase
    {
        [Test]
        public void Should_Add_Wildcards_PartialWildcard1()
        {
            var queryParser = new PartialWordTermQueryParser(Version.LUCENE_30, "test", new StandardAnalyzer(Version.LUCENE_30));
            var query = queryParser.Parse("*word1 word2");
            var result = query.ToString();

            Assert.AreEqual(result, "test:*word1 test:*word2*");
        }
        
        [Test]
        public void Should_Add_Wildcards_PartialWildcard2()
        {
            var queryParser = new PartialWordTermQueryParser(Version.LUCENE_30, "test", new StandardAnalyzer(Version.LUCENE_30));
            var query = queryParser.Parse("*word1 word2*");
            var result = query.ToString();

            Assert.AreEqual(result, "test:*word1 test:word2*");
        }
        
        [Test]
        public void Should_Add_Wildcards_PartialWildcard3()
        {
            var queryParser = new PartialWordTermQueryParser(Version.LUCENE_30, "test", new StandardAnalyzer(Version.LUCENE_30));
            var query = queryParser.Parse("word1 *word2");
            var result = query.ToString();

            Assert.AreEqual(result, "test:*word1* test:*word2");
        }
        
        [Test]
        public void Should_Add_Wildcards_PartialWildcard4()
        {
            var queryParser = new PartialWordTermQueryParser(Version.LUCENE_30, "test", new StandardAnalyzer(Version.LUCENE_30));
            var query = queryParser.Parse("word1* *word2");
            var result = query.ToString();

            Assert.AreEqual(result, "test:word1* test:*word2");
        }
        
        [Test]
        public void Should_Add_Wildcards_NoWildcards()
        {
            var queryParser = new PartialWordTermQueryParser(Version.LUCENE_30, "test", new StandardAnalyzer(Version.LUCENE_30));
            var query = queryParser.Parse("word1 word2");
            var result = query.ToString();

            Assert.AreEqual(result, "test:*word1* test:*word2*");
        }
        
        [Test]
        public void Should_Add_Wildcards_AllWildcards()
        {
            var queryParser = new PartialWordTermQueryParser(Version.LUCENE_30, "test", new StandardAnalyzer(Version.LUCENE_30));
            var query = queryParser.Parse("*word1* *word2*");
            var result = query.ToString();

            Assert.AreEqual(result, "test:*word1* test:*word2*");
        }
    }
}
