using System;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Pages.Command.History.GetContentHistory
{
    public class GetContentHistoryRequest : SearchableGridOptions
    {
        public Guid PageContentId { get; set; }

        public int PageContentVersion { get; set; }

        public Guid ContentId { get; set; }

        public int ContentVersion { get; set; }
    }
}