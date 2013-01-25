using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Pages.ViewModels.History
{
    public class PageContentHistoryViewModel : SearchableGridViewModel<PageContentHistoryItem> 
    {
        public PageContentHistoryViewModel(IEnumerable<PageContentHistoryItem> items, SearchableGridOptions options, int totalCount, Guid contentId, Guid pageContentId)
            : base(items, options, totalCount)
        {
            ContentId = contentId;
            PageContentId = pageContentId;
        }

        public Guid PageContentId { get; set; }
        
        public Guid ContentId { get; set; }

        public override string ToString()
        {
            return string.Format("ContentId: {0}", ContentId);
        }
    }
}