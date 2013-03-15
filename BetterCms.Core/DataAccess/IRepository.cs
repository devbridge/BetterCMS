using System;
using System.Linq;
using System.Linq.Expressions;
using BetterCms.Core.Models;

using NHibernate;

namespace BetterCms.Core.DataAccess
{
    /// <summary>
    /// </summary>
    public interface IRepository
    {
        TEntity UnProxy<TEntity>(TEntity entity);

        TEntity AsProxy<TEntity>(Guid id) where TEntity : Entity;

        TEntity First<TEntity>(Guid id) where TEntity : Entity;

        TEntity First<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : Entity;

        TEntity FirstOrDefault<TEntity>(Guid id) where TEntity : Entity;

        TEntity FirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : Entity;

        IQueryOver<TEntity, TEntity> AsQueryOver<TEntity>() where TEntity : Entity;

        IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : Entity;

        IQueryable<TEntity> AsQueryable<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : Entity;

        bool Any<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : Entity;

        void Save<TEntity>(TEntity entity) where TEntity : Entity;

        void Delete<TEntity>(TEntity entity) where TEntity : Entity;

        TEntity Delete<TEntity>(Guid id, int version, bool useProxy = true) where TEntity : Entity;

        void Attach<TEntity>(TEntity entity) where TEntity : Entity;

        void Detach<TEntity>(TEntity entity) where TEntity : Entity;

        void Refresh<TEntity>(TEntity entity) where TEntity : Entity;        
    }
}
