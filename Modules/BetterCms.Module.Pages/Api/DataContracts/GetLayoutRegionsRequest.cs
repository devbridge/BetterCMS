using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class GetLayoutRegionsRequest : GetDataRequest<LayoutRegion>
    {
        public GetLayoutRegionsRequest(Guid layoutId,
            Expression<Func<LayoutRegion, bool>> filter = null,
            Expression<Func<LayoutRegion, dynamic>> order = null, 
            bool orderDescending = false, 
            int? itemsCount = null, 
            int startItemNumber = 1)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
            LayoutId = layoutId;
        }

        public Guid LayoutId { get; set; }
    }
}