using System;
using System.Data;

using NHibernate;

namespace BetterCms.Core.DataAccess.DataContext
{
    public interface IUnitOfWork : IDisposable
    {
        ISession Session { get; }
        void Commit();
        bool IsActiveTransaction { get; }
        bool Disposed { get; }
        void BeginTransaction();
        void BeginTransaction(IsolationLevel isolationLevel);
        void Rollback();
    }
}