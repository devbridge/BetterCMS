using System;
using System.Collections.Generic;

using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Blog.ViewModels.Filter
{
    public class BlogsGridViewModel<TModel> : SearchableGridViewModel<TModel> where TModel : IEditableGridItem
    {
        public IEnumerable<LookupKeyValue> Tags { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? LanguageId { get; set; }
        public IEnumerable<LookupKeyValue> Categories { get; set; }
        public IList<LookupKeyValue> Languages { get; set; }
        public IList<LookupKeyValue> Statuses { get; set; }
        public IList<LookupKeyValue> SeoStatuses { get; set; }
        public bool IncludeArchived { get; set; }
        public PageStatusFilterType? Status { get; set; }
        public SeoStatusFilterType? SeoStatus { get; set; }

        public BlogsGridViewModel(IEnumerable<TModel> items, BlogsFilter filter, int totalCount, IEnumerable<LookupKeyValue> categories)
            : base(items, filter, totalCount)
        {
            Tags = filter.Tags;
            CategoryId = filter.CategoryId;
            LanguageId = filter.LanguageId;
            Categories = categories;
            IncludeArchived = filter.IncludeArchived;
            Status = filter.Status;
            SeoStatus = filter.SeoStatus;

            Statuses = PagesFilter.PageStatuses;
            SeoStatuses = PagesFilter.SeoStatuses;
        }
    }
}