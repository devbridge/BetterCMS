// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportBlogPostsCommand.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Filter;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Blog.Commands.ExportBlogPostsCommand
{
    public class ExportBlogPostsCommand : CommandBase, ICommand<BlogsFilter, string>
    {
        private readonly IBlogMLExportService exportService;
        
        private readonly IBlogService blogService;

        public ExportBlogPostsCommand(IBlogMLExportService exportService, IBlogService blogService)
        {
            this.exportService = exportService;
            this.blogService = blogService;
        }

        public string Execute(BlogsFilter request)
        {
            // Get all ONLY PUBLISHED blog posts, filtered out by the request filter
            var query = blogService.GetFilteredBlogPostsQuery(request, true);
            var blogPosts = query.Where(p => p.Status == PageStatus.Published).AddOrder(request).List().Distinct().ToList();

            var xml = exportService.ExportBlogPosts(blogPosts.ToList());

            return xml;
        }
    }
}