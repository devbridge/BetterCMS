using System.Security.Principal;

namespace Devbridge.Platform.Core.Web.Mvc.Commands
{
    public interface ICommandContext
    {
        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
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
