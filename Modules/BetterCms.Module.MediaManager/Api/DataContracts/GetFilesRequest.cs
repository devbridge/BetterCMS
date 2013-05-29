using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Api.DataContracts
{
    public class GetFilesRequest : GetDataRequest<MediaFile>
    {
        public GetFilesRequest(Expression<Func<MediaFile, bool>> filter = null,
            Expression<Func<MediaFile, dynamic>> order = null, 
            bool orderDescending = false, 
            int? itemsCount = null, 
            int startItemNumber = 1)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
            SetDefaultOrder(m => m.Title);
        }
    }
}