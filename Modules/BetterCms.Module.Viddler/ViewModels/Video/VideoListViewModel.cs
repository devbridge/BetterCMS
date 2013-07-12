using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Viddler.ViewModels.Video
{
    public class VideoListViewModel : ViddlerSearchableGridViewModel<VideoViewModel>
    {
        public bool OnlyMine { get; set; }
        public VideoListViewModel(IEnumerable<VideoViewModel> items, SearchableGridOptions options, int totalCount) : 
            base(items, options, totalCount)
        {
            
        }
    }
}