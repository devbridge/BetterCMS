using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Vimeo.ViewModels.Video
{
    public class VideoListSearchViewModel : SearchableGridOptions
    {
        public bool OnlyMine { get; set; }
        public VideoListSearchViewModel()
        {
            SetDefaultPaging();
            OnlyMine = true;
        }
    }
}