// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetMultiFileUploadCommand.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Core.Security;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.Upload;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.DataAccess.DataContext;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Upload.GetMultiFileUpload
{
    public class GetMultiFileUploadCommand : CommandBase, ICommand<GetMultiFileUploadRequest, MultiFileUploadViewModel>
    {
        private readonly ICmsConfiguration cmsConfiguration;

        public GetMultiFileUploadCommand(ICmsConfiguration cmsConfiguration)
        {
            this.cmsConfiguration = cmsConfiguration;
        }

        public MultiFileUploadViewModel Execute(GetMultiFileUploadRequest request)
        {
            var model = new MultiFileUploadViewModel();
            model.RootFolderId = request.FolderId;
            model.RootFolderType = request.Type;
            model.ReuploadMediaId = request.ReuploadMediaId;
            model.SelectedFolderId = Guid.Empty;
            model.UploadedFiles = null;

            var foldersQuery = Repository.AsQueryable<MediaFolder>().Where(f => f.Type == request.Type);
            if (request.FolderId == Guid.Empty)
            {
                foldersQuery = foldersQuery.Where(f => f.Folder == null);
            }
            else
            {
                foldersQuery = foldersQuery.Where(f => f.Folder.Id == request.FolderId);
            }

            model.Folders = foldersQuery
                .OrderBy(f => f.Title)
                .Select(f => new Tuple<Guid, string>(f.Id, f.Title))
                .ToList();

            model.Folders.Insert(0, new Tuple<Guid, string>(Guid.Empty, ".."));

            if (request.Type != MediaType.Image && cmsConfiguration.Security.AccessControlEnabled)
            {
                if (!request.ReuploadMediaId.HasDefaultValue())
                {
                    var media = Repository.AsQueryable<Media>(m => m.Id == request.ReuploadMediaId).FirstOne();
                    var file = media as MediaFile;
                    if (file != null)
                    {
                        AccessControlService.DemandAccess(file, Context.Principal, AccessLevel.ReadWrite);
                    }
                }
                else
                {
                    var principal = SecurityService.GetCurrentPrincipal();
                    model.UserAccessList = AccessControlService.GetDefaultAccessList(principal).Cast<UserAccessViewModel>().ToList();
                    model.AccessControlEnabled = cmsConfiguration.Security.AccessControlEnabled;                    
                }

            }
            return model;
        }
    }
}