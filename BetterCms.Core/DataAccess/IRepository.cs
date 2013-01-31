using System;
using System.Linq;
using System.Linq.Expressions;
using BetterCms.Core.Models;

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

        IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : Entity;

        IQueryable<TEntity> AsQueryable<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : Entity;

        bool Any<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : Entity;

        void Save<TEntity>(TEntity entity) where TEntity : Entity;

        void Delete<TEntity>(TEntity entity) where TEntity : Entity;

        void Delete<TEntity>(Guid id, int version) where TEntity : Entity;

        void Attach<TEntity>(TEntity entity) where TEntity : Entity;

        void Detach<TEntity>(TEntity entity) where TEntity : Entity;

        void Refresh<TEntity>(TEntity entity) where TEntity : Entity;        
    }
}
