using System.Collections.Generic;

namespace BetterCms.Module.ImagesGallery.ViewModels
{
    public class GalleryViewModel
    {
        public IList<AlbumEditViewModel> Albums { get; set; }

        public string AlbumUrl { get; set; }
    }
}