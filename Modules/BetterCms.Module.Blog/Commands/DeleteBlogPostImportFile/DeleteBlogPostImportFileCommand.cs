// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteBlogPostImportFileCommand.cs" company="Devbridge Group LLC">
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
using System.Threading.Tasks;

using BetterCms.Core.Services.Storage;

using BetterCms.Module.Blog.Services;
using BetterCms.Module.Root.Mvc;

using Common.Logging;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Blog.Commands.DeleteBlogPostImportFile
{
    public class DeleteBlogPostImportFileCommand : CommandBase, ICommand<Guid, bool>
    {
        private readonly IBlogMLService importService;

        private readonly IStorageService storageService;

        public DeleteBlogPostImportFileCommand(IBlogMLService importService, IStorageService storageService)
        {
            this.importService = importService;
            this.storageService = storageService;
        }

        public bool Execute(Guid fileId)
        {
            var fileUri = importService.ConstructFilePath(fileId);

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

            return true;
        }
    }
}