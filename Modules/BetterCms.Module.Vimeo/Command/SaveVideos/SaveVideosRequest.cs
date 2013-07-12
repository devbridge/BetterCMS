using System;
using System.Collections.Generic;

namespace BetterCms.Module.Vimeo.Command.Videos.SaveVideos
{
    public class SaveVideosRequest
    {
        public Guid FolderId { get; set; }
        public List<string> VideosIds { get; set; }
    }
}