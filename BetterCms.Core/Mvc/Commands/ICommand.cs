using System.Security.Principal;

using BetterCms.Core.Services;

namespace BetterCms.Core.Mvc.Commands
{
    /// <summary>
    /// Defines command base contract.
    /// </summary>
    public interface ICommandBase
    {
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        ICommandContext Context { get; set; }

        /// <summary>
        /// Gets or sets the security service.
        /// </summary>
        /// <value>
        /// The security service.
        /// </value>
        ISecurityService SecurityService { get; set; }

        /// <summary>
        /// Determines whether this instance can execute.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can execute; otherwise, <c>false</c>.
        /// </returns>
        bool CanExecute();
    }

    /// <summary>
    /// Defines contract of the parameter less command.
    /// </summary>
    public interface ICommand : ICommandBase
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        void Execute();
    }

    /// <summary>
    /// Defines contract of the command with input and no output. 
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    public interface ICommand<in TRequest> : ICommandBase
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        void Execute(TRequest request);
    }

    /// <summary>
    /// Defines contract of the command with input and output.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public interface ICommand<in TRequest, out TResponse> : ICommandBase
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Executed command result.</returns>
         TResponse Execute(TRequest request);
    }
}