namespace BetterCms.Core.Mvc.Commands
{
    public interface ICommandContext
    {
        IMessagesIndicator Messages { get; }
    }    
}
