using System;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    [Serializable]
    public class PageContentChildRegionViewModel
    {
        public PageContentChildRegionViewModel()
        {
            
        }

        public PageContentChildRegionViewModel(ContentRegion contentRegion)
        {
            if (contentRegion != null)
            {
                RegionId = contentRegion.Region.Id;
                RegionIdentifier = contentRegion.Region.RegionIdentifier;
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