using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Pages.ViewModels.History
{
    public class PageContentHistoryViewModel : SearchableGridViewModel<PageContentHistoryItem> 
    {
        public PageContentHistoryViewModel(IEnumerable<PageContentHistoryItem> items, SearchableGridOptions options, int totalCount, Guid pageContentId)
            : base(items, options, totalCount)
        {
            PageContentId = pageContentId;
        }

        public Guid PageContentId { get; set; }

        public override string ToString()
        {
            return string.Format("PageContentId: {0}", PageContentId);
        }
    }
}