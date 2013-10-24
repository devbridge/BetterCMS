using Autofac;
using Autofac.Core;

using BetterCms.Core.Dependencies;
using BetterCms.Core.Exceptions.Api;

namespace BetterCms.Module.Api
{
    public static class ApiFactory
    {        
        public static IApiFacade Create()
        {
            ILifetimeScope lifetimeScope = ContextScopeProvider.CreateChildContainer();

            if (!lifetimeScope.IsRegistered<IApiFacade>())
            {
                throw new CmsApiException(string.Format("A '{0}' is unknown type in the Better CMS scope.", typeof(IApiFacade).FullName));
            }

            var api = lifetimeScope.Resolve<IApiFacade>();
            api.Scope = lifetimeScope;
            
            return api;
        }
    }
}