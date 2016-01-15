// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultPagesApiOperations.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Operations.Pages.Contents.Content;
using BetterCms.Module.Api.Operations.Pages.Pages;
using BetterCms.Module.Api.Operations.Pages.Redirects;
using BetterCms.Module.Api.Operations.Pages.Sitemaps;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget;

using IPageService = BetterCms.Module.Api.Operations.Pages.Pages.Page.IPageService;
using IRedirectService = BetterCms.Module.Api.Operations.Pages.Redirects.Redirect.IRedirectService;
using IWidgetsService = BetterCms.Module.Api.Operations.Pages.Widgets.IWidgetsService;

namespace BetterCms.Module.Api.Operations.Pages
{
    public class DefaultPagesApiOperations : IPagesApiOperations
    {
        public DefaultPagesApiOperations(IPagesService pages, IPageService page, IContentService content, IWidgetService widget, IWidgetsService widgets,
            IRedirectsService redirects, IRedirectService redirect, Sitemap.ISitemapService sitemap, ISitemapsService sitemaps, ISitemapService sitemapNew)
        {
            Pages = pages;
            Page = page;
            Content = content;
            Widget = widget;
            Widgets = widgets;
            Redirect = redirect;
            Redirects = redirects;
            Sitemap = sitemap;
            Sitemaps = sitemaps;
            SitemapNew = sitemapNew;
        }

        public IPagesService Pages
        {
            get; 
            private set;
        }
        
        public IPageService Page
        {
            get; 
            private set;
        }
        
        public IContentService Content
        {
            get; 
            private set;
        }


        public IWidgetsService Widgets
        {
            get;
            private set;
        }

        public IWidgetService Widget
        {
            get;
            private set;
        }

        public IRedirectsService Redirects
        {
            get;
            private set;
        }

        public IRedirectService Redirect
        {
            get;
            private set;
        }

        public ISitemapsService Sitemaps
        {
            get;
            private set;
        }

        public ISitemapService SitemapNew
        {
            get;
            private set;
        }

        public Sitemap.ISitemapService Sitemap
        {
            get;
            private set;
        }
    }
}
