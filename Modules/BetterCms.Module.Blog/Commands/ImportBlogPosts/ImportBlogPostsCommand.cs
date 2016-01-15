// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImportBlogPostsCommand.cs" company="Devbridge Group LLC">
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
using System.Threading.Tasks;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Services.Storage;

using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Root.Mvc;

using Common.Logging;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Blog.Commands.ImportBlogPosts
{
    public class ImportBlogPostsCommand : CommandBase, ICommand<ImportBlogPostsViewModel, ImportBlogPostsResponse>
    {
        private readonly IBlogMLService importService;

        private readonly IStorageService storageService;

        public ImportBlogPostsCommand(IBlogMLService importService, IStorageService storageService)
        {
            this.importService = importService;
            this.storageService = storageService;
        }

        public ImportBlogPostsResponse Execute(ImportBlogPostsViewModel request)
        {
            var fileUri = importService.ConstructFilePath(request.FileId);
            DownloadResponse downloadResponse;
            try
            {
                downloadResponse = storageService.DownloadObject(fileUri);
            }
            catch (Exception exc)
            {
                throw new ValidationException(() => BlogGlobalization.ImportBlogPosts_FailedToRetrieveFileFileStorage_Message,
                    "Failed to retrieve blog posts import file from storage.", exc);
            }

            List<BlogPostImportResult> results;
            if (request.BlogPosts != null && request.BlogPosts.Count > 0)
            {
                downloadResponse.ResponseStream.Position = 0;
                var blogs = importService.DeserializeXMLStream(downloadResponse.ResponseStream);
                results = importService.ImportBlogs(blogs, request.BlogPosts, Context.Principal, request.CreateRedirects, request.RecreateCategoryTree, request.ReuseExistingCategories);
            }
            else
            {
                results = new List<BlogPostImportResult>();
            }

            Task.Factory
                    .StartNew(() => { })
                    .ContinueWith(task =>
                    {
                        try
                        {
                            storageService.RemoveObject(fileUri);
                        }
                        catch (Exception exc)
                        {
                            LogManager.GetCurrentClassLogger().Error("Failed to delete blog posts import file.", exc);
                        }
                    })
                    .ContinueWith(task =>
                    {
                        try
                        {
                            storageService.RemoveFolder(fileUri);
                        }
                        catch (Exception exc)
                        {
                            LogManager.GetCurrentClassLogger().Error("Failed to delete blog posts import folder.", exc);
                        }
                    });

            return new ImportBlogPostsResponse { Results = results };
        }
    }
}