// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogMLExportServiceTests.cs" company="Devbridge Group LLC">
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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

using BetterModules.Core.DataAccess;
using BetterModules.Core.Web.Web;

using FluentNHibernate.Utils;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Blog.ServiceTests
{
    public class BlogMLExportServiceTests : TestBase
    {
        [Test]
        public void ShouldExportBlogPosts_Successfully()
        {
            var contextAccesssor = new Mock<IHttpContextAccessor>();
            var repository = new Mock<IRepository>();

            var service = new DefaultBlogMLExportService(contextAccesssor.Object, repository.Object);
            var fakeBlogPosts = GetFakeBlogPosts();

            var xml = service.ExportBlogPosts(fakeBlogPosts);
            Assert.IsNotNull(xml);

            Assert.IsTrue(xml.Contains(fakeBlogPosts[0].Title));
            Assert.IsTrue(xml.Contains(fakeBlogPosts[0].MetaTitle));
            Assert.IsTrue(xml.Contains(fakeBlogPosts[0].PageUrl));
        }

        [Test]
        public void ShouldExportBlogPosts_AndDeserializeWithReferences_Successfully()
        {
            var contextAccesssor = new Mock<IHttpContextAccessor>();
            var repository = new Mock<IRepository>();

            var service = new DefaultBlogMLExportService(contextAccesssor.Object, repository.Object);
            var fakeBlogPosts = GetFakeBlogPosts();

            var xml = service.ExportBlogPosts(fakeBlogPosts);

            AssertXml(xml, fakeBlogPosts);
        }

        [Test]
        public void ShouldExportBlogPosts_AndDeserializeWithoutReferences_Successfully()
        {
            var contextAccesssor = new Mock<IHttpContextAccessor>();
            var repository = new Mock<IRepository>();

            var service = new DefaultBlogMLExportService(contextAccesssor.Object, repository.Object);
            var fakeBlogPosts = GetFakeBlogPosts();
            fakeBlogPosts[0].Author = null;
            fakeBlogPosts[0].Categories = null;
            fakeBlogPosts[0].Description = null;

            var xml = service.ExportBlogPosts(fakeBlogPosts);

            AssertXml(xml, fakeBlogPosts);
        }

        private void AssertXml(string xml, List<BlogPost> fakeBlogPosts)
        {
            byte[] encodedString = Encoding.UTF8.GetBytes(xml);
            var xmlDoc = new XmlDocument();
            using (MemoryStream ms = new MemoryStream(encodedString))
            {
                ms.Flush();
                ms.Position = 0;

                xmlDoc.Load(ms);
            }

            Assert.AreEqual(xmlDoc.GetElementsByTagName("post").Count, 1);
            Assert.AreEqual(xmlDoc.GetElementsByTagName("blog").Count, 1);

            var xmlPost = xmlDoc.GetElementsByTagName("post")[0];
            var post = fakeBlogPosts[0];
            var childNodes = xmlPost.ChildNodes.Cast<XmlNode>().ToList();
            var attributes = xmlPost.Attributes.Cast<XmlAttribute>().ToList();
            var docNodes = xmlDoc.GetElementsByTagName("blog")[0].ChildNodes.Cast<XmlNode>().ToList();

            Assert.AreEqual(childNodes.First(node => node.Name == "title").InnerText, post.MetaTitle);
            Assert.AreEqual(childNodes.First(node => node.Name == "post-name").InnerText, post.Title);
            Assert.AreEqual(childNodes.First(node => node.Name == "content").InnerText, ((BlogPostContent)post.PageContents[1].Content).Html);
            Assert.AreEqual(attributes.First(attribute => attribute.Name == "post-url").InnerText, post.PageUrl);

            if (!string.IsNullOrWhiteSpace(post.Description))
            {
                Assert.NotNull(attributes.FirstOrDefault(node => node.Name == "hasexcerpt"));
                Assert.NotNull(attributes.First(node => node.Name == "hasexcerpt").InnerText);
                Assert.AreEqual(attributes.First(node => node.Name == "hasexcerpt").InnerText.ToLower(), "true");
                Assert.IsTrue(childNodes.Any(node => node.Name == "excerpt"));
            }
            else
            {
                Assert.NotNull(attributes.FirstOrDefault(node => node.Name == "hasexcerpt"));
                Assert.NotNull(attributes.First(node => node.Name == "hasexcerpt").InnerText);
                Assert.AreEqual(attributes.First(node => node.Name == "hasexcerpt").InnerText.ToLower(), "false");
                Assert.IsFalse(childNodes.Any(node => node.Name == "excerpt"));
            }

            if (post.Author != null)
            {
                Assert.IsTrue(childNodes.First(node => node.Name == "authors").HasChildNodes);
                Assert.IsTrue(docNodes.First(node => node.Name == "authors").HasChildNodes);

                var xmlAuthor = docNodes.First(node => node.Name == "authors").ChildNodes[0];
                Assert.IsNotNull(xmlAuthor);
                Assert.AreEqual(xmlAuthor.Attributes.Cast<XmlAttribute>().First(a => a.Name == "id").InnerText, post.Author.Id.ToString().ToLower());

                xmlAuthor = childNodes.First(node => node.Name == "authors").ChildNodes[0];
                Assert.IsNotNull(xmlAuthor);
                Assert.AreEqual(xmlAuthor.Attributes.Cast<XmlAttribute>().First(a => a.Name == "ref").InnerText, post.Author.Id.ToString().ToLower());
            }
            else
            {
                Assert.IsNull(childNodes.FirstOrDefault(node => node.Name == "authors"));
                Assert.IsNotNull(docNodes.FirstOrDefault(node => node.Name == "authors"));
                Assert.IsFalse(docNodes.First(node => node.Name == "authors").HasChildNodes);
            }

            if (post.Categories != null)
            {
                var categoriesIds = post.Categories.Select(c => c.Category.Id.ToLowerInvariantString()).ToList();
                var pageCategoriesIds = post.Categories.Select(c => c.Id.ToLowerInvariantString()).ToList();
                var categoriesNode = docNodes.First(node => node.Name == "categories");
                Assert.IsTrue(childNodes.First(node => node.Name == "categories").HasChildNodes);
                Assert.IsTrue(docNodes.First(node => node.Name == "categories").HasChildNodes);

                for (var i = 0; i < categoriesNode.ChildNodes.Count; i++)
                {
                    var xmlCategory = categoriesNode.ChildNodes[0];
                    Assert.IsNotNull(xmlCategory);
                    Assert.Contains(xmlCategory.Attributes.Cast<XmlAttribute>().First(a => a.Name == "id").InnerText, pageCategoriesIds);

                    xmlCategory = childNodes.First(node => node.Name == "categories").ChildNodes[0];
                    Assert.IsNotNull(xmlCategory);
                    Assert.Contains(xmlCategory.Attributes.Cast<XmlAttribute>().First(a => a.Name == "ref").InnerText, categoriesIds);
                }

                //var xmlCategory = docNodes.First(node => node.Name == "categories").ChildNodes[0];

            }
            else
            {
                Assert.IsNull(childNodes.FirstOrDefault(node => node.Name == "categories"));
                Assert.IsNotNull(docNodes.FirstOrDefault(node => node.Name == "categories"));
                Assert.IsFalse(docNodes.First(node => node.Name == "categories").HasChildNodes);
            }
        }

        private List<BlogPost> GetFakeBlogPosts()
        {
            var blog = new BlogPost
            {
                Id = Guid.NewGuid(),
                Author = new Author { Id = Guid.NewGuid(), Name = "Test Author" },
                PageUrl = "/test/url/",
                Title = "Test title",
                MetaTitle = "Test Meta Title",
                CreatedOn = new DateTime(2012, 10, 9),
                ModifiedOn = new DateTime(2012, 10, 15),
                Description = "Intro Text",
                PageContents =
                    new List<PageContent>
                    {
                        new PageContent { Content = new BlogPostContent { Html = "Unpbulished content <p>with HTML</p>" } },
                        new PageContent { Content = new BlogPostContent { Html = "Test content <p>with HTML</p>", Status = ContentStatus.Published } }
                    }
            };
            var categoryTree = new CategoryTree { Title = "Test Category Tree" };
            var category = new Category { Id = Guid.NewGuid(), Name = "Test Category", CategoryTree = categoryTree };
            category.CategoryTree = categoryTree;
            var pageCategory = new PageCategory { Category = category, Page = blog };
            categoryTree.Categories = new List<Category> { pageCategory.Category };
            blog.Categories = new List<PageCategory> { pageCategory };
            return new List<BlogPost> { blog };
        }
    }
}
