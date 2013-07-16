using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class GetLayoutsRequest : GetDataRequest<Layout>
    {
        public GetLayoutsRequest(Expression<Func<Layout, bool>> filter = null, 
            Expression<Func<Layout, dynamic>> order = null, 
            bool orderDescending = false, 
            int? itemsCount = null, 
            int startItemNumber = 1)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
        }
    }
}