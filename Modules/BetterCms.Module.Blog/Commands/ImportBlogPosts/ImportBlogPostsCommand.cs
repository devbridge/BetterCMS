using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services.Storage;
using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Root.Mvc;

using BlogML.Xml;

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

            var blogs = importService.DeserializeXMLStream(downloadResponse.ResponseStream);
            var results = importService.ImportBlogs(blogs, request.BlogPosts, Context.Principal, request.CreateRedirects);

            return new ImportBlogPostsResponse { Results = results };
        }
    }
}