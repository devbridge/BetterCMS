using Autofac;

using BetterCms.Core.Dependencies;
using BetterCms.Core.Services;

namespace BetterCms.Core.Mvc.Commands
{
    public class DefaultCommandResolver : ICommandResolver
    {
        private readonly PerWebRequestContainerProvider containerProvider;

        public DefaultCommandResolver(PerWebRequestContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
        }

        public TCommand ResolveCommand<TCommand>(ICommandContext context) where TCommand : ICommandBase
        {
            if (containerProvider.CurrentScope.IsRegistered<TCommand>())
            {
                var command = containerProvider.CurrentScope.Resolve<TCommand>();
                command.Context = context;                

                return command;
            }

            return default(TCommand);
        }
    }
}
