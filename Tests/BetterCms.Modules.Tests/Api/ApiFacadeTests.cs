// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiFacadeTests.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api
{
    [TestFixture]
    public class ApiFacadeTests : TestBase
    {
        [Test]
        public void ShouldCreateAndDisposeApiFacade()
        {            
            IApiFacade apiContainer;

            using (var api = ApiFactory.Create())
            {
                apiContainer = api;

                Assert.IsNotNull(api.Root);
                Assert.IsNotNull(api.Root.Layout);
                Assert.IsNotNull(api.Root.Layout.Regions);
                Assert.IsNotNull(api.Root.Layouts);
                Assert.IsNotNull(api.Root.Tags);
                Assert.IsNotNull(api.Root.Tag);
                Assert.IsNotNull(api.Root.Categories);
                Assert.IsNotNull(api.Root.Category);
                Assert.IsNotNull(api.Root.Category.Node);
                Assert.IsNotNull(api.Root.Category.Nodes);
                Assert.IsNotNull(api.Root.Category.Tree);
                Assert.IsNotNull(api.Root.Languages);
                Assert.IsNotNull(api.Root.Language);
                Assert.IsNotNull(api.Root.Version);

                Assert.IsNotNull(api.Pages);
                Assert.IsNotNull(api.Pages.Pages);
                Assert.IsNotNull(api.Pages.Page);
                Assert.IsNotNull(api.Pages.Content);
                Assert.IsNotNull(api.Pages.Content.Html);
                Assert.IsNotNull(api.Pages.Content.History);
                Assert.IsNotNull(api.Pages.Content.Draft);
                Assert.IsNotNull(api.Pages.Page.Contents);
                Assert.IsNotNull(api.Pages.Page.Properties);
                Assert.IsNotNull(api.Pages.Redirect);
                Assert.IsNotNull(api.Pages.Redirects);
                Assert.IsNotNull(api.Pages.Widget);
                Assert.IsNotNull(api.Pages.Widget.HtmlContent);
                Assert.IsNotNull(api.Pages.Widget.HtmlContent.Options);
                Assert.IsNotNull(api.Pages.Widget.ServerControl);
                Assert.IsNotNull(api.Pages.Widget.ServerControl.Options);
                Assert.IsNotNull(api.Pages.Widgets);
                Assert.IsNotNull(api.Pages.Sitemap);
                Assert.IsNotNull(api.Pages.Sitemap.Node);
                Assert.IsNotNull(api.Pages.Sitemap.Nodes);
                Assert.IsNotNull(api.Pages.Sitemap.Tree);
                Assert.IsNotNull(api.Pages.SitemapNew);
                Assert.IsNotNull(api.Pages.SitemapNew.Node);
                Assert.IsNotNull(api.Pages.SitemapNew.Nodes);
                Assert.IsNotNull(api.Pages.SitemapNew.Tree);
                Assert.IsNotNull(api.Pages.Sitemaps);

                Assert.IsNotNull(api.Blog);
                Assert.IsNotNull(api.Blog.BlogPost);
                Assert.IsNotNull(api.Blog.BlogPost.Content);
                Assert.IsNotNull(api.Blog.BlogPost.Properties);
                Assert.IsNotNull(api.Blog.BlogPosts);
                Assert.IsNotNull(api.Blog.Author);
                Assert.IsNotNull(api.Blog.Authors);

                Assert.IsNotNull(api.Media);
                Assert.IsNotNull(api.Media.MediaTree);
                Assert.IsNotNull(api.Media.Folders);
                Assert.IsNotNull(api.Media.Image);
                Assert.IsNotNull(api.Media.Images);
                Assert.IsNotNull(api.Media.File);
                Assert.IsNotNull(api.Media.Files);

                Assert.IsNotNull(api.Users);
                Assert.IsNotNull(api.Users.User);
                Assert.IsNotNull(api.Users.Users);
                Assert.IsNotNull(api.Users.Role);
                Assert.IsNotNull(api.Users.Roles);
            }

            Assert.IsNull(apiContainer.Scope);
        }
    }
}