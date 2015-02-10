namespace Devbridge.Platform.Core.Web.Mvc.Commands
{
    public interface ICommandResolver
    {
        TCommand ResolveCommand<TCommand>(ICommandContext context) where TCommand : ICommandBase;
    }
}
