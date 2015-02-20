using System;
using System.Security.Principal;
using System.Transactions;

using Autofac;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Module.Api;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Root.Mvc;

using NHibernate;
using NUnit.Framework;

using ServiceStack.ServiceHost;

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
                using (new TransactionScope())
                {
                    var unitOfWork = api.Scope.Resolve<IUnitOfWork>();
                    actionInTransaction(api, unitOfWork.Session);
                }
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

        protected virtual TRequest CreateRequest<TRequest, TModel>(TModel data, ApiIdentity user = null)
            where TModel : SaveModelBase, new()
            where TRequest : RequestBase<TModel>, new()
        {
            return new TRequest { Data = data, User = user ?? new ApiIdentity() };
        }

        protected virtual TResponse CreateResponse<TRequest, TResponse, TModel>(
            TRequest request,
            Func<TRequest, TResponse> method)
            where TModel : SaveModelBase, new()
            where TRequest : RequestBase<TModel>, new()
            where TResponse : SaveResponseBase, new()
        {
            var response = method.Invoke(request);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsTrue(response.Data.HasValue);
            Assert.IsFalse(response.Data.Value.HasDefaultValue());

            return response;
        }

        protected virtual TResponse UpdateResponse<TRequest, TResponse, TModel>(
            TRequest request,
            Func<TRequest, TResponse> method)
            where TModel : SaveModelBase, new()
            where TRequest : PutRequestBase<TModel>, new()
            where TResponse : SaveResponseBase, new()
        {
            var response = method.Invoke(request);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual(request.Id, response.Data.Value);

            return response;
        }

        protected virtual TResponse GetResponse<TRequest, TResponse, TModel>(
            TRequest request,
            Func<TRequest, TResponse> method)
            where TModel : ModelBase, new()
            where TRequest : IReturn<TResponse>, new()
            where TResponse : ResponseBase<TModel>, new()
        {
            var response = method.Invoke(request);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);

            return response;
        }

        protected virtual TResponse DeleteResponse<TRequest, TResponse>(
            TRequest request,
            Func<TRequest, TResponse> method)
            where TRequest : DeleteRequestBase, new()
            where TResponse : DeleteResponseBase
        {
            var response = method.Invoke(request);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Data);

            return response;
        }

        protected void SetCurrentPrincipal(string[] roles)
        {
            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("John Doe"), roles);
        }
    }
}
