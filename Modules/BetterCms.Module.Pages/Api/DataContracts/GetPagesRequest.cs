using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Pages.DataContracts.Enums;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class GetPagesRequest : GetDataRequest<PageProperties>
    {
        public GetPagesRequest(
            Expression<Func<PageProperties, bool>> filter = null,
            Expression<Func<PageProperties, dynamic>> order = null, 
            bool orderDescending = false, 
            int? itemsCount = null, 
            int startItemNumber = 1,
            PageLoadableChilds loadChilds = PageLoadableChilds.None, 
            bool includeUnpublished = false, 
            bool includePrivate = false)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
            SetDefaultOrder(s => s.Title);

            LoadChilds = loadChilds;
            IncludeUnpublished = includeUnpublished;
            IncludePrivate = includePrivate;
        }

        public PageLoadableChilds LoadChilds { get; set; }

        public bool IncludeUnpublished { get; set; }

        public bool IncludePrivate { get; set; }
    }
}