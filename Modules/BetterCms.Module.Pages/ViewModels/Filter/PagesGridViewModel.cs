using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using BetterCms.Module.Pages.Controllers;
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
        public Guid? CultureId { get; set; }
        public IEnumerable<LookupKeyValue> Categories { get; set; }
        public IList<LookupKeyValue> Cultures { get; set; }
        public bool IncludeArchived { get; set; }
        public bool IncludeMasterPages { get; set; }
        public Expression<Action<PageController>> Action { get; set; }
        public bool HideMasterPagesFiltering { get; set; }

        public PagesGridViewModel(IEnumerable<TModel> items, PagesFilter filter, int totalCount, IEnumerable<LookupKeyValue> categories) : base(items, filter, totalCount)
        {
            Tags = filter.Tags;
            CategoryId = filter.CategoryId;
            CultureId = filter.CultureId;
            Categories = categories;
            IncludeArchived = filter.IncludeArchived;
            IncludeMasterPages = filter.IncludeMasterPages;

            Action = controller => controller.Pages(null);
        }
    }
}