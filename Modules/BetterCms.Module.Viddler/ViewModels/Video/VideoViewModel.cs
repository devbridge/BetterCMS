using System;

using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Viddler.ViewModels.Video
{
    public class VideoViewModel: IEditableGridItem
    {
        public Guid Id { get; set; }

        public int Version { get; set; }


        public string VideoId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TimeSpan Duration { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public string ThumbnailUrl { get; set; }

        public string OwnerName { get; set; }

        public bool IsPublic { get; set; }

        public bool IsTranscoding { get; set; }
    }
}