using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class GetWidgetsRequest : GetDataRequest<Widget>
    {
        public GetWidgetsRequest(Expression<Func<Widget, bool>> filter = null,
            Expression<Func<Widget, dynamic>> order = null, 
            bool orderDescending = false, 
            int? itemsCount = null, 
            int startItemNumber = 1)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
            SetDefaultOrder(w => w.Name);
        }
    }
}