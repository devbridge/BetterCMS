using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.Api.DataContracts
{
    public class GetAuthorsRequest : GetDataRequest<Author>
    {
        public GetAuthorsRequest(Expression<Func<Author, bool>> filter = null, 
            Expression<Func<Author, dynamic>> order = null, 
            bool orderDescending = false, 
            int? itemsCount = null, 
            int startItemNumber = 1)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
        }
    }
}