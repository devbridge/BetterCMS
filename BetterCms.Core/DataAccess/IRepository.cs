using System;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataContracts;

using NHibernate;

namespace BetterCms.Core.DataAccess
{
    /// <summary>
    /// </summary>
    public interface IRepository
    {
        TEntity UnProxy<TEntity>(TEntity entity);

        TEntity AsProxy<TEntity>(Guid id) where TEntity : IEntity;

        TEntity First<TEntity>(Guid id) where TEntity : IEntity;

        TEntity First<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IEntity;

        TEntity FirstOrDefault<TEntity>(Guid id) where TEntity : IEntity;

        TEntity FirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IEntity;

        IQueryOver<TEntity, TEntity> AsQueryOver<TEntity>() where TEntity : class, IEntity;

        IQueryOver<TEntity, TEntity> AsQueryOver<TEntity>(Expression<Func<TEntity>> alias) where TEntity : class;

        IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : IEntity;

        IQueryable<TEntity> AsQueryable<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IEntity;

        bool Any<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : IEntity;

        void Save<TEntity>(TEntity entity) where TEntity : IEntity;

        void Delete<TEntity>(TEntity entity) where TEntity : IEntity;

        TEntity Delete<TEntity>(Guid id, int version, bool useProxy = true) where TEntity : IEntity;

        void Attach<TEntity>(TEntity entity) where TEntity : IEntity;

        void Detach<TEntity>(TEntity entity) where TEntity : IEntity;

        void Refresh<TEntity>(TEntity entity) where TEntity : IEntity;        
    }
}
