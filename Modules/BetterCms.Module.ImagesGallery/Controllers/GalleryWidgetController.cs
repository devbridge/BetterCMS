// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GalleryWidgetController.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.ImagesGallery.Command.GetAlbum;
using BetterCms.Module.ImagesGallery.Command.GetGalleryAlbums;
using BetterCms.Module.MediaManager.Provider;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Cms;
using BetterCms.Module.Root.Mvc.Helpers;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.ImagesGallery.Controllers
{
    [ActionLinkArea(ImagesGalleryModuleDescriptor.ImagesGalleryAreaName)]
    public class GalleryWidgetController : CmsControllerBase
    {
        public ActionResult Gallery(RenderWidgetViewModel request)
        {
            var folderIdString = Request.QueryString[ImagesGalleryModuleConstants.GalleryFolderIdQueryParameterName];

            if (!string.IsNullOrWhiteSpace(folderIdString) && request.Options != null 
                && request.Options.Any(o => o.Type == OptionType.Custom
                    && o.CustomOption != null && o.CustomOption.Identifier == MediaManagerFolderOptionProvider.Identifier
                    && o.Key == ImagesGalleryModuleConstants.OptionKeys.GalleryFolder))
            {
                Guid folderId;
                if (Guid.TryParse(folderIdString, out folderId))
                {
                    var albumRequest = new GetAlbumCommandRequest
                    {
                        FolderId = folderId,
                        WidgetViewModel = request,
                        RenderBackUrl = true
                    };
                    var albumViewModel = GetCommand<GetAlbumCommand>().ExecuteCommand(albumRequest);
                    return View("Album", albumViewModel);
                }
            }

            var listViewModel = GetCommand<GetGalleryAlbumsCommand>().ExecuteCommand(request);
            return View("List", listViewModel);
        }
        
        public ActionResult Album(RenderWidgetViewModel request)
        {
            var albumRequest = new GetAlbumCommandRequest
                                   {
                                       FolderId = request.GetOptionValue<Guid?>(ImagesGalleryModuleConstants.OptionKeys.AlbumFolder), 
                                       WidgetViewModel = request, 
                                       RenderBackUrl = false
                                   };
            var albumViewModel = GetCommand<GetAlbumCommand>().ExecuteCommand(albumRequest);
            return View("Album", albumViewModel);
        }
    }
}