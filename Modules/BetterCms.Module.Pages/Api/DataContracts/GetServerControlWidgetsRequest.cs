using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class GetServerControlWidgetsRequest : GetDataRequest<ServerControlWidget>
    {
        public GetServerControlWidgetsRequest(Expression<Func<ServerControlWidget, bool>> filter = null,
            Expression<Func<ServerControlWidget, dynamic>> order = null, 
            bool orderDescending = false, 
            int? itemsCount = null, 
            int startItemNumber = 1)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
            SetDefaultOrder(s => s.Name);
        }
    }
}