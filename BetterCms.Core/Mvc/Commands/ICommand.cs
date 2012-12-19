namespace BetterCms.Core.Mvc.Commands
{
    public interface ICommandBase
    {
        ICommandContext Context { get; set; }
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