using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Vimeo.Models.Maps
{
    [Serializable]
    public class VideoMap : EntitySubClassMapBase<Video>
    {
        public VideoMap()
            : base(VimeoModuleDescriptor.ModuleName)
        {
            Table("Videos");

            Map(x => x.VideoId).Not.Nullable();
            Map(x => x.ThumbnailUrl).Nullable();
            Map(x => x.AuthorName).Nullable();
            Map(x => x.AuthorUrl).Nullable();
        }
    }
}