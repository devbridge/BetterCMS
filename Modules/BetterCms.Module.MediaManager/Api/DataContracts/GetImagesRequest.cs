using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Api.DataContracts
{
    public class GetImagesRequest : GetDataRequest<MediaImage>
    {
        public GetImagesRequest(Expression<Func<MediaImage, bool>> filter = null,
            Expression<Func<MediaImage, dynamic>> order = null, 
            bool orderDescending = false, 
            int? itemsCount = null, 
            int startItemNumber = 1,
            bool includeArchived = false)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
            IncludeArchivedItems = includeArchived;
        }

        public bool IncludeArchivedItems { get; set; }
    }
}