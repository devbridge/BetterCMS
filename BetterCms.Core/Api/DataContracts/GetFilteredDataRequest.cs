using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BetterCms.Core.Api.DataContracts
{
    public abstract class GetFilteredDataRequest<TModel>
    {
        private IList<OrderByModel<TModel>> orderBy;

        public GetFilteredDataRequest(Expression<Func<TModel, bool>> filter = null,
            Expression<Func<TModel, dynamic>> order = null,
            bool orderDescending = false)
        {
            Filter = filter;

            if (order != null)
            {
                SetDefaultOrder(order, orderDescending);
            }
        }

        public Expression<Func<TModel, bool>> Filter { get; set; }

        public IList<OrderByModel<TModel>> OrderBy
        {
            get
            {
                if (orderBy == null)
                {
                    orderBy = new List<OrderByModel<TModel>>();
                }

                return orderBy;
            }
        }

        public void SetDefaultOrder(Expression<Func<TModel, dynamic>> order, bool orderByDescending = false)
        {
            // TODO: Cannot set default order from constructor anymore: ordering properties can be added later -> need to set default order from service methods
//            if (OrderBy.Count == 0)
//            {
//                AddOrder(order, orderByDescending);
//            }
        }

        public void AddOrder(Expression<Func<TModel, dynamic>> order, bool orderByDescending = false)
        {
            OrderBy.Add(new OrderByModel<TModel>(order, orderByDescending));
        }
    }
}
