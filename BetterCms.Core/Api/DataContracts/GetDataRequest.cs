using System;
using System.Linq.Expressions;

namespace BetterCms.Core.Api.DataContracts
{
    public abstract class GetDataRequest<TModel> : GetFilteredDataRequest<TModel>
    {
        public GetDataRequest(Expression<Func<TModel, bool>> filter = null,
            Expression<Func<TModel, dynamic>> order = null,
            bool orderDescending = false,
            int? itemsCount = null,
            int startItemNumber = 1)
            : base(filter, order, orderDescending)
        {
            StartItemNumber = startItemNumber;
            ItemsCount = itemsCount;
        }

        public int StartItemNumber { get; set; }

        public int? ItemsCount { get; set; }

        public void AddPaging(int itemsPerPage, int pageNumber = 1)
        {
            if (itemsPerPage <= 0)
            {
                throw new InvalidOperationException("Items per page count must be greater than zero.");
            }

            if (pageNumber <= 0)
            {
                throw new InvalidOperationException("Page number must be greater than zero.");
            }

            StartItemNumber = (pageNumber - 1) * itemsPerPage + 1;
            ItemsCount = itemsPerPage;
        }
    }
}