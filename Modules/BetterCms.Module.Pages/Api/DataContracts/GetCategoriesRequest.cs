using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class GetCategoriesRequest : GetDataRequest<Category>
    {
        public GetCategoriesRequest(Expression<Func<Category, bool>> filter = null,
            Expression<Func<Category, dynamic>> order = null, 
            bool orderDescending = false, 
            int? itemsCount = null, 
            int startItemNumber = 1)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
            SetDefaultOrder(s => s.Name);
        }
    }
}