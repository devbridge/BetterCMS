using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.ImagesGallery.Command.GetAlbum
{
    public class GetAlbumCommandRequest
    {
        public System.Guid AlbumId { get; set; }

        public RenderWidgetViewModel WidgetViewModel { get; set; }
    }
}