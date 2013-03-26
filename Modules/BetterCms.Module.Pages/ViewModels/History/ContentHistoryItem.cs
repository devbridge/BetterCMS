using System;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Pages.ViewModels.History
{    
    public class ContentHistoryItem : IEditableGridItem
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? PublishedOn { get; set; }

        public string PublishedByUser { get; set; }

        public DateTime? ArchivedOn { get; set; }

        public string ArchivedByUser { get; set; }

        public TimeSpan? DisplayedFor { get; set; }

        public ContentStatus Status { get; set; }

        public string StatusName { get; set; }

        public bool CanCurrentUserRestoreIt { get; set; }
    }
}