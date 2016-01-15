// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultMediaManagerApiOperations.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Operations.MediaManager.Files;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.Api.Operations.MediaManager.Folders;
using BetterCms.Module.Api.Operations.MediaManager.Folders.Folder;
using BetterCms.Module.Api.Operations.MediaManager.Images;
using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Module.Api.Operations.MediaManager.MediaTree;

namespace BetterCms.Module.Api.Operations.MediaManager
{
    /// <summary>
    /// The default media manager api operations.
    /// </summary>
    public class DefaultMediaManagerApiOperations : IMediaManagerApiOperations
    {
        public DefaultMediaManagerApiOperations(IMediaTreeService mediaTree, IImagesService images, IImageService image,
            IFilesService files, IFileService file, IFoldersService folders, IFolderService folder)
        {
            MediaTree = mediaTree;
            Folders = folders;
            Folder = folder;
            Images = images;
            Image = image;
            Files = files;
            File = file;
        }

        public IMediaTreeService MediaTree { get; private set; }

        public IFoldersService Folders { get; private set; }

        public IFolderService Folder { get; private set; }

        public IImagesService Images { get; private set; }

        public IImageService Image { get; private set; }

        public IFilesService Files { get; private set; }

        public IFileService File { get; private set; }
    }
}