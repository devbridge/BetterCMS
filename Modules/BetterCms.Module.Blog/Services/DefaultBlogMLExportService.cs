// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultBlogMLExportService.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

using BlogML;
using BlogML.Xml;

using BetterModules.Core.DataAccess;
using BetterModules.Core.Web.Web;

namespace BetterCms.Module.Blog.Services
{
    public class DefaultBlogMLExportService : BlogMLWriterBase, IBlogMLExportService
    {
        private List<BlogPost> posts;

        private readonly IHttpContextAccessor httpContextAccessor;
        
        private readonly IRepository repository;

        public DefaultBlogMLExportService(IHttpContextAccessor httpContextAccessor, IRepository repository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.repository = repository;
        }

        /// <summary>
        /// Exports the blog posts.
        /// </summary>
        /// <param name="postsToExport">The posts to export.</param>
        /// <returns>
        /// string in blogML format
        /// </returns>
        public string ExportBlogPosts(List<BlogPost> postsToExport)
        {
            posts = postsToExport;

            var settings = new XmlWriterSettings();
            settings.Indent = true;

            using (var stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    Write(writer);
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        protected override void InternalWriteBlog()
        {
            WriteStartBlog("Better CMS", ContentTypes.Text, "Better CMS", ContentTypes.Text, httpContextAccessor.MapPublicPath("/") ?? "/", GetMinBlogPostDate());

            WriteAuthors();
            WriteCategories();
            WritePosts();

            WriteEndElement();
            Writer.Flush();
        }

        private DateTime GetMinBlogPostDate()
        {
            var firstBlogPostDate = repository
                .AsQueryable<BlogPost>()
                .OrderBy(b => b.CreatedOn)
                .Select(b => b.CreatedOn)
                .FirstOrDefault();

            if (firstBlogPostDate != DateTime.MinValue)
            {
                return firstBlogPostDate.Date;
            }

            return DateTime.Now.Date;
        }

        private void WriteAuthors()
        {
            WriteStartAuthors();
            foreach (var author in posts.Where(p => p.Author != null).Select(p => p.Author).Distinct())
            {
                WriteAuthor(
                    author.Id.ToString(),
                    author.Name,
                    null,
                    author.CreatedOn,
                    author.ModifiedOn,
                    true);
            }
            WriteEndElement(); // </authors>
        }

        private void WriteCategories()
        {
            WriteStartCategories();
            var postsCategoriesTrees = posts.Where(t=>t.Categories != null)
                                            .SelectMany(t => t.Categories)
                                            .Select(t => t.Category.CategoryTree)
                                            .Distinct()
                                            .ToList();
            var allPostsCategories = postsCategoriesTrees
                                    .SelectMany(p => p.Categories)
                                    .Distinct()
                                    .ToList();
            foreach (var categoriesTree in postsCategoriesTrees)
            {
                WriteCategory(categoriesTree.Id.ToString(), 
                    categoriesTree.Title, 
                    ContentTypes.Text, 
                    categoriesTree.CreatedOn, 
                    categoriesTree.ModifiedOn, 
                    true, 
                    null, 
                    null);
            }
            foreach (var category in allPostsCategories)
            {
                WriteCategory(category.Id.ToString(), 
                    category.Name, 
                    ContentTypes.Text, 
                    category.CreatedOn, 
                    category.ModifiedOn, 
                    true, 
                    null,
                    (category.ParentCategory == null ? category.CategoryTree.Id : category.ParentCategory.Id).ToString());
            }
            WriteEndElement();
        }

        private void WritePosts()
        {
            WriteStartPosts();
            foreach (var post in posts)
            {
                WriteStartBlogMLPost(post);
                if (post.Categories != null)
                {
                    WritePostCategories(post.Categories);
                }
                if (post.Author != null)
                {
                    WritePostAuthor(post.Author);
                }

                WriteEndElement(); // </post>
                Writer.Flush();
            }
            WriteEndElement();
        }

        protected void WriteStartBlogMLPost(BlogPost post)
        {
            WriteStartElement("post");
            WriteNodeAttributes(post.Id.ToString(), post.ActivationDate, post.ModifiedOn, post.Status == PageStatus.Published);
            WriteAttributeString("post-url", post.PageUrl);
            WriteAttributeStringRequired("type", "normal");
            WriteAttributeStringRequired("hasexcerpt", (!string.IsNullOrWhiteSpace(post.Description)).ToString().ToLower());
            WriteAttributeStringRequired("views", "0");
            WriteContent("title", BlogMLContent.Create(post.MetaTitle ?? post.Title, ContentTypes.Text));
            WriteContent("post-name", BlogMLContent.Create(post.Title, ContentTypes.Text));

            if (!string.IsNullOrWhiteSpace(post.Description))
            {
                WriteBlogMLContent("excerpt", BlogMLContent.Create(post.Description, ContentTypes.Text));
            }

            var content = post.PageContents
                .Where(pc => pc.Content is BlogPostContent && pc.Content.Status == ContentStatus.Published)
                .Select(pc => pc.Content)
                .FirstOrDefault();
            if (content != null)
            {
                WriteBlogMLContent("content", BlogMLContent.Create(((BlogPostContent)content).Html, ContentTypes.Text));
            }
        }

        protected void WriteBlogMLContent(string elementName, BlogMLContent content)
        {
            WriteContent(elementName, content);
        }

        protected void WritePostCategories(IEnumerable<PageCategory> categories)
        {
            WriteStartCategories();
            foreach (var category in categories)
            {
                WriteCategoryReference(category.Category.Id.ToString());    
            }            
            WriteEndElement();
        }

        private void WritePostAuthor(Author author)
        {
            WriteStartAuthors();
            WriteAuthorReference(author.Id.ToString());
            WriteEndElement();
        }
    }
}