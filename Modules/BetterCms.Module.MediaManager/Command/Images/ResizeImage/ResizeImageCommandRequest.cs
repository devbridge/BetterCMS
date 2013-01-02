using System;

namespace BetterCms.Module.MediaManager.Command.Images
{
    public class ResizeImageCommandRequest
    {
        public Guid Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Version { get; set; }
    }
}
