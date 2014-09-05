using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    public class InsertContentToPageResultViewModel
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
    }
}