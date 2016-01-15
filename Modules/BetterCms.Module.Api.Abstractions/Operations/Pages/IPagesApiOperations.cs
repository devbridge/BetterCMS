// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPagesApiOperations.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Api.Operations.Pages.Contents.Content;
using BetterCms.Module.Api.Operations.Pages.Pages;
using BetterCms.Module.Api.Operations.Pages.Pages.Page;
using BetterCms.Module.Api.Operations.Pages.Redirects;
using BetterCms.Module.Api.Operations.Pages.Redirects.Redirect;
using BetterCms.Module.Api.Operations.Pages.Sitemaps;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap;
using BetterCms.Module.Api.Operations.Pages.Widgets;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget;

namespace BetterCms.Module.Api.Operations.Pages
{
    public interface IPagesApiOperations
    {
        IPagesService Pages { get; }
        
        IPageService Page { get; }
        
        IContentService Content { get; }
        
        IWidgetsService Widgets { get; }
        
        IWidgetService Widget { get; }
        
        IRedirectsService Redirects { get; }
        
        IRedirectService Redirect { get; }

        [Obsolete("Use SitemapNew method instead.")]
        Sitemap.ISitemapService Sitemap { get; }

        ISitemapsService Sitemaps { get; }

        ISitemapService SitemapNew { get; }
    }
}
