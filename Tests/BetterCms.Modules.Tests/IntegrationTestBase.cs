using System;
using System.Transactions;

using Autofac;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Models;

using FluentNHibernate.Testing;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module
{
    public abstract class IntegrationTestBase : TestBase
    {
        protected void RunEntityMapTestsInTransaction<TEntity>(TEntity testEntity, Action<TEntity> resultAssertions = null, ILifetimeScope childScope = null) where TEntity : Entity
        {  
            var sessionFactory = (childScope ?? Container).Resolve<ISessionFactoryProvider>();
            using (var session = sessionFactory.OpenSession())
            {
                using (new TransactionScope())
                {
                    var result = new PersistenceSpecification<TEntity>(session).VerifyTheMappings(testEntity);
                    if (resultAssertions != null)
                    {
                        resultAssertions(result);
                    }
                }
            }
        }

        protected void SaveEntityAndRunAssertionsInTransaction<TEntity>(
            TEntity entity, 
            Action<TEntity> resultAssertions = null, 
            Action<TEntity> assertionsBeforeSave = null, 
            Action<TEntity> assertionsAfterSave = null,
            ILifetimeScope childScope = null) where TEntity : Entity
        {
            RunDatabaseActionAndAssertionsInTransaction(entity, 
                session => session.SaveOrUpdate(entity),
                (result, session) =>
                {
                    if (resultAssertions != null)
                    {
                        resultAssertions(result);
                    }
                },
                 (result, session) =>
                 {
                     if (assertionsBeforeSave != null)
                     {
                         assertionsBeforeSave(result);
                     }
                 },
                (result, session) =>
                {
                    if (assertionsAfterSave != null)
                    {
                        assertionsAfterSave(result);
                    }
                },
                childScope);
        }

        protected void DeleteCreatedEntityAndRunAssertionsInTransaction<TEntity>(
            TEntity entity,
            Action<TEntity> resultAssertions = null,
            Action<TEntity> assertionsBeforeSave = null,
            Action<TEntity> assertionsAfterSave = null,
            ILifetimeScope childScope = null) where TEntity : Entity
        {
            RunDatabaseActionAndAssertionsInTransaction(entity, session =>
                {
                    session.SaveOrUpdate(entity);
                    session.Flush();
                    session.Delete(entity);
                    session.Flush();
                },
                (result, session) =>
                    {
                        if (resultAssertions != null)
                        {
                            resultAssertions(result);
                        }
                    },
                (result, session) =>
                    {
                        if (assertionsBeforeSave != null)
                        {
                            assertionsBeforeSave(result);
                        }
                    },
                (result, session) =>
                    {
                        if (assertionsAfterSave != null)
                        {
                            assertionsAfterSave(result);
                        }
                    },
                childScope);
        }

        protected void RunDatabaseActionAndAssertionsInTransaction<TEntity>(TEntity entity, 
            Action<ISession> databaseAction = null,
            Action<TEntity, ISession> resultAssertions = null, 
            Action<TEntity, ISession> assertionsBeforeSave = null, 
            Action<TEntity, ISession> assertionsAfterSave = null,
            ILifetimeScope childScope = null) where TEntity : Entity
        {
            if (databaseAction == null)
            {
                Assert.Fail("No database action specified.");
            }

            if (resultAssertions == null && assertionsBeforeSave == null && assertionsAfterSave == null) 
            {
                Assert.Fail("No assertion specified!");
            }

            var sessionFactory = (childScope ?? Container).Resolve<ISessionFactoryProvider>();

            using (var session = sessionFactory.OpenSession())
            {
                using (new TransactionScope())
                {
                    if (assertionsBeforeSave != null)
                    {
                        assertionsBeforeSave(entity, session);
                    }

                    databaseAction(session);

                    if (assertionsAfterSave != null)
                    {
                        assertionsAfterSave(entity, session);
                    }

                    session.Flush();
                    session.Clear();
                    TEntity result = session.Get<TEntity>(entity.Id);

                    if (resultAssertions != null)
                    {
                        resultAssertions(result, session);
                    }
                }
            }
        }

        protected void RunActionInTransaction(Action<ISession> actionInTransaction, ILifetimeScope childScope = null)
        {
            if (actionInTransaction == null)
            {
                Assert.Fail("No action specified.");
            }

            var sessionFactory = (childScope ?? Container).Resolve<ISessionFactoryProvider>();

            using (var session = sessionFactory.OpenSession())
            {
                using (new TransactionScope())
                {
                    actionInTransaction(session);         
                }
            }
        }
    }
}
