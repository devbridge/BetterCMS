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