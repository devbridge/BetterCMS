using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    public class ChangedContentResultViewModel
    {
        public string Title { get; set; }
        
        public Guid PageId { get; set; }

        public Guid ContentId { get; set; }

        public Guid PageContentId { get; set; }

        public Guid RegionId { get; set; }

        public ContentStatus DesirableStatus { get; set; }

        public string ContentType { get; set; }
        
        public int ContentVersion { get; set; }
        
        public int PageContentVersion { get; set; }

        public List<PageContentChildRegionViewModel> Regions { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, Title: {1}, PageId: {2}, RegionId: {3}, ContentId: {4}, PageContentId: {5}", 
                base.ToString(), Title, PageId, RegionId, ContentId, PageContentId);
        }
    }
}