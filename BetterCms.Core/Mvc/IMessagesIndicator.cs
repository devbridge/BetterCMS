using System.Collections.Generic;

namespace BetterCms.Core.Mvc
{
    public interface IMessagesIndicator
    {
        /// <summary>
        /// Gets the success messages.
        /// </summary>
        /// <value>
        /// The success.
        /// </value>
        IList<string> Success { get; }

        /// <summary>
        /// Gets the warning messages.
        /// </summary>
        /// <value>
        /// The warn.
        /// </value>
        IList<string> Warn { get; }

        /// <summary>
        /// Gets the information messages.
        /// </summary>
        /// <value>
        /// The info.
        /// </value>
        IList<string> Info { get; }

        /// <summary>
        /// Gets the error messages.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        IList<string> Error { get; }

        /// <summary>
        /// Adds success messages.
        /// </summary>
        /// <param name="success">An array of success messages.</param>
        void AddSuccess(params string[] success);

        /// <summary>
        /// Adds warning messages.
        /// </summary>
        /// <param name="warn">An array of warning messages.</param>
        void AddWarn(params string[] warn);

        /// <summary>
        /// Adds information messages.
        /// </summary>
        /// <param name="warn">An array of information messages.</param>
        void AddInfo(params string[] warn);

        /// <summary>
        /// Adds error messages.
        /// </summary>
        /// <param name="error">An array of error messages.</param>
        void AddError(params string[] error);

        /// <summary>
        /// Clears all messages.
        /// </summary>
        void Clear();
    }    
}
