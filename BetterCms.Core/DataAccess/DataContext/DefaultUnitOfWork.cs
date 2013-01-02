using System;
using System.Data;

using NHibernate;

namespace BetterCms.Core.DataAccess.DataContext
{
    public class DefaultUnitOfWork : IUnitOfWork
    {
        private readonly ISessionFactoryProvider sessionFactoryProvider;
        private volatile ISession session;
        private ITransaction transaction;
        private bool disposed;

        public bool Disposed
        {
            get
            {
                return disposed;
            }
        }

        public ISession Session
        {
            get
            {
                CheckDisposed();
                if (session == null)
                {
                    lock (this)
                    {
                        if (session == null)
                        {
                            ISession newSession = sessionFactoryProvider.OpenSession();
                            newSession.FlushMode = FlushMode.Auto;

                            session = newSession;
                        }
                    }
                }
                return session;
            }
        }

        public bool IsActiveTransaction
        {
            get { return transaction != null && transaction.IsActive; }
        }

        ~DefaultUnitOfWork()
        {
            Dispose(false);
        }

        public DefaultUnitOfWork(ISessionFactoryProvider sessionFactoryProvider)
        {
            this.sessionFactoryProvider = sessionFactoryProvider;
            session = null;
            transaction = null;
        }

        public DefaultUnitOfWork(ISession session)
        {
            this.session = session;
            sessionFactoryProvider = null;
            transaction = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (transaction != null && transaction.IsActive && !transaction.WasRolledBack)
                    {
                        transaction.Rollback();
                        transaction.Dispose();
                    }

                    if (session != null)
                    {
                        session.Close();
                        session.Dispose();
                    }
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Commit()
        {
            CheckDisposed();

            if (transaction != null)
            {
                if (!transaction.IsActive)
                {
                    throw new InvalidOperationException("No active transaction.");
                }

                transaction.Commit();
                transaction = null;
            }
            else if (session != null)
            {
                session.Flush();
            }
        }

        public void BeginTransaction()
        {
            CheckDisposed();

            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            CheckDisposed();

            if (transaction == null)
            {
                transaction = Session.BeginTransaction(isolationLevel);
            }
            else
            {
                throw new DataException("Transaction already created for this unit of work.");
            }
        }

        public void Rollback()
        {
            CheckDisposed();

            if (transaction != null)
            {
                if (transaction.IsActive)
                {
                    transaction.Rollback();
                    transaction = null;
                }
            }
            else
            {
                throw new DataException("No transaction created for this unit of work.");
            }
        }

        private void CheckDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("UnitOfWork");
            }
        }
    }
}
