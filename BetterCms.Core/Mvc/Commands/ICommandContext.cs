using System.Security.Principal;

namespace BetterCms.Core.Mvc.Commands
{
    public interface ICommandContext
    {
        IMessagesIndicator Messages { get; }

        IPrincipal User { get; }
    }    
}
