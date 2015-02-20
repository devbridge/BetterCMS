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
                results = importService.ImportBlogs(blogs, request.BlogPosts, Context.Principal, request.CreateRedirects);
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