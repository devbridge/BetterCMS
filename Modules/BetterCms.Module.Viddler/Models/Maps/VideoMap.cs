using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Viddler.Models.Maps
{
    [Serializable]
    public class VideoMap : EntitySubClassMapBase<Video>
    {
        public VideoMap()
            : base(ViddlerModuleDescriptor.ModuleName)
        {
            Table("Videos");

            Map(x => x.VideoId).Not.Nullable();
            Map(x => x.ThumbnailUrl).Nullable();
        }
    }
}