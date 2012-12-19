using System;

namespace BetterCms.Module.Root.ViewModels.Cms
{
    [Serializable]
    public class PageRegionViewModel
    {
        public Guid RegionId { get; set; }

        public string RegionIdentifier { get; set; }

        public override string ToString()
        {
            return string.Format("RegionId: {0}, RegionIdentifier: {1}", RegionId, RegionIdentifier);
        }
    }
}