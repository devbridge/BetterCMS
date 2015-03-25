using System;
using System.Collections.Generic;
using System.Globalization;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Pages.ViewModels.Filter
{
    public class PagesFilter : SearchableGridOptions
    {
        public List<LookupKeyValue> Tags { get; set; }
        public List<LookupKeyValue> Categories { get; set; }
        public Guid? LanguageId { get; set; }
        public Guid? ContentId { get; set; }
        public bool IncludeArchived { get; set; }
        public bool OnlyMasterPages { get; set; }
        public bool IncludeMasterPages { get; set; }
        public PageStatusFilterType? Status { get; set; }
        public SeoStatusFilterType? SeoStatus { get; set; }
        public string Layout { get; set; }

        public static IList<LookupKeyValue> PageStatuses = new[]
            {
                new LookupKeyValue(((int)PageStatusFilterType.OnlyPublished).ToString(CultureInfo.InvariantCulture), PagesGlobalization.PageStatusFilterType_Published),
                new LookupKeyValue(((int)PageStatusFilterType.OnlyUnpublished).ToString(CultureInfo.InvariantCulture), PagesGlobalization.PageStatusFilterType_Unpublished),
                new LookupKeyValue(((int)PageStatusFilterType.ContainingUnpublishedContents).ToString(CultureInfo.InvariantCulture), PagesGlobalization.PageStatusFilterType_ContainingUnpublishedContents)
            };

        public static IList<LookupKeyValue> SeoStatuses = new[]
            {
                new LookupKeyValue(((int)SeoStatusFilterType.HasSeo).ToString(CultureInfo.InvariantCulture), PagesGlobalization.SeoStatusFilterType_HasSEO),
                new LookupKeyValue(((int)SeoStatusFilterType.HasNotSeo).ToString(CultureInfo.InvariantCulture), PagesGlobalization.SeoStatusFilterType_HasNotSEO),
            };
    }
}