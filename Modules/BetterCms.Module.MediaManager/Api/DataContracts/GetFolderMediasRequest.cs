using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Api.DataContracts
{
    public class GetFolderMediasRequest : GetDataRequest<Media>
    {
        public GetFolderMediasRequest(MediaType mediaType,
            Guid? folderId = null,
            Expression<Func<Media, bool>> filter = null, 
            Expression<Func<Media, dynamic>> order = null, 
            bool orderDescending = false, 
            int? itemsCount = null, 
            int startItemNumber = 1)
            : base(filter, order, orderDescending, itemsCount, startItemNumber)
        {
            SetDefaultOrder(m => m.Title);

            MediaType = mediaType;
            FolderId = folderId;
        }

        public MediaType MediaType { get; set; }

        public Guid? FolderId { get; set; }
    }
}