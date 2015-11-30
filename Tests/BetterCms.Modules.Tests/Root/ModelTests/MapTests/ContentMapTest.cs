// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentMapTest.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Module.Root.Models;

using NHibernate.Linq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.ModelTests.MapTests
{
    [TestFixture]
    public class ContentMapTest : IntegrationTestBase
    {
        [Test]
        public void Should_Insert_And_Retrieve_Content_Successfully()
        {
            var content = TestDataProvider.CreateNewContent();
            RunEntityMapTestsInTransaction(content);            
        }

        [Test]
        public void Should_Insert_And_Retrieve_Content_PageContents_Successfully()
        {
            var content = TestDataProvider.CreateNewContent();
            var pageContents = new[]
                {
                    TestDataProvider.CreateNewPageContent(content),
                    TestDataProvider.CreateNewPageContent(content)
                };
            content.PageContents = pageContents;

            SaveEntityAndRunAssertionsInTransaction(
                content,
                result =>
                    {
                        Assert.AreEqual(content, result);
                        Assert.AreEqual(pageContents.OrderBy(f => f.Id), result.PageContents.OrderBy(f => f.Id));
                    });
        }

        [Test]
        public void Should_Insert_And_Retrieve_Content_ContentOptions_Successfully()
        {
            var content = TestDataProvider.CreateNewContent();
            var contentOptions = new[]
                {
                    TestDataProvider.CreateNewContentOption(content),
                    TestDataProvider.CreateNewContentOption(content)
                };
            content.ContentOptions = contentOptions;

            SaveEntityAndRunAssertionsInTransaction(
                content,
                result =>
                    {
                        Assert.AreEqual(content, result);
                        Assert.AreEqual(contentOptions.OrderBy(f => f.Id), result.ContentOptions.OrderBy(f => f.Id));
                    });
        }

        [Test]
        public void Should_Remove_ContentOptions_From_Content()
        {
            var content = TestDataProvider.CreateNewContent();
            var contentOptions = new[]
                {
                    TestDataProvider.CreateNewContentOption(content),
                    TestDataProvider.CreateNewContentOption(content)
                };

            content.ContentOptions = contentOptions;

            RunActionInTransaction(
                session =>
                {
                    session.SaveOrUpdate(content);
                    session.Flush();
                    Guid contentId = content.Id;
                    session.Clear();

                    session.Delete(content.ContentOptions[0]);
                    session.Flush();
                    session.Clear();

                    var dbContent = session.Query<Content>().FetchMany(f => f.ContentOptions).FirstOrDefault(f => f.Id == contentId);
                    Assert.IsNotNull(dbContent);
                    Assert.AreEqual(content, dbContent);
                    Assert.AreEqual(1, dbContent.ContentOptions.Count);
                });
        }
    }
}
