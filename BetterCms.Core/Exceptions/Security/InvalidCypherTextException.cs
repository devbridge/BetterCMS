using System;

namespace BetterCms.Core.Exceptions.Security
{    
    /// <summary>
    /// Represents errors that occur when a text can't be decoded or the text is tampered 
    /// </summary>
    [Serializable]
    public class InvalidCypherTextException : CmsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCypherTextException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidCypherTextException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCypherTextException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidCypherTextException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
