using System;

using Autofac;

using ServiceStack.Configuration;

namespace BetterCms.Module.Api
{
    public class AutofacContainerAdapter : IContainerAdapter
    {
        private readonly Func<ILifetimeScope> lifetimeScopeResolver;

        public AutofacContainerAdapter(Func<ILifetimeScope> lifetimeScopeResolver)
        {
            this.lifetimeScopeResolver = lifetimeScopeResolver;
        }

        public T Resolve<T>()
        {
            return lifetimeScopeResolver().Resolve<T>();
        }

        public T TryResolve<T>()
        {
            T result;

            if (lifetimeScopeResolver().TryResolve<T>(out result))
            {
                return result;
            }

            return default(T);
        }
    }
}