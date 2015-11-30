// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestoreMediaVersionCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.MediaManager.Helpers;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.History.RestoreMediaVersion
{
    public class RestoreMediaVersionCommand : CommandBase, ICommand<RestoreMediaVersionRequest, bool>
    {
        private readonly IMediaImageService imageService;

        public RestoreMediaVersionCommand(IMediaImageService imageService)
        {
            this.imageService = imageService;
        }

        public bool Execute(RestoreMediaVersionRequest request)
        {
            var imageToRevert = Repository
                .AsQueryable<MediaImage>(i => i.Id == request.VersionId)
                .Fetch(f => f.Original)

                .FirstOrDefault();

            if (imageToRevert != null)
            {
                var currentOriginal = Repository
                .AsQueryable<MediaImage>(i => imageToRevert.Original != null && i.Id == imageToRevert.Original.Id)
                .Fetch(f => f.Original)
                .FirstOrDefault();

                if (currentOriginal != null)
                {
                    var archivedImage = imageService.MoveToHistory(currentOriginal);
                    var newOriginalImage = imageService.MakeAsOriginal(imageToRevert, currentOriginal, archivedImage, request.ShouldOverridUrl);
                    Events.MediaManagerEvents.Instance.OnMediaRestored(newOriginalImage);
                }

                return true;
            }
            
            var versionToRevert = Repository.AsQueryable<Media>(p => p.Id == request.VersionId).Fetch(f => f.Original).First();

            var original = versionToRevert.Original;

            if (original != null)
            {
                UnitOfWork.BeginTransaction();
                Repository.Save(original.CreateHistoryItem());
                versionToRevert.CopyDataTo(original, false);
                MediaHelper.SetCollections(Repository, versionToRevert, original);
                original.Original = null;
                original.PublishedOn = DateTime.Now;
                Repository.Save(original);
                UnitOfWork.Commit();

                Events.MediaManagerEvents.Instance.OnMediaRestored(original);

                return true;
            }

            return false;
        }
    }
}