using System;
using BetterCms.Core.Models;

namespace BetterCms.Module.MediaManager.Models.Maps
{
    [Serializable]
    public class MediaFolderDependencyMap : EntityMapBase<MediaFolderDependency>
    {
        public MediaFolderDependencyMap()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            Table("MediaFolderDependencies");

            References(f => f.Parent).Cascade.SaveUpdate().LazyLoad().Nullable();
            References(f => f.Child).Cascade.SaveUpdate().LazyLoad().Nullable();
        }
    }
}