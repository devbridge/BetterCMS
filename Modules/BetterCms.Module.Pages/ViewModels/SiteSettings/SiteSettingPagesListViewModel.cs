using System;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Pages.ViewModels.SiteSettings
{
    public class SiteSettingPagesListViewModel : IEditableGridItem
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public PageStatus PageStatus { get; set; }

        public Guid? LanguageId { get; set; }

        public bool IsMasterPage { get; set; }
    }
}