// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadBlogsImportFileCommand.cs" company="Devbridge Group LLC">
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
using System.IO;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Services.Storage;

using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Blog.Commands.UploadBlogsImportFile
{
    public class UploadBlogsImportFileCommand : CommandBase, ICommand<UploadImportFileViewModel, UploadBlogsImportFileResponse>
    {
        private readonly IBlogMLService importService;
        
        private readonly IStorageService storageService;

        public UploadBlogsImportFileCommand(IBlogMLService importService, IStorageService storageService)
        {
            this.importService = importService;
            this.storageService = storageService;
        }

        public UploadBlogsImportFileResponse Execute(UploadImportFileViewModel request)
        {
            var fileId = Guid.NewGuid();
            var fileUri = importService.ConstructFilePath(fileId);

            using (var stream = new MemoryStream())
            {
                request.FileStream.CopyTo(stream);

                try
                {
                    var upload = new UploadRequest();
                    upload.CreateDirectory = true;
                    upload.Uri = fileUri;
                    upload.InputStream = request.FileStream;
                    upload.IgnoreAccessControl = true;

                    storageService.UploadObject(upload);
                }
                catch (Exception exc)
                {
                    throw new ValidationException(() => BlogGlobalization.ImportBlogPosts_FailedToSaveFileToStorage_Message,
                            "Failed to save blog posts import file to storage.",
                        exc);
                }

                stream.Position = 0;
                var blogs = importService.DeserializeXMLStream(stream);
                var results = importService.ValidateImport(blogs);

                return new UploadBlogsImportFileResponse { Results = results, FileId = fileId };
            }
        }
    }
}