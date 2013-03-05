using System;
using System.Linq;
using System.Linq.Expressions;

using Autofac;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Models;

using NHibernate;
using NHibernate.Linq;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    public abstract class DataApiContext : ApiContext
    {        
        protected IUnitOfWork UnitOfWork { get; private set; }

        protected IRepository Repository { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataApiContext" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        public DataApiContext(ILifetimeScope lifetimeScope)
            : base(lifetimeScope)
        {
            Initialize();
        }

        public IQueryOver<TEntity> QueryOver<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : Entity
        {
            var query = UnitOfWork.Session.QueryOver<TEntity>().Where(e => !e.IsDeleted);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public IQueryable<TEntity> Queryable<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : Entity
        {
            var query = UnitOfWork.Session.Query<TEntity>().Where(e => !e.IsDeleted);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        private void Initialize()
        {
            if (UnitOfWork == null)
            {
                UnitOfWork = Resolve<IUnitOfWork>();
            }
            
            if (Repository == null)
            {
                Repository = Resolve<IRepository>();
            }

            if (UnitOfWork.Disposed)
            {
                throw new CmsException("UnitOfWork is disposed.");
            }
        }       
    }
}
