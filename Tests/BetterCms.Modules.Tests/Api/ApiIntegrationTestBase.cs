using System;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Api;
using NHibernate;
using NUnit.Framework;

namespace BetterCms.Test.Module.Api
{
    public class ApiIntegrationTestBase : IntegrationTestBase
    {
        protected void RunApiActionInTransaction(Action<IApiFacade, ISession> actionInTransaction)
        {
            if (actionInTransaction == null)
            {
                Assert.Fail("No action specified.");
            }

            using (var api = ApiFactory.Create())
            {
                RunActionInTransaction(
                    session =>
                    {
                        actionInTransaction(api, session);
                    }, api.Scope);
            }
        }

        protected IUnitOfWork GetUnitOfWork(ISession session)
        {
            return new DefaultUnitOfWork(session);
        }
        
        protected IRepository GetRepository(ISession session, IUnitOfWork unitOfWork = null)
        {
            if (unitOfWork == null)
            {
                unitOfWork = GetUnitOfWork(session);
            }

            return new DefaultRepository(unitOfWork);
        }
    }
}
