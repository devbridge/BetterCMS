// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagesFilter.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Enums;
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
        
        public static IList<SortAlias> SortAliases = new []
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