using System;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Pages.Command.History.GetSitemapHistory
{
    public class GetSitemapHistoryRequest : SearchableGridOptions
    {
        public Guid SitemapId { get; set; }
    }
}