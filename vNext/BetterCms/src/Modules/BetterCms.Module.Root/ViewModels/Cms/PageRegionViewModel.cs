using System;

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.ViewModels.Cms
{
    [Serializable]
    public class PageRegionViewModel
    {
        public PageRegionViewModel()
        {
        }

        public PageRegionViewModel(IRegion region)
        {
            if (region != null)
            {
                RegionId = region.Id;
                RegionIdentifier = region.RegionIdentifier;
            }
        }

        public Guid RegionId { get; set; }

        public string RegionIdentifier { get; set; }

        public override string ToString()
        {
            return string.Format("RegionId: {0}, RegionIdentifier: {1}", RegionId, RegionIdentifier);
        }
    }
}