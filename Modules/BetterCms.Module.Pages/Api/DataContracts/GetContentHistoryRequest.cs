using System;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class GetContentHistoryRequest : GetFilteredDataRequest<Root.Models.Content>
    {
        public GetContentHistoryRequest(Guid contentId,
            Expression<Func<Root.Models.Content, bool>> filter = null,
            Expression<Func<Root.Models.Content, dynamic>> order = null, 
            bool orderDescending = false)
            : base(filter, order, orderDescending)
        {
            ContentId = contentId;
        }

        public Guid ContentId { get; set; }
    }
}