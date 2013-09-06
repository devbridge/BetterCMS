using System.Security.Principal;

namespace BetterCms.Core.Mvc.Commands
{
    public interface ICommandContext
    {
        IMessagesIndicator Messages { get; }

        /// <summary>
        /// Gets the principal of current command context.
        /// </summary>
        /// <value>
        /// The current command principal.
        /// </value>
        IPrincipal Principal { get; }
    }    
}
