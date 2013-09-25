using System.Web.Mvc;

using BetterCms.Module.ImagesGallery.Command.GetAlbumImages;
using BetterCms.Module.ImagesGallery.Command.GetGalleryAlbums;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Cms;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.ImagesGallery.Controllers
{
    [ActionLinkArea(ImagesGalleryModuleDescriptor.ImagesGalleryAreaName)]
    public class GalleryController : CmsControllerBase
    {
        public ActionResult Index(RenderWidgetViewModel request)
        {
            var response = GetCommand<GetGalleryAlbumsCommand>().ExecuteCommand(request);

            return View(response);
        }
        
        public ActionResult Album(string id)
        {
            var response = GetCommand<GetAlbumImagesCommand>().ExecuteCommand(id.ToGuidOrDefault());

            return View(response);
        }
    }
}