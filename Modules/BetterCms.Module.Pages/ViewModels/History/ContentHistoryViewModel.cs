using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Pages.ViewModels.History
{
    public class ContentHistoryViewModel : SearchableGridViewModel<ContentHistoryItem> 
    {
        public ContentHistoryViewModel(IEnumerable<ContentHistoryItem> items, SearchableGridOptions options, int totalCount, Guid contentId)
            : base(items, options, totalCount)
        {
            ContentId = contentId;
        }
        
        public Guid ContentId { get; set; }

        public override string ToString()
        {
            return string.Format("ContentId: {0}", ContentId);
        }
    }
}