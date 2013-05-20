using System;
using System.Linq;
using System.Linq.Expressions;

using Autofac;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Models;

using NHibernate;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    public abstract class DataApiContext : ApiContext
    {        
        protected IRepository Repository { get; private set; }

        protected IUnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataApiContext" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        protected DataApiContext(ILifetimeScope lifetimeScope, IRepository repository = null, IUnitOfWork unitOfWork = null)
            : base(lifetimeScope)
        {
            if (repository == null)
            {
                Repository = Resolve<IRepository>();
            }
            else
            {
                Repository = repository;
            }

            if (unitOfWork == null)
            {
                UnitOfWork = Resolve<IUnitOfWork>();
            }
            else
            {
                UnitOfWork = unitOfWork;
            }
        }        

        public IQueryOver<TEntity> QueryOver<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : Entity
        {
            var query = Repository.AsQueryOver<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public IQueryable<TEntity> Queryable<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : Entity
        {
            var query = Repository.AsQueryable<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }
    }
}
