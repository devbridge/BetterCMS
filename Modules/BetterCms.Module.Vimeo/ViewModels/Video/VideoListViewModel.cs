using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Vimeo.ViewModels.Video
{
    public class VideoListViewModel : VimeoSearchableGridViewModel<VideoViewModel>
    {
        public bool OnlyMine { get; set; }
        public VideoListViewModel(IEnumerable<VideoViewModel> items, SearchableGridOptions options, int totalCount) : 
            base(items, options, totalCount)
        {
            
        }
    }
}