using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class GetNodesRequest : GetDataRequest<SitemapNode>
    {
        public GetNodesRequest(Expression<Func<SitemapNode, bool>> filter = null, 
            Expression<Func<SitemapNode, dynamic>> order = null, 
            bool orderDescending = false, 
            int? itemsCount = null, 
            int startItemNumber = 1)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
        }
    }
}