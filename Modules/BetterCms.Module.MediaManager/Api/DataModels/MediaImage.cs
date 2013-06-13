using System;

namespace BetterCms.Module.MediaManager.Api.DataModels
{
    public class MediaImage
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string PublicUrl { get; set; }
        public string PublicThumbnailUrl { get; set; }
        public string Caption { get; set; }
    }
}