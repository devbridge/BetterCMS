// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartialWordTermQueryParserTests.cs" company="Devbridge Group LLC">
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
