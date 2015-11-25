using System;
using System.Collections.Generic;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Enums;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Blog.ViewModels.Filter
{
    public class BlogsFilter : SearchableGridOptions
    {
        public List<LookupKeyValue> Tags { get; set; }
        public List<LookupKeyValue> Categories { get; set; }
        public Guid? LanguageId { get; set; }
        public bool IncludeArchived { get; set; }
        public PageStatusFilterType? Status { get; set; }
        public SeoStatusFilterType? SeoStatus { get; set; }

        public static IList<SortAlias> SortAliases = new[]
            {
                new SortAlias(PagesGlobalization.SiteSettings_Pages_Sort_Newest, "CreatedOn", SortDirection.Descending), 
                new SortAlias(PagesGlobalization.SiteSettings_Pages_Sort_Oldest, "CreatedOn", SortDirection.Ascending), 
                new SortAlias(PagesGlobalization.SiteSettings_Pages_Sort_AZ, "Title", SortDirection.Ascending), 
                new SortAlias(PagesGlobalization.SiteSettings_Pages_Sort_ZA, "Title", SortDirection.Descending), 
                new SortAlias(PagesGlobalization.SiteSettings_Pages_Sort_Recent, "ModifiedOn", SortDirection.Descending), 
                new SortAlias(PagesGlobalization.SiteSettings_Pages_Sort_LeastRecent, "ModifiedOn", SortDirection.Ascending), 
                new SortAlias(PagesGlobalization.SiteSettings_Pages_Sort_StatusAsc, "Status", SortDirection.Ascending), 
                new SortAlias(PagesGlobalization.SiteSettings_Pages_Sort_StatusDesc, "Status", SortDirection.Descending), 

            };
    }
}