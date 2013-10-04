using System;
using System.Web.Mvc;

using BetterCms.Module.ImagesGallery.Command.GetAlbum;
using BetterCms.Module.ImagesGallery.Command.GetGalleryAlbums;

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
            var albumIdString = Request.QueryString[ImagesGalleryModuleConstants.GalleryAlbumIdQueryParameterName];

            if (!string.IsNullOrEmpty(albumIdString))
            {
                Guid albumId;
                if (Guid.TryParse(albumIdString, out albumId))
                {
                    var albumRequest = new GetAlbumCommandRequest
                                           {
                                               AlbumId = albumId, 
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
                                       AlbumId = request.GetOptionValue<Guid>(ImagesGalleryModuleConstants.AlbumWidgetAlbumOptionKey), 
                                       WidgetViewModel = request, 
                                       RenderBackUrl = false
                                   };
            var albumViewModel = GetCommand<GetAlbumCommand>().ExecuteCommand(albumRequest);
            return View("Album", albumViewModel);
        }
    }
}