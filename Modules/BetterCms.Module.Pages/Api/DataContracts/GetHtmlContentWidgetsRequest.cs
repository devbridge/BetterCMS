using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class GetHtmlContentWidgetsRequest : GetDataRequest<HtmlContentWidget>
    {
        public GetHtmlContentWidgetsRequest(Expression<Func<HtmlContentWidget, bool>> filter = null,
            Expression<Func<HtmlContentWidget, dynamic>> order = null, 
            bool orderDescending = false, 
            int? itemsCount = null, 
            int startItemNumber = 1)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
            SetDefaultOrder(s => s.Name);
        }
    }
}