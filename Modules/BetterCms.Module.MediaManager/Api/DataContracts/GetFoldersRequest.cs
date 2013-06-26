using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Api.DataContracts
{
    public class GetFoldersRequest : GetDataRequest<MediaFolder>
    {
        public GetFoldersRequest(MediaType? mediaType,
            Expression<Func<MediaFolder, bool>> filter = null,
            Expression<Func<MediaFolder, dynamic>> order = null, 
            bool orderDescending = false, 
            int? itemsCount = null,
            int startItemNumber = 1,
            bool includeArchived = false)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
            SetDefaultOrder(m => m.Title);

            MediaType = mediaType;

            IncludeArchivedItems = includeArchived;
        }

        public MediaType? MediaType { get; set; }

        public bool IncludeArchivedItems { get; set; }
    }
}