using System;
using System.Collections.Generic;
using System.Globalization;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Pages.ViewModels.Filter
{
    [Serializable]
    public class PagesGridViewModel<TModel> : SearchableGridViewModel<TModel> where TModel : IEditableGridItem
    {
        public IEnumerable<LookupKeyValue> Tags { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? LanguageId { get; set; }
        public IEnumerable<LookupKeyValue> Categories { get; set; }
        public IList<LookupKeyValue> Languages { get; set; }
        public IList<LookupKeyValue> Statuses { get; set; }
        public IList<LookupKeyValue> SeoStatuses { get; set; }
        public IList<LookupKeyValue> Layouts { get; set; }
        public bool IncludeArchived { get; set; }
        public bool IncludeMasterPages { get; set; }
        public bool HideMasterPagesFiltering { get; set; }
        public PageStatusFilterType? Status { get; set; }
        public SeoStatusFilterType? SeoStatus { get; set; }
        public string Layout { get; set; }

        public PagesGridViewModel(IEnumerable<TModel> items, PagesFilter filter, int totalCount, IEnumerable<LookupKeyValue> categories) : base(items, filter, totalCount)
        {
            Tags = filter.Tags;
            CategoryId = filter.CategoryId;
            LanguageId = filter.LanguageId;
            Status = filter.Status;
            SeoStatus = filter.SeoStatus;
            Layout = filter.Layout;
            Categories = categories;
            IncludeArchived = filter.IncludeArchived;
            IncludeMasterPages = filter.IncludeMasterPages;

            Statuses = PagesFilter.PageStatuses;
            SeoStatuses = PagesFilter.SeoStatuses;
        }
    }
}