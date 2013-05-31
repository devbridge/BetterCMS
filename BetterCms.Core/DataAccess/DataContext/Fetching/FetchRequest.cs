using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using NHibernate.Linq;

namespace BetterCms.Core.DataAccess.DataContext.Fetching
{
    public class FetchRequest<TQueried, TFetch> : IFetchRequest<TQueried, TFetch>
    {
        public IEnumerator<TQueried> GetEnumerator()
        {
            return this.NhFetchRequest.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.NhFetchRequest.GetEnumerator();
        }

        public Type ElementType
        {
            get
            {
                return this.NhFetchRequest.ElementType;
            }
        }

        public Expression Expression
        {
            get
            {
                return this.NhFetchRequest.Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return this.NhFetchRequest.Provider;
            }
        }

        public FetchRequest(INhFetchRequest<TQueried, TFetch> nhFetchRequest)
        {
            this.NhFetchRequest = nhFetchRequest;
        }

        public INhFetchRequest<TQueried, TFetch> NhFetchRequest { get; private set; }
    }
}
