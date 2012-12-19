using System.Collections.Generic;

using BetterCms.Core.Mvc;

namespace BetterCms.Module.Root.Models
{
    /// <summary>
    /// Data contract to handle user messages.
    /// </summary>
    public class UserMessages : IMessagesIndicator
    {
        /// <summary>
        /// Messages container.
        /// </summary>
        private IDictionary<MessageType, List<string>> messages;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserMessages" /> class.
        /// </summary>
        public UserMessages()
        {
            messages = new Dictionary<MessageType, List<string>>(); 
      
            messages.Add(MessageType.Success, new List<string>());
            messages.Add(MessageType.Info, new List<string>());
            messages.Add(MessageType.Warn, new List<string>());
            messages.Add(MessageType.Error, new List<string>());
        }

        /// <summary>
        /// Available message types.
        /// </summary>
        private enum MessageType
        {
            Success,
            Info,
            Warn,
            Error
        }

        /// <summary>
        /// Gets the success messages.
        /// </summary>
        /// <value>
        /// The success.
        /// </value>
        public IList<string> Success
        {
            get
            {
                return messages[MessageType.Success].AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the warning messages.
        /// </summary>
        /// <value>
        /// The warn.
        /// </value>
        public IList<string> Warn
        {
            get
            {
                return messages[MessageType.Warn].AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the information messages.
        /// </summary>
        /// <value>
        /// The info.
        /// </value>
        public IList<string> Info
        {
            get
            {
                return messages[MessageType.Info].AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the error messages.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public IList<string> Error
        {
            get
            {
                return messages[MessageType.Error].AsReadOnly();
            }
        }        
        
        /// <summary>
        /// Adds success messages.
        /// </summary>
        /// <param name="success">An array of success messages.</param>
        public void AddSuccess(params string[] success)
        {
            messages[MessageType.Success].AddRange(success);
        }

        /// <summary>
        /// Adds warning messages.
        /// </summary>
        /// <param name="warn">An array of warning messages.</param>
        public void AddWarn(params string[] warn)
        {
            messages[MessageType.Warn].AddRange(warn);
        }

        /// <summary>
        /// Adds information messages.
        /// </summary>
        /// <param name="warn">An array of information messages.</param>
        public void AddInfo(params string[] warn)
        {
            messages[MessageType.Info].AddRange(warn);
        }

        /// <summary>
        /// Adds error messages.
        /// </summary>
        /// <param name="error">An array of error messages.</param>
        public void AddError(params string[] error)
        {
            messages[MessageType.Error].AddRange(error);
        }
        
        /// <summary>
        /// Clears all messages.
        /// </summary>
        public void Clear()
        {
            messages[MessageType.Success].Clear();
            messages[MessageType.Info].Clear();
            messages[MessageType.Warn].Clear();
            messages[MessageType.Error].Clear();
        }        
    }
}