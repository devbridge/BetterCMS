using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts;
using BetterCms.Core.Exceptions.DataTier;

using NHibernate;
using NHibernate.Linq;
using NHibernate.Proxy;

namespace BetterCms.Core.DataAccess
{
    public class DefaultRepository : IRepository, IUnitOfWorkRepository
    {
        private readonly IUnitOfWork defaultUnitOfWork;
        private IUnitOfWork unitOfWork;

        private IUnitOfWork UnitOfWork
        {
            get
            {
                if (unitOfWork != null && !unitOfWork.Disposed)
                {
                    return unitOfWork;
                }

                if (defaultUnitOfWork != null && !defaultUnitOfWork.Disposed)
                {
                    return defaultUnitOfWork;
                }

                throw new DataException(string.Format("Repository {0} has no assigned unit of work or it was disposed.", GetType().Name));
            }
        }

        public DefaultRepository(IUnitOfWork unitOfWork)
        {
            defaultUnitOfWork = unitOfWork;
        }

        public void Use(IUnitOfWork unitOfWorkToUse)
        {
            unitOfWork = unitOfWorkToUse;
        }

        public TEntity UnProxy<TEntity>(TEntity entity)
        {
            INHibernateProxy proxy = entity as INHibernateProxy;
            if (proxy != null)
            {
               return (TEntity)proxy.HibernateLazyInitializer.GetImplementation();
            }

            return entity;
        }

        public virtual TEntity AsProxy<TEntity>(Guid id) where TEntity : IEntity
        {
            return UnitOfWork.Session.Load<TEntity>(id, LockMode.None);
        }

        public virtual TEntity First<TEntity>(Guid id) where TEntity : IEntity
        {
            TEntity entity = FirstOrDefault<TEntity>(id);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual TEntity First<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IEntity
        {
            TEntity entity = FirstOrDefault(filter);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), filter.ToString());
            }

            return entity;
        }

        public virtual TEntity FirstOrDefault<TEntity>(Guid id) where TEntity : IEntity
        {
            return AsQueryable<TEntity>().FirstOrDefault(f => f.Id == id);
        }

        public virtual TEntity FirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IEntity
        {
            return AsQueryable<TEntity>().Where(filter).FirstOrDefault();
        }

        public IQueryOver<TEntity, TEntity> AsQueryOver<TEntity>() where TEntity : class, IEntity
        {
            return UnitOfWork.Session.QueryOver<TEntity>().Where(f => !f.IsDeleted);
        }

        public IQueryOver<TEntity, TEntity> AsQueryOver<TEntity>(Expression<Func<TEntity>> alias = null) where TEntity : class
        {
            if (alias != null)
            {
                return UnitOfWork.Session.QueryOver(alias);
            }

            return UnitOfWork.Session.QueryOver<TEntity>();
        }

        public virtual IQueryable<TEntity> AsQueryable<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IEntity
        {
            return AsQueryable<TEntity>().Where(filter);
        }

        public virtual IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : IEntity
        {
            return UnitOfWork.Session.Query<TEntity>().Where(f => !f.IsDeleted);
        }

        public virtual bool Any<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IEntity
        {
            return AsQueryable<TEntity>().Where(filter).Any();
        }

        public virtual void Save<TEntity>(TEntity entity) where TEntity : IEntity
        {
            UnitOfWork.Session.SaveOrUpdate(entity);
        }

        public virtual void Delete<TEntity>(TEntity entity) where TEntity : IEntity
        {
            UnitOfWork.Session.Delete(entity);
        }

        public virtual TEntity Delete<TEntity>(Guid id, int version, bool useProxy = true) where TEntity : IEntity
        {
            TEntity entity = useProxy
                                ? AsProxy<TEntity>(id)
                                : First<TEntity>(id);

            entity.Version = version;
            UnitOfWork.Session.Delete(entity);

            return entity;
        }

        public virtual void Attach<TEntity>(TEntity entity) where TEntity : IEntity
        {
            UnitOfWork.Session.Lock(entity, LockMode.None);
        }

        public virtual void Detach<TEntity>(TEntity entity) where TEntity : IEntity
        {
            UnitOfWork.Session.Evict(entity);
        }

        public void Refresh<TEntity>(TEntity entity) where TEntity : IEntity
        {
            UnitOfWork.Session.Refresh(entity);
        }        
    }
}
