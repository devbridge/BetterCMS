using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Viddler.ViewModels.Video
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