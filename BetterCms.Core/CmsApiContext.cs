using System;
using System.Linq;
using System.Linq.Expressions;

using Autofac;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Models;

using NHibernate;
using NHibernate.Linq;

namespace BetterCms.Core
{
    public class CmsApiContext : IDisposable
    {
        protected readonly ILifetimeScope container;
        protected IUnitOfWork unitOfWork;

        public IQueryOver<TEntity> QueryOver<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : Entity
        {
            InitializeSession();

            var query = unitOfWork.Session.QueryOver<TEntity>().Where(e => !e.IsDeleted);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public IQueryable<TEntity> Queryable<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : Entity
        {
            InitializeSession();

            var query = unitOfWork.Session.Query<TEntity>().Where(e => !e.IsDeleted);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        private void InitializeSession()
        {
            if (unitOfWork == null && container.IsRegistered<IUnitOfWork>())
            {
                unitOfWork = container.Resolve<IUnitOfWork>();
            }
            if (unitOfWork == null)
            {
                throw new CmsException("UnitOfWork was not initialized.");
            }
            if (unitOfWork.Disposed)
            {
                throw new CmsException("UnitOfWork is disposed.");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsApiContext" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public CmsApiContext(ILifetimeScope container)
        {
            this.container = container;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (unitOfWork != null)
            {
                unitOfWork.Dispose();
            }
            if (container != null)
            {
                container.Dispose();
            }
        }
    }
}
