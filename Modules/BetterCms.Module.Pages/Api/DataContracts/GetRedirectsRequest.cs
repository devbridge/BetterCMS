using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class GetRedirectsRequest : GetDataRequest<Redirect>
    {
        public GetRedirectsRequest(Expression<Func<Redirect, bool>> filter = null,
            Expression<Func<Redirect, dynamic>> order = null, 
            bool orderDescending = false, 
            int? itemsCount = null, 
            int startItemNumber = 1)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
            SetDefaultOrder(s => s.PageUrl);
        }
    }
}